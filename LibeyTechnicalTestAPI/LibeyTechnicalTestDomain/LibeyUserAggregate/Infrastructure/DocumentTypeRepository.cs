using LibeyTechnicalTestDomain.EFCore;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Infrastructure;

public class DocumentTypeRepository:IDocumentTypeRepository
{   private readonly Context _context;
    public DocumentTypeRepository(Context context)
    {
        _context = context;
    }
    public async Task<IEnumerable<DocumentTypeResponse>> GetAllDocumentTypesAsync()
    {
       return await _context.DocumentType
            .Select(dt => new DocumentTypeResponse
            {
                DocumentTypeId = dt.DocumentTypeId,
                DocumentTypeDescription = dt.DocumentTypeDescription
            }).AsNoTracking().ToListAsync();
    }
}