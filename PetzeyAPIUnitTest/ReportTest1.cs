using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class ReportTest1
    {
        private Mock<IAppointmentRepository> mockDbContext;
        private ReportController controller;

        [TestInitialize]
        public void Init()
        {
            mockDbContext = new Mock<IAppointmentRepository>();
            controller = new ReportController(mockDbContext.Object);

        }
        [TestMethod]
        public void GetSymptoms_Returns_Ok_With_Valid_Data()
        {

            //Arrange
            var expectedSymptoms = new List<Symptom>
            {
                 new Symptom { SymptomID = 1, SymptomName = "Fever" },
                 new Symptom { SymptomID = 2, SymptomName = "Headache" },
                 new Symptom { SymptomID = 3, SymptomName = "Cough" },
                 new Symptom { SymptomID = 4, SymptomName = "Fatigue" },
                 new Symptom { SymptomID = 5, SymptomName = "Nausea" }
             };



            mockDbContext.Setup(db => db.GetAllSymptoms()).Returns(expectedSymptoms);



            // Act
            IHttpActionResult actionResult = controller.GetSymptoms();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Symptom>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedSymptoms, (System.Collections.ICollection)contentResult.Content);
        }

        [TestMethod]
        public void GetReport_Returns_Ok_With_Valid_Data()
        {
            // Arrange
            var expectedReport = new Report
            {
                ReportID = 1,
                Prescription = null,
                Symptoms = new List<ReportSymptom>(),
                Tests = new List<ReportTest>(),
                HeartRate = 80,
                Temperature = 37.5f,
                OxygenLevel = 98.0f,
                Comment = "Normal"
            };

            mockDbContext.Setup(db => db.GetReportByID(1)).Returns(expectedReport);

            // Act
            IHttpActionResult actionResult = controller.GetReport(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Report>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedReport.ReportID, contentResult.Content.ReportID);
            Assert.AreEqual(expectedReport.HeartRate, contentResult.Content.HeartRate);
            Assert.AreEqual(expectedReport.Temperature, contentResult.Content.Temperature);
            Assert.AreEqual(expectedReport.OxygenLevel, contentResult.Content.OxygenLevel);
            Assert.AreEqual(expectedReport.Comment, contentResult.Content.Comment);
            Assert.IsNull(contentResult.Content.Prescription); 
            Assert.AreEqual(0, contentResult.Content.Symptoms.Count); 
            Assert.AreEqual(0, contentResult.Content.Tests.Count);
        }

        [TestMethod]
        public void GetTests_Returns_Ok_With_Valid_Data()
        {
            // Arrange
            var expectedTests = new List<Test>
            {
                new Test { TestID = 1, TestName = "Blood Test" },
                new Test { TestID = 2, TestName = "X-Ray" },
                new Test { TestID = 3, TestName = "MRI" },
            };

            mockDbContext.Setup(db => db.GetAllTests()).Returns(expectedTests);

            // Act
            IHttpActionResult actionResult = controller.GetTests();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<Test>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedTests, (System.Collections.ICollection)contentResult.Content);
        }

        [TestMethod]
        public void GetMedicine_Returns_Ok_With_Valid_Data()
        {
            // Arrange
            var expectedMedicine = new Medicine
            {
                MedicineID = 1,
                MedicineName = "Paracetamol"
                // Add more properties as needed
            };

            mockDbContext.Setup(db => db.GetMedicineById(1)).Returns(expectedMedicine);

            // Act
            IHttpActionResult actionResult = controller.GetMedicine(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Medicine>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedMedicine.MedicineID, contentResult.Content.MedicineID);
            Assert.AreEqual(expectedMedicine.MedicineName, contentResult.Content.MedicineName);
            // Add more assertions for other properties
        }

        [TestMethod]
        public void GetMedicine_Returns_NotFound_When_Medicine_Not_Found()
        {
            // Arrange
            mockDbContext.Setup(db => db.GetMedicineById(99)).Returns((Medicine)null);

            // Act
            IHttpActionResult actionResult = controller.GetMedicine(99);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetRecentAppointments_Returns_BadRequest_When_PetID_Less_Than_Or_Equal_To_Zero()
        {
            // Arrange
            int invalidPetId = 0; // Invalid pet ID

            // Act
            IHttpActionResult actionResult = controller.GetRecentAppointments(invalidPetId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetRecentAppointments_Returns_Ok_With_Valid_Data()
        {
            // Arrange
            int petId = 1; // Replace with a valid pet ID
            var expectedAppointments = new List<AppointmentDetail>
            {
                new AppointmentDetail
                {
                    AppointmentID = 1,
                    PetID = petId,
                    ScheduleDate = DateTime.Now.AddDays(-1),
                    Status = Status.Closed,
                    PetIssues = null
                    },
                new AppointmentDetail
                {
                    AppointmentID = 2,
                    PetID = petId,
                    ScheduleDate = DateTime.Now.AddDays(-2),
                    Status = Status.Closed,
                    PetIssues = null 
                },
               
            };
            mockDbContext.Setup(db => db.GetRecentAppointmentsByPetID(petId)).Returns(expectedAppointments);

            // Act
            IHttpActionResult actionResult = controller.GetRecentAppointments(petId);
            var contentResult = actionResult as OkNegotiatedContentResult<List<AppointmentDetail>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedAppointments,contentResult.Content);
        }

        [TestMethod]
        public void GetAllMedicine_Returns_Ok_With_Valid_Data()
        {
            // Arrange
            var expectedMedicines = new List<Medicine>
            {
                new Medicine { MedicineID = 1, MedicineName = "Paracetamol" },
                new Medicine { MedicineID = 2, MedicineName = "Ibuprofen" },
                // Add more medicine objects as needed
            };

            mockDbContext.Setup(db => db.GetAllMedicines()).Returns(expectedMedicines);
            // Act
            IHttpActionResult actionResult = controller.GetAllMedicine();
            var contentResult = actionResult as OkNegotiatedContentResult<List<Medicine>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedMedicines, contentResult.Content);
        }

        [TestMethod]
        public void GetReportHistoryOfThePet_Returns_BadRequest_When_PetID_Isinvlaid()
        {
            // Arrange
            int invalidPetId = 0; // Invalid pet ID

            // Act
            IHttpActionResult actionResult = controller.GetReportHistoryOfThePet(invalidPetId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void GetReportHistoryOfThePet_Returns_NotFound_When_MostRecentAppointment_Not_Found()
        {
            // Arrange
            int PetId = 1; // Invalid pet ID
            mockDbContext.Setup(db => db.MostRecentAppointmentByPetID(PetId)).Returns((AppointmentDetail)null);

            // Act
            IHttpActionResult actionResult = controller.GetReportHistoryOfThePet(PetId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void AddSymptomToReport_Returns_Ok()
        {
            // Arrange
            int reportId = 1; // Replace with a valid report ID
            var reportSymptom = new ReportSymptom
            {
                ReportSymptomID = 1,
                SymptomID = 10, 
                Symptom = new Symptom { SymptomID = 10, SymptomName = "Fever" } 
            };

            // Act
            IHttpActionResult actionResult = controller.AddSymptomToReport(reportId, reportSymptom);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
    }

        [TestMethod]
        public void AddSymptomToReport_CallsRepoAddSymptomToReport()
        {
            // Arrange
            int reportId = 1; // Replace with a valid report ID
            var reportSymptom = new ReportSymptom
            {
                ReportSymptomID = 1,
                SymptomID = 10,
                Symptom = new Symptom { SymptomID = 10, SymptomName = "Fever" }
            };

            // Act
            IHttpActionResult actionResult = controller.AddSymptomToReport(reportId, reportSymptom);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));

            // Verify that the symptom was added to the database
            mockDbContext.Verify(db => db.AddSymptomToReport(reportId, reportSymptom), Times.Once);
        }

        [TestMethod]
        public void DeleteSymptomFromReport_ReturnsOkResult()
        {
            // Arrange
            int reportSymptomId = 123; // Replace with your actual report symptom ID

            // Act
            var result = controller.DeleteSymptomFromReport(reportSymptomId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void DeleteSymptomFromReport_CallsRepoDeleteSymptomFromReport()
        {
            // Arrange
            int reportSymptomId = 123; // Replace with your actual report symptom ID

            // Act
            controller.DeleteSymptomFromReport(reportSymptomId);

            // Assert
            mockDbContext.Verify(repo => repo.DeleteSymptomFromReport(reportSymptomId), Times.Once);
        }

        [TestMethod]
        public void AddTestToReport_ReturnsOkResult()
        {
            // Arrange
            int reportId = 123; // Replace with your actual report ID
            var reportTest = new ReportTest(); // Replace with your actual ReportTest object

            // Act
            var result = controller.AddTestToReport(reportId, reportTest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void AddTestToReport_CallsRepoAddTestToReport()
        {
            // Arrange
            int reportId = 123; // Replace with your actual report ID
            var reportTest = new ReportTest(); // Replace with your actual ReportTest object

            // Act
            controller.AddTestToReport(reportId, reportTest);

            // Assert
            mockDbContext.Verify(repo => repo.AddTestToReport(reportId, reportTest), Times.Once);
        }


    }
}
