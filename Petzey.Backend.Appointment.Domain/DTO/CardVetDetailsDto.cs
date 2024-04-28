using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class CardVetDetailsDto
    {
        public int VetId { get; set; }  
        public string DoctorName { get; set; }
        public string VetSpecialization { get; set; }  
        // DOCTOR PHOTO
    }
}
