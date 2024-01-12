using Asp.Server.GrpcClientServices;
using Microsoft.AspNetCore.Mvc;

namespace Asp.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoClientService _todoClientService;

        public TodoController(TodoClientService todoClientService)
        {
            _todoClientService = todoClientService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _todoClientService.GetAll(cancellationToken));
        }
    }
}
