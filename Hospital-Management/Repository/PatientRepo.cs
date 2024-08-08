using Hospital_Management.Data;
using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;
namespace Hospital_Management.Repository
{
    public class PatientRepo(ApplicationDbContext context) : IpatientRepo
    {
        public void Delete(int id)
        {
            var patient = context.Patients.Find(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
            }
        }

        public List<Patient> GetAll()
        {
            return context.Patients.Include(p => p.Records)
                                               .Include(p => p.Reservations)
                                               .Include(p => p.Rates)
                                               .ToList();
        }

        public Patient GetById(int id)
        {
            return context.Patients.Include(p => p.Records)
                                   .Include(p => p.Reservations)
                                   .Include(p => p.Rates)
                                   .FirstOrDefault(p => p.Id == id.ToString()); 
        }

        public List<Patient> GetPage(int page)
        {
            int pageSize = 5;

            return context.Patients.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Patients.Count() / pageSize);
        }

        public void Insert(Patient entity)
        {
            context.Patients.Add(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Patient> Search(string searchString, string searchProperty)
        {
            switch (searchProperty)
            {
                case "FullName":
                    return context.Patients.Where(p => p.FullName.Contains(searchString)).ToList();
                case "FirstName":
                    return context.Patients.Where(p => p.FirstName.Contains(searchString)).ToList();
                default:
                    return context.Patients.Where(p => p.LastName.Contains(searchString)).ToList();
                    
            }
        }
        public void Update(Patient entity)
        {
            context.Patients.Update(entity);
        }
    }
}
