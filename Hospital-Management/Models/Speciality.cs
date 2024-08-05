namespace Hospital_Management.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Doctor> Doctors { get; set; }
    }
}