using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibeyTechnicalTestAPI.Controllers.Ubigeo;
[ApiController]
[Route("[controller]")]
public class UbigeoController : Controller
{
   private readonly IUbigeoAggregate _aggregate;
    public UbigeoController(IUbigeoAggregate aggregate)
    {
        _aggregate = aggregate;
    }
    
    [HttpGet]
    [Route("regions")]
    public async Task<IActionResult> GetRegionsAsync()
    {
        var rows = await _aggregate.GetRegionsAsync();
        return Ok(rows);
    }
    
    [HttpGet]
    [Route("provinces/{regionId}")]
    public async Task<IActionResult> GetProvincesByRegionIdAsync(string regionId)
     {
         var rows = await _aggregate.GetProvincesByRegionIdAsync(regionId);
         return Ok(rows);
     }
    
    [HttpGet]
    [Route("districts/{regionId}/{provinceId}")]
    public async Task<IActionResult> GetDistrictsByProvinceIdAsync(string regionId, string provinceId)
     {
         var rows = await _aggregate.GetDistrictsByProvinceIdAsync(provinceId, regionId);
         return Ok(rows);
     }
}