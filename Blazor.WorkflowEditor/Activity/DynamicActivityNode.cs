using System.Activities;

namespace Blazor.WorkflowEditor.Activity;

[Pair(typeof(System.Activities.DynamicActivity), typeof(DefaultControl))]
public class DynamicActivityNode : DefaultNode {

    private readonly DynamicActivity dynamicActivity;

    public DynamicActivityNode(Service service, System.Activities.DynamicActivity dynamicActivity) : base(service, dynamicActivity) {
        this.dynamicActivity = dynamicActivity;
        this.IsContainer = true;
    }

    public override void LoadChilds(Func<System.Activities.Activity, ActivityDesignerPair> addActivity) {
        var activity = dynamicActivity.Implementation?.Invoke();
        if (activity != null)
            addActivity(activity);
    }

    public override IEnumerable<Variable> GetVariables() {
        return GetVariables(dynamicActivity.Properties);
    }

}

