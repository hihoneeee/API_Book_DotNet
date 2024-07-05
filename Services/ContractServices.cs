using AutoMapper;
using TestWebAPI.DTOs.Contract;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class ContractServices : IContractServices
    {
        private readonly IMapper _mapper;
        private readonly IContractRepositories _contractRepo;
        private readonly IAppointmentRepositories _appointmentRepo;
        private readonly IPropertyRepositories _propertyRepo;

        public ContractServices(IMapper mapper, IContractRepositories contractRepo, IAppointmentRepositories appointmentRepo, IPropertyRepositories propertyRepo) {
            _mapper = mapper;
            _contractRepo = contractRepo;
            _appointmentRepo = appointmentRepo;
            _propertyRepo = propertyRepo;
        }

        public async Task<ServiceResponse<GetContractDTO>> CreateContractAsync(ContractDTO contractDTO)
        {
            var serviceResponse = new ServiceResponse<GetContractDTO>();
            try
            {
                var checkAppointment = await _appointmentRepo.GetAppointmentByIdAsync(contractDTO.appointmentId);
                if(checkAppointment == null)
                {
                    serviceResponse.SetError("Appointment not found.");
                    return serviceResponse;
                }
                var contract = _mapper.Map<Contract>(contractDTO);
                var createContract = await _contractRepo.CreateContractAsync(contract);
                serviceResponse.data = _mapper.Map<GetContractDTO>(createContract);
                serviceResponse.SetSuccess("contract created successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
