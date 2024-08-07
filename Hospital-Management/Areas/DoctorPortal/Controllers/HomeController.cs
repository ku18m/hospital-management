using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Areas.DoctorPortal.Controllers
{
    public class HomeController : Controller
    {
        [Area("DoctorPortal")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
