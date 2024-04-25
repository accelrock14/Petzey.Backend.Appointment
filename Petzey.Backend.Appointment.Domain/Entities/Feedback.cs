﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Domain.Entities
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int AppointmentID { get; set; }
        //public Appointment Appointment { get; set; } // Navigation property for one-to-one
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        public string Comment { get; set; }
    }
}
