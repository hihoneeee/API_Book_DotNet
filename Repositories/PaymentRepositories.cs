using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class PaymentRepositories : IPaymentRepositories
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepositories(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync (Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<List<Payment>> GetPaymentAsync()
        {
            return await _context.Payments.ToListAsync();
        }
    }
}
