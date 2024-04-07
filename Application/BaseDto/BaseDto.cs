using System.ComponentModel.DataAnnotations;

namespace Application.BaseDto;

public class BaseDto<Tkey>
{
    [Key] public Tkey Id { get; set; }
}