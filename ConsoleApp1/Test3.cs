using System.Activities;
using System.Activities.Statements;
using System.Xaml;

namespace test {
    partial class TestHelper {
        public static void Test3() {

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

        }

    }
}