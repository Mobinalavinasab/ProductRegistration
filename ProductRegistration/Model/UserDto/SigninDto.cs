using Application.BaseDto;

namespace ProductRegistration.Model.UserDto;

public class SigninDto : BaseDto<int>
{
    public string UsreName { get; set; }
    public string Password { get; set; }
}