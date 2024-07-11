
using AutoMapper;
using TestWebAPI.DTOs.Evaluate;
using TestWebAPI.DTOs.Permisstion;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class EvaluateServices : IEvaluateServices
    {
        private readonly IEvaluateRepositories _evaluateRepo;
        private readonly IMapper _mapper;
        public EvaluateServices(IMapper mapper, IEvaluateRepositories evaluateRepo) {
            _mapper = mapper;
            _evaluateRepo = evaluateRepo;
        }

        public async Task<ServiceResponse<EvaluateDTO>> CreateEvaluateAsync(EvaluateDTO evaluateDTO)
        {
            var serviceResponse = new ServiceResponse<EvaluateDTO>();
            try
            {
                var evaluate = _mapper.Map<Evaluate>(evaluateDTO);
                var createEvaluate = await _evaluateRepo.CreateEvaluateAsync(evaluate);
                serviceResponse.SetSuccess("Evaluate created successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<EvaluateDTO>> UpdateEvaluateAsync(int id, EvaluateDTO evaluateDTO)
        {
            var serviceResponse = new ServiceResponse<EvaluateDTO>();
            try
            {
                var checkEvaluate = await _evaluateRepo.GetEvaluateByIdAsync(id);
                if (checkEvaluate == null)
                {
                    serviceResponse.SetNotFound("Evaluate");
                }
                var evaluate = _mapper.Map<Evaluate>(evaluateDTO);
                var updateEvaluate = await _evaluateRepo.UpdateEvaluateAsync(checkEvaluate, evaluate);
                serviceResponse.SetSuccess("Evaluate update successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
