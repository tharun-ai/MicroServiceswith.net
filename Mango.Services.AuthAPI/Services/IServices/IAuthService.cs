using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        public Task<string> Register(RegistrationRequestDto registrationRequestDto);

        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<bool> AssignRole(string email,string roleName);
    }
}
