using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.Entities;
using System.Data.Entity;

namespace Petzey.Backend.Appointment.Data
{
    public class PetzeyDbContext:DbContext
    {

        public PetzeyDbContext():base("DefaultConnection")
        {
            
        }

        public DbSet<Symptom> Symptoms { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<PetIssue> PetIssues { get; set; }
        public DbSet<GeneralPetIssue> GeneralPetIssues {  get; set; }
        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescribedMedicine> PrescribedMedics { get; set; }

        public DbSet<ReportSymptom> ReportSymptoms { get; set; }
        public DbSet<ReportTest> ReportTests { get; set; }
        public DbSet<RecommendedDoctor> RecommendedDoctors { get;set; }
        public DbSet<FeedbackQuestion> FeedbackQuestions { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }

    }
}
