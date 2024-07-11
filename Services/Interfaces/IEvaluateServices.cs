using TestWebAPI.DTOs.Evaluate;
using TestWebAPI.Response;

namespace TestWebAPI.Services.Interfaces
{
    public interface IEvaluateServices
    {
        Task<ServiceResponse<EvaluateDTO>> CreateEvaluateAsync(EvaluateDTO evaluateDTO);
        Task<ServiceResponse<EvaluateDTO>> UpdateEvaluateAsync(int id, EvaluateDTO evaluateDTO);
    }
}
