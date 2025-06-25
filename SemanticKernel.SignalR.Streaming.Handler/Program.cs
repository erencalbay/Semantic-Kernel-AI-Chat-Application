using Microsoft.SemanticKernel;
using OpenAI;
using SemanticKernel.SignalR.Streaming.Handler.Hubs;
using SemanticKernel.SignalR.Streaming.Handler.Services;
using SemanticKernel.SignalR.Streaming.Handler.ViewModels;
using System.ClientModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.0-pro-exp-02-05:free",
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential("sk-or-v1-0959a8d8aed****d06e78ff68ceaa"),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    );

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowCredentials()
                                       .SetIsOriginAllowed(s => true)));

builder.Services.AddSignalR();
builder.Services.AddSingleton<AIService>();

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => "Hello World!");

app.MapHub<AIHub>("ai-hub");
app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken)
    => await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));


app.Run();