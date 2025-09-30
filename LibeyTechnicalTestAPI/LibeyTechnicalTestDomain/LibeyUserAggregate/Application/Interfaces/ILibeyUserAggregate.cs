using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Domain;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces
{
    public interface ILibeyUserAggregate
    {
        LibeyUserResponse FindResponse(string documentNumber);
        Task<IEnumerable<LibeyUserResponse>> GetAllUsersAsync();
        Result<LibeyUser> Create(UserUpdateorCreateCommand command);
        
        Result<LibeyUser> Update(UserUpdateorCreateCommand command);
        
        void Delete(string documentNumber);
        
    }
}