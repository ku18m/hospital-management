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
    public class UserController(IUserServices<ApplicationUser> userServices) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if(User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            var model = new RegisterationViewModel();

            return View("Register", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Patient
                {
                    UserName = model.GenerateUsername(),
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = model.DateOfBirth,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true
                };

                var result = await userServices.Register(user, model.Password);

                if (result)
                {
                    await Login(new LoginViewModel { UsernameOrEmail = user.UserName, Password = model.Password, RememberMe = true });
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Register", model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View("Login", model);
            }

            var result = await userServices.Login(model.UsernameOrEmail, model.Password);

            if (result)
            {
                var user = await userServices.GetUserByUsernameAsync(User.Identity.Name);
                var roles = await userServices.GetUserRolesAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString("yyyy-MM-dd")),
                    new Claim(ClaimTypes.AuthorizationDecision, (user.EmailConfirmed || user.PhoneNumberConfirmed).ToString()),
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(15) : null
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return roles switch
                {
                    _ when roles.Contains("Admin") => RedirectToAction("Index", "Home", new { area = "adminportal" }),
                    _ when roles.Contains("Doctor") => RedirectToAction("Index", "Home", new { area = "doctorportal" }),
                    _ when roles.Contains("Assistant") => RedirectToAction("Index", "Home", new { area = "assistantportal" }),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            else
            {
                ModelState.AddModelError("", "An Error Occured While logging you in.");
                return View("Login", model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View("AccessDenied");

    }
}
