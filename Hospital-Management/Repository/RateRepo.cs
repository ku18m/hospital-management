using Hospital_Management.Data;
using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Management.Repository
{
    public class RateRepo(ApplicationDbContext context) : IRateRepo
    {
        void IRepository<Rate>.Delete(int id)
        {
            var rate = context.Rates.Find(id);
            if (rate != null)
            {
                context.Rates.Remove(rate);
            }
        }

        List<Rate> IRepository<Rate>.GetAll()
        {
            return context.Rates.Include(r => r.Doctor).Include(r => r.Patient).ToList();
        }

        Rate IRepository<Rate>.GetById(int id)
        {
            return context.Rates.Include(r => r.Doctor).Include(r => r.Patient)
                  .FirstOrDefault(r => r.Id == id);
        }

        List<Rate> IRepository<Rate>.GetPage(int page)
        {
            int pageSize = 5;

            return context.Rates.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        int IRepository<Rate>.GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Rates.Count() / pageSize);
        }

        void IRepository<Rate>.Insert(Rate rate)
        {
            context.Rates.Add(rate);
        }

        void IRepository<Rate>.Save()
        {
            context.SaveChanges();
        }

        List<Rate> IRepository<Rate>.Search(string searchString, string searchProperty)
        {
            switch (searchProperty)
            {
                case "Value":
                    return context.Rates.Where(r => r.Value.ToString().Contains(searchString)).ToList();
                case "Doctor":
                    return context.Rates.Where(r => r.Doctor.FullName.Contains(searchString)).ToList();

                default:
                    return context.Rates.Where(r => r.Patient.FullName.Contains(searchString)).ToList();
            }
        }
        void IRepository<Rate>.Update(Rate entity)
        {
            context.Rates.Update(entity);
        }
    }
}
