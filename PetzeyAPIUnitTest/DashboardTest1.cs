using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain.Interfaces;
using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petzey.Backend.Appointment.Domain.DTO;
using System.Web.Http.Results;
using Moq;

namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class DashboardTest1
    {
        private Mock<IAppointmentRepository> _mockRepo;
        private DashboardController _controller;
        [TestInitialize]
        public void Setup()
        {
            // Create a mock repository
            _mockRepo = new Mock<IAppointmentRepository>();

            // Initialize the controller with the mocked repository
            _controller = new DashboardController(_mockRepo.Object);
        }

        [TestMethod]
        public void GetStatusCounts_ReturnsOk()
        {
            // Arrange
            var statusCounts = new AppointmentStatusCountsDto
            {
                Pending = 5,
                Confirmed = 10,
                Cancelled = 3,
                Total = 18,
                Closed = 0
            };
            _mockRepo.Setup(repo => repo.AppointmentStatusCounts()).Returns(statusCounts);

            // Act
            var actionResult = _controller.GetStatusCounts() as OkNegotiatedContentResult<AppointmentStatusCountsDto>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(statusCounts.Pending, actionResult.Content.Pending);
            Assert.AreEqual(statusCounts.Confirmed, actionResult.Content.Confirmed);
            Assert.AreEqual(statusCounts.Cancelled, actionResult.Content.Cancelled);
            Assert.AreEqual(statusCounts.Total, actionResult.Content.Total);
            Assert.AreEqual(statusCounts.Closed, actionResult.Content.Closed);
        }
        //

        [TestMethod]
        public void FilterDateStatus_ReturnsOk()
        {
            // Arrange
            var filters = new FilterParamsDto { ScheduleDate = new DateTime(2024, 4, 25), Status = Status.Confirmed };
            var appointments = new List<AppointmentCardDto>
    {
        new AppointmentCardDto { AppointmentID = 1, DoctorID = 1, PetID = 1, ScheduleDate = new DateTime(2024, 4, 25) },
        new AppointmentCardDto { AppointmentID = 2, DoctorID = 2, PetID = 2, ScheduleDate = new DateTime(2024, 4, 25) }
    };
            _mockRepo.Setup(repo => repo.FilterDateStatus(filters)).Returns(appointments);

            // Act
            var actionResult = _controller.FilterDateStatus(filters) as OkNegotiatedContentResult<IEnumerable<AppointmentCardDto>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Content.Count());
        }
        

    }
}
