using Microsoft.ML.OnnxRuntimeGenAI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Loading model...");

string modelPath = @"c:\Models\phi-4-onnx\cpu_and_mobile\cpu-int4-rtn-block-32-acc-level-4\";
using OnnxRuntimeGenAIChatClient chatClient = new(modelPath);

Console.WriteLine("Model loaded...");

builder.Services.AddChatClient(chatClient);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Hello World!");

app.UseHttpsRedirection();

app.Run();
