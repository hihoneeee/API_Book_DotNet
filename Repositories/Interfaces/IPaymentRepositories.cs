using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IPaymentRepositories
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<List<Payment>> GetPaymentAsync();
    }
}
