using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application;

public class UbigeoAggregate:IUbigeoAggregate
{
    private readonly IUbigeoRepository _ubigeoRepository;
    public UbigeoAggregate(IUbigeoRepository ubigeoRepository)
    {
        _ubigeoRepository = ubigeoRepository;
    }
    public Task<IEnumerable<RegionResponse>> GetRegionsAsync()
    {
       return _ubigeoRepository.GetRegionsAsync();
    }

    public Task<IEnumerable<ProvinceResponse>> GetProvincesByRegionIdAsync(string regionId)
    {
        return _ubigeoRepository.GetProvincesByRegionIdAsync(regionId);
    }

    public Task<IEnumerable<UbigeoResponse>> GetDistrictsByProvinceIdAsync(string provinceId, string regionId)
    {
        return _ubigeoRepository.GetDistrictsByProvinceIdAsync(provinceId, regionId);
    }
}