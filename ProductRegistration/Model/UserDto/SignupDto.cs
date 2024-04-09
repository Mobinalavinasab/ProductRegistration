using System.ComponentModel.DataAnnotations;
using Application.BaseDto;
using Infrastructure.Entity.User;

namespace ProductRegistration.Model.UserDto;

public class SignupDto : BaseDto<SignupDto, User, long>
{
    [Required(ErrorMessage = "نام خود را وارد کنید")]
    [StringLength(60, ErrorMessage = "نمی توانید بیش از 60 کاراکتر استفاده کنید")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "نام کاربری خود را وارد کنید")]
    public string UsreName { get; set; } 
    
    [Required(ErrorMessage = "ایمیل خود را وارد کنید")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "رمزعبور خود را وارد کنید")]
    public string Password { get; set; }
}