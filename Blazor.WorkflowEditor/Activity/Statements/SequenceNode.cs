using System.Activities.Statements;
using System.Collections.ObjectModel;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity.Statements;

public class SequenceNode : DefaultNode {
    private readonly Sequence sequenceActivity;

    public SequenceNode(Service service, System.Activities.Statements.Sequence sequenceActivity) : base(service, sequenceActivity) {
        this.sequenceActivity = sequenceActivity;
        this.IsContainer = true;

        this.service.SelectedOnMove += onMove;
    }



    public override Collection<System.Activities.Variable> GetVariables() {
        return new Collection<System.Activities.Variable>(sequenceActivity.Variables);
    }

    void linkFromTo(ActivityDesignerPair from, ActivityDesignerPair to) {
        var link = service.LinkFromTo(from, to);
        link.Router = Diagrams.Core.Routers.Orthogonal;
        link.PathGenerator = Diagrams.Core.PathGenerators.Straight;
    }

    public override void LoadChilds(Func<System.Activities.Activity, ActivityDesignerPair> addActivity) {
        ActivityDesignerPair? last = default;
        foreach (var activity in this.sequenceActivity.Activities) {
            var result = addActivity(activity);

            //lock ports for manual connect 
            result.Node.Ports.ToList().ForEach(p => p.Locked = true);

            if (last != null)
                linkFromTo(last, result);

            last = result;
        }
    }

    public override void AddChild(ActivityDesignerPair child) {
        //lock ports for manual connect 
        child.Node.Ports.ToList().ForEach(p => p.Locked = true);

        if (service.SelectedLinks.Count() == 1) {
            var source = service.SelectedLinks.First().source;
            var target = service.SelectedLinks.First().target;

            service.RemoveLinkFromTo(source, target);

            linkFromTo(source, child);
            linkFromTo(child, target);

            var index = this.sequenceActivity.Activities.IndexOf(target.Activity);
            this.sequenceActivity.Activities.Insert(index, child.Activity);

            return;
        }

        if (service.Items.Count() > 2) {
            var last = service.Items.SkipLast(1).Last();
            linkFromTo(last, child);
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

    private void onMove() {
        if (sequenceActivity.Activities.Count() < 2)
            return;

        foreach (var pair in service.SelectedItems) {

            var index = sequenceActivity.Activities.IndexOf(pair.Activity);

            if (index >= 0 && index < sequenceActivity.Activities.Count - 1) {
                var first = pair;
                var second = service.GetPair(sequenceActivity.Activities[index + 1]);
                reconnect(first, second);
            }

            if (index >= 1 && index < sequenceActivity.Activities.Count) {
                var first = service.GetPair(sequenceActivity.Activities[index - 1]);
                var second = pair;
                reconnect(first, second);
            }

        }
    }

    private void reconnect(ActivityDesignerPair first, ActivityDesignerPair second) {
        if (first.Node.Position.X < second.Node.Position.X && first.Node.Position.Y < second.Node.Position.Y) {
            //first.Node.GetFromPort().Alignment  
        }
    }
}