using TestWebAPI.DTOs.Payment;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IPaymentServices
    {
        Task<ServiceResponse<GetPaymentDTO>> CreatePaymentAsync(PaymentDTO paymentDTO);
        Task<ServiceResponse<List<GetPaymentDTO>>> ListPaymentAsync();
    }
}
