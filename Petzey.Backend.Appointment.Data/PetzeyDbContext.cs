using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
