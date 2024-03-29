﻿namespace Blazor.WorkflowEditor.Activity.Statements;

[Pair(typeof(System.Activities.Statements.Assign), typeof(AssignControl))]
public class AssignNode : DefaultNode {
    //private readonly Assign assignActivity;

    public AssignNode(Service service, System.Activities.Statements.Assign assignActivity) : base(service, assignActivity) {
        //this.assignActivity = assignActivity;
    }

    public string? Source { get; set; }
    public string? Destination { get; set; }
}
