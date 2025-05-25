using ABPDCW9.Models;
using Microsoft.EntityFrameworkCore;

namespace ABPDCW9.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var doctors = new List<Doctor>
        {
            new()
            {
                IdDoctor = 1,
                FirstName = "Marek",
                LastName = "Nowak",
                Email = "marek.nowak@example.com"
            }
        };


        var medicaments = new List<Medicament>
        {
            new()
            {
                IdMedicament = 1,
                Name = "Ibuprofen",
                Type = "Drug",
                Description = "Painkiller"
            },
            new()
            {
                IdMedicament = 2,
                Name = "Suberb",
                Type = "Drug",
                Description = "Antibiotic"
            },
            new()
            {
                IdMedicament = 3,
                Name = "Noshpa",
                Type = "Drug",
                Description = "Antihistamine"
            }
        };


        var patients = new List<Patient>
        {
            new()
            {
                IdPatient = 1,
                FirstName = "Anna",
                LastName = "Zielinska",
                BirthDate = new DateTime(1985, 10, 15)
            }
        };


        var prescriptions = new List<Prescription>
        {
            new()
            {
                IdPrescription = 1,
                Date = new DateTime(2025, 5, 24),
                DueDate = new DateTime(2025, 5, 30),
                IdDoctor = 1,
                IdPatient = 1
            }
        };


        var prescriptionMedicaments = new List<PrescriptionMedicament>
        {
            new()
            {
                IdPrescription = 1,
                IdMedicament = 1,
                Dose = 1,
                Details = "test1"
            },
            new()
            {
                IdPrescription = 1,
                IdMedicament = 2,
                Dose = 2,
                Details = "test2"
            }
        };

        modelBuilder.Entity<Doctor>().HasData(doctors);
        modelBuilder.Entity<Medicament>().HasData(medicaments);
        modelBuilder.Entity<Patient>().HasData(patients);
        modelBuilder.Entity<Prescription>().HasData(prescriptions);
        modelBuilder.Entity<PrescriptionMedicament>().HasData(prescriptionMedicaments);
    }
}