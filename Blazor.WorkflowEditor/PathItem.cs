namespace Blazor.WorkflowEditor {
    public class PathItem {
        public static PathItem Root => new();

        public string Name => Reference?.Activity?.DisplayName ?? "root";
        public ActivityDesignerPair? Reference { get; set; }

        private PathItem() { }

        public PathItem(ActivityDesignerPair reference) {
            Reference = reference;
        }

    }


}
