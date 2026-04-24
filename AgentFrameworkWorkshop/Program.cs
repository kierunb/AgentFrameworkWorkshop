using Microsoft.Agents.AI;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using OpenAI;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Loading model...");

string modelPath = @"c:\Models\phi-4-onnx\cpu_and_mobile\cpu-int4-rtn-block-32-acc-level-4\";
using OnnxRuntimeGenAIChatClient chatClient = new(modelPath);

Console.WriteLine("Model loaded...");

builder.Services.AddChatClient(chatClient);

builder.AddAIAgent(
    name: "agent2",
    instructions: "A helpful assistant that can answer questions and provide information.");

builder.Services.AddOpenApi();

builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/scalar");
    app.MapDevUI(); // /devui
}

app.MapOpenAIResponses();
app.MapOpenAIConversations();

app.MapGet("/", () => "Hello World!");


app.MapGet("/chat/{prompt}", async (string prompt, IChatClient chatClient) =>
{
    var agent = chatClient.AsAIAgent(
        name: "agent1", 
        instructions: "A helpful assistant that can answer questions and provide information.");
    
    var response = await agent.RunAsync(prompt);

    return Results.Ok(response);
});


app.MapGet("/agent/{prompt}", async (string prompt, [FromKeyedServices("agent2")]AIAgent agent) =>
{
    var response = await agent.RunAsync(prompt);
    return Results.Ok(response);
});


app.UseHttpsRedirection();

app.Run();
