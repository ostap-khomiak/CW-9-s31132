using ABPDCW9.Data;
using ABPDCW9.DTOs;
using ABPDCW9.Exceptions;
using ABPDCW9.Models;
using Microsoft.EntityFrameworkCore;

namespace ABPDCW9.Services;

public interface IDbService
{
    Task AddPrescriptionAsync(PrescriptionCreateDTO dto);
    Task<PatientGetDTO> GetPatientAsync(int patientId);
}

public class DbService : IDbService
{
    private readonly AppDbContext _context;
    public DbService(AppDbContext context) => _context = context;

    public async Task AddPrescriptionAsync(PrescriptionCreateDTO dto)
    {
        if (dto.Medicaments.Count > 10)
            throw new ArgumentException("A prescription can contain at most 10 medicaments.");
        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate must be on or after Date.");

        var doctor = await _context.Doctors.FindAsync(dto.DoctorId)
            ?? throw new NotFoundException("Doctor not found.");

        Patient patient;
        if (dto.Patient.IdPatient.HasValue)
        {
            patient = await _context.Patients.FindAsync(dto.Patient.IdPatient.Value)
                      ?? new Patient { FirstName = dto.Patient.FirstName, LastName = dto.Patient.LastName, BirthDate = dto.Patient.BirthDate };
        }
        else
        {
            patient = new Patient { FirstName = dto.Patient.FirstName, LastName = dto.Patient.LastName, BirthDate = dto.Patient.BirthDate };
        }

        _context.Patients.Attach(patient);

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            Patient = patient,
            Doctor = doctor,
            PrescriptionMedicaments = new List<PrescriptionMedicament>()
        };

        foreach (var m in dto.Medicaments)
        {
            var medicament = await _context.Medicaments.FindAsync(m.IdMedicament)
                             ?? throw new NotFoundException($"Medicament with Id={m.IdMedicament} not found.");
            prescription.PrescriptionMedicaments.Add(new PrescriptionMedicament
            {
                Medicament = medicament,
                Dose = m.Dose,
                Details = m.Details
            });
        }

        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientGetDTO> GetPatientAsync(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId)
            ?? throw new NotFoundException("Patient not found.");

        var dto = new PatientGetDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionGetDTO
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorGetDTO
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName,
                        Email = pr.Doctor.Email
                    },
                    Medicaments = pr.PrescriptionMedicaments.Select(pm => new PrescriptionMedicamentGetDTO
                    {
                        IdMedicament = pm.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Details
                    }).ToList()
                }).ToList()
        };

        return dto;
    }
}
