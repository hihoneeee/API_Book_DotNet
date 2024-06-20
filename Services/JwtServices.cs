using AutoMapper;
using TestWebAPI.DTOs.JWT;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class JwtServices : IJwtServices
    {
        private readonly IJwtRepositories _jwtRepo;
        private readonly IMapper _mapper;
        public JwtServices(IMapper mapper, IJwtRepositories jwtRepo) { 
            _mapper = mapper;
            _jwtRepo = jwtRepo;
        }

        public async Task InsertJWTToken(jwtDTO jwt)
        {
            var existingJwt = await _jwtRepo.GetJwtByUserId(jwt.user_id);
            if (existingJwt != null)
            {
                // Cập nhật các trường của refresh token cũ với các giá trị mới
                existingJwt.expired_date = jwt.expired_date;
                existingJwt.value = jwt.value;
                existingJwt.issued_date = jwt.issued_date;

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _jwtRepo.UpdateJwtAsync(existingJwt);
            }
            else
            {
                // Nếu không có refresh token cũ, thêm mới refresh token
                var needToAdd = _mapper.Map<JWT>(jwt);
                if (needToAdd != null)
                {
                    await _jwtRepo.AddJwtAsync(needToAdd);
                }
                else
                {
                    throw new Exception("Unable to map jwtDTO to JWT.");
                }
            }
        }
    }
}
