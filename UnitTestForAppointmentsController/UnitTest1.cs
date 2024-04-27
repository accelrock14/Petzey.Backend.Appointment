using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using System.Web.Http.Results;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Data;

namespace UnitTestForAppointmentsController
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFor_GetAppointmentById_ShouldReturnCorrectAppointmentObject_WhenGivenCorrectInput()
        {
            // Arrange
            var controller = new AppointmentController();
            controller.db = new PetzeyDbContext();
            AppointmentDetail appointmentDetail = new AppointmentDetail();
            appointmentDetail.Status = Petzey.Backend.Appointment.Domain.Entities.Status.Cancelled;
            appointmentDetail.Report = null;
            appointmentDetail.ReasonForVisit = "my crocodile is having cold";
            appointmentDetail.BookingDate = DateTime.Parse("2024-04-20T00:00:00.000");
            appointmentDetail.ScheduleDate = DateTime.Parse("2024-05-03T15:00:00.000");
            appointmentDetail.ScheduleTimeSlot = 10;
            appointmentDetail.OwnerID = 3;
            appointmentDetail.AppointmentID = 4;
            appointmentDetail.PetID = 3;
            appointmentDetail.DoctorID = 3;

            // Act 
            var resutl = controller.GetAppointmentDetail(4) as OkNegotiatedContentResult<AppointmentDetail>;

            // Assert
            Assert.IsNotNull(resutl);
            Assert.AreEqual(appointmentDetail,resutl);
        }
    }
}
