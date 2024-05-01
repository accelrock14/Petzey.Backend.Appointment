using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Web.Http.Results;
using System.Web.Http;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class FeedbackTest1
    {
        /*[TestMethod]
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
        }*/
        [TestMethod]
        public void GetFeedback_Returns_AllFeedbacks()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            var expectedFeedbacks = new List<Feedback>
            {
                new Feedback { FeedbackID = 1, Recommendation = "Recommendation 1" },
                new Feedback { FeedbackID = 2, Recommendation = "Recommendation 2" }
            };
            mockRepo.Setup(repo => repo.getAllFeedbacks()).Returns(expectedFeedbacks.AsQueryable());

            // Act
            var result = controller.GetFeedback();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedFeedbacks, result.ToList());
        }

        [TestMethod]
        public void GetFeedback_Returns_SingleFeedback_ById()
        {
            // Arrange
            int appointmentId = 123; // Use a valid appointment ID
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            var expectedFeedback = new Feedback { FeedbackID = 1, Recommendation = "Recommendation 1", AppointmentId = appointmentId };
            mockRepo.Setup(repo => repo.getFeedbackByAppointmrntId(appointmentId)).Returns(expectedFeedback);

            // Act
            var result = controller.GetFeedback(appointmentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<Feedback>));
            var contentResult = (OkNegotiatedContentResult<Feedback>)result;
            Assert.AreEqual(expectedFeedback, contentResult.Content);
        }


        [TestMethod]
        public void PostFeedback_Returns_Created()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            var newFeedback = new Feedback { FeedbackID = 1, Recommendation = "New Recommendation" };

            // Act
            var result = controller.PostFeedback(newFeedback);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is System.Web.Http.Results.CreatedAtRouteNegotiatedContentResult<Feedback>);
        }
        [TestMethod]
        public void GetFeedback_Returns_EmptyList_IfNoFeedbacks()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            mockRepo.Setup(repo => repo.getAllFeedbacks()).Returns(new List<Feedback>().AsQueryable());

            // Act
            var result = controller.GetFeedback();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetFeedback_Returns_InternalServerError_IfExceptionThrown()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            mockRepo.Setup(repo => repo.getFeedbackByAppointmrntId(id)).Throws<Exception>();

            // Act
            var result = controller.GetFeedback(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }


        [TestMethod]
        public void GetFeedback_Returns_NotFound_IfNoFeedbackFoundById()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            mockRepo.Setup(repo => repo.getFeedbackByAppointmrntId(id)).Returns((Feedback)null);

            // Act
            var result = controller.GetFeedback(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostFeedback_Returns_CreatedAtRoute_IfModelStateValid()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            var feedback = new Feedback { FeedbackID = 1, Recommendation = "New Recommendation" };

            // Act
            var result = controller.PostFeedback(feedback);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<Feedback>));
        }





        [TestMethod]
        public void PostFeedback_Returns_InternalServerError_IfExceptionThrown()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackController(mockRepo.Object);
            mockRepo.Setup(repo => repo.Addfeedback(It.IsAny<Feedback>())).Throws<Exception>();
            var feedback = new Feedback();

            // Act
            var result = controller.PostFeedback(feedback);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }
    }
}
