namespace Hospital_Management.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Diagnosis { get; set; }
        public string? Prescription { get; set; }
        public string? Notes { get; set; }

        public DateTime Date { get; set; }
        public string PatientId { get; set; }
        public virtual Patient Patient { get; set; }

        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}