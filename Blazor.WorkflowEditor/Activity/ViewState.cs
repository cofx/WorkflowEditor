namespace Blazor.WorkflowEditor.Activity.State;

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

    public int? cX { get; set; }
    public int? cY { get; set; }
    public int? W { get; set; }
    public int? H { get; set; }
    public int? oX { get; set; }
    public int? oY { get; set; }
    public string? Comment { get; set; }
    public bool? IsExpanded { get; set; }
    public double? Zoom { get; set; }
    public PortAlignment? IncomingPortAlign { get; set; }
    public PortAlignment? OutcomingPortAlign { get; set; }

    public bool IsEmpty() {
        return cX == null && cY == null &&
            W == null && H == null &&
            Comment == null &&
            IsExpanded == null &&
            Zoom == null &&
            oX == null && oY == null &&
            IncomingPortAlign == null &&
            OutcomingPortAlign == null;
    }

}

