using System.Activities;
using System.Activities.Statements;
using System.Xaml;

namespace test {
    partial class TestHelper {
        public static void Test1() {
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
        }

    }
}