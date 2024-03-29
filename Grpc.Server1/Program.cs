using Grpc.Contracts;
using Grpc.Server1.Data;
using Grpc.Server1.GrpcServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=TodoDatabase.db;");
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<TodoService>();


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();