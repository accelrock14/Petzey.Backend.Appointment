using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public AppointmentStatusCountsDto AppointmentStatusCounts()
        {
            throw new NotImplementedException();
        }

        public List<AppointmentCardDto> FilterDateStatus(FilterParamsDto filterParams)
        {
            throw new NotImplementedException();
        }
    }
}
