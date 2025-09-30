using AutoMapper;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Domain;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application
{
    public class LibeyUserAggregate : ILibeyUserAggregate
    {
        private readonly ILibeyUserRepository _repository;
        private readonly IMapper _mapper;

        public LibeyUserAggregate(ILibeyUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<IEnumerable<LibeyUserResponse>> GetAllUsersAsync()
        {
            return _repository.GetAllUsersAsync();
        }

        public Result<LibeyUser> Create(UserUpdateorCreateCommand command)
        {
            var existingUser = _repository.FindResponse(command.DocumentNumber);
            if (existingUser.DocumentNumber == null)
            {
                var libeyUser = _mapper.Map<LibeyUser>(command);
                _repository.Create(libeyUser);
                return Result<LibeyUser>.Ok(libeyUser, "Usuario actualizado correctamente");
            }

            return Result<LibeyUser>.Fail("Ya existe un usuario con ese número de documento");
        }

        public Result<LibeyUser> Update(UserUpdateorCreateCommand libeyUser)
        {
            var count = _repository.FindCount(libeyUser.DocumentNumber);
            if (count > 2)
            {
                return Result<LibeyUser>.Fail(
                    "El número de documento está duplicado en más de 2 registros. No se puede actualizar.");
            }

            var existingUser = _repository.FindResponse(libeyUser.DocumentNumber);
            if (existingUser.DocumentNumber == null)
            {
                return Result<LibeyUser>.Fail("No existe un usuario con ese número de documento");
            }

            var libeyUserEntity = _mapper.Map<LibeyUser>(libeyUser);

            _repository.Update(libeyUserEntity);
            return Result<LibeyUser>.Ok(libeyUserEntity, "Usuario actualizado correctamente");
        }

        public void Delete(string documentNumber)
        {
            _repository.Delete(documentNumber);
        }

        public LibeyUserResponse FindResponse(string documentNumber)
        {
            var row = _repository.FindResponse(documentNumber);
            return row;
        }
    }
}