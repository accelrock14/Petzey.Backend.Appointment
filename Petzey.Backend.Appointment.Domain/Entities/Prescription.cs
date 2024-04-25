using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public virtual List<PrescribedMedicine> PrescribedMedicines {  get; set; }
    }

}
