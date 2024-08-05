using Hospital_Management.Models;
using Hospital_Management.Models.DataTypes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Hospital_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Convert DaysOfWeek to int and vice versa.
            builder.Entity<Doctor>()
                .Property(u => u.WorkingDays)
                .HasConversion(
                    v => (int)v,
                    v => (DaysOfWeek)v);

            // Convert ReservationStatus to int and vice versa.
            builder.Entity<Reservation>()
                .Property(res => res.ReservationStatus)
                .HasConversion(
                    v => (int)v,
                    v => (ReservationStatus)v);

            // Set the precision of ExaminationFees to 2 decimal places.
            builder.Entity<Doctor>()
            .Property(d => d.ExaminationFees)
            .HasPrecision(18, 2);

            // Don't Delete the Assistants if a Doctor is deleted.
            builder.Entity<Assistant>()
                .HasOne(d => d.Doctor)
                .WithMany(d => d.Assistants)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Don't Delete the Rates if a Patient is deleted.
            builder.Entity<Rate>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.Rates)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
            
            // Don't Delete the Record if a Doctor is deleted.
            builder.Entity<Record>()
                .HasOne(r => r.Doctor)
                .WithMany()
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Don't Delete the Reservation if a Patient is deleted.
            builder.Entity<Reservation>()
                .HasOne(r => r.Patient)
                .WithMany(p => p.Reservations)
                .HasForeignKey(r => r.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            // Don't Delete the Reservation if a Doctor is deleted.
            builder.Entity<Reservation>()
                .HasOne(r => r.Doctor)
                .WithMany(d => d.Reservations)
                .HasForeignKey(r => r.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
