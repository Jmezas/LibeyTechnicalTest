using LibeyTechnicalTestDomain.LibeyUserAggregate.Application;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibeyTechnicalTestAPI.Controllers.Document;
[ApiController]
[Route("[controller]")]
public class DocumentTypeController : Controller
{
    private readonly IDocumentTypeAggregate _documentTypeAggregate;
    public DocumentTypeController(IDocumentTypeAggregate documentTypeAggregate)
    {
        _documentTypeAggregate = documentTypeAggregate;
    } 
    
    [HttpGet("GetAllDocumentTypes")]
    public async Task<IActionResult> GetAllDocumentTypes()
    {
        var result = await _documentTypeAggregate.GetAllDocumentTypesAsync();
        return Ok(result);
    }
}