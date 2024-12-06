using ManitApp.API.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ManitApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IVectorizeService _vectorizeService;

        public OrderController(IVectorizeService vectorizeService)
        {
            _vectorizeService = vectorizeService;
        }

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> VectorizeOrder(int userId)
        {
            await _vectorizeService.VectorizeOrderForUser(userId);
            return Ok();
        }
    }
}
