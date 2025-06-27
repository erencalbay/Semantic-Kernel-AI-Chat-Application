using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernel.SignalR.Streaming.Handler.Plugins
{
    public class CalculatorPlugin
    {
        [KernelFunction("add")]
        [Description("Performs addition on two numeric values.")]
        [return: Description("Returns total value")]
        public int Add(int number1, int number2)
            => number1 + number2;
    }
}
