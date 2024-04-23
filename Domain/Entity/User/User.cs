using Infrastructure.Entity.BaseEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Entity.User;

public class User : IdentityUser<long> , IEntity
{
    public string Name { get; set; }
    
    public List<Product.Product> Products  { get; set; }
   
   public class PatientConfiguration:IEntityTypeConfiguration<User>
   {
       public void Configure(EntityTypeBuilder<User> builder)
       {
           builder.HasMany(p => p.Products)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId);
       }
   }
}