namespace Blazor.WorkflowEditor;

public class ToolBoxGroup {
    public string Name { get; set; } = "DefaultGroupName";
    public string? Description { get; set; }

    public List<ToolBoxItem> Items { get; set; } = new();

    public void Add<T>(string? imageToolbox = null) where T : System.Activities.Activity {
        var ti = new ToolBoxItem {
            Name = typeof(T).Name,
            Image = imageToolbox,
            TypeOfActivity = typeof(T)
        };
        Items.Add(ti);
    }
}

