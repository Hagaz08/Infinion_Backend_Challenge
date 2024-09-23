using Infinion.Core.DTOs;

namespace Infinion.Core.Abstractions
{
    public interface IAuthService
    {
        Task<Result<UserRegistrationResponseDto>> Register(RegisterUserDto registerUserDto);
        Task<Result<UserRegistrationResponseDto>> RegisterAdmin(RegisterUserDto registerUserDto);
        Task<Result<LoginResponseDto>> Login(LoginUserDto loginUserDto);
        Task<Result<bool>> ResetPasswordAsync(string email, string token, ResetPasswordDto resetPasswordDto);
        Task<Result<ForgotPasswordResponseDto>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<Result<bool>> ConfirmEmailAsymc(string email, string token);


    }
}
