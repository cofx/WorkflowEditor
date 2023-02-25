using System.Activities.Statements;

namespace Blazor.WorkflowEditor.Activity.Statements;

[Pair(typeof(System.Activities.Statements.WriteLine), typeof(WriteLineControl))]
public class WriteLineNode : DefaultNode {
    private readonly WriteLine activity;

    public WriteLineNode(Service service, System.Activities.Statements.WriteLine activity) : base(service, activity) {
        this.activity = activity;
        if (this.activity.Text == null) {
            this.activity.Text = new System.Activities.InArgument<string>(string.Empty);
        }
    }

    //TODO: ..\CoreWF\src\Test\TestCases.Workflows\ExpressionTests.cs 
    public string? Text {
        get {
            return (activity.Text.Expression as System.Activities.Expressions.Literal<string>)!.Value;
        }
        set {
            (activity.Text.Expression as System.Activities.Expressions.Literal<string>)!.Value = value ?? string.Empty;
        }
    }
}
