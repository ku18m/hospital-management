using Hospital_Management.Models.DataTypes;

namespace Hospital_Management.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public string AssistantId { get; set; }
        public virtual Assistant Assistant { get; set; }

        public int? RateId { get; set; }
        public virtual Rate Rate { get; set; }
    }
}