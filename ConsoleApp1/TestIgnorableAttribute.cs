using System;
using System.Activities.XamlIntegration;
using System.IO;

namespace test {
    partial class TestHelper {

        public static void TestIgnorableAttribute() {

            var source = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Activity xmlns=""http://schemas.microsoft.com/netfx/2009/xaml/activities""
xmlns:av=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
xmlns:mva=""clr-namespace:Microsoft.VisualBasic.Activities;assembly=System.Activities""
xmlns:sads=""http://schemas.microsoft.com/netfx/2010/xaml/activities/debugger""
xmlns:sap=""http://schemas.microsoft.com/netfx/2009/xaml/activities/presentation""
xmlns:sap2010=""http://schemas.microsoft.com/netfx/2010/xaml/activities/presentation""
xmlns:scg=""clr-namespace:System.Collections.Generic;assembly=mscorlib""
xmlns:sco=""clr-namespace:System.Collections.ObjectModel;assembly=mscorlib""
xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
mc:Ignorable=""sap sap2010 sads"" 
x:Class=""{x:Null}"">
<x:Members></x:Members>
<mva:VisualBasic.Settings>
    <x:Null />
</mva:VisualBasic.Settings>
<TextExpression.NamespacesForImplementation>
    <sco:Collection x:TypeArguments=""x:String"">
        <x:String>System.Activities</x:String>
        <x:String>System.Activities.Statements</x:String>
        <x:String>System.Activities.Expressions</x:String>
        <x:String>System.Activities.Validation</x:String>
        <x:String>System.Activities.XamlIntegration</x:String>
        <x:String>Microsoft.VisualBasic.Activities</x:String>
        <x:String>System</x:String>
        <x:String>System.Collections</x:String>
        <x:String>System.Collections.Generic</x:String>
        <x:String>System.Data</x:String>
        <x:String>System.Linq</x:String>
        <x:String>System.Xml</x:String>
    </sco:Collection>
</TextExpression.NamespacesForImplementation>
<TextExpression.ReferencesForImplementation>
    <sco:Collection x:TypeArguments=""AssemblyReference"">
        <AssemblyReference>System.Activities</AssemblyReference>
        <AssemblyReference>mscorlib</AssemblyReference>
        <AssemblyReference>system</AssemblyReference>
        <AssemblyReference>System.Data</AssemblyReference>
        <AssemblyReference>System.Core</AssemblyReference>
        <AssemblyReference>System.Xml</AssemblyReference>
        <AssemblyReference>PresentationFramework</AssemblyReference>
        <AssemblyReference>WindowsBase</AssemblyReference>
        <AssemblyReference>PresentationCore</AssemblyReference>
        <AssemblyReference>System.Xaml</AssemblyReference>
    </sco:Collection>
</TextExpression.ReferencesForImplementation>
<Flowchart>
    <Flowchart.StartNode>
        <FlowStep x:Name=""__ReferenceID0"">
            <Assign DisplayName=""Assign 1"" sap2010:WorkflowViewState.IdRef=""Assign_1"" />
            <FlowStep.Next>
                <FlowStep x:Name=""__ReferenceID1"">
                    <Sequence>
                        <Assign DisplayName=""Assign 2"" sap2010:WorkflowViewState.IdRef=""Assign_2"" />
                        <Assign DisplayName=""Assign 3"" sap2010:WorkflowViewState.IdRef=""Assign_3"" />
                        <sap2010:WorkflowViewState.IdRef>Sequence_1</sap2010:WorkflowViewState.IdRef>
                    </Sequence>
                    <sap2010:WorkflowViewState.IdRef>FlowStep_1</sap2010:WorkflowViewState.IdRef>
                </FlowStep>
            </FlowStep.Next>
            <sap2010:WorkflowViewState.IdRef>FlowStep_2</sap2010:WorkflowViewState.IdRef>
        </FlowStep>
    </Flowchart.StartNode>
    <x:Reference>__ReferenceID0</x:Reference>
    <x:Reference>__ReferenceID1</x:Reference>
    <sap2010:WorkflowViewState.IdRef>Flowchart_1</sap2010:WorkflowViewState.IdRef>
    <sads:DebugSymbol.Symbol>dw1DOlxFbXB0eS54YW1sAA==</sads:DebugSymbol.Symbol>
</Flowchart>
<sap2010:WorkflowViewState.IdRef>ActivityBuilder_1</sap2010:WorkflowViewState.IdRef>
    <sap2010:WorkflowViewState.ViewStateManager>
        <sap2010:ViewStateManager>
            <sap2010:ViewStateData Id=""Assign_1"" sap:VirtualizedContainerService.HintSize=""240.5,60"">
                <sap:WorkflowViewStateService.ViewState>
                    <scg:Dictionary x:TypeArguments=""x:String, x:Object"">
                        <x:Boolean x:Key=""IsExpanded"">True</x:Boolean>
                    </scg:Dictionary>
                </sap:WorkflowViewStateService.ViewState>
            </sap2010:ViewStateData>
        <sap2010:ViewStateData Id=""Assign_2"" sap:VirtualizedContainerService.HintSize=""240.5,60"" />
        <sap2010:ViewStateData Id=""Assign_3"" sap:VirtualizedContainerService.HintSize=""240.5,60"" />
        <sap2010:ViewStateData Id=""Sequence_1"" sap:VirtualizedContainerService.HintSize=""216,49"">
            <sap:WorkflowViewStateService.ViewState>
                <scg:Dictionary x:TypeArguments=""x:String, x:Object"">
                    <x:Boolean x:Key=""IsExpanded"">True</x:Boolean>
                </scg:Dictionary>
            </sap:WorkflowViewStateService.ViewState>
        </sap2010:ViewStateData>
        <sap2010:ViewStateData Id=""FlowStep_1"">
            <sap:WorkflowViewStateService.ViewState>
                <scg:Dictionary x:TypeArguments=""x:String, x:Object"">
                    <av:Point x:Key=""ShapeLocation"">162,315.5</av:Point>
                    <av:Size x:Key=""ShapeSize"">216,49</av:Size>
                </scg:Dictionary>
            </sap:WorkflowViewStateService.ViewState>
        </sap2010:ViewStateData>
        <sap2010:ViewStateData Id=""FlowStep_2"">
            <sap:WorkflowViewStateService.ViewState>
                <scg:Dictionary x:TypeArguments=""x:String, x:Object"">
                    <av:Point x:Key=""ShapeLocation"">59.75,150</av:Point>
                    <av:Size x:Key=""ShapeSize"">240.5,60</av:Size>
                    <av:PointCollection x:Key=""ConnectorLocation"">180,210 180,240 270,240 270,315.5</av:PointCollection>
                </scg:Dictionary>
            </sap:WorkflowViewStateService.ViewState>
        </sap2010:ViewStateData>
        <sap2010:ViewStateData Id=""Flowchart_1"" sap:VirtualizedContainerService.HintSize=""614,636"">
            <sap:WorkflowViewStateService.ViewState>
                <scg:Dictionary x:TypeArguments=""x:String, x:Object"">
                    <x:Boolean x:Key=""IsExpanded"">False</x:Boolean>
                    <av:Point x:Key=""ShapeLocation"">270,2.5</av:Point>
                    <av:Size x:Key=""ShapeSize"">60,75</av:Size>
                    <av:PointCollection x:Key=""ConnectorLocation"">270,40 180,40 180,150</av:PointCollection>
                </scg:Dictionary>
            </sap:WorkflowViewStateService.ViewState>
        </sap2010:ViewStateData>
        <sap2010:ViewStateData Id=""ActivityBuilder_1"" sap:VirtualizedContainerService.HintSize=""654,716"" />
        </sap2010:ViewStateManager>
    </sap2010:WorkflowViewState.ViewStateManager>
</Activity>";

            var da = ActivityXamlServices.Load(new StringReader(source)) as System.Activities.DynamicActivity;
            if (da is null)
                throw new NotSupportedException();
            var func = da.Implementation as Func<System.Activities.Activity>;
            var activity = func.Invoke();

        }

    }
}