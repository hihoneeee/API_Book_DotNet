using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class EvaluateRepositories : IEvaluateRepositories
    {
        private readonly ApplicationDbContext _context;

        public EvaluateRepositories(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Evaluate> CreateEvaluateAsync(Evaluate evaluate)
        {
            
            _context.Evaluates.Add(evaluate);
            await _context.SaveChangesAsync();
            return evaluate;
        }

        public async Task<Evaluate> GetEvaluateByIdAsync(int id)
        {
            return await _context.Evaluates.FirstOrDefaultAsync(e=>e.id == id);
        }

        public async Task<Evaluate> UpdateEvaluateAsync(Evaluate oldEva, Evaluate newEva)
        {
            oldEva.review = newEva.review;
            oldEva.star = newEva.star;
            await _context.SaveChangesAsync();
            return oldEva;
        }
    }
}
