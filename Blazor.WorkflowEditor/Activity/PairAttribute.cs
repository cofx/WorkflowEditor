namespace Blazor.WorkflowEditor.Activity;

public class PairAttribute : Attribute {
    public Type Activity { get; set; } = default!;
    public Type Control { get; set; } = default!;

    public PairAttribute(Type activity, Type control) {
        Activity = activity;
        Control = control;
    }
}
