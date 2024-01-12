using Asp.Server.GrpcClientServices.Dtos;
using Grpc.Contracts;

namespace Asp.Server.GrpcClientServices
{
    public sealed class TodoClientService
    {
        private readonly ToDoGrpcService.ToDoGrpcServiceClient _client;

        public TodoClientService(ToDoGrpcService.ToDoGrpcServiceClient client)
        {
            _client = client;
        }

        public async Task<List<ToDoItemOutpDto>> GetAll(CancellationToken cancellationToken)
        {
            var grpcResponse = await _client.ReadAllAsync(new ReadAllToDoRequest(), cancellationToken: cancellationToken);

            return grpcResponse.ToDo.ToList().Select(p => new ToDoItemOutpDto()
            {
                Description = p.Description,
                Id = p.Id,
                Status = p.ToDoStatus,
                Title = p.Title
            }).ToList();


        }
    }
}
