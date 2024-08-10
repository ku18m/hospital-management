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

            // Seed Specialities
            var cardiology = new Speciality { Name = "Cardiology" };
            var diagnosis = new Speciality { Name = "Diagnosis" };
            var surgery = new Speciality { Name = "Surgery" };
            var firstAid = new Speciality { Name = "First Aid" };

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Specialities.Any())
                {
                    context.Specialities.AddRange(cardiology, diagnosis, surgery, firstAid);
                }

                await context.SaveChangesAsync();
            }

            // Seed Doctors
            var doctors = new List<Doctor>
        {
            new Doctor
            {
                UserName = "doctor1",
                Email = "doctor1@example.com",
                FirstName = "Jane",
                LastName = "Doe",
                BirthDate = new DateTime(1975, 5, 1),
                SpecialityId = cardiology.Id,
                EmailConfirmed = true
            },
            new Doctor
            {
                UserName = "doctor2",
                Email = "doctor2@example.com",
                FirstName = "David",
                LastName = "Smith",
                BirthDate = new DateTime(1980, 8, 15),
                SpecialityId = diagnosis.Id,
                EmailConfirmed = true
            },
            new Doctor
            {
                UserName = "doctor3",
                Email = "doctor3@example.com",
                FirstName = "Emily",
                LastName = "Jones",
                BirthDate = new DateTime(1982, 10, 5),
                SpecialityId = surgery.Id,
                EmailConfirmed = true
            }
        };

            foreach (var doctor in doctors)
            {
                if (userManager.Users.All(u => u.UserName != doctor.UserName))
                {
                    await userManager.CreateAsync(doctor, "DoctorPassword123!");
                    await userManager.AddToRoleAsync(doctor, "Doctor");
                }
            }

            // Seed Assistants
            var assistants = new List<Assistant>
        {
            new Assistant
            {
                UserName = "assistant1",
                Email = "assistant1@example.com",
                FirstName = "John",
                LastName = "Smith",
                BirthDate = new DateTime(1985, 3, 1),
                DoctorId = doctors[0].Id,
                EmailConfirmed = true
            },
            new Assistant
            {
                UserName = "assistant2",
                Email = "assistant2@example.com",
                FirstName = "Sarah",
                LastName = "Brown",
                BirthDate = new DateTime(1990, 6, 20),
                DoctorId = doctors[1].Id,
                EmailConfirmed = true
            }
        };

            foreach (var assistant in assistants)
            {
                if (userManager.Users.All(u => u.UserName != assistant.UserName))
                {
                    await userManager.CreateAsync(assistant, "AssistantPassword123!");
                    await userManager.AddToRoleAsync(assistant, "Assistant");
                }
            }

            // Seed Patients
            var patients = new List<Patient>
        {
            new Patient
            {
                UserName = "patient1",
                Email = "patient1@example.com",
                FirstName = "Alice",
                LastName = "Johnson",
                BirthDate = new DateTime(1990, 7, 1),
                EmailConfirmed = true
            },
            new Patient
            {
                UserName = "patient2",
                Email = "patient2@example.com",
                FirstName = "Robert",
                LastName = "Miller",
                BirthDate = new DateTime(1987, 2, 17),
                EmailConfirmed = true
            },
            new Patient
            {
                UserName = "patient3",
                Email = "patient3@example.com",
                FirstName = "Laura",
                LastName = "Davis",
                BirthDate = new DateTime(1992, 11, 30),
                EmailConfirmed = true
            }
        };

            foreach (var patient in patients)
            {
                if (userManager.Users.All(u => u.UserName != patient.UserName))
                {
                    await userManager.CreateAsync(patient, "PatientPassword123!");
                    await userManager.AddToRoleAsync(patient, "Patient");
                }
            }

            // Seed Articles
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Articles.Any())
                {
                    context.Articles.AddRange(
                        new Article { Title = "Heart Health", Content = "Content about heart health...", DoctorId = doctors[0].Id },
                        new Article { Title = "Skin Care", Content = "Content about skin care...", DoctorId = doctors[1].Id },
                        new Article { Title = "Neurological Disorders", Content = "Content about neurological disorders...", DoctorId = doctors[2].Id }
                    );
                }

                // Seed Rates
                if (!context.Rates.Any())
                {
                    context.Rates.AddRange(
                        new Rate { DoctorId = doctors[0].Id, PatientId = patients[0].Id, Value = 5, Comment = "Excellent cardiologist!" },
                        new Rate { DoctorId = doctors[1].Id, PatientId = patients[1].Id, Value = 4, Comment = "Very good dermatologist." },
                        new Rate { DoctorId = doctors[2].Id, PatientId = patients[2].Id, Value = 5, Comment = "Highly knowledgeable neurologist." }
                    );
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
