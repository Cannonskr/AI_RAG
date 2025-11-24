
using Microsoft.OpenApi;   // สำหรับ OpenAPI/Swagger
using APIRagSoleItem.Repository;
using APIRagSoleItem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load from config
string connStr = builder.Configuration.GetConnectionString("PgDb")!;
string ollamaEndpoint = builder.Configuration["Ollama:Endpoint"]!;
string embedModel = builder.Configuration["Ollama:EmbeddingModel"]!;

builder.Services.AddSingleton(new EmbeddingService(ollamaEndpoint, embedModel));
builder.Services.AddSingleton(new SoldItemRepository(connStr));
builder.Services.AddSingleton<SoldItemService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();