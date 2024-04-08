using Application.JWT;
using Infrastructure.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductRegistration.Model.UserDto;

namespace ProductRegistration.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJWTRepository _jwtRepository;

    public UserController(UserManager<User> userManager, IJWTRepository jwtRepository)
    {
        _userManager = userManager;
        _jwtRepository = jwtRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignupDto signupDto)
    {
        var newUser = new User()
        {
            Name = signupDto.Name,
            UserName = signupDto.UsreName,
            Email = signupDto.Email,
            PasswordHash = signupDto.Password
        };
        var Result = await _jwtRepository.SignupAsync(newUser);

        if (Result.Succeeded)
        {
            return Ok("Signup was successful");
        }

        return Unauthorized();
    }

    [HttpPost]
    public async Task<IActionResult> Signin(SigninDto signinDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(signinDto.UsreName);
        
        if (user == null)
            throw new Exception(message: "نام کاربری یا رمز عبور اشتباه است");
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, signinDto.Password);
        if (!isPasswordValid)
            throw new Exception("نام کاربری یا رمز عبور اشتباه است");
            
        var jwt = await _jwtRepository.LoginAsync(user);

        return new JsonResult(jwt);
    }

}