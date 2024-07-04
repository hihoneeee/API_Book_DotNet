using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Contract;
using TestWebAPI.DTOs.Role;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class contractController : ControllerBase
    {
        private readonly IContractServices _contractServices;

        public contractController(IContractServices contractServices) {
            _contractServices = contractServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContractAsync([FromBody] ContractDTO contractDTO)
        {
            var serviceResponse = await _contractServices.CreateContractAsync(contractDTO);
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
