using LibeyTechnicalTestDomain.LibeyUserAggregate.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibeyTechnicalTestDomain.EFCore.Configuration;

public class ProvinceConfiguration: IEntityTypeConfiguration<Province>
{
    
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("Province").HasKey(x => x.ProvinceCode);
    }
} 