using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Areas.AssistantPortal.Controllers
{
    [Area("AssistantPortal")]
    [Authorize(Policy = "RequireAssistantRole")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
