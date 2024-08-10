using Hospital_Management.Models;
using Hospital_Management.Services;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital_Management.Controllers
{
    public class ProfileController(IUserServices<ApplicationUser> services) : Controller
    {

      
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChangesAsync(UserProfileData model, IFormFile Img)
        { 
            var Patient=await services.GetUserByIdAsync(model.UserId);
            if (ModelState.IsValid&& Patient != null)
            {
                
                if (Img != null && Img.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Img.CopyToAsync(memoryStream);
                        Patient.Img = memoryStream.ToArray();
                   
                    }
                  
                }

            }
           
            if (Patient != null) 
            {
                Patient.Email =model.Email;
                Patient.FirstName = model.FirstName;
                Patient.LastName = model.LastName;
                Patient.BirthDate = DateTime.Parse(model.BirthDate);
              
                var result = await services.UpdateUserAsync(Patient);
                if (result)
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;

                    
                    UpdateClaim(claimsIdentity, ClaimTypes.Email, model.Email);
                    UpdateClaim(claimsIdentity, ClaimTypes.GivenName, model.FirstName);
                    UpdateClaim(claimsIdentity, ClaimTypes.Surname, model.LastName);
                    if (DateTime.TryParse(model.BirthDate, out DateTime birthDate))
                    {
                        UpdateClaim(claimsIdentity, ClaimTypes.DateOfBirth, birthDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid date format for Birth Date.");
                    }
                    await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity));


                    return RedirectToAction("Index","Profile"); 
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving your profile.");
                }
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
            }

            return View("Index",model);

        }
        private void UpdateClaim(ClaimsIdentity identity, string claimType, string newValue)
        {
            
            var existingClaim = identity.FindFirst(claimType);
            if (existingClaim != null)
            {
              
                identity.RemoveClaim(existingClaim);
            }
            
                identity.AddClaim(new Claim(claimType, newValue));
            
           
           
        }
    }
}
