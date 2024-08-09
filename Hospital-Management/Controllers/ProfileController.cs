using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hospital_Management.ViewModels;
using Hospital_Management.Models;
using Hospital_Management.Repository;
using Hospital_Management.Services;

namespace Hospital_Management.Controllers
{
    public class ProfileController : Controller
    {
        

        public ProfileController(UserServices)
        {
            
        }
        public IActionResult Index()
        {
            var model = new UserProfileData
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                UserName = User.Identity.Name,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                FirstName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                LastName = User.FindFirst(ClaimTypes.Surname)?.Value,
                BirthDate = User.FindFirst(ClaimTypes.DateOfBirth)?.Value,
            };
            return View(model);
        }
        public async Task<IActionResult> SaveChangesAsync(UserProfileData model, IFormFile Img)
        {
            if (ModelState.IsValid)
            {
                // Handle the profile picture upload
                if (Img != null && Img.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Img.CopyToAsync(memoryStream);
                        model.Img = memoryStream.ToArray();
                    }
                  
                }

            }
           
           
            return View(model);
        }
    }
}
