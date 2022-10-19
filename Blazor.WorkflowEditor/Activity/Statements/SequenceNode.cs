using System.Activities.Statements;
using System.Collections.ObjectModel;

namespace Blazor.WorkflowEditor.Activity.Statements;

public class SequenceNode : DefaultNode {
    private readonly Sequence sequenceActivity;

    public SequenceNode(Service service, System.Activities.Statements.Sequence sequenceActivity) : base(service, sequenceActivity) {
        this.sequenceActivity = sequenceActivity;
        this.IsContainer = true;
    }

    public override Collection<System.Activities.Variable> GetVariables() {
        return new Collection<System.Activities.Variable>(sequenceActivity.Variables);
    }

    public override void LoadChilds(Func<System.Activities.Activity, ActivityDesignerPair> addActivity) {
        ActivityDesignerPair? last = default;
        foreach (var activity in this.sequenceActivity.Activities) {
            var result = addActivity(activity);

            if (last != null)
                service.LinkFromTo(last, result);

            last = result;
        }
    }

    public override void AddChild(ActivityDesignerPair child) {

        if (service.SelectedLinks.Count() == 1) {
            var source = service.SelectedLinks.First().source;
            var target = service.SelectedLinks.First().target;

            service.RemoveLinkFromTo(source, target);

            service.LinkFromTo(source, child);
            service.LinkFromTo(child, target);

            var index = this.sequenceActivity.Activities.IndexOf(target.Activity);
            this.sequenceActivity.Activities.Insert(index, child.Activity);

            return;
        }

        if (service.Items.Count() > 2) {
            var last = service.Items.SkipLast(1).Last();
            service.LinkFromTo(last, child);
        }

        this.sequenceActivity.Activities.Add(child.Activity);
    }

    public override void RemoveChild(System.Activities.Activity child) {
        var index = sequenceActivity.Activities.IndexOf(child);
        if (index >= 0 && index < sequenceActivity.Activities.Count - 1) {
            var prevActivity = sequenceActivity.Activities[index - 1];
            var nextActivity = sequenceActivity.Activities[index + 1];
            service.LinkFromTo(service.GetPair(prevActivity), service.GetPair(nextActivity));
        }

        sequenceActivity.Activities.Remove(child);
    }

}