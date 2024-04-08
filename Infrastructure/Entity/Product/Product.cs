using Infrastructure.Entity.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Entity.Product;

public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public DateTime ProduceDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
    
    public User.User? User { get; set; }
    public long? UserId { get; set; }

}