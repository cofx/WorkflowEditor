using System.Activities;
using System.Activities.Statements;
using System.Xaml;

namespace test {

    public class State {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsExpanded { get; set; }
    }

    partial class TestHelper {
        public static void TestAttachedProperty() {

            var sourceActivity = new System.Activities.DynamicActivity() {
                Name = "DynamicAcitivty",
                DisplayName = "Display name"
            };
            sourceActivity.Properties.Add(
                new DynamicActivityProperty {
                    Name = "Numbers",
                    Type = typeof(InArgument<int>)
                });
            sourceActivity.Properties.Add(
                new DynamicActivityProperty {
                    Name = "Average",
                    Type = typeof(OutArgument<string>)
                });

            sourceActivity.AttachedProperties.Add(
                 "state",
                 new State() { X = 1, Y = 2, IsExpanded = true }
             );

            var xaml = XamlServices.Save(sourceActivity);

            var restoredActivity = XamlServices.Parse(xaml);

            var properties = (restoredActivity as DynamicActivity)?.AttachedProperties;
            if (properties != null) {
                properties.TryGetValue("state", out var value);
                if (value is State state && state != null) {
                    //..
                }

            }


        }

    }
}