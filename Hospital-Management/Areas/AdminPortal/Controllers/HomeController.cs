using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Areas.admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("AdminPortal")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
