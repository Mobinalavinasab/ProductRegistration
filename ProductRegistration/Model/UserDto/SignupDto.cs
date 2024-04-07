using Application.BaseDto;

namespace ProductRegistration.Model.UserDto;

public class SignupDto : BaseDto<int>
{
    public string Name { get; set; }
    public string UsreName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}