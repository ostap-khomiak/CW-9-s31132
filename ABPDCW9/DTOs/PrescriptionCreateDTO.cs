namespace ABPDCW9.DTOs;

public class PrescriptionCreateDTO
{
    public PatientCreateDTO Patient { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<PrescriptionMedicamentDTO> Medicaments { get; set; }
}
