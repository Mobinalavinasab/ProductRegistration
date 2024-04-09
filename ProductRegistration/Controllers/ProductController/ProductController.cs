using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Infrastructure.Entity.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductRegistration.Model.ProductDto;

namespace ProductRegistration.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class ProductController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _repository;
    public ProductController(IMapper mapper, IRepository<Product> repository)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpPost, Authorize]
    public async Task<IActionResult> NewProduct(ProductDto productDto, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Product>(productDto);
        var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        entity.UserId = long.Parse(id);
        await _repository.AddAsync(entity, cancellationToken);
        var model = _repository
            .TableNoTracking
            .Where(p => p.Id.Equals(entity.Id))
            .ProjectTo<ProductSelectDto>(_mapper.ConfigurationProvider);
        return Ok(model);
    }

    [HttpGet]
    public async Task<ActionResult> GetbyId(int id)
    {
        var model = _mapper.ProjectTo<ProductSelectDto>(_repository.TableNoTracking)
            .FirstOrDefault(p => p.Id.Equals(id));
        return Ok(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _repository.TableNoTracking.ToListAsync(cancellationToken);
        return Ok(list);
    }

    [HttpPut, Authorize]
    public IActionResult UpdateProduct(ProductDto productDto)
    {
        var data = _mapper.Map<Product>(productDto);
        _repository.Update(data);
        return Ok("Update done");
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteProduct(ProductDto productDto)
    {
        if (productDto != null)
        {
            var data = _mapper.Map<Product>(productDto);
            _repository.Delete(data);
            return Ok("Deleted");
        }
        else
        {
            return BadRequest("Something went wrong");
        }
    }

}