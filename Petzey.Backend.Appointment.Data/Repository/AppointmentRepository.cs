using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Repository
{
    public class AppointmentRepository : IAppointmentRepository, IReportRepository
    {
        public AppointmentStatusCountsDto AppointmentStatusCounts()
        {
            throw new NotImplementedException();
        }

        public List<AppointmentCardDto> FilterDateStatus(FilterParamsDto filterParams)
        {
            throw new NotImplementedException();
        }





        // --------------------------------------------------------------------------------
        // Report Repository Methods
        // --------------------------------------------------------------------------------

        // Get Report details by ID
        public Report GetReportByID(int id) 
        { 
            throw new NotImplementedException(); 
        }

        // Update the detials of a report
        public void UpdateReport(Report report)
        {
            throw new NotImplementedException();
        }

        // Get list of all symptoms
        public List<Symptom> GetSymptoms()
        {
            throw new NotImplementedException();
        }

        // Get list of all tests
        public List<Test> GetTests()
        {
            throw new NotImplementedException();
        }
    }
}
