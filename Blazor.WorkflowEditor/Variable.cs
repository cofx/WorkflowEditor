namespace Blazor.WorkflowEditor {
    public class Variable {

        /// <summary>
        /// Owner
        /// </summary>
        public System.Activities.Activity Activity { get; set; } = default!;

        /// <summary>
        /// Name 
        /// </summary>
        public string Name { get; set; } = default!;
        public Type Type { get; set; } = default!;
        public object DefaultValue { get; set; } = default!;

        public bool IsArgument {
            get {
                var tArg = typeof(System.Activities.Argument);
                if (this.Type == tArg ||
                    this.Type.BaseType == tArg ||
                    this.Type.BaseType?.BaseType == tArg)
                    return true;
                return false;
            }
        }
    }


}
