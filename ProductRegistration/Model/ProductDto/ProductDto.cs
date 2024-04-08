using Application.BaseDto;
using Infrastructure.Entity.Product;

namespace ProductRegistration.Model.ProductDto;

public class ProductDto : BaseDto<ProductDto, Product, int>
{
    public string Name { get; set; }
    public int ProduceDate { get; set; }
    public int ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
}