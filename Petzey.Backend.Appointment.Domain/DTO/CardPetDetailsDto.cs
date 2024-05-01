using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.DTO
{
    public class CardPetDetailsDto 
    {
        public int PetId { get; set; }
        public string PetName { get; set; }
        public int PetAge { get; set; }
        public string PetGender { get; set; }
        public string OwnerName { get; set; }

        //PET PHOTO
    }
}
