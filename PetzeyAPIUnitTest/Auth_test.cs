using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Data.Repository;
using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class Auth_test
    {
        [TestMethod]
        public async Task GetAllUser_ReturnsCorrectUsers()
        {
            // Arrange
            var mockGraphServiceClient = new Mock<IGraphServiceClient>();
            var expectedUsers = new List<Users>
            {
                new Users { Id = "1", Name = "User 1", City = "City 1", State = "State 1", Country = "Country 1" },
                new Users { Id = "2", Name = "User 2", City = "City 2", State = "State 2", Country = "Country 2" }
            };
            mockGraphServiceClient.Setup(m => m.GetUsersAsync()).ReturnsAsync(expectedUsers);

            var controller = new AuthController(mockGraphServiceClient.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetAllUser();
            var contentResult = actionResult as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedUsers, contentResult.Content);
        }

        [TestMethod]
        public async Task GetAllUser_HandlesException()
        {
            // Arrange
            var mockGraphServiceClient = new Mock<IGraphServiceClient>();
            mockGraphServiceClient.Setup(m => m.GetUsersAsync()).ThrowsAsync(new Exception("Test exception"));

            var controller = new AuthController(mockGraphServiceClient.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetAllUser();
            var exceptionResult = actionResult as ExceptionResult;

            // Assert
            Assert.IsNotNull(exceptionResult);
            Assert.AreEqual("Test exception", exceptionResult.Exception.Message);
        }
    }
}
