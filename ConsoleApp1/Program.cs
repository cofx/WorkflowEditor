// See https://aka.ms/new-console-template for more information
using System.Activities;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.Xaml;

Console.WriteLine("Hello, World!");


var seq = new Sequence() {
    Activities =
    {
        new WriteLine
        {
            Text = "Hello World!"
        }
    },
    Variables = {
        new Variable<string>("MyVar", "me default value"),
        new Variable<int>("MyVar2", 1)

    }
};
var seqText = XamlServices.Save(seq);

/* 
var fcVar1 = new System.Activities.Variable<double>("Name");
var fs1 = new FlowStep() {
    Action = new Assign {
        DisplayName = "discount = 10.0",
        To = new OutArgument<double>(fcVar1),
        Value = new InArgument<double>(10.0)
    }
};
var fs2 = new FlowStep() {
    Action = new Assign {
        DisplayName = "discount = 20.0",
        To = new OutArgument<double>(fcVar1),
        Value = new InArgument<double>(20.0)
    },
    Next = fs1,
};
var fc = new Flowchart() {
    Variables = {
        fcVar1
    },
    Nodes = { fs1, fs2 },
    StartNode = fs2
};
var fcText = XamlServices.Save(fc);

 */


var ab = new System.Activities.DynamicActivity();
ab.Properties.Add(
    new DynamicActivityProperty {
        Name = "Numbers",
        Type = typeof(InArgument<int>)
    });
ab.Properties.Add(
    new DynamicActivityProperty {
        Name = "Average",
        Type = typeof(OutArgument<string>)
    });
//ab.Implementation = () => new Sequence();
//var result2 = XamlServices.Save(ab);


var aaa = @"<Activity 
 x:Class=""WorkflowConsoleApplication1.HelloWorld""
 xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities""
 xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib""
 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
  <x:Members>
    <x:Property Name=""test1"" Type=""InArgument(x:String)"" />
    <x:Property Name=""test2"" Type=""OutArgument(x:String)"" />
  </x:Members>
  <TextExpression.NamespacesForImplementation>
    <scg:List x:TypeArguments=""x:String"" Capacity=""16"">
      <x:String>System</x:String>
      <x:String>System.Linq</x:String>
      <x:String>System.Collections.Generic</x:String>
    </scg:List>
  </TextExpression.NamespacesForImplementation>
  <TextExpression.ReferencesForImplementation>
    <scg:List x:TypeArguments=""AssemblyReference"" Capacity=""1"">
      <AssemblyReference>System.Activities, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35</AssemblyReference>
    </scg:List>
  </TextExpression.ReferencesForImplementation>
  <Sequence>
    <WriteLine Text=""Hello World!"" />
  </Sequence>
</Activity>";
var da = ActivityXamlServices.Load(new StringReader(aaa));



//var da1 = XamlServices.Parse(result2);
int i = 0;