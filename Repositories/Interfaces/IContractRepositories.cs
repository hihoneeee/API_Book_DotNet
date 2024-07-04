using TestWebAPI.Models;

namespace TestWebAPI.Repositories.Interfaces
{
    public interface IContractRepositories
    {
        Task<Contract> CreateContractAsync(Contract contract);
        Task<Contract> GetContractById(int id);
    }
}
