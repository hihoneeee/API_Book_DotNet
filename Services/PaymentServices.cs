using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestWebAPI.Configs;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.Payment;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepositories _paymentRepo;

        public PaymentServices(IMapper mapper, IPaymentRepositories paymentRepo)
        {
            _mapper = mapper;
            _paymentRepo = paymentRepo;

        }

        public async Task<ServiceResponse<GetPaymentDTO>> CreatePaymentAsync(PaymentDTO paymentDTO)
        {
            var serviceResponse = new ServiceResponse<GetPaymentDTO>();
            try
            {
                var payment = _mapper.Map<Payment>(paymentDTO);
                var createPayment = await _paymentRepo.CreatePaymentAsync(payment);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPaymentDTO>>> ListPaymentAsync()
        {
            var serviceResponse = new ServiceResponse<List<GetPaymentDTO>>();
            try
            {
                var payments = await _paymentRepo.GetPaymentAsync();
                if (payments == null)
                {
                    serviceResponse.SetNotFound("List Categories");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<List<GetPaymentDTO>>(payments);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
