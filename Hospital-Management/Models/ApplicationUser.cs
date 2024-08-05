using Microsoft.AspNetCore.Identity;

namespace Hospital_Management.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string? Address { get; set; }

        public byte[]? Img { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
