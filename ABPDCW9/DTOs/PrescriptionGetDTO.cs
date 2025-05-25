namespace ABPDCW9.DTOs;

public class PrescriptionGetDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorGetDTO Doctor { get; set; }
    public List<PrescriptionMedicamentGetDTO> Medicaments { get; set; }
}
