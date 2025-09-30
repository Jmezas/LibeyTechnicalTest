using Microsoft.AspNetCore.Mvc;

namespace LibeyTechnicalTestAPI.Controllers.Ubigeo;

public class UbigeoController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}