using Infrastructure.Entity.BaseEntity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entity.User;

public class User : IdentityUser<int> , IEntity
{
    public string Name { get; set; }
}