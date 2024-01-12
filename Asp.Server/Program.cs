using Asp.Server.GrpcClientServices;
using Grpc.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<ToDoGrpcService.ToDoGrpcServiceClient>(
    opt => { opt.Address = new Uri("https://localhost:7018"); })
    .EnableCallContextPropagation();

builder.Services.TryAddScoped<TodoClientService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
