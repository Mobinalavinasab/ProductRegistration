using Infrastructure.Entity.BaseEntity;


namespace Infrastructure.Entity.Product;

public class Product : BaseEntity<int>
{
    public string Name { get; set; }
    public int ProduceDate { get; set; }
    public int ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
}