using Microsoft.AspNetCore.Mvc;
using TestWebAPI.DTOs.Appointment;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class appointmentController : ControllerBase
    {
        private readonly IAppointmentServices _appointmentServices;

        public appointmentController (IAppointmentServices appointmentServices) {
            _appointmentServices = appointmentServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointmentAsync([FromBody] AppointmentDTO appointmentDTO) 
        {
            var serviceResponse = await _appointmentServices.CreateAppointmentAsync(appointmentDTO);
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
