using System.Activities;
using System.Activities.Statements;
using System.Activities.XamlIntegration;
using System.IO;
using System.Xaml;

namespace test {
    partial class TestHelper {
        public static void Test4() {
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


        }

    }
}