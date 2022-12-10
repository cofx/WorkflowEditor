using System.Collections.ObjectModel;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity;

public class DefaultNode : NodeModel {
    private Size defaultSize = new Size(250, 114);

    internal readonly Service service;
    private readonly System.Activities.Activity activity;

    public string Text { get => activity.DisplayName; set => activity.DisplayName = value; }
    public string? Comment { get; set; }
    public bool IsContainer { get; init; } = false;
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

    //for save/restore state
    private State.ViewState? viewState;

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

    static bool? activityContainsAttachedPropertiesType;
    static System.Reflection.PropertyInfo? attachedPropertiesType;

    public bool RestoreViewState() {
        if (activityContainsAttachedPropertiesType == null) {
            attachedPropertiesType = activity.GetType().GetProperty("AttachedProperties");
            activityContainsAttachedPropertiesType = attachedPropertiesType != null;
        }

        if (activityContainsAttachedPropertiesType == false)
            return false;

        //Check ViewState property
        if (attachedPropertiesType != null) {
            var attachedProperties = attachedPropertiesType.GetValue(this.activity) as Dictionary<string, Object>;
            if (attachedProperties == null)
                attachedProperties = new Dictionary<string, object>();

            attachedProperties.TryGetValue("ViewState", out var storedState);
            if (storedState is State.ViewState defaultViewState && defaultViewState != null)
                this.viewState = defaultViewState;
            else {
                this.viewState = new State.ViewState();
                attachedProperties.Add("ViewState", this.viewState);
            }
        }
        if (viewState == null)
            return false;

        if (viewState.IsEmpty()) {
            this.CenterPosition = new Diagrams.Core.Geometry.Point(
                this.service.DiagramContainer!.Width / 2,
                this.service.DiagramContainer!.Height / 2);
            return true;
        }

        //This is tempory code...

        if (viewState.cX.HasValue && viewState.cY.HasValue)
            this.CenterPosition = new Point((double)viewState.cX, (double)viewState.cY);

        if (viewState.Integers != null) {
            if (viewState.Integers.TryGetValue("cX", out int x) && viewState.Integers.TryGetValue("cY", out int y))
                this.CenterPosition = new Point((double)x, (double)y);
        }

        if (viewState.W.HasValue && viewState.H.HasValue)
            this.Size = new Size((double)viewState.W, (double)viewState.H);

        if (viewState.Comment != null)
            this.Comment = viewState.Comment;

        if (viewState.IsExpanded != null)
            this.IsExpanded = viewState.IsExpanded.Value;

        if (viewState.Zoom != null)
            this.Zoom = viewState.Zoom;

        if (viewState.oX.HasValue && viewState.oY.HasValue)
            this.Offcet = new Point((double)viewState.oX, (double)viewState.oY);

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

        return true;
    }

    public bool HasViewState => viewState != null && !viewState.IsEmpty();

    public void UpdateViewState() {
        //This is tempory code...

        if (viewState == null)
            return;


        this.viewState.cX = (int)this.CenterPosition.X;
        this.viewState.cY = (int)this.CenterPosition.Y;

        if (this.viewState.Integers == null)
            this.viewState.Integers = new();

        this.viewState.Integers["cX"] = (int)this.CenterPosition.X;
        this.viewState.Integers["cY"] = (int)this.CenterPosition.Y;

        if (this.Size != null && this.Size != this.defaultSize) {
            this.viewState.W = (int)this.Size.Width;
            this.viewState.H = (int)this.Size.Height;
        } else {
            this.viewState.W = null;
            this.viewState.H = null;
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
            this.viewState.oX = (int)this.Offcet.X;
            this.viewState.oY = (int)this.Offcet.Y;
        } else {
            this.viewState.oX = null;
            this.viewState.oY = null;
        }
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

