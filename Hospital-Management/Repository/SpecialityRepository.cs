using Hospital_Management.Data;
using Hospital_Management.Models;


namespace Hospital_Management.Repository
{
    public class SpecialityRepo(ApplicationDbContext context) : ISpecialityRepo
    {
        public void Delete(int id)
        {
            context.Specialities.Remove(context.Specialities.Find(id));
        }

        public List<Speciality> GetAll()
        {
            return context.Specialities.ToList();
        }

        public Speciality GetById(int id)
        {
            return context.Specialities.Find(id);
        }

        public List<Speciality> GetPage(int page)
        {
            int pageSize = 10;
            return context.Specialities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Specialities.Count() / pageSize);
        }

        public void Insert(Speciality entity)
        {
            context.Specialities.Add(entity);

        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Speciality> Search(string searchString, string searchProperty)
        {
            switch (searchProperty)
            {
                case "DoctorName":
                  return context.Specialities.Where(spec => spec.Doctors.Any(doc => doc.FullName.Contains(searchString))).ToList();
                  
                default: // Otherwise, search by name.
                    return context.Specialities.Where(spec => spec.Name.Contains(searchString)).ToList();
            }
        }

        public void Update(Speciality entity)
        {
            context.Specialities.Update(entity);
        }
    }
}
