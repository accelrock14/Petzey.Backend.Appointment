using Petzey.Backend.Appointment.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
       List<AppointmentCardDto> FilterDateStatus(FilterParamsDto filterParams);
       AppointmentStatusCountsDto AppointmentStatusCounts();
       List<AppointmentCardDto> AppointmentByPetIdAndDate(int petId, DateTime date);
       List<AppointmentCardDto> AppointmentByPetId(int petId);
    }
}
