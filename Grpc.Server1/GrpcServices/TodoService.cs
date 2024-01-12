using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Grpc.Server1.Data;
using Grpc.Contracts;
using Grpc.Server1.Models;

namespace Grpc.Server1.GrpcServices;

public class TodoService : ToDoGrpcService.ToDoGrpcServiceBase
{
    private readonly AppDbContext _dbContext;

    public TodoService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<ReadToDoResponse> Read(ReadToDoRequest request, ServerCallContext context)
    {
        if (request is null || request.Id <= 0)
            throw new RpcException(status: new Status(StatusCode.InvalidArgument, "The request input is invalid"),
                message: "The request input is invalid");

        var todoItem = await _dbContext.ToDoItems.FindAsync(request.Id);

        if (todoItem is null)
            throw new RpcException(status: new Status(StatusCode.NotFound, "There is no record with this id"),
                message: "There is no record with this id");

        return GetReadToDoResponse(todoItem);
    }

    public override async Task<ReadAllToDoResponse> ReadAll(ReadAllToDoRequest request, ServerCallContext context)
    {
        if (request is null)
            throw new RpcException(status: new Status(StatusCode.InvalidArgument, "The request input is invalid"),
                message: "The request input is invalid");

        var todoItems = await _dbContext.ToDoItems.ToListAsync(context.CancellationToken);

        List<ReadToDoResponse> list = todoItems.Select(p => GetReadToDoResponse(p)).ToList();

        var result = new ReadAllToDoResponse();
        result.ToDo.AddRange(list);
        return result;
    }

    public override async Task<ReadToDoResponse> Create(CreateToDoRequest request, ServerCallContext context)
    {
        if (request is null || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.Description))
        {
            throw new RpcException(status: new Status(StatusCode.InvalidArgument, "The request input is invalid"),
                message: "The request input is invalid");
        }

        var todoItem = new ToDoItem(request.Title, request.Description);

        _dbContext.ToDoItems.Add(todoItem);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return GetReadToDoResponse(todoItem);
    }

    public override async Task<DeleteToDoResponse> Delete(DeleteToDoRequest request, ServerCallContext context)
    {
        if (request is null || request.Id <= 0)
            throw new RpcException(status: new Status(StatusCode.InvalidArgument, "The request input is invalid"),
                message: "The request input is invalid");

        var todoItem = _dbContext.ToDoItems.Find(request.Id);

        if (todoItem is null)
            throw new RpcException(status: new Status(StatusCode.NotFound, "There is no record with this id"),
                message: "There is no record with this id");

        _dbContext.ToDoItems.Remove(todoItem);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return new DeleteToDoResponse()
        {
            Id = todoItem.Id,
        };
    }

    public override async Task<ReadToDoResponse> Update(UpdateToDoRequest request, ServerCallContext context)
    {
        if (request is null || request.Id <= 0)
            throw new RpcException(status: new Status(StatusCode.InvalidArgument, "The request input is invalid"),
                message: "The request input is invalid");

        var todoItem = _dbContext.ToDoItems.Find(request.Id);

        if (todoItem is null)
            throw new RpcException(status: new Status(StatusCode.NotFound, "There is no record with this id"),
                message: "There is no record with this id");

        todoItem.Title = request.Title;
        todoItem.Description = request.Description;

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        return GetReadToDoResponse(todoItem);
    }

    private static ReadToDoResponse GetReadToDoResponse(ToDoItem todoItem)
    {
        return new ReadToDoResponse()
        {
            Id = todoItem.Id,
            Title = todoItem.Title,
            Description = todoItem.Description,
            ToDoStatus = todoItem.Status
        };
    }
}