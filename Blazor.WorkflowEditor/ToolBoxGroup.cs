namespace Blazor.WorkflowEditor;

public class ToolBoxGroup {
    public string Name { get; set; } = "DefaultGroupName";
    public string? Description { get; set; }

    /// <summary>
    /// State in toolbox component
    /// </summary>
    public bool Collapsed { get; set; }

    public List<ToolBoxItem> Items { get; set; } = new();

    public void Add<T>(string? imageToolbox = null) where T : System.Activities.Activity {
        Add(typeof(T), imageToolbox);
    }

    public void Add(Type type, string? imageToolbox = null) {
        var ti = new ToolBoxItem {
            Name = type.Name,
            Image = imageToolbox,
            TypeOfActivity = type
        };
        Items.Add(ti);
    }
}

