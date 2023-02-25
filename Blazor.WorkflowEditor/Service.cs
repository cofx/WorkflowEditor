using System.Activities;
using System.Collections.ObjectModel;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.WorkflowEditor.Activity;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WorkflowEditor {

    public partial class Service : IDisposable {
        private ActivityBuilder activityBuilder = default!;
        private readonly Diagram designer = default!;
        private readonly Action updateState = default!;
        private readonly List<ActivityDesignerPair> items = new();
        private readonly List<ActivityDesignerPair> selectedItems = new();
        private readonly List<(ActivityDesignerPair, ActivityDesignerPair)> selectedLinks = new();

        public IEnumerable<ActivityDesignerPair> Items => items;
        public IEnumerable<ActivityDesignerPair> SelectedItems => selectedItems;
        public IEnumerable<(ActivityDesignerPair source, ActivityDesignerPair target)> SelectedLinks => selectedLinks;

        public ObservableCollection<PathItem> Path = new();
        public ObservableCollection<Variable> Variables { get; set; } = new();

        public Diagrams.Core.Geometry.Rectangle? DiagramContainer => this.designer.Container;

        public event Action? SelectedOnMove;

        public Service(Diagram designer, Action updateState) {
            this.designer = designer;

            this.designer.SelectionChanged += selectionChanged;
            this.designer.MouseDoubleClick += mouseDoubleClick;
            this.designer.MouseUp += mouseUp;
            this.designer.PanChanged += panChanged;
            this.designer.ZoomChanged += zoomChanged;

            this.updateState = updateState;
        }

        public void Dispose() {
            this.designer.SelectionChanged -= selectionChanged;
            this.designer.MouseDoubleClick -= mouseDoubleClick;

            this.designer.MouseUp -= mouseUp;

            this.designer.PanChanged -= panChanged;
            this.designer.ZoomChanged -= zoomChanged;
        }

        public void Delete(Activity.DefaultNode node) {
            var item = getById(node.Id);
            if (item is null)
                return;

            designer.Nodes.Remove(node);

            //Remove activity in parent
            Path.Last()?.Reference?.Node?.RemoveChild(item.Activity);

            selectedItems.Remove(item);
            items.Remove(item);
        }

        /// <summary>
        /// Add by activity type
        /// </summary>
        public (bool hasAdded, ActivityDesignerPair result) AddActivity(Type activityType, params Type[] types) {
            object? activityObject;
            if (types != null && types.Count() > 0) {
                activityObject = Activator.CreateInstance(activityType.MakeGenericType(types));
            } else {
                activityObject = Activator.CreateInstance(activityType);
            }

            if (activityObject == null)
                return (false, default!);

            var activity = activityObject as System.Activities.Activity;
            var result = addActivity(activity!);

            var lastNode = Path.LastOrDefault()?.Reference?.Node;
            lastNode?.AddChild(result);

            return (true, result);
        }

        public ActivityBuilder GetActivityBuilder() {
            return this.activityBuilder;
        }

        public void SetActivityBuilder(ActivityBuilder activityBuilder) {
            this.activityBuilder = activityBuilder;

            var da = new DynamicActivity {
                Implementation = () => activityBuilder.Implementation,
                DisplayName = "ActivityBuilder"
            };
            activityBuilder.Properties.ToList().ForEach(p => da.Properties.Add(p));
            activityBuilder.Constraints.ToList().ForEach(p => da.Constraints.Add(p));
            activityBuilder.Attributes.ToList().ForEach(p => da.Attributes.Add(p));
            var pair = addActivity(da);

            Path.Clear();
            Path.Add(new(pair));
            updatePath();
        }

        public void Open(Activity.DefaultNode node) {
            var item = getById(node.Id);
            if (item is null)
                return;

            Path.Add(new PathItem(item));
            updatePath();

        }

        public void OpenPath(PathItem pathItem) {
            if (Path.Contains(pathItem) == false)
                return;

            while (Path.Last() != pathItem)
                Path.RemoveAt(Path.Count - 1);

            updatePath();
        }

        public bool CheckAddActivity(Type activityType) {
            if (activityBuilder?.Implementation == null || activityType == null)
                return false;

            var last = Path.Last();
            if (last.Reference == null)
                return false;

            if (last.Reference.Node.IsContainer == false)
                return false;

            return true;
        }

        internal LinkModel LinkFromTo(ActivityDesignerPair from, ActivityDesignerPair to) {
            var linkModel = new LinkModel(from.Node.OutcomingPort, to.Node.IncomingPort) {
                TargetMarker = LinkMarker.Arrow
            };
            designer.Links.Add(linkModel);
            return linkModel;
        }
        internal void RemoveLinkFromTo(ActivityDesignerPair from, ActivityDesignerPair to) {
            var link = designer.Links.FirstOrDefault(p => p.SourceNode == from.Node && p.TargetNode == to.Node);
            if (link != null)
                designer.Links.Remove(link);

            selectedLinks.Remove((from, to));
        }

        internal ActivityDesignerPair GetPair(System.Activities.Activity source) => this.items.First(p => p.Activity == source);
        internal ActivityDesignerPair GetPair(DefaultNode node) => this.items.First(p => p.Node == node);

        private ActivityDesignerPair? getById(string id) => this.items.FirstOrDefault(p => p.Node.Id == id);

        private void selectionChanged(Diagrams.Core.Models.Base.SelectableModel obj) {
            if (obj is NodeModel) {
                var item = getById(obj.Id);
                if (item == null)
                    return;

                if (obj.Selected)
                    selectedItems.Add(item);
                else
                    selectedItems.Remove(item);

            } else
            if (obj is LinkModel link) {
                if (link.TargetNode == null)
                    return;

                var source = getById(link.SourceNode.Id);
                var target = getById(link.TargetNode.Id);

                if (source == null || target == null)
                    return;

                if (obj.Selected) {
                    selectedLinks.Add((source, target));
                } else {
                    selectedLinks.Remove((source, target));
                }
            }
        }

        private void mouseDoubleClick(Diagrams.Core.Models.Base.Model arg1, Microsoft.AspNetCore.Components.Web.MouseEventArgs arg2) {
            var item = getById(arg1.Id);
            if (item == null || item.Node.IsContainer == false)
                return;

            Open(item.Node);
        }

        private void mouseUp(Model model, MouseEventArgs arg) {
            if (arg.OffsetX > 50 || arg.OffsetY > 50) {
                if (model is DefaultNode node) {
                    SelectedOnMove?.Invoke();

                    node.UpdateViewState();
                }
            }
        }
        private void zoomChanged() {
            //this.Path.Last().Reference.Node.
            //throw new NotImplementedException();
        }

        private void panChanged() {
            //throw new NotImplementedException();
        }

        private void updatePath() {
            //TODO: for variable try use
            //System.Activities.ScopeUtils.GetLocals(this Activity activity) 

            this.designer.Nodes.Clear();

            this.selectedItems.Clear();
            this.selectedLinks.Clear();

            Variables.Clear();
            foreach (var item in Path.SelectMany(p => p.Node.GetVariables()))
                Variables.Add(item);

            Path.Last().Reference.Node.LoadChilds(addActivity);
            updateState();

        }

        private ActivityDesignerPair addActivity(System.Activities.Activity activity) {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
                foreach (Type type in assembly.GetTypes()) {
                    DefaultNode? node = null;
                    if (type.GetCustomAttributes(typeof(PairAttribute), true).Length > 0) {
                        var attr = type.GetCustomAttributes(typeof(PairAttribute), true).FirstOrDefault();
                        if (attr == null) continue;
                        var activityType = activity.GetType();
                        if (activityType.IsGenericType) {
                            if (activityType.GetGenericTypeDefinition() != ((PairAttribute)attr).Activity) continue;
                            var genericTypes = activityType.GenericTypeArguments;
                            node = (Activator.CreateInstance(type.MakeGenericType(genericTypes), this, activity) as Activity.DefaultNode)!;
                            if (designer.GetComponentForModel(node) == null) {
                                designer.RegisterModelComponent(type.MakeGenericType(genericTypes), ((PairAttribute)attr).Control.MakeGenericType(genericTypes));
                            }
                        } else {
                            if (activityType != ((PairAttribute)attr).Activity) continue;
                            node = (Activator.CreateInstance(type, this, activity) as Activity.DefaultNode)!;
                            if (designer.GetComponentForModel(node) == null) {
                                designer.RegisterModelComponent(type, ((PairAttribute)attr).Control);
                            }
                        }
                        if (node == null) continue;
                        designer.Nodes.Add(node);
                        node.RestoreViewState();
                        ActivityDesignerPair result = new() { Activity = activity!, Node = node };
                        items.Add(result);
                        return result;
                    }
                }
            }
            throw new NotSupportedException();
        }

        public void RefreshVariables() {
            Variables.Clear();
            foreach (var item in Path.SelectMany(p => p.Node.GetVariables()))
                Variables.Add(item);
        }

        public virtual void AddVariable<TActivity>(TActivity activity, string name, Type type, string defaultValue) where TActivity : class {
            var genType = typeof(System.Activities.Variable<>).MakeGenericType(type);
            object?[] constructorParams;
            try {
                var defType = Convert.ChangeType(defaultValue, type);
                constructorParams = new object?[] { name, defType };
            } catch {
                constructorParams = new object?[] { name };
            }
            var variable = Activator.CreateInstance(genType, constructorParams);
            if (variable != null) {
                if (activity is System.Activities.Statements.Sequence) {
                    (activity as System.Activities.Statements.Sequence)!.Variables.Add((System.Activities.Variable)variable);
                }

                if (activity is System.Activities.Statements.Flowchart) {
                    (activity as System.Activities.Statements.Flowchart)!.Variables.Add((System.Activities.Variable)variable);
                }
            }
        }

        public virtual void RemoveVariable<TActivity>(TActivity activity, string name) where TActivity : class {
            if (activity is System.Activities.Statements.Sequence) {
                var variable = (activity as System.Activities.Statements.Sequence)!.Variables.FirstOrDefault(p => p.Name == name);
                if (variable != null) {
                    (activity as System.Activities.Statements.Sequence)!.Variables.Remove(variable);
                }
            }
            if (activity is System.Activities.Statements.Flowchart) {
                var variable = (activity as System.Activities.Statements.Flowchart)!.Variables.FirstOrDefault(p => p.Name == name);
                if (variable != null) {
                    (activity as System.Activities.Statements.Flowchart)!.Variables.Remove(variable);
                }
            }
        }

        public virtual void UpdateVariable<TActivity>(TActivity activity, string oldName, string name, Type type, string defaultValue) where TActivity : class {
            RemoveVariable(activity, oldName);
            AddVariable(activity, name, type, defaultValue);
        }
    }
}
