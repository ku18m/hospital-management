﻿using Hospital_Management.Data;
using Hospital_Management.Models;
using Hospital_Management.Models.DataTypes;
using Hospital_Management.Repository;
using Hospital_Management.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Area("DoctorPortal")]
[Authorize(Policy = "RequireDoctorRole")]

public class ShiftController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IUnitOfWork unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var doctorId = userManager.GetUserId(User);
            var doctor = await userManager.FindByIdAsync(doctorId);

            if (doctor == null)
            {
                return RedirectToAction("Error");
            }

            var firstReservation = await context.Reservations
                .Where(r => r.DoctorId == doctorId &&r.Date.Date==DateTime.Now.Date&&r.ReservationStatus==ReservationStatus.Pending)

                .Include(r => r.Patient)
                .OrderBy(r => r.Date)
                .FirstOrDefaultAsync();

            if (firstReservation != null && !string.IsNullOrEmpty(firstReservation.PatientId))
            {
                return RedirectToAction("ShiftPatientRecords", new { patientId = firstReservation.PatientId });
            }

            return View("NoReservations");
        }
        public async Task<IActionResult> ShiftPatientRecords(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return RedirectToAction("Index");
            }

            var patient = await context.Patients
                .Include(p => p.Records)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                return NotFound();
            }

            var model = new PatientRecordsVM
            {
                PatientName = patient.FullName,
                Records = patient.Records.Select(r => new RecordViewModel
                {
                    Id = r.Id,
                    Description = r.Description,
                    Diagnosis = r.Diagnosis,
                    Prescription = r.Prescription,
                    Notes = r.Notes,
                    Date = r.Date
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(RecordViewModel recordModel)
        {
            if (ModelState.IsValid)
            {
                var record = new Record
                {
                    Description = recordModel.Description,
                    Diagnosis = recordModel.Diagnosis,
                    Prescription = recordModel.Prescription,
                    Notes = recordModel.Notes,
                    Date = DateTime.Now,
                    PatientId = recordModel.PatientId,
                    DoctorId = userManager.GetUserId(User)
                };

                context.Records.Add(record);
                unitOfWork.Save();

            var doctorId = userManager.GetUserId(User);
                var nextReservation = await context.Reservations
                    .Where(r => r.DoctorId == doctorId && r.Date.Date == DateTime.Now.Date && r.ReservationStatus == ReservationStatus.Pending)
                    .OrderBy(r => r.Date)
                    .FirstOrDefaultAsync();

                if (nextReservation != null && !string.IsNullOrEmpty(nextReservation.PatientId))
                {
                    return RedirectToAction("ShiftPatientRecords", new { patientId = nextReservation.PatientId });
                }
                else
                {
                    return RedirectToAction("NoReservations");
                }
            }

            return RedirectToAction("ShiftPatientRecords", new { patientId = recordModel.PatientId });
        }


        public IActionResult Error()
        {
            return View();
        }
    }
