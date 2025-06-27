using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernel.SignalR.Streaming.Handler.Hubs;

namespace SemanticKernel.SignalR.Streaming.Handler.Services
{
    public class AIService(IHubContext<AIHub> hubContext, IChatCompletionService chatCompletionService)
    {
        public async Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!)
        {
            var history = HistoryService.GetChatHistory(connectionId);

            history.AddUserMessage(prompt);
            string responseContent = "";
            await foreach (var response in chatCompletionService.GetStreamingChatMessageContentsAsync(history))
            {
                cancellationToken?.ThrowIfCancellationRequested();

                await hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", response.ToString());
                responseContent += response.ToString();
            }
            history.AddAssistantMessage(responseContent);
        }
    }
}
