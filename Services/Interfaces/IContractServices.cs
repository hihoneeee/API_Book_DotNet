using TestWebAPI.DTOs.Contract;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IContractServices
    {
        Task<ServiceResponse<GetContractDTO>> CreateContractAsync(ContractDTO contractDTO);
    }
}
