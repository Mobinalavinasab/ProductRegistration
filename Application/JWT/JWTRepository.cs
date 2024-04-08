using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.JWT;

public class JWTRepository : IJWTRepository
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public JWTRepository(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _signInManager = signInManager;
    }
    
    public async Task<IdentityResult> SignupAsync(User user)
    {
        var doctor = new User()
        {
            Name = user.Name,
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
        };
        return await _userManager.CreateAsync(doctor, user.PasswordHash);
    }

    public async Task<string> LoginAsync(User user)
    {
        var secretKey = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]); // longer that 16 character
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

        var encryptionkey = Encoding.UTF8.GetBytes(_configuration["JWT:Encryptkey"]); //must be 16 character
        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

        var claims = await _getClaimsAsync(user);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"],
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(0),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
    {
        var result = await _signInManager.ClaimsFactory.CreateAsync(user);
        //add custom claims
        var list1 = new List<Claim>(result.Claims);
        //list1.Add(new Claim("DepartmentId", user.DepartmentId.ToString()));
        return list1;
    }
}