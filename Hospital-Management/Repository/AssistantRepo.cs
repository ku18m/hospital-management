using Hospital_Management.Data;
using Hospital_Management.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital_Management.Repository
{
    public class AssistantRepo(ApplicationDbContext context) : IAssistantRepo
    {
        void IRepository<Assistant>.Delete(int id)
        {
            var assistant = context.Assistants.Find(id);
            if (assistant != null)
            {
                context.Assistants.Remove(assistant);
            }
        }

        List<Assistant> IRepository<Assistant>.GetAll()
        {
            return context.Assistants.Include(a => a.Doctor).ToList();
        }

        Assistant IRepository<Assistant>.GetById(int id)
        {
            return context.Assistants.Include(a => a.Doctor)
                .FirstOrDefault(a => a.Id == id.ToString());
        }

        List<Assistant> IRepository<Assistant>.GetPage(int page)
        {
            int pageSize = 5;

            return context.Assistants.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        int IRepository<Assistant>.GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Assistants.Count() / pageSize);
        }

        void IRepository<Assistant>.Insert(Assistant entity)
        {
            context.Assistants.Add(entity);
        }

        void IRepository<Assistant>.Save()
        {
            context.SaveChanges();
        }

        List<Assistant> IRepository<Assistant>.Search(string searchString, string searchProperty)
        {
            switch (searchProperty)
            {
                case "FullName":
                    return context.Assistants.Where(a =>a.FullName.Contains(searchString)).ToList();
                case "Doctor":
                    return context.Assistants.Where(a => a.Doctor.FullName.Contains(searchString)).ToList();

                default:
                    return context.Assistants.Where(a => a.FirstName.Contains(searchString)).ToList();
            }
        }

        void IRepository<Assistant>.Update(Assistant entity)
        {
            context.Assistants.Update(entity);
        }
    }
}
