using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {

        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db,UserManager<ApplicationUser> userManager,
         RoleManager<IdentityRole> roleManager ,IJwtTokenGenerator jwtTokenGenerator  )
        {
            _db = db;  
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
                
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var _user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (_user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                { 

                    //create role,if doesn't exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                }
                await _userManager.AddToRoleAsync(_user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var _user=_db.ApplicationUsers.FirstOrDefault(u=>u.Email.ToLower()==loginRequestDto.Username.ToLower());
            bool isValid=await _userManager.CheckPasswordAsync(_user,loginRequestDto.Password);
            if(_user==null || isValid==false) {
               return new LoginResponseDto() { User=null,Token=""};
            }
            //If User was Found, Generate a new Token
            var roles=await _userManager.GetRolesAsync(_user);
            var token = _jwtTokenGenerator.GenerateToken(_user,roles);
            UserDto userDto = new()
            {
                Email = _user.Email,
                Id = _user.Id,
                Name = _user.Name,
                PhoneNumber = _user.PhoneNumber
            };

            LoginResponseDto loginResponseDto=new LoginResponseDto()
            {
                User=userDto,
                Token=token
            };
            return loginResponseDto;

        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Name,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };
            try
            {
                var result=await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn=_db.ApplicationUsers.First(u=>u.Email== registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber,
                        Id = userToReturn.Id
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex) { }
            return "Error Encountered";
        }
    }
}
