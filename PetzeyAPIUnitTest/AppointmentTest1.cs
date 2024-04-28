using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain.Interfaces;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using System.Linq;
using System.Net;
using Petzey.Backend.Appointment.Domain.Entities;

namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class AppointmentTest1
    {
        private Mock<IAppointmentRepository> _mockRepo;
        private AppointmentController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Create a mock repository
            _mockRepo = new Mock<IAppointmentRepository>();

            // Initialize the controller with the mocked repository
            _controller = new AppointmentController(_mockRepo.Object);
        }

        [TestMethod]
        public void GetAppointmentDetails_ReturnsAllAppointments()
        {
            // Arrange
            var appointments = new List<AppointmentDetail>
            {
                new AppointmentDetail { AppointmentID = 1 },
                new AppointmentDetail { AppointmentID = 2 }
            }.AsQueryable();

            _mockRepo.Setup(repo => repo.GetAppointmentDetails()).Returns(appointments);

            // Act
            var result = _controller.GetAppointmentDetails();

            // Assert
            Assert.AreEqual(2, result.Count());
            _mockRepo.Verify(repo => repo.GetAppointmentDetails(), Times.Once);
        }

        [TestMethod]
        public void GetAppointmentDetail_WithExistingId_ReturnsOk()
        {
            // Arrange
            var appointment = new AppointmentDetail { AppointmentID = 1 };
            _mockRepo.Setup(repo => repo.GetAppointmentDetail(1)).Returns(appointment);

            // Act
            var actionResult = _controller.GetAppointmentDetail(1);
            var contentResult = actionResult as OkNegotiatedContentResult<AppointmentDetail>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.AppointmentID);
        }

        [TestMethod]
        public void GetAppointmentDetail_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAppointmentDetail(It.IsAny<int>())).Returns((AppointmentDetail)null);

            // Act
            var actionResult = _controller.GetAppointmentDetail(99);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostAppointmentDetail_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var badResponse = _controller.PostAppointmentDetail(new AppointmentDetail());

            // Assert
            Assert.IsInstanceOfType(badResponse, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public void PostAppointmentDetail_ValidModel_ReturnsCreatedAtRoute()
        {
            // Arrange
            var appointmentDetail = new AppointmentDetail { AppointmentID = 1, ScheduleTimeSlot = 1 };
            _mockRepo.Setup(repo => repo.PostAppointmentDetail(appointmentDetail)).Returns(true);

            // Act
            var result = _controller.PostAppointmentDetail(appointmentDetail) as CreatedAtRouteNegotiatedContentResult<AppointmentDetail>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DefaultApi", result.RouteName);
            Assert.AreEqual(result.RouteValues["id"], result.Content.AppointmentID);
        }

        [TestMethod]
        public void DeleteAppointmentDetail_ExistingId_ReturnsOk()
        {
            // Arrange
            var appointmentDetail = new AppointmentDetail { AppointmentID = 1 };
            _mockRepo.Setup(repo => repo.GetAppointmentDetail(1)).Returns(appointmentDetail);
            _mockRepo.Setup(repo => repo.DeleteAppointmentDetail(1)).Returns(true);

            // Act
            var actionResult = _controller.DeleteAppointmentDetail(1);
            var contentResult = actionResult as OkNegotiatedContentResult<AppointmentDetail>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(1, contentResult.Content.AppointmentID);
        }

        [TestMethod]
        public void DeleteAppointmentDetail_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAppointmentDetail(It.IsAny<int>())).Returns((AppointmentDetail)null);

            // Act
            var actionResult = _controller.DeleteAppointmentDetail(99);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutAppointmentDetail_InvalidAppointmentId_ReturnsBadRequest()
        {
            // Arrange
            var appointment = new AppointmentDetail { AppointmentID = 1, ScheduleTimeSlot = 3 };
            _mockRepo.Setup(repo => repo.PutAppointmentDetail(2, appointment)).Returns(false); // ID mismatch

            // Act
            var result = _controller.PutAppointmentDetail(2, appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutAppointmentDetail_ValidAppointment_ReturnsNoContent()
        {
            // Arrange
            var appointment = new AppointmentDetail { AppointmentID = 1, ScheduleTimeSlot = 3 };
            _mockRepo.Setup(repo => repo.PutAppointmentDetail(1, appointment)).Returns(true);

            // Act
            var result = _controller.PutAppointmentDetail(1, appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void PatchAppointmentStatus_InvalidAppointment_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAppointmentDetail(1)).Returns((AppointmentDetail)null);

            // Act
            var result = _controller.PatchAppointmentStatus(1, Status.Confirmed);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetScheduledTimeSlotsBasedOnDocIDandDate_ReturnsCorrectTimeSlots()
        {
            // Arrange
            int doctorId = 1;
            DateTime date = new DateTime(2024, 04, 26);
            var expectedTimeSlots = new List<bool> { true, false, true, true, false, true, false, false, true, false, true, true, false, true, false, false, true, false };

            _mockRepo.Setup(repo => repo.GetScheduledTimeSlotsBasedOnDocIDandDate(doctorId, date)).Returns(expectedTimeSlots);

            // Act
            var actionResult = _controller.GetScheduledTimeSlotsBasedOnDocIDandDate(doctorId, date) as OkNegotiatedContentResult<List<bool>>;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(expectedTimeSlots.Count, actionResult.Content.Count);
            for (int i = 0; i < expectedTimeSlots.Count; i++)
            {
                Assert.AreEqual(expectedTimeSlots[i], actionResult.Content[i], $"Mismatch at index {i}");
            }
        }

        [TestMethod]
        public void GetAppointmentsOfDocOnDate_ReturnsAppointmentsForSpecificDoctorAndDate()
        {
            // Arrange
            int doctorId = 1;
            DateTime specificDate = new DateTime(2024, 04, 26);
            var appointments = new List<AppointmentDetail>
    {
        new AppointmentDetail { AppointmentID = 1, DoctorID = doctorId, ScheduleDate = specificDate },
        new AppointmentDetail { AppointmentID = 2, DoctorID = doctorId, ScheduleDate = specificDate }
    };

            _mockRepo.Setup(repo => repo.GetAppointmentsOfDocOnDate(doctorId, specificDate)).Returns(appointments);

            // Act
            var results = _controller.GetAppointmentsOfDocOnDate(doctorId, specificDate);

            // Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.All(app => app.DoctorID == doctorId && app.ScheduleDate.Date == specificDate.Date), "All appointments should match the specific doctor and date.");
        }

        [TestMethod]
        public void PostGeneralPetIssue_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("IssueName", "Required");

            var generalPetIssue = new GeneralPetIssue { IssueName = "" }; // Invalid data based on ModelState

            // Act
            var result = _controller.PostGeneralPetIssue(generalPetIssue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public void PostGeneralPetIssue_RepositoryReturnsFalse_ReturnsBadRequest()
        {
            // Arrange
            var generalPetIssue = new GeneralPetIssue { IssueName = "Anxiety" };
            _mockRepo.Setup(repo => repo.PostGeneralPetIssue(generalPetIssue)).Returns(false);

            // Act
            var result = _controller.PostGeneralPetIssue(generalPetIssue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

       




    }
}

