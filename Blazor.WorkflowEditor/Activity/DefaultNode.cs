using System.Activities;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity;

public class DefaultNode : NodeModel {
    private readonly Size defaultSize = new(250, 114);

    //    internal readonly Service service;
    public readonly Service service;
    private readonly System.Activities.Activity activity;

    public string DisplayName { get => activity.DisplayName; set => activity.DisplayName = value; }
    public string? Comment { get; set; }
    public bool IsContainer { get; init; } = false;
    public bool IsGeneric { get; set; } = false;
    public bool IsExpanded { get; set; } = false;
    public double? Zoom { get; set; }
    public Point? Offcet { get; set; }

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
            var result = string.Empty;
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

    private readonly PortModel leftPort = default!;
    private readonly PortModel topPort = default!;
    private readonly PortModel rightPort = default!;
    private readonly PortModel bottomPort = default!;

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

    public bool HasViewState => State.Designer.HasProperty(activity);

    public bool RestoreViewState() {

        var centerX = State.Designer.GetCenterX(activity);
        var centerY = State.Designer.GetCenterY(activity);
        if (centerX != null && centerY != null)
            this.CenterPosition = new Point((double)centerX, (double)centerY);
        else
            this.CenterPosition = new Diagrams.Core.Geometry.Point(
                this.service.DiagramContainer!.Width / 2,
                this.service.DiagramContainer!.Height / 2);
        /*
        this.viewState = Blazor.WorkflowEditor.Activity.State.Designer.Get(activity);

        if (this.viewState == null) {
            this.viewState = new();
            Blazor.WorkflowEditor.Activity.State.Designer.Set(activity, this.viewState);
        }

        if (viewState.IsEmpty()) {
            if (this.service.DiagramContainer != null)
                this.CenterPosition = new Diagrams.Core.Geometry.Point(
                    this.service.DiagramContainer!.Width / 2,
                    this.service.DiagramContainer!.Height / 2);

            return true;
        }

        //This is tempory code...

        if (viewState.CenterX.HasValue && viewState.CenterY.HasValue)
            this.CenterPosition = new Point((double)viewState.CenterX, (double)viewState.CenterY);

        if (viewState.Widht.HasValue && viewState.Height.HasValue)
            this.Size = new Size((double)viewState.Widht, (double)viewState.Height);

        if (viewState.Comment != null)
            this.Comment = viewState.Comment;

        if (viewState.IsExpanded != null)
            this.IsExpanded = viewState.IsExpanded.Value;

        if (viewState.Zoom != null)
            this.Zoom = viewState.Zoom;

        if (viewState.OffcetX.HasValue && viewState.OffcetY.HasValue)
            this.Offcet = new Point((double)viewState.OffcetX, (double)viewState.OffcetY);

        if (viewState.IncomingPortAlign != null) {
            var _incomingPort = viewState.IncomingPortAlign switch {
                State.PortAlignment.Top => topPort,
                State.PortAlignment.Left => leftPort,
                State.PortAlignment.Right => rightPort,
                State.PortAlignment.Bottom => bottomPort,
                _ => topPort
            };
            SetIncoming(_incomingPort);
        }

        if (viewState.OutcomingPortAlign != null) {
            var _outcomingPort = viewState.OutcomingPortAlign switch {
                State.PortAlignment.Top => topPort,
                State.PortAlignment.Left => leftPort,
                State.PortAlignment.Right => rightPort,
                State.PortAlignment.Bottom => bottomPort,
                _ => bottomPort
            };
            SetOutcoming(_outcomingPort);
        }
        */
        return true;
    }

    public void UpdateViewState() {
        static bool sizeCompare(Blazor.Diagrams.Core.Geometry.Size sourse, Blazor.Diagrams.Core.Geometry.Size destination) =>
            Math.Abs(sourse.Width - destination.Width) < 1 && Math.Abs(sourse.Height - destination.Height) < 1;

        State.Designer.SetCenterX(activity, (int?)this.CenterPosition.X);
        State.Designer.SetCenterY(activity, (int?)this.CenterPosition.Y);


        /*
        if (this.Size != null && sizeCompare(this.Size, this.defaultSize) == false) {
            this.viewState.Widht = (int)this.Size.Width;
            this.viewState.Height = (int)this.Size.Height;
        } else {
            this.viewState.Widht = null;
            this.viewState.Height = null;
        }

        this.viewState.Comment = this.Comment;
        this.viewState.IsExpanded = this.IsExpanded ? true : null;

        this.viewState.IncomingPortAlign = this.IncomingPort.Alignment switch {
            PortAlignment.Top => State.PortAlignment.Top,
            PortAlignment.Left => State.PortAlignment.Left,
            PortAlignment.Right => State.PortAlignment.Right,
            PortAlignment.Bottom => State.PortAlignment.Bottom,
            _ => State.PortAlignment.Top
        };
        this.viewState.OutcomingPortAlign = this.OutcomingPort.Alignment switch {
            PortAlignment.Top => State.PortAlignment.Top,
            PortAlignment.Left => State.PortAlignment.Left,
            PortAlignment.Right => State.PortAlignment.Right,
            PortAlignment.Bottom => State.PortAlignment.Bottom,
            _ => State.PortAlignment.Top
        };

        this.viewState.Zoom = Zoom;

        if (this.Offcet != null) {
            this.viewState.OffcetX = (int)this.Offcet.X;
            this.viewState.OffcetY = (int)this.Offcet.Y;
        } else {
            this.viewState.OffcetX = null;
            this.viewState.OffcetY = null;
        }
        */
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

    protected IEnumerable<Variable> GetVariables(IEnumerable<System.Activities.Variable> source) {
        var result = new List<Variable>();
        foreach (var property in source) {
            Variable variable = new() {
                Activity = activity,
                Name = property.Name,
                Type = property.Type,
                DefaultValue = property.Default
            };
            result.Add(variable);
        }
        return result;
    }

    protected IEnumerable<Variable> GetVariables(IEnumerable<DynamicActivityProperty> source) {
        var result = new List<Variable>();
        foreach (var property in source) {
            Variable variable = new() {
                Activity = activity,
                Name = property.Name,
                Type = property.Type,
                DefaultValue = property.Value
            };
            result.Add(variable);
        }
        return result;
    }

    public virtual IEnumerable<Variable> GetVariables() {
        return Enumerable.Empty<Variable>();
    }

    public virtual void LoadChilds(Func<System.Activities.Activity, ActivityDesignerPair> addActivity) {

    }
    public virtual void AddChild(ActivityDesignerPair source) {

    }
    public virtual void RemoveChild(System.Activities.Activity child) {

    }

}

