using Microsoft.SemanticKernel;
using OpenAI;
using SemanticKernel.SignalR.Streaming.Handler.Hubs;
using SemanticKernel.SignalR.Streaming.Handler.Plugins;
using SemanticKernel.SignalR.Streaming.Handler.Services;
using SemanticKernel.SignalR.Streaming.Handler.ViewModels;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.5-flash-lite-preview-06-17",
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential("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"),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    )
    .Plugins.AddFromType<CalculatorPlugin>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowCredentials()
                                       .SetIsOriginAllowed(s => true)));

builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddSingleton<AIService>();

var app = builder.Build();
app.UseCors();

app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken)
    => await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));

app.MapHub<AIHub>("ai-hub");

app.Run();