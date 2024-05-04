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
        public string Name { get; set; }
        public string Specialization { get; set; }  
        public string Photo { get; set; }
        public string NPINumber { get; set; }
    }
}
