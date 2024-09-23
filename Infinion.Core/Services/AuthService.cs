using Infinion.Core.Abstractions;
using Infinion.Core.DTOs;
using Infinion.Domain.Constants;
using Infinion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Web;


namespace Infinion.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public AuthService(IConfiguration configuration, UserManager<AppUser> userManager, IEmailService emailService, IJwtService jwtService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _emailService = emailService;
            _jwtService = jwtService;
        }
        public async Task<Result<UserRegistrationResponseDto>> Register(RegisterUserDto registerUserDto)
        {
            var emailExist = await _userManager.FindByEmailAsync(registerUserDto.Email);

            if (emailExist != null)
                return new Result<UserRegistrationResponseDto>
                {
                    Message = "Email already taken",
                    Content = null,
                    IsSuccess = false,
                    Error = null
                };
            var newUser = new AppUser
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                MiddleName = registerUserDto.MiddleName,
                PhoneNumber = registerUserDto.PhoneNumber,
                UserName = registerUserDto.Email,
                Address = registerUserDto.Address,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            };
            var confirmEmailUrl = _configuration["ConfirmEmailUrl"];
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var encodedEmail = HttpUtility.UrlEncode(newUser.Email);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"{confirmEmailUrl}?email={encodedEmail}&token={encodedToken}";
            const string emailSubject = "Confirm Email";
            var body =
                @$"Hi {newUser.FirstName} {newUser.LastName}, Please click the link <a href='{confirmationLink}'>here</a> to confirm your account's email";
            var emailResult = await _emailService.SendEmailAsync(newUser.Email,emailSubject , body);

            var result = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!result.Succeeded)
                return new Result<UserRegistrationResponseDto>
                {
                    Message = "User registration failed",
                    Content = null,
                    IsSuccess = false,
                    Error = result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray()
                };

            result = await _userManager.AddToRoleAsync(newUser, RoleConstant.User);
            if (!result.Succeeded)
                return new Result<UserRegistrationResponseDto>
                {
                    Message = "Role assignation Failed",
                    Content = null,
                    IsSuccess = false,
                    Error = result.Errors.Select(error => new Error(error.Code, error.Description)).AsEnumerable()
                };
            var userRoles=  await _userManager.GetRolesAsync(newUser);
            var response = new UserRegistrationResponseDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,
                Email = newUser.Email,
                id = newUser.Id,
                Roles = userRoles,
            };

            return new Result<UserRegistrationResponseDto> { Message = "User Registered Succesfully", Content =response , IsSuccess = true, };
        }

        public async Task<Result<UserRegistrationResponseDto>> RegisterAdmin(RegisterUserDto registerUserDto)
        {
            var emailExist = await _userManager.FindByEmailAsync(registerUserDto.Email);

            if (emailExist != null)
                return new Result<UserRegistrationResponseDto>
                {
                    Message = "Email already taken",
                    Content = null,
                    IsSuccess = false,
                    Error = null
                };
            var newUser = new AppUser
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                MiddleName = registerUserDto.MiddleName,
                PhoneNumber = registerUserDto.PhoneNumber,
                UserName = registerUserDto.Email,
                Address = registerUserDto.Address,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            };
            var confirmEmailUrl = _configuration["ConfirmEmailUrl"];
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var encodedEmail = HttpUtility.UrlEncode(newUser.Email);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"{confirmEmailUrl}?email={encodedEmail}&token={encodedToken}";
            const string emailSubject = "Confirm Email";
            var body =
                @$"Hi {newUser.FirstName} {newUser.LastName}, Please click the link <a href='{confirmationLink}'>here</a> to confirm your account's email";
            var emailResult = await _emailService.SendEmailAsync(newUser.Email, emailSubject, body);

            var result = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!result.Succeeded)
                return new Result<UserRegistrationResponseDto>
                {
                    Message = "User registration failed",
                    Content = null,
                    IsSuccess = false,
                    Error = result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray()
                };

            result = await _userManager.AddToRoleAsync(newUser, RoleConstant.Admin);
            if (!result.Succeeded)
                return new Result<UserRegistrationResponseDto>
                {
                    Message = "Role assignation Failed",
                    Content = null,
                    IsSuccess = false,
                    Error = result.Errors.Select(error => new Error(error.Code, error.Description)).AsEnumerable()
                };
            var userRoles = await _userManager.GetRolesAsync(newUser);
            var response = new UserRegistrationResponseDto
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = newUser.PhoneNumber,
                Email = newUser.Email,
                id = newUser.Id,
                Roles = userRoles,
            };

            return new Result<UserRegistrationResponseDto> { Message = "Admin User Registered Succesfully", Content = response, IsSuccess = true, };
        }

        public async Task<Result<LoginResponseDto>> Login(LoginUserDto loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

            if (user is null)
                return new Result<LoginResponseDto>
                {
                    Content = null,
                    IsSuccess = false,
                    Error = null,
                    Message = "User not Found"
                };

            var isValidUser = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

            if (!isValidUser)
                return new Result<LoginResponseDto>
                {
                    Content = null,
                    IsSuccess = false,
                    Message = "Email or Password is incorrect"
                };

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);
            var appUserDto = new AppUserDto { FirstName = user.FirstName, LastName = user.LastName, MiddleName = user.MiddleName,
                                                        PhoneNumber=user.PhoneNumber, Address=user.Address};
            return new Result<LoginResponseDto>
            {
                Content = new LoginResponseDto { Token=token,User=appUserDto},
                IsSuccess = true,
                Message = "Log In Successfull"
            };

        }

        public async Task<Result<ForgotPasswordResponseDto>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var result = new Result<ForgotPasswordResponseDto>();
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {
                result.IsSuccess = false;
                result.Message = "User not found";
                return result;
            }    

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink =
                $"{_configuration["ResetPasswordUrl"]}?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";

            const string emailSubject = "Your New Password";

            var emailBody = $"Hello {user.FirstName} {user.LastName}, click this link <a href='{resetLink}'>here</a> to reset your password.";

            var isSuccessful = await _emailService.SendEmailAsync(forgotPasswordDto.Email, emailSubject, emailBody);
            if (!isSuccessful)
            {
                result.IsSuccess = false;
                result.Message = "An error occured ";
                return result;
            }
            var generatedResetToken = new ForgotPasswordResponseDto(token);
             result.IsSuccess = true;
             result.Message = "A link has been sent to your email to reset your password";
             result.Content = generatedResetToken;

            return result;
           
        }

        public async Task<Result<bool>> ResetPasswordAsync(string email, string token, ResetPasswordDto resetPasswordDto)
        {
            var result = new Result<bool>();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                result.IsSuccess = false;
                result.Message = "User not found";
                return result;
            }

            var resetPasswordResult =
                await _userManager.ResetPasswordAsync(user,token, resetPasswordDto.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                result.IsSuccess = false;
                result.Message = "Password reset failed";
                result.Error = resetPasswordResult.Errors.Select(error => new Error(error.Code, error.Description)).AsEnumerable();
                result.Content = false;
                return result;
            }

            result.IsSuccess = true;
            result.Message = "Password reset succesfully";
            result.Content = true;
            return result;
        }

        public async Task<Result<bool>> ConfirmEmailAsymc(string email, string token)
        {
            var result = new Result<bool>();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                result.IsSuccess = false;
                result.Message = "User not found";
                return result;
            }


            var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

            if (!confirmEmailResult.Succeeded)
            {
                result.IsSuccess = false;
                result.Message = "Password reset failed";
                result.Error = confirmEmailResult.Errors.Select(error => new Error(error.Code, error.Description)).AsEnumerable();
                result.Content = false;
                return result;
            }

            user.EmailConfirmed = true;

            var updateResult = await _userManager.UpdateAsync(user);



            if (!updateResult.Succeeded)
            {
                result.IsSuccess = false;
                result.Message = "Password reset failed";
                result.Error = updateResult.Errors.Select(error => new Error(error.Code, error.Description)).AsEnumerable();
                result.Content = false;
                return result;
            }

            result.IsSuccess= true;
            result.Message = "Email confirmed";
            result.Content = true;
            return result;
        }

    }
}
