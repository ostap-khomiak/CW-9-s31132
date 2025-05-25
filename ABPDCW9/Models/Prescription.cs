using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABPDCW9.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    [ForeignKey(nameof(IdPatient))]
    public virtual Patient Patient { get; set; }  = null!;
    public int IdDoctor { get; set; }
    [ForeignKey(nameof(IdDoctor))]
    public virtual Doctor Doctor { get; set; }  = null!;
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }  = null!;
    
}