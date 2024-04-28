namespace Petzey.Backend.Appointment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentDetails",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        DoctorID = c.Int(nullable: false),
                        PetID = c.Int(nullable: false),
                        OwnerID = c.Int(nullable: false),
                        ScheduleDate = c.DateTime(nullable: false),
                        ScheduleTimeSlot = c.Int(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        ReasonForVisit = c.String(),
                        Status = c.Int(nullable: false),
                        Report_ReportID = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Reports", t => t.Report_ReportID)
                .Index(t => t.Report_ReportID);
            
            CreateTable(
                "dbo.PetIssues",
                c => new
                    {
                        PetIssueID = c.Int(nullable: false, identity: true),
                        IssueName = c.String(),
                        AppointmentDetail_AppointmentID = c.Int(),
                    })
                .PrimaryKey(t => t.PetIssueID)
                .ForeignKey("dbo.AppointmentDetails", t => t.AppointmentDetail_AppointmentID)
                .Index(t => t.AppointmentDetail_AppointmentID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        HeartRate = c.Int(nullable: false),
                        Temperature = c.Single(nullable: false),
                        OxygenLevel = c.Single(nullable: false),
                        Comment = c.String(),
                        Prescription_PrescriptionID = c.Int(),
                    })
                .PrimaryKey(t => t.ReportID)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_PrescriptionID)
                .Index(t => t.Prescription_PrescriptionID);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        PrescriptionID = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.PrescriptionID);
            
            CreateTable(
                "dbo.PrescribedMedicines",
                c => new
                    {
                        PrescribedMedicineID = c.Int(nullable: false, identity: true),
                        NumberOfDays = c.Int(nullable: false),
                        Consume = c.Boolean(nullable: false),
                        Dosages = c.Int(nullable: false),
                        Medicine_MedicineID = c.Int(),
                        Prescription_PrescriptionID = c.Int(),
                    })
                .PrimaryKey(t => t.PrescribedMedicineID)
                .ForeignKey("dbo.Medicines", t => t.Medicine_MedicineID)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_PrescriptionID)
                .Index(t => t.Medicine_MedicineID)
                .Index(t => t.Prescription_PrescriptionID);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        MedicineID = c.Int(nullable: false, identity: true),
                        MedicineName = c.String(),
                    })
                .PrimaryKey(t => t.MedicineID);
            
            CreateTable(
                "dbo.Symptoms",
                c => new
                    {
                        SymptomID = c.Int(nullable: false, identity: true),
                        SymptomName = c.String(),
                        Report_ReportID = c.Int(),
                    })
                .PrimaryKey(t => t.SymptomID)
                .ForeignKey("dbo.Reports", t => t.Report_ReportID)
                .Index(t => t.Report_ReportID);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestID = c.Int(nullable: false, identity: true),
                        TestName = c.String(),
                        Report_ReportID = c.Int(),
                    })
                .PrimaryKey(t => t.TestID)
                .ForeignKey("dbo.Reports", t => t.Report_ReportID)
                .Index(t => t.Report_ReportID);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackID = c.Int(nullable: false, identity: true),
                        AppointmentID = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.FeedbackID);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingID = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        RatingValue = c.Int(nullable: false),
                        Feedback_FeedbackID = c.Int(),
                    })
                .PrimaryKey(t => t.RatingID)
                .ForeignKey("dbo.Feedbacks", t => t.Feedback_FeedbackID)
                .Index(t => t.Feedback_FeedbackID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "Feedback_FeedbackID", "dbo.Feedbacks");
            DropForeignKey("dbo.AppointmentDetails", "Report_ReportID", "dbo.Reports");
            DropForeignKey("dbo.Tests", "Report_ReportID", "dbo.Reports");
            DropForeignKey("dbo.Symptoms", "Report_ReportID", "dbo.Reports");
            DropForeignKey("dbo.Reports", "Prescription_PrescriptionID", "dbo.Prescriptions");
            DropForeignKey("dbo.PrescribedMedicines", "Prescription_PrescriptionID", "dbo.Prescriptions");
            DropForeignKey("dbo.PrescribedMedicines", "Medicine_MedicineID", "dbo.Medicines");
            DropForeignKey("dbo.PetIssues", "AppointmentDetail_AppointmentID", "dbo.AppointmentDetails");
            DropIndex("dbo.Ratings", new[] { "Feedback_FeedbackID" });
            DropIndex("dbo.Tests", new[] { "Report_ReportID" });
            DropIndex("dbo.Symptoms", new[] { "Report_ReportID" });
            DropIndex("dbo.PrescribedMedicines", new[] { "Prescription_PrescriptionID" });
            DropIndex("dbo.PrescribedMedicines", new[] { "Medicine_MedicineID" });
            DropIndex("dbo.Reports", new[] { "Prescription_PrescriptionID" });
            DropIndex("dbo.PetIssues", new[] { "AppointmentDetail_AppointmentID" });
            DropIndex("dbo.AppointmentDetails", new[] { "Report_ReportID" });
            DropTable("dbo.Ratings");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Tests");
            DropTable("dbo.Symptoms");
            DropTable("dbo.Medicines");
            DropTable("dbo.PrescribedMedicines");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Reports");
            DropTable("dbo.PetIssues");
            DropTable("dbo.AppointmentDetails");
        }
    }
}
