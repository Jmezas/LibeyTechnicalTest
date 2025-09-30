using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;

public interface IUbigeoAggregate
{
    public Task<IEnumerable<RegionResponse>> GetRegionsAsync();
    public Task<IEnumerable<ProvinceResponse>> GetProvincesByRegionIdAsync(string regionId);
    public Task<IEnumerable<UbigeoResponse>> GetDistrictsByProvinceIdAsync(string provinceId, string regionId);

}