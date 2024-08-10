using Hospital_Management.Areas.AdminPortal.ViewModel;
using Hospital_Management.Models;
using Hospital_Management.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Hospital_Management.Areas.AdminPortal.Controllers
{
    [Area("AdminPortal")]
    [Authorize(Policy = "RequireAdminRole")]
    public class DoctorController(IWebHostEnvironment webHostEnvironment, IDoctorRepo DoctorRepo, IReservationRepo ReservationRepo, ISpecialityRepo SpecialityRepo) : Controller
    {
        //private readonly object webHostEnv;

        public IActionResult Index(int page=1)
        {
            int pageSize = 10;
            var doctorsVM = new PaginationVM<Doctor>()
            {
                CurrentPage = page,
                TotalPages = DoctorRepo.GetTotalPages(pageSize),
                Items = DoctorRepo.GetPage(page)
            };
            return View("Index", doctorsVM);
        }

        [HttpPost]
        public IActionResult Search(SearchVM<Doctor> searchVM)
        {
            searchVM.Items = DoctorRepo.Search(searchVM.SearchString, searchVM.SearchProperty);
            return View("Search", searchVM); 
        }

        public IActionResult Details(int id)
        {
            //create BindGetDoctorDetails
            Doctor doctorModel = DoctorRepo.GetById(id);

            return View("Details", doctorModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            //var addDoctorVM = new DoctorDetailsVM();

            //addDoctorVM.Reservations = ReservationRepo.GetAll();
            //addDoctorVM.Specialities = SpecialityRepo.GetAll();
            var addDoctorVM = new DoctorDetailsVM()
            {
                Reservations = SpecialityRepo.GetAll().Select(res => new SelectListItem
                {
                    Value = res.Id.ToString(),
                    Text = res.Name,
                }).ToList(),

                Specialities = SpecialityRepo.GetAll().Select(spec => new SelectListItem
                {
                    Value = spec.Id.ToString(),
                    Text = spec.Name,
                }).ToList()
            };
            return View("Add", addDoctorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DoctorDetailsVM newDoctorFromReq, IFormFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                newDoctorFromReq.Reservations = SpecialityRepo.GetAll().Select(res => new SelectListItem
                {
                    Value = res.Id.ToString(),
                    Text = res.Name,
                }).ToList();

                newDoctorFromReq.Specialities = SpecialityRepo.GetAll().Select(spec => new SelectListItem
                {
                    Value = spec.Id.ToString(),
                    Text = spec.Name,
                }).ToList(); ;
                
                return View("Add", newDoctorFromReq);
            }

            var doctor = new Doctor()
            {
                FirstName = newDoctorFromReq.FirstName,
                LastName = newDoctorFromReq.LastName,
                SpecialityId = newDoctorFromReq.SpecialityId,
                Img = await UploadImage(imgFile),
            };

            DoctorRepo.Insert(doctor);

            DoctorRepo.Save();

            return RedirectToAction("Index");
        }

        private async Task<string> UploadImage(IFormFile imgFile)
        {
            string imgPath;

            if (imgFile != null)
            {
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var fileName = timestamp + "-" + imgFile.FileName;
                imgPath = Path.Combine(webHostEnvironment.WebRootPath, "Images", fileName);

                try
                {
                    using (var fileStream = new FileStream(imgPath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }
                    return $"/Images/{fileName}";
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }


    }
}
