using System;
using System.Collections.Generic;

namespace Petzey.Backend.Appointment.Domain
{
    public class PrescribedMedicine
    {
        public int PrescribedMedicineID {  get; set; }
        public int MedicineID { get; set; }
        public virtual Medicine Medicine { get; set; }
        public int NumberOfDays { get; set; }
        public bool Consume {  get; set; } //before food false, after food true
        public virtual Dosage Dosages { get; set; }
        public string Comment {  get; set; }

    }

}
