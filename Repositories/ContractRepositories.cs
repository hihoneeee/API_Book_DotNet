using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class ContractRepositories : IContractRepositories
    {
        private readonly ApplicationDbContext _context;

        public ContractRepositories(ApplicationDbContext context) { 
            _context = context;
        }
        public async Task<Contract> CreateContractAsync(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return contract;
        }

        public async Task<Contract> GetContractById(int id)
        {
            return _context.Contracts.FirstOrDefault(c => c.id == id);
        }
    }
}
