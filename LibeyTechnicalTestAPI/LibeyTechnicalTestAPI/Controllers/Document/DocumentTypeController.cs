using Microsoft.AspNetCore.Mvc;

namespace LibeyTechnicalTestAPI.Controllers.Document;

public class DocumentTypeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}