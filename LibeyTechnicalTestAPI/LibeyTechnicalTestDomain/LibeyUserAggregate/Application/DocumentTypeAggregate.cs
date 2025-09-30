using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;

namespace LibeyTechnicalTestDomain.LibeyUserAggregate.Application;

public class DocumentTypeAggregate:IDocumentTypeAggregate
{
    private readonly IDocumentTypeRepository _documentTypeRepository;
    public DocumentTypeAggregate(IDocumentTypeRepository documentTypeRepository)
    {
        _documentTypeRepository = documentTypeRepository;
    }

    public Task<IEnumerable<DocumentTypeResponse>> GetAllDocumentTypesAsync()
    {
        return _documentTypeRepository.GetAllDocumentTypesAsync();
    }
}