using Blazor.WorkflowEditor.Activity;

namespace Blazor.WorkflowEditor {
    public class PathItem {
        readonly ActivityDesignerPair reference;

        public ActivityDesignerPair Reference => reference;
        public System.Activities.Activity Activity => reference.Activity;
        public DefaultNode Node => reference.Node;

        public string Name => reference.Activity.DisplayName;

        public PathItem(ActivityDesignerPair reference) {
            if (reference is null) {
                throw new ArgumentNullException(nameof(reference));
            }
            this.reference = reference;
        }
    }

}
