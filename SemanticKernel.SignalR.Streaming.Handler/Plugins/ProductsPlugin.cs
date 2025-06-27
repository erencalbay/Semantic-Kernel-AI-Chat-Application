using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernel.SignalR.Streaming.Handler.Plugins
{
    public class ProductsPlugin(HttpClient httpClient)
    {
        [KernelFunction("bestSellingProducts")]
        [Description("Gets most the best-selling products.")]
        [return: Description("Returns most the best-selling products in JSON format.")]
        public async Task<string> BestSellingProducts()
        {
            var response = await httpClient.GetAsync("https://localhost:7020/best-selling-products");
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}
