using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Configs;
using TestWebAPI.DTOs.Contract;
using TestWebAPI.DTOs.Payment;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class contractController : ControllerBase
    {
        private readonly IContractServices _contractServices;


        public contractController(IContractServices contractServices)
        {
            _contractServices = contractServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateContractAsync([FromBody] ContractDTO contractDTO)
        {
            var serviceResponse = await _contractServices.CreateContractAsync(contractDTO);
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }
    }
}
