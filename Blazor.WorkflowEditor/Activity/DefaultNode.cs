using System.Collections.ObjectModel;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity;

public class DefaultNode : NodeModel {
    internal readonly Service service;
    private readonly System.Activities.Activity activity;

    public string Text { get => activity.DisplayName; set => activity.DisplayName = value; }

    public bool IsContainer { get; init; } = false;
    public bool IsExpanded { get; set; } = false;

    public string CssClass {
        get {
            var result = String.Empty;
            if (base.Selected)
                result += " isSelected";
            if (this.IsContainer)
                result += " isContainer";
            if (this.IsExpanded)
                result += " isExpanded";

            return result;
        }
    }

    private PortModel FromPort = default!;
    private PortModel ToPort = default!;


    public DefaultNode(Service service, System.Activities.Activity activity) : base() {
        this.service = service;
        this.activity = activity;

        ToPort = this.AddPort(PortAlignment.Top);
        FromPort = this.AddPort(PortAlignment.Bottom);
    }

    public virtual PortModel GetFromPort() => FromPort;
    public virtual PortModel GetToPort() => ToPort;

    public virtual Collection<System.Activities.Variable> GetVariables() {
        return new Collection<System.Activities.Variable>();
    }

    public virtual void LoadChilds(Func<System.Activities.Activity, ActivityDesignerPair> addActivity) {


    }
    public virtual void AddChild(ActivityDesignerPair source) {

    }

    public virtual void RemoveChild(System.Activities.Activity child) {

    }

}

