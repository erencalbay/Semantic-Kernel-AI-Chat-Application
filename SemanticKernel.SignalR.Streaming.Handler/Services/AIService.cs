using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernel.SignalR.Streaming.Handler.Hubs;

namespace SemanticKernel.SignalR.Streaming.Handler.Services
{
    public class AIService(IHubContext<AIHub> hubContext, IChatCompletionService chatCompletionService)
    {
        public async Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!)
        {
            await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(prompt))
            {
                cancellationToken?.ThrowIfCancellationRequested();

                await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", response.ToString());
            }
        }
    }
}
