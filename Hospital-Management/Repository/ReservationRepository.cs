using Hospital_Management.Data;
using Hospital_Management.Models;
using Hospital_Management.Models.DataTypes;
namespace Hospital_Management.Repository
{
    public class ReservationRepository(ApplicationDbContext context) : IRepository<Reservation>
    {
        public void Delete(int id)
        {
            context.Reservations.Remove(context.Reservations.Find(id));
        }

        public List<Reservation> GetAll()
        {
           return context.Reservations.ToList();
        }

        public Reservation GetById(int id)
        {
            return context.Reservations.Find(id);
        }

        public List<Reservation> GetPage(int page)
        {
            int pageSize = 10;
            return context.Reservations.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetTotalPages(int pageSize)
        {
            return (int)Math.Ceiling((double)context.Reservations.Count() / pageSize);
        }

        public void Insert(Reservation entity)
        {
            context.Reservations.Add(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<Reservation> Search(string searchString, string searchProperty)
        {
            switch (searchProperty)
            {
                case "Date":
                    DateTime Date;
                    DateTime.TryParse(searchString, out Date);
                    return context.Reservations.Where(res => res.Date == Date).ToList();
                case "Status":
                    ReservationStatus Status;
                    ReservationStatus.TryParse(searchString, out Status);
                    return context.Reservations.Where(res => res.ReservationStatus == Status).ToList();
                default: // Otherwise, search by name.
                    return context.Reservations.Where(res => res.Doctor.FullName.Contains(searchString)).ToList();
            }
        }

        public void Update(Reservation entity)
        {
            context.Reservations.Update(entity); 
        }
    }
}
