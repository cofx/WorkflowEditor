using System.Collections.ObjectModel;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity;

public class DefaultNode : NodeModel {
    private Size defaultSize = new Size(250, 114);

    internal readonly Service service;
    private readonly System.Activities.Activity activity;

    public string Text { get => activity.DisplayName; set => activity.DisplayName = value; }

    public bool IsContainer { get; init; } = false;
    public bool IsExpanded { get; set; } = false;


    /// <summary>
    /// Center point
    /// </summary>
    public Point CenterPosition {
        get {
            if (Size != null)
                return this.Position.Add(this.Size.Width / 2.0, this.Size.Height / 2.0);
            else
                return this.Position;
        }
        set {
            if (Size != null)
                this.Position = value.Add(-this.Size.Width / 2.0, -this.Size.Height / 2.0);
            else
                this.Position = value;
        }
    }

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

    public PortModel IncomingPort { get; private set; }
    public PortModel OutcomingPort { get; private set; }

    private PortModel leftPort = default!;
    private PortModel topPort = default!;
    private PortModel rightPort = default!;
    private PortModel bottomPort = default!;


    public DefaultNode(Service service, System.Activities.Activity activity) : base() {
        this.service = service;
        this.activity = activity;

        this.Size = defaultSize;

        topPort = this.AddPort(PortAlignment.Top);
        topPort.Locked = true;
        bottomPort = this.AddPort(PortAlignment.Bottom);
        bottomPort.Locked = true;
        leftPort = this.AddPort(PortAlignment.Left);
        leftPort.Locked = true;
        rightPort = this.AddPort(PortAlignment.Right);
        rightPort.Locked = true;

        IncomingPort = topPort;
        OutcomingPort = bottomPort;
    }


    public void SetOutcoming(PortModel port) {
        if (this.OutcomingPort == port)
            return;
        var links = this.AllLinks.Where(p => p.SourcePort == this.OutcomingPort).ToList();
        this.OutcomingPort = port;
        links.ForEach(p => p.SetSourcePort(this.OutcomingPort));
        this.RefreshAll();
    }

    public void SetIncoming(PortModel port) {
        if (this.IncomingPort == port)
            return;
        var links = this.AllLinks.Where(p => p.TargetPort == this.IncomingPort).ToList();
        this.IncomingPort = port;
        links.ForEach(p => p.SetTargetPort(this.IncomingPort));
        this.RefreshAll();
    }

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

