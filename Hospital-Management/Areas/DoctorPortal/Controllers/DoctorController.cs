using Hospital_Management.Data;
using Hospital_Management.Models;
using Hospital_Management.Repository;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Hospital_Management.Areas.DoctorPortal.Controllers
{
    [Area("DoctorPortal")]

    public class DoctorController(ApplicationDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {
        public IActionResult MyReservations()
        {
            var doctorId = userManager.GetUserId(User);

            var reservations = unitOfWork.ReservationRepository.GetByDoctorId(doctorId);

            var reservationViewModels = reservations.Select(r => new ReservationViewModel
            {
                Id = r.Id,
                PatientName = r.Patient.FullName,
                Date = r.Date,
                Status = r.ReservationStatus.ToString()
            }).ToList();

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

        public IActionResult DeleteRecord(int id)
        {
            var record = unitOfWork.RecordRepository.GetById(id);
            if (record == null) return NotFound();
            unitOfWork.RecordRepository.Delete(id);
            unitOfWork.Save();

            return RedirectToAction("PatientRecords");
        }

        #region Articles

        public IActionResult Articles()
        {
            var doctorId = userManager.GetUserId(User);
            var articles = unitOfWork.ArticleRepository.GetByDoctorId(doctorId);
            var articleViewModels = articles.Select(a => new ArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                DateTime = a.DateTime,
                DoctorId = a.DoctorId
            }).ToList();
            return View("Articles", articleViewModels);
        }

        public IActionResult CreateArticle()
        {
            return View("CreateArticle");
        }

        [HttpPost]
        public IActionResult CreateArticle(ArticleViewModel model)
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
                return RedirectToAction("Articles");
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
        public IActionResult SaveEditArticle(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = unitOfWork.ArticleRepository.GetById(model.Id);
                if (article == null) return NotFound();
                article.Title = model.Title;
                article.Content = model.Content;
                article.DateTime = model.DateTime;
                unitOfWork.ArticleRepository.Update(article);
                unitOfWork.Save(); 
                return RedirectToAction("Articles");
            }
            return View("EditArticle", model);
        }

        public IActionResult DeleteArticle(int id)
        {
            var article = unitOfWork.ArticleRepository.GetById(id);
            if (article == null) return NotFound();
            unitOfWork.ArticleRepository.Delete(id);
            unitOfWork.Save();

            return RedirectToAction("Articles");
        }

        #endregion
        public IActionResult ViewRates()
        {
            var doctorId = userManager.GetUserId(User);

            var rates = unitOfWork.RateRepository.GetByDoctorId(doctorId);

            var rateViewModels = rates.Select(r => new RateViewModel
            {
                Id = r.Id,
                Value = r.Value,
                Comment = r.Comment,
                PatientName = r.Patient.FullName
            }).ToList();

            return View("ViewRates", rateViewModels);
        }


    }
}
