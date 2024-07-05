using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Configs;
using TestWebAPI.DTOs.Payment;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class paymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly PaypalConfig _paypalConfig;
        public paymentController(IPaymentServices paymentServices, PaypalConfig paypalConfig)
        {
            _paypalConfig = paypalConfig;
            _paymentServices = paymentServices;
        }

        [HttpPost]
        [Authorize(Policy = "get-all-payment")]
        public async Task<IActionResult> ListPaymentAsync()
        {
            var serviceResponse = await _paymentServices.ListPaymentAsync();
            if (serviceResponse.statusCode == EHttpType.Success)
            {
                return Ok(new { serviceResponse.success, serviceResponse.message, serviceResponse.data });
            }
            else
            {
                return StatusCode((int)serviceResponse.statusCode, new { serviceResponse.success, serviceResponse.message });
            }
        }

        [HttpPost("create-paypal-order")]
        public async Task<IActionResult> CreatePaypalAsync(int amount)
        {
            var sum = amount.ToString();
            var Unit = "USD";
            var order = "DH" + DateTime.Now.Ticks.ToString();
            try
            {
                var response = await _paypalConfig.CreateOrder(sum, Unit, order);
                if (response == null)
                {
                    throw new Exception("Response from PayPal was null.");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new { ex.GetBaseException().Message };
                return BadRequest(err);
            }
        }

        [HttpPost("capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalAsync(string id, int contractId)
        {
            try
            {
                var response = await _paypalConfig.CaptureOrder(id);
                var payment = new PaymentDTO
                {
                    contractId = contractId,
                    paypalId = response.id,
                    status = response.status,
                };
                var createPayment = await _paymentServices.CreatePaymentAsync(payment);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var err = new { ex.GetBaseException().Message };
                return BadRequest(err);
            }
        }
    }
}
