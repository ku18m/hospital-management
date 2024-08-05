using System.ComponentModel.DataAnnotations;

namespace Hospital_Management.Models
{
    public class Rate
    {
        public int Id { get; set; }

        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        public string? Comment { get; set; }
    }
}