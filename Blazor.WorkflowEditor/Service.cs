using System.Activities;
using System.Collections.ObjectModel;
using System.Reflection;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.WorkflowEditor.Activity;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WorkflowEditor {

    public partial class Service : IDisposable {
        private System.Activities.Activity activity = default!;
        private Diagram designer = default!;
        private Action updateState = default!;
        private List<ActivityDesignerPair> items = new();
        private List<ActivityDesignerPair> selectedItems = new();
        private List<(ActivityDesignerPair, ActivityDesignerPair)> selectedLinks = new();

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

            this.Path.CollectionChanged += pathChanged;

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
            this.activity ??= activity!;

            var result = addActivity(activity!);

            var lastNode = Path.LastOrDefault()?.Reference?.Node;
            if (lastNode != null)
                lastNode.AddChild(result);

            return (true, result);
        }

        public System.Activities.Activity GetActivity() {
            return activity;
        }

        public void New() {
            SetActivity(new System.Activities.DynamicActivity());
        }

        public void SetActivity(System.Activities.Activity activity) {
            Path.CollectionChanged -= pathChanged;
            Path.Clear();

            this.activity = activity;

            //read in/out arguments
            if (activity is IDynamicActivity dynamicActivity) {
                foreach (var property in dynamicActivity.Properties) {
                    Variable variable = new() {
                        Activity = activity,
                        Name = property.Name,
                        Type = property.Type,
                        DefaultValue = property.Value
                    };
                    Variables.Add(variable);
                }
            }

            Path.CollectionChanged += pathChanged;
            Path.Add(PathItem.Root);
        }

        public void Open(Activity.DefaultNode node) {
            var item = getById(node.Id);
            if (item is null)
                return;

            Path.Add(new PathItem(item));
        }

        public void OpenPath(PathItem pathItem) {
            if (Path.Contains(pathItem) == false)
                return;

            while (Path.Last() != pathItem)
                Path.RemoveAt(Path.Count - 1);
        }

        public bool CheckAddActivity(Type activityType) {
            if (activity == null)
                return false;

            var last = Path.Last();
            if (last.Reference == null)
                return false;

            if (last.Reference.Node.IsContainer == false)
                return false;

            return true;
        }


        internal LinkModel LinkFromTo(ActivityDesignerPair from, ActivityDesignerPair to) {
            var linkModel = new LinkModel(from.Node.OutcomingPort, to.Node.IncomingPort);
            linkModel.TargetMarker = LinkMarker.Arrow;
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

        private void pathChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
                var items = e.NewItems!.OfType<PathItem>();
                foreach (var newItem in items) {
                    var node = newItem.Reference?.Node;
                    if (node == null)
                        continue;

                    foreach (var _var in node.GetVariables()) {
                        Variable variable = new() {
                            Activity = newItem.Reference!.Activity,
                            Name = _var.Name,
                            Type = _var.Type,
                            DefaultValue = _var.Default
                        };
                        Variables.Add(variable);
                    }

                }

                load(items.Last()?.Reference);
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
                var variablesForRemove = (from old in e.OldItems!.OfType<PathItem>()
                                          join variable in this.Variables on old?.Reference?.Activity equals variable.Activity
                                          where !variable.IsArgument
                                          select variable).ToList();
                variablesForRemove.ForEach(p => this.Variables.Remove(p));

                load(Path.Last().Reference);
            }

            updateState();

        }

        private void load(ActivityDesignerPair? activityDesignerPair) {
            designer.Nodes.Clear();

            this.selectedItems.Clear();
            this.selectedLinks.Clear();

            if (activityDesignerPair is not null)
                activityDesignerPair.Node.LoadChilds(addActivity);
            else
                addActivity(activity);
        }

        private ActivityDesignerPair addActivity(System.Activities.Activity activity) {
            if (this.activity == null)
                this.activity = activity!;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
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
            return null;
        }
    }

}
