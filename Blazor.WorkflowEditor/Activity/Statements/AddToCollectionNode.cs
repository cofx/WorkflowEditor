using System.Activities.Statements;

namespace Blazor.WorkflowEditor.Activity.Statements;

[Pair(typeof(System.Activities.Statements.AddToCollection<>), typeof(AddToCollectionControl<>))]
public class AddToCollectionNode<T> : DefaultNode {
    private readonly AddToCollection<T> activity;

    public AddToCollectionNode(Service service, System.Activities.Statements.AddToCollection<T> activity) : base(service, activity) {
        this.activity = activity;
        this.IsGeneric = true;
    }
}
