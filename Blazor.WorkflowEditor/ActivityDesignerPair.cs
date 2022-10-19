namespace Blazor.WorkflowEditor {

    public class TypeActivityDesignerPair {
        public Type Activity { get; set; } = default!;
        public Type Node { get; set; } = default!;
        public Type Designer { get; set; } = default!;
    }

    public class ActivityDesignerPair {
        public System.Activities.Activity Activity { get; set; } = default!;
        public Activity.DefaultNode Node { get; set; } = default!;
    }

}
