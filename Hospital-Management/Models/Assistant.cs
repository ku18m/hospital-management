using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_Management.Models
{
    public class Assistant: ApplicationUser
    {
        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}