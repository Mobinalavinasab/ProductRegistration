using System.ComponentModel.DataAnnotations;
using Application.BaseDto;
using Infrastructure.Entity.Product;

namespace ProductRegistration.Model.ProductDto;

public class ProductDto : BaseDto<ProductDto, Product, int>
{
    [Required(ErrorMessage = "نام محصول را وارد کنبد")]
    [StringLength(60, ErrorMessage = "نام محصول باید کمتر از 60 کاراکتر باشد")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "تاریخ محصول وارد کنید")]
    public DateTime ProduceDate { get; set; }
    
    [Required(ErrorMessage = "شماره تماس را وارد کنید")]
    public string ManufacturePhone { get; set; }
    
    [Required(ErrorMessage = "ایمیل را وارد کنید")]
    public string ManufactureEmail { get; set; }
    
    public bool IsAvailable { get; set; }
}