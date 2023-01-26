using System.Xaml;

namespace Blazor.WorkflowEditor.Activity.State;

public class ViewStateStore {
    public static readonly AttachableMemberIdentifier ViewStateIdentifier =
        new AttachableMemberIdentifier(typeof(ViewStateStore), "ViewState");

    public static ViewState Get(object target) {
        AttachablePropertyServices.TryGetProperty<ViewState>(target, ViewStateIdentifier, out ViewState value);
        return value;
    }

    public static void Set(object target, ViewState value) {
        AttachablePropertyServices.SetProperty(target, ViewStateIdentifier, value);
    }
}

public enum PortAlignment {
    Top = 0,
    TopRight = 1,
    Right = 2,
    BottomRight = 3,
    Bottom = 4,
    BottomLeft = 5,
    Left = 6,
    TopLeft = 7
}

public class ViewState {

    public int? CenterX { get; set; }
    public int? CenterY { get; set; }
    public int? Widht { get; set; }
    public int? Height { get; set; }
    public int? OffcetX { get; set; }
    public int? OffcetY { get; set; }
    public string? Comment { get; set; }
    public bool? IsExpanded { get; set; }
    public double? Zoom { get; set; }
    public PortAlignment? IncomingPortAlign { get; set; }
    public PortAlignment? OutcomingPortAlign { get; set; }

    public bool IsEmpty() {
        return CenterX == null && CenterY == null &&
            Widht == null && Height == null &&
            Comment == null &&
            IsExpanded == null &&
            Zoom == null &&
            OffcetX == null && OffcetY == null &&
            IncomingPortAlign == null &&
            OutcomingPortAlign == null;
    }

}

