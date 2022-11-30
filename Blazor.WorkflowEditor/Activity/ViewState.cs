using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity;

public class ViewState {
    public Point? CenterPosition { get; set; }
    public Size? Size { get; set; }
    public string? Comment { get; set; }
    public bool? IsExpanded { get; set; }
    public double? Zoom { get; set; }
    public Point? Offcet { get; set; }
    public PortAlignment? IncomingPortAlign { get; set; }
    public PortAlignment? OutcomingPortAlign { get; set; }
}

