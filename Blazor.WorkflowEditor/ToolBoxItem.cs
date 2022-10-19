namespace Blazor.WorkflowEditor;

public class ToolBoxItem {
    public string Name { get; set; } = "DefaultToolBoxName";
    public string? Image { get; set; }
    public Type TypeOfActivity { get; set; } = default!;
}


