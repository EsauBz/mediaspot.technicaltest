using Mediaspot.API.Endpoints;
using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Queues;
using Mediaspot.Infrastructure;
using Mediaspot.Infrastructure.Persistence;
using Mediaspot.Worker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<TranscodeJobWorker>();
builder.Services.AddInfrastructure("Mediaspot.Backend.TechnicalTest");
builder.Services.AddSingleton<ITranscodeQueue, InMemoryTranscodeQueue>();
builder.Services.AddSingleton<ITaskDelayer, TaskDelayer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MediaspotDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapAssetEndpoints();
app.MapTitleEndpoints();

app.Run();
