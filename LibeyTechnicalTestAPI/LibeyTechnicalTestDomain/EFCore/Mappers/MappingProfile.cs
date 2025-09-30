using AutoMapper;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Domain;

namespace LibeyTechnicalTestDomain.EFCore.Mappers;

public class MappingProfile:Profile
{
    public MappingProfile()
    { 
        CreateMap<UserUpdateorCreateCommand, LibeyUser>();
        CreateMap<LibeyUser, UserUpdateorCreateCommand>();


    }
}