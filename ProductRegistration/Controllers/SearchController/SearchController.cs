using Application.AppDbContext;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Infrastructure.Entity.Product;
using Infrastructure.Entity.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductRegistration.Model.ProductDto;

namespace ProductRegistration.Controllers.SearchController;

[ApiController]
[Route("api/[controller]/[action]")]
public class SearchController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _repository;
    public SearchController(IMapper mapper, IRepository<Product> repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Search(int id)
    {
        var list = _repository.TableNoTracking;
        if (!string.IsNullOrEmpty(id.ToString()))
            list = list.Where(p => p.UserId.ToString().Contains(id.ToString()));
            var newlist = list.ProjectTo<ProductSelectDto>(_mapper.ConfigurationProvider).ToList();
            return Ok(newlist);
    }
    
    
}