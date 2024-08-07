using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Areas.AssistantPortal.Controllers
{
    [Area("AssistantPortal")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
