namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Question
    {
        public int QuestionId {  get; set; }
        public int FeedbackQuestionId {  get; set; }
        public int Rating { get; set; }
    }
}