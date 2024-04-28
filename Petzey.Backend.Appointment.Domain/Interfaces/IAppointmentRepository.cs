using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        


        IQueryable<AppointmentDetail> GetAppointmentDetails();


        AppointmentDetail GetAppointmentDetail(int id);



        bool PutAppointmentDetail(int id, AppointmentDetail appointmentDetail);


        bool PostAppointmentDetail(AppointmentDetail appointmentDetail);


        bool DeleteAppointmentDetail(int id);

        


        bool AppointmentDetailExists(int id);




        IQueryable<GeneralPetIssue> GetAllGeneralPetIssues();



        bool PostGeneralPetIssue(GeneralPetIssue generalPetIssue);


        List<AppointmentDetail> GetAppointmentsOfDocOnDate(int doctorId, DateTime date);



        bool PatchAppointmentStatus(int id, Status status);


        List<bool> GetScheduledTimeSlotsBasedOnDocIDandDate(int doctorId, DateTime date);


    }
}








