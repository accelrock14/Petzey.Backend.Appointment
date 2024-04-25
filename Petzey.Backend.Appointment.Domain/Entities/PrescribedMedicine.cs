using System;
using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain
{
    public class PrescribedMedicine
    {
        public int PrescribedMedicineID {  get; set; }
        public Medicine Medicine { get; set; }
        public int PrescriptionID { get; set; }
        public int NumberOfDays { get; set; }
        public Boolean Consume {  get; set; } //before food false, after food true
        public List<Dosage> Dosages { get; set; }

    }

}
