using System.Activities.Statements;
using Blazor.Diagrams.Core.Models;

namespace Blazor.WorkflowEditor.Activity.Statements;

[Pair(typeof(System.Activities.Statements.Sequence), typeof(SequenceControl))]
public class SequenceNode : DefaultNode {
    private readonly Sequence sequenceActivity;

    public SequenceNode(Service service, System.Activities.Statements.Sequence sequenceActivity) : base(service, sequenceActivity) {
        this.sequenceActivity = sequenceActivity;
        this.IsContainer = true;

        this.service.SelectedOnMove += onMove;
    }

    public override IEnumerable<Variable> GetVariables() {
        return GetVariables(sequenceActivity.Variables);
    }

    void linkFromTo(ActivityDesignerPair from, ActivityDesignerPair to) {
        var link = service.LinkFromTo(from, to);
        //link.Router = Diagrams.Core.Routers.Orthogonal;
        //link.PathGenerator = Diagrams.Core.PathGenerators.Straight;
    }

    public override void LoadChilds(Func<System.Activities.Activity, ActivityDesignerPair> addActivity) {
        bool childHasViewState = false;
        ActivityDesignerPair? last = default;
        foreach (var activity in this.sequenceActivity.Activities) {
            var result = addActivity(activity);

            //lock ports for manual connect 
            result.Node.Ports.ToList().ForEach(p => p.Locked = true);

            if (last != null)
                linkFromTo(last, result);

            last = result;

            childHasViewState |= result.Node.HasViewState;
        }

        //Calc position
        if (childHasViewState == false) {
            var horizontal = this.service.DiagramContainer!.Width / 2;
            var minHeight = (int)this.service.Items.Min(p => p.Node.Size!.Height);
            int y = minHeight;
            int distance = minHeight;
            foreach (var activity in this.sequenceActivity.Activities) {
                var node = this.service.GetPair(activity).Node;
                var size = node.Size!;

                node.CenterPosition = new Diagrams.Core.Geometry.Point(horizontal, y);
                y += ((int)size.Height) / 2 + distance;

                node.UpdateViewState();
            }
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
            reconnect(source, child);

            linkFromTo(child, target);
            reconnect(child, target);

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
                var source = pair;
                var dest = service.GetPair(sequenceActivity.Activities[index + 1]);
                reconnect(source, dest);
            }

            if (index >= 1 && index < sequenceActivity.Activities.Count) {
                var source = service.GetPair(sequenceActivity.Activities[index - 1]);
                var dest = pair;
                reconnect(source, dest);
            }

        }
    }

    private void reconnect(ActivityDesignerPair source, ActivityDesignerPair dest) {
        var avalableSourcePorts = source.Node.Ports.ToList();
        if (source.Node.IncomingPort?.Links.Count > 0)
            avalableSourcePorts.Remove(source.Node.IncomingPort);

        var avalableDestPorts = dest.Node.Ports.ToList();
        if (dest.Node.OutcomingPort?.Links.Count > 0)
            avalableDestPorts.Remove(dest.Node.OutcomingPort);

        List<(PortModel sourcePort, PortModel destPort, double Distance)> items =
                (from fp in avalableSourcePorts
                 from sp in avalableDestPorts
                 select (fp, sp, Math.Abs(fp.Position.DistanceTo(sp.Position)))).ToList();

        (PortModel sourcePort, PortModel destPort, double _) = items.OrderBy(p => p.Distance).First();
        source.Node.SetOutcoming(sourcePort);
        dest.Node.SetIncoming(destPort);
    }
}
