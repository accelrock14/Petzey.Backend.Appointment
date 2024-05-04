using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Net;
using System.Web.Http.Results;
using Petzey.Backend.Appointment.API.Controllers;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System.Collections.Generic;
using System.Web.Http;
namespace PetzeyAPIUnitTest
{
    [TestClass]
    public class FeedbackQuestionsControllerTests
    {
        [TestMethod]
        public void GetFeedbackQuestions_Returns_AllQuestions()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            var expectedQuestions = new List<FeedbackQuestion>
            {
                new FeedbackQuestion { FeedbackQuestionId = 1, FeedbackQuestionName = "Question 1" },
                new FeedbackQuestion { FeedbackQuestionId = 2, FeedbackQuestionName = "Question 2" }
            };
            mockRepo.Setup(repo => repo.getfeedbackquestion()).Returns(expectedQuestions);

            // Act
            var result = controller.GetFeedbackQuestions();

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedQuestions, result);
        }
        [TestMethod]
        public void GetFeedbackQuestions_Returns_InternalServerError_IfExceptionThrown()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            mockRepo.Setup(repo => repo.getfeedbackquestion()).Throws<Exception>();

            // Act
            var result = controller.GetFeedbackQuestions();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public void GetFeedbackQuestion_Returns_RequestedQuestion()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            var expectedQuestion = new FeedbackQuestion { FeedbackQuestionId = id, FeedbackQuestionName = "Question 1" };
            mockRepo.Setup(repo => repo.getfeedbackquestionbyid(id)).Returns(expectedQuestion);

            // Act
            IHttpActionResult actionResult = controller.GetFeedbackQuestion(id);
            var contentResult = actionResult as OkNegotiatedContentResult<FeedbackQuestion>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(id, contentResult.Content.FeedbackQuestionId);
        }
        [TestMethod]
        public void PostFeedbackQuestion_Returns_Created()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            var newQuestion = new FeedbackQuestion { FeedbackQuestionId = 1, FeedbackQuestionName = "New Question" };

            // Act
            IHttpActionResult actionResult = controller.PostFeedbackQuestion(newQuestion);
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<FeedbackQuestion>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual(1, createdResult.RouteValues["id"]);
            Assert.AreEqual(newQuestion, createdResult.Content);
        }
        [TestMethod]
        public void PostFeedbackQuestion_Returns_CreatedAtRoute_IfModelStateValid()
        {
            // Arrange
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            var feedbackQuestion = new FeedbackQuestion { FeedbackQuestionId = 1, FeedbackQuestionName = "New Question" };

            // Act
            var result = controller.PostFeedbackQuestion(feedbackQuestion);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<FeedbackQuestion>));
        }

       




        [TestMethod]
        public void PutFeedbackQuestion_Returns_NoContent()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            var updatedQuestion = new FeedbackQuestion { FeedbackQuestionId = id, FeedbackQuestionName = "Updated Question" };

            // Act
            IHttpActionResult actionResult = controller.PutFeedbackQuestion(id, updatedQuestion);
            var statusCodeResult = actionResult as StatusCodeResult;

            // Assert
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void DeleteFeedbackQuestion_Returns_OK()
        {
            // Arrange
            int id = 1;
            var mockRepo = new Mock<IAppointmentRepository>();
            var controller = new FeedbackQuestionsController(mockRepo.Object);
            var existingQuestion = new FeedbackQuestion { FeedbackQuestionId = id, FeedbackQuestionName = "Existing Question" };
            mockRepo.Setup(repo => repo.getfeedbackquestionbyid(id)).Returns(existingQuestion);

            // Act
            IHttpActionResult actionResult = controller.DeleteFeedbackQuestion(id);
            var contentResult = actionResult as OkNegotiatedContentResult<FeedbackQuestion>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(existingQuestion, contentResult.Content);
        }

        
    }
}

