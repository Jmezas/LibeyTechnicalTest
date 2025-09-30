using LibeyTechnicalTestDomain.EFCore;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Domain;
using Microsoft.EntityFrameworkCore;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Infrastructure
{
    public class LibeyUserRepository : ILibeyUserRepository
    {
        private readonly Context _context;

        public LibeyUserRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LibeyUserResponse>> GetAllUsersAsync()
        {
            return await _context.LibeyUsers.Select(libeyUser => new LibeyUserResponse()
            {
                DocumentNumber = libeyUser.DocumentNumber,
                Active = libeyUser.Active,
                Address = libeyUser.Address,
                DocumentTypeId = libeyUser.DocumentTypeId,
                Email = libeyUser.Email,
                FathersLastName = libeyUser.FathersLastName,
                MothersLastName = libeyUser.MothersLastName,
                Name = libeyUser.Name,
                Password = libeyUser.Password,
                Phone = libeyUser.Phone,
                DocumentType = _context.DocumentType.Where(x => x.DocumentTypeId == libeyUser.DocumentTypeId)
                    .Select(dt => new DocumentTypeResponse
                    {
                        DocumentTypeId = dt.DocumentTypeId,
                        DocumentTypeDescription = dt.DocumentTypeDescription
                    }).FirstOrDefault(),
                Region = _context.Region.Where(x => x.RegionCode == libeyUser.UbigeoCode.Substring(0, 2))
                    .Select(r => new RegionResponse
                    {
                        RegionCode = r.RegionCode,
                        RegionDescription = r.RegionDescription
                    }).FirstOrDefault(),

                Province = _context.Province.Where(x => x.ProvinceCode == libeyUser.UbigeoCode.Substring(0, 4))
                    .Select(p => new ProvinceResponse
                    {
                        ProvinceCode = p.ProvinceCode,
                        ProvinceDescription = p.ProvinceDescription,
                        RegionCode = p.RegionCode
                    }).FirstOrDefault(),

                Ubigeo = _context.Ubigeo.Where(x => x.UbigeoCode == libeyUser.UbigeoCode)
                    .Select(ub => new UbigeoResponse
                    {
                        UbigeoCode = ub.UbigeoCode,
                        UbigeoDescription = ub.UbigeoDescription,
                        ProvinceCode = ub.ProvinceCode,
                        RegionCode = ub.RegionCode
                    }).FirstOrDefault(),
            }).AsNoTracking().ToListAsync();
        }

        public void Create(LibeyUser libeyUser)
        {
            _context.LibeyUsers.Add(libeyUser);
            _context.SaveChanges();
        }

        public void Update(LibeyUser libeyUser)
        {
            _context.LibeyUsers.Update(libeyUser);
            _context.SaveChanges();
        }

        public void Delete(string documentNumber)
        {
            var libeyUser = _context.LibeyUsers.FirstOrDefault(x => x.DocumentNumber == documentNumber);
            if (libeyUser != null)
            {
                libeyUser.Active = false;
                _context.LibeyUsers.Update(libeyUser);
                _context.SaveChanges();
            }
        }


        public LibeyUserResponse FindResponse(string documentNumber)
        {
            var q = from libeyUser in _context.LibeyUsers.Where(x => x.DocumentNumber.Equals(documentNumber))
                select new LibeyUserResponse()
                {
                    DocumentNumber = libeyUser.DocumentNumber,
                    Active = libeyUser.Active,
                    Address = libeyUser.Address,
                    DocumentTypeId = libeyUser.DocumentTypeId,
                    Email = libeyUser.Email,
                    FathersLastName = libeyUser.FathersLastName,
                    MothersLastName = libeyUser.MothersLastName,
                    Name = libeyUser.Name,
                    Password = libeyUser.Password,
                    Phone = libeyUser.Phone,
                    Ubigeo = _context.Ubigeo.Where(x => x.UbigeoCode == libeyUser.UbigeoCode)
                        .Select(ub => new UbigeoResponse
                        {
                            UbigeoCode = ub.UbigeoCode,
                            UbigeoDescription = ub.UbigeoDescription,
                            ProvinceCode = ub.ProvinceCode,
                            RegionCode = ub.RegionCode
                        }).FirstOrDefault(),
                };
            var list = q.ToList();
            if (list.Any()) return list.First();
            else return new LibeyUserResponse();
        }

        public int FindCount(string documentNumber)
        {
            return _context.LibeyUsers.Count(x => x.DocumentNumber == documentNumber);
        }
    }
}