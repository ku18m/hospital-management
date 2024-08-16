using Hospital_Management.Data;
using Hospital_Management.Models;
using Hospital_Management.Repository;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Hospital_Management.Areas.DoctorPortal.Controllers
{
    [Area("DoctorPortal")]
    [Authorize(Policy = "RequireDoctorRole")]

    public class DoctorController(ApplicationDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {

        public IActionResult MyReservations(string searchString = "", string searchProperty = "Status", int page = 1)
        {
            var doctorId = userManager.GetUserId(User);
            int pageSize = 5;
            var totalReservations = unitOfWork.ReservationRepository.Search(searchString, searchProperty);
            int totalPages = (int)Math.Ceiling((double)totalReservations.Count / pageSize);
            var reservations = unitOfWork.ReservationRepository.GetByDoctorId(doctorId);
            var pagedReservations = reservations.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            var reservationViewModels = pagedReservations.Select(r => new ReservationViewModel
            {
                Id = r.Id,
                PatientName = r.Patient.FullName,
                Date = r.Date,
                Status = r.ReservationStatus.ToString()
            }).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.SearchProperty = searchProperty;

            return View(reservationViewModels);
        }
        public IActionResult PatientRecords()
        {
            var doctorId = userManager.GetUserId(User);
            var records = unitOfWork.RecordRepository.GetByDoctorId(doctorId);
            var recordViewModels = records.Select(r => new RecordViewModel
            {
                Id = r.Id,
                Description = r.Description,
                Diagnosis = r.Diagnosis,
                Prescription = r.Prescription,
                Notes = r.Notes,
                Date = r.Date,
                PatientId = r.PatientId,
                DoctorId = r.DoctorId
            }).ToList();
            return View(recordViewModels);
        }
        public IActionResult CreateRecord()
        {
            ViewData["Patients"] = new SelectList(unitOfWork.PatientRepository.GetAll(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        public IActionResult CreateRecord(RecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var record = new Record
                {
                    Description = model.Description,
                    Diagnosis = model.Diagnosis,
                    Prescription = model.Prescription,
                    Notes = model.Notes,
                    Date = model.Date,
                    PatientId = model.PatientId,
                    DoctorId = userManager.GetUserId(User)
                };
                unitOfWork.RecordRepository.Insert(record);
                unitOfWork.Save();
                return RedirectToAction("PatientRecords");
            }
            ViewData["Patients"] = new SelectList(unitOfWork.PatientRepository.GetAll(), "Id", "FullName", model.PatientId);
            return View(model);
        }


        [HttpGet]
        public IActionResult EditRecord(int id)
        {
            var record = unitOfWork.RecordRepository.GetById(id);

            if (record == null) return NotFound();
            var recordVM = new RecordViewModel
            {
                Id = record.Id,
                Description = record.Description,
                Diagnosis = record.Diagnosis,
                Prescription = record.Prescription,
                Notes = record.Notes,
                Date = record.Date,
                PatientId = record.PatientId,
                DoctorId = record.DoctorId
            };
            ViewData["Patients"] = new SelectList(unitOfWork.PatientRepository.GetAll(), "Id", "FullName", recordVM.PatientId);
            return View("EditRecord", recordVM);
        }

        [HttpPost]
        public IActionResult SaveEditRecord(RecordViewModel recordModel)
        {
            if (ModelState.IsValid)
            {
                var record = unitOfWork.RecordRepository.GetById(recordModel.Id);
                if (record == null) return NotFound();
                record.Description = recordModel.Description;
                record.Diagnosis = recordModel.Diagnosis;
                record.Prescription = recordModel.Prescription;
                record.Notes = recordModel.Notes;
                record.Date = recordModel.Date;
                record.PatientId = recordModel.PatientId;
                unitOfWork.RecordRepository.Update(record);
                unitOfWork.Save();
                return RedirectToAction("PatientRecords");
            }
            ViewData["Patients"] = new SelectList(unitOfWork.PatientRepository.GetAll(), "Id", "FullName", recordModel.PatientId);
            return View("EditRecord", recordModel);
        }

        #region Articles
        public IActionResult Articles(string searchString = "", string searchProperty = "Title", int page = 1)
        {
            var doctorId = userManager.GetUserId(User);
            int pageSize = 5;

            var allArticles = unitOfWork.ArticleRepository.Search(searchString, searchProperty)
                                    .Where(a => a.DoctorId == doctorId);

            int totalPages = (int)Math.Ceiling((double)allArticles.Count() / pageSize);

            var pagedArticles = allArticles.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var articleViewModels = pagedArticles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                DateTime = a.DateTime,
                DoctorId = a.DoctorId
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.SearchProperty = searchProperty;

            return View("Articles", articleViewModels);
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
            return View("CreateArticle");
        }

        [HttpPost]
        public IActionResult Save(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    DateTime = model.DateTime,
                    DoctorId = userManager.GetUserId(User)
                };
                unitOfWork.ArticleRepository.Insert(article);
                unitOfWork.Save();
                return View("Articles");
            }

            return View("CreateArticle", model);
        }
        [HttpGet]
        public IActionResult EditArticle(int id)
        {

            var article = unitOfWork.ArticleRepository.GetById(id);


            if (article == null) return NotFound();

            var articleVM = new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                DateTime = article.DateTime,
                DoctorId = article.DoctorId
            };

            return View("EditArticle", articleVM);
        }

        [HttpPost]
        [Route("DoctorPortal/Doctor/SaveEditArticle")]

        public IActionResult SaveEditArticle(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {
                var article = unitOfWork.ArticleRepository.GetById(articleVM.Id);
                if (article == null) return NotFound();
                article.Title = articleVM.Title;
                article.Content = articleVM.Content;
                article.DateTime = articleVM.DateTime;
                unitOfWork.ArticleRepository.Update(article);
                unitOfWork.Save();
                return RedirectToAction("Articles");

            }

            return View("EditArticle", articleVM);
        }


        #endregion
        public IActionResult ViewRates(string searchString = "", string searchProperty = "Value", int page = 1)
        {
            int pageSize = 5;

            var filteredRates = unitOfWork.RateRepository.Search(searchString, searchProperty);

            int totalPages = (int)Math.Ceiling((double)filteredRates.Count / pageSize);

            var rates = filteredRates
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var rateViewModels = rates.Select(r => new RateVM
            {
                Id = r.Id,
                Value = r.Value,
                Comment = r.Comment ?? string.Empty,
                PatientName = r.Patient?.FullName ?? string.Empty
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.SearchProperty = searchProperty;

            return View("ViewRates", rateViewModels);
        }

    }
}
