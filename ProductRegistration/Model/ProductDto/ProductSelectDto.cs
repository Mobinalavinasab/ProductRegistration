using Application.BaseDto;

namespace ProductRegistration.Model.ProductDto;

public class ProductSelectDto : BaseDto<int>
{
    public string Name { get; set; }
    public int ProduceDate { get; set; }
    public int ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
    public bool IsAvailable { get; set; }
}