using LibeyTechnicalTestDomain.EFCore;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Infrastructure;

public class UbigeoRepository : IUbigeoRepository
{
    private readonly Context _context;

    public UbigeoRepository(Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RegionResponse>> GetRegionsAsync()
    {
        return await _context.Region.Select(r => new RegionResponse
        {
            RegionCode = r.RegionCode,
            RegionDescription = r.RegionDescription
        }).AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<ProvinceResponse>> GetProvincesByRegionIdAsync(string regionId)
    {
        return await _context.Province
            .Where(p => p.RegionCode == regionId)
            .Select(p => new ProvinceResponse
            {
                ProvinceCode = p.ProvinceCode,
                RegionCode = p.RegionCode,
                ProvinceDescription = p.ProvinceDescription
            }).AsNoTracking().ToListAsync();
    }

    public Task<IEnumerable<UbigeoResponse>> GetDistrictsByProvinceIdAsync(string provinceId, string regionId)
    {
        return _context.Ubigeo
            .Where(d => d.ProvinceCode == provinceId && d.RegionCode == regionId)
            .Select(d => new UbigeoResponse
            {
                UbigeoCode = d.UbigeoCode,
                ProvinceCode = d.ProvinceCode,
                RegionCode = d.RegionCode,
                UbigeoDescription = d.UbigeoDescription
            }).AsNoTracking().ToListAsync().ContinueWith(t => (IEnumerable<UbigeoResponse>)t.Result);
    }
}