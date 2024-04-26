using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface IRepo
    {
       IEnumerable<Symptom> GetSymptoms();
    }
}
