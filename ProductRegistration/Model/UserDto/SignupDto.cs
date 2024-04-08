using Application.BaseDto;
using Infrastructure.Entity.User;

namespace ProductRegistration.Model.UserDto;

public class SignupDto : BaseDto<SignupDto, User, int>
{
    public string Name { get; set; }
    public string UsreName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}