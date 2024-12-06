using ManitApp.API.Application.RequestModels;
using ManitApp.API.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ManitApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionController : ControllerBase
    {
        private readonly IVectorizeService _vectorizeService;

        public SuggestionController(IVectorizeService vectorizeService)
        {
            _vectorizeService = vectorizeService;
        }

        [HttpPost]
        public async Task<IActionResult> GetSuggestion([FromBody] SuggestionRequestModel requestModel)
        {
            var response = await _vectorizeService.GetSuggestions(requestModel);
            return Ok(response);
        }
    }
}
