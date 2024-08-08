using Hospital_Management.Models;
using Microsoft.AspNetCore.Identity;

namespace Hospital_Management.Data
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed Roles
            string[] roles = { "Admin", "Doctor", "Assistant", "Patient" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                FirstName = "Admin",
                LastName = "User",
                BirthDate = new DateTime(1980, 1, 1),
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.UserName != admin.UserName))
            {
                await userManager.CreateAsync(admin, "AdminPassword123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Seed Speciality data
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Seed Specialities
                if (!context.Specialities.Any())
                {
                    context.Specialities.AddRange(
                        new Speciality { Name = "Cardiology" },
                        new Speciality { Name = "Dermatology" }
                    );
                }

                // Save all changes
                await context.SaveChangesAsync();
            }

            // Seed Doctors
            var doctor1 = new Doctor
            {
                UserName = "doctor1",
                Email = "doctor1@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                BirthDate = new DateTime(1975, 5, 1),
                SpecialityId = 1, // Assuming Speciality with Id 1 exists
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.UserName != doctor1.UserName))
            {
                await userManager.CreateAsync(doctor1, "DoctorPassword123!");
                await userManager.AddToRoleAsync(doctor1, "Doctor");
            }

            // Seed Assistants
            var assistant1 = new Assistant
            {
                UserName = "assistant1",
                Email = "assistant1@example.com",
                FirstName = "John",
                LastName = "Smith",
                BirthDate = new DateTime(1985, 3, 1),
                DoctorId = doctor1.Id,
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.UserName != assistant1.UserName))
            {
                await userManager.CreateAsync(assistant1, "AssistantPassword123!");
                await userManager.AddToRoleAsync(assistant1, "Assistant");
            }

            // Seed Patients
            var patient1 = new Patient
            {
                UserName = "patient1",
                Email = "patient1@example.com",
                FirstName = "Alice",
                LastName = "Johnson",
                BirthDate = new DateTime(1990, 7, 1),
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.UserName != patient1.UserName))
            {
                await userManager.CreateAsync(patient1, "PatientPassword123!");
                await userManager.AddToRoleAsync(patient1, "Patient");
            }

            // Seed Articles Data
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Seed Articles
                if (!context.Articles.Any())
                {
                    context.Articles.AddRange(
                        new Article { Title = "Heart Health", Content = "Content about heart health...", DoctorId = doctor1.Id },
                        new Article { Title = "Skin Care", Content = "Content about skin care...", DoctorId = doctor1.Id }
                    );
                }

                // Save all changes
                await context.SaveChangesAsync();
            }
        }
    }
}
