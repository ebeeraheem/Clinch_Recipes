using Microsoft.AspNetCore.Mvc;

namespace Clinch_Recipes.Controllers;
public class NotesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
