using Hospital_Management.Models.DataTypes;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Models
{
    public class Doctor: ApplicationUser
    {
        public int SpecialityId { get; set; }
        public virtual Speciality Speciality { get; set; }

        public virtual List<Reservation> Reservations { get; set; }

        public virtual List<Assistant> Assistants { get; set; }

        public virtual List<Rate> Rates { get; set; }

        public virtual List<Article> Articles { get; set; }


        // Reservation Properties.
        public DaysOfWeek WorkingDays { get; set; }
        
        public int WorkingHours { get; set; }

        [Range(0, 23)]
        public int StartHour { get; set; }

        public int ExaminationsMinutes { get; set; }

        public decimal ExaminationFees { get; set; }
    }
}
