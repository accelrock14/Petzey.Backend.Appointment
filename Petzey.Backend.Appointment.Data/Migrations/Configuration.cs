namespace Petzey.Backend.Appointment.Data.Migrations
{
    using CsvHelper;
    using Petzey.Backend.Appointment.Domain.Entities;
    using Petzey.Backend.Appointment.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Petzey.Backend.Appointment.Data.PetzeyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Petzey.Backend.Appointment.Data.PetzeyDbContext context)
        {
            string migrationsDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            string csvFilesDirectoryPath = Path.Combine(migrationsDirectoryPath, "..", "CSV Files");

            string filePath = Path.Combine(csvFilesDirectoryPath, "tests.csv");

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Test>().ToList(); // Assuming Book is your model class
                context.Tests.AddRange(records); // Add parsed data to the context
                context.SaveChanges(); // Save changes to the database
            }



            string filePath2 = Path.Combine(csvFilesDirectoryPath, "medicines.csv");

            using (var reader = new StreamReader(filePath2))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Medicine>().ToList(); // Assuming Book is your model class
                context.Medicines.AddRange(records); // Add parsed data to the context
                context.SaveChanges(); // Save changes to the database
            }
            string filePath3 = Path.Combine(csvFilesDirectoryPath, "symptoms.csv");

            using (var reader = new StreamReader(filePath3))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Symptom>().ToList(); // Assuming Book is your model class
                context.Symptoms.AddRange(records); // Add parsed data to the context
                context.SaveChanges(); // Save changes to the database
            }

            string filePath4 = Path.Combine(csvFilesDirectoryPath, "petIssues.txt");

            using (var reader = new StreamReader(filePath4))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<GeneralPetIssue>().ToList(); // Assuming Book is your model class
                context.GeneralPetIssues.AddRange(records); // Add parsed data to the context
                context.SaveChanges(); // Save changes to the database
            }



        }
    }
}
