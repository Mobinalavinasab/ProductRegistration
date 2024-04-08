using Azure.Core;
using Infrastructure.Entity.User;
using Microsoft.AspNetCore.Identity;

namespace Application.JWT;

public interface IJWTRepository
{
    Task<IdentityResult> SignupAsync(User user);
 
    Task<string> LoginAsync(User user);

}