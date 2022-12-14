using System.Activities.Statements;

namespace Blazor.WorkflowEditor.Activity.Statements;

[Pair(typeof(System.Activities.Statements.WriteLine), typeof(WriteLineControl))]
public class WriteLineNode : DefaultNode {
    private readonly WriteLine activity;

    public WriteLineNode(Service service, System.Activities.Statements.WriteLine activity) : base(service, activity) {
        this.activity = activity;
    }

    public string? Text { get; set; }
}
