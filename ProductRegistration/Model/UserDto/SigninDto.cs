using Application.BaseDto;
using Infrastructure.Entity.User;

namespace ProductRegistration.Model.UserDto;

public class SigninDto : BaseDto<SigninDto, User, long>
{
    public string UsreName { get; set; }
    public string Password { get; set; }
}