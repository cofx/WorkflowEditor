using System.Activities;
using System.Activities.Statements;
using System.Xaml;

namespace test {
    partial class TestHelper {
        public static void Test2() {

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


        }
    }
}