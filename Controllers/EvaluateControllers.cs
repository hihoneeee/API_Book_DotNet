using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Evaluate;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class evaluateController : ControllerBase
    {
        private readonly IEvaluateServices _evaluateServices;

        public evaluateController(IEvaluateServices evaluateServices) {
            _evaluateServices = evaluateServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvaluateAsync([FromBody] EvaluateDTO evaluateDTO)
        {
            var serviceResponse = await _evaluateServices.CreateEvaluateAsync(evaluateDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvaluateAsync(int id, [FromBody] EvaluateDTO evaluateDTO)
        {
            var serviceResponse = await _evaluateServices.UpdateEvaluateAsync(id, evaluateDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }
    }
}
