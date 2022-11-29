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
    private ViewState? viewState;

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
            if (storedState is ViewState defaultViewState && defaultViewState != null)
                this.viewState = defaultViewState;
            else {
                this.viewState = new ViewState();
                attachedProperties.Add("ViewState", this.viewState);
            }
        }

        if (viewState == null)
            return false;

        if (viewState.CenterPosition != null && this.CenterPosition != viewState.CenterPosition)
            this.CenterPosition = viewState.CenterPosition;

        if (viewState.Size != null && this.Size != viewState.Size)
            this.Size = viewState.Size;

        if (viewState.Comment != null && this.Comment != viewState.Comment)
            this.Comment = viewState.Comment;

        if (viewState.IsExpanded != null && this.IsExpanded != viewState.IsExpanded)
            this.IsExpanded = viewState.IsExpanded.Value;

        if (viewState.Zoom != null && this.Zoom != viewState.Zoom)
            this.Zoom = viewState.Zoom;

        if (viewState.Offcet != null && this.Offcet != viewState.Offcet)
            this.Offcet = viewState.Offcet;

        if (viewState.IncomingPortAlign != null) {
            var _incomingPort = viewState.IncomingPortAlign switch {
                PortAlignment.Top => topPort,
                PortAlignment.Left => leftPort,
                PortAlignment.Right => rightPort,
                PortAlignment.Bottom => bottomPort,
                _ => topPort
            };
            SetIncoming(_incomingPort);
        }

        if (viewState.OutcomingPortAlign != null) {
            var _outcomingPort = viewState.OutcomingPortAlign switch {
                PortAlignment.Top => topPort,
                PortAlignment.Left => leftPort,
                PortAlignment.Right => rightPort,
                PortAlignment.Bottom => bottomPort,
                _ => bottomPort
            };
            SetOutcoming(_outcomingPort);
        }

        return true;
    }

    public void UpdateViewState() {
        if (viewState == null)
            return;

        this.viewState.CenterPosition = this.CenterPosition;
        this.viewState.Size = this.Size != this.defaultSize ? this.Size : null;
        this.viewState.Comment = this.Comment;
        this.viewState.IsExpanded = this.IsExpanded ? true : null;

        this.viewState.IncomingPortAlign = this.IncomingPort.Alignment;
        this.viewState.OutcomingPortAlign = this.OutcomingPort.Alignment;

        this.viewState.Zoom = Zoom;
        this.viewState.Offcet = Offcet;
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

