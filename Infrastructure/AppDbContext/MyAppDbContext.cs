using Core.Extension;
using Infrastructure.Entity.BaseEntity;
using Infrastructure.Entity.Product;
using Infrastructure.Entity.Role;
using Infrastructure.Entity.RoleClaim;
using Infrastructure.Entity.User;
using Infrastructure.Entity.UserClaim;
using Infrastructure.Entity.UserLogin;
using Infrastructure.Entity.UserRole;
using Infrastructure.Entity.UserToken;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.AppDbContext;

public class MyAppDbContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public MyAppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var EntitiesAssembly = typeof(IEntity).Assembly;
        builder.allEntity<IEntity>(EntitiesAssembly);
        builder.ApplyConfigurationsFromAssembly(EntitiesAssembly);
        
    }
}