using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Web.Http.Results;
using System.Web.Http;
using Petzey.Backend.Appointment.Domain.Interfaces;

namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class FeedbackTest1
    {
        [TestMethod]
        public void PostFeedback_ShouldReturnCreatedAtRoute()
        {
            var mockRepo = new Mock<IAppointmentRepository>();

            var controller = new FeedbackController(mockRepo.Object);
            // Arrange

            var feedback = new Feedback
            {
                FeedbackID = 1,
                Competence = 5,
                Outcome = 4,
                Booking = 3,
                Recommendation = "Great!",
                Comments = "No comments",
                AppointmentId = 10
            };

            // Act
            IHttpActionResult actionResult = controller.PostFeedback(feedback);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult
            <Feedback>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(feedback.FeedbackID, createdResult.RouteValues["id"]);
            Assert.AreEqual(feedback, createdResult.Content);
        }

        [TestMethod]
        public void PostFeedback_InvalidModel_ShouldReturnBadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();

            var controller = new FeedbackController(mockRepo.Object);
            var invalidFeedback = new Feedback(); // This feedback is invalid as no properties are set
            controller.ModelState.AddModelError("Error", "Model state is invalid");

            // Act
            IHttpActionResult actionResult = controller.PostFeedback(invalidFeedback);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public void GetFeedback_ExistingId_ReturnsFeedback()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            int id = 1;
            var expectedFeedback = new Feedback
            {
                FeedbackID = 1,
                Competence = 3,
                Outcome = 4,
                Booking = 5,
                Recommendation = "Yes",
                Comments = "Some comments here",
                AppointmentId = 123
            };
            mockRepo.Setup(m => m.getFeedbackByAppointmrntId(id)).Returns(expectedFeedback);
            var controller = new FeedbackController(mockRepo.Object);

            // Act
            var actionResult = controller.GetFeedback(id);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<Feedback>));
            var contentResult = actionResult as OkNegotiatedContentResult<Feedback>;
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(expectedFeedback.FeedbackID, contentResult.Content.FeedbackID);
            // Optionally, assert on other properties of the returned feedback object
        }

        [TestMethod]
        public void GetFeedback_ShouldReturnNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();

            var controller = new FeedbackController(mockRepo.Object);

            int id = 100; // Assuming this ID does not exist in your database

            // Act
            IHttpActionResult actionResult = controller.GetFeedback(id);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
