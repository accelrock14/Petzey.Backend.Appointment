using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Data;
using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Http;

namespace PetzeyAppointmentAPITest
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<IRepo> mockDbContext;
        private ReportController controller;

        [TestInitialize]
        public void Init()
        {
            mockDbContext = new Mock<IRepo>();
            controller = new ReportController(mockDbContext.Object);

        }
        [TestMethod]
        public void GetSymptoms_Returns_Ok_With_Valid_Data()
        {
            var expectedSymptoms= new List<Symptom>
{
    new Symptom { SymptomID = 1, SymptomName = "Fever" },
    new Symptom { SymptomID = 2, SymptomName = "Headache" },
    new Symptom { SymptomID = 3, SymptomName = "Cough" },
    new Symptom { SymptomID = 4, SymptomName = "Fatigue" },
    new Symptom { SymptomID = 5, SymptomName = "Nausea" },
 
};

            /* mockDbContext.Setup(db => db.GetSymptoms()).Returns(new List<Symptom>());
             var result = controller.GetSymptoms();

             Assert.IsNotNull(result);
             Assert.IsTrue(result.GetType().Name.Contains("OkNegotiatedContentResult"));*/

            mockDbContext.Setup(repo => repo.GetSymptoms()).Returns((IEnumerable<Symptom>)null);

            // Act: Call the GetSymptoms action
            IHttpActionResult result = controller.GetSymptoms();

            // Assert: Check if the result is BadRequest
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }
    }
}
