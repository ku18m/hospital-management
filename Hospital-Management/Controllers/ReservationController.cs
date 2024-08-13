using Hospital_Management.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hospital_Management.Controllers
{
    public class ReservationController(IReservationServices reservationServices) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> New(string doctorId)
        {
            var doctorWithRes = await reservationServices.GetDoctorWithResPropertiesAsync(doctorId);

            return View(doctorWithRes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableAppointments(DateTime date, string doctorId)
        {
            var reservations = await reservationServices.GetReservationsInListAsync(doctorId, date);

            return Ok(reservations);
        }

        public async Task<IActionResult> Create(string doctorId, DateTime date)
        {
            var model = await reservationServices.GetDoctorWithResPropertiesAsync(doctorId);
            var userName = User.Identity.Name;
            var result = await reservationServices.ReserveAsync(doctorId, date, userName);


            if (result) return View("New", model);

            return View("New", model);
        }
    }
}
