using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Domain;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces
{
    public interface ILibeyUserRepository
    {
        LibeyUserResponse FindResponse(string documentNumber);
        Task<IEnumerable<LibeyUserResponse>> GetAllUsersAsync();
         void Create(LibeyUser libeyUser);
         void Update(LibeyUser libeyUser);
        void Delete(string documentNumber);
        int FindCount(string documentNumber);

    }
}
