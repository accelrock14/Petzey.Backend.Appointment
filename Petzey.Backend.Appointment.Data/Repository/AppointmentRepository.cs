using Petzey.Backend.Appointment.Domain;
using Petzey.Backend.Appointment.Domain.DTO;
using Petzey.Backend.Appointment.Domain.Entities;
using Petzey.Backend.Appointment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace Petzey.Backend.Appointment.Data.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        PetzeyDbContext db = new PetzeyDbContext();

        

        
        //-------------------------------------------------
        // Get details of all appointments
        public IQueryable<AppointmentDetail> GetAppointmentDetails()
        {
            return db.AppointmentDetails;
        }

        // Get the details of appointment with the repective appointmentID
        public AppointmentDetail GetAppointmentDetail(int id)

        {
            //AppointmentCaching.ClearAppointmentsCache();
            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                
                return null;
            }

            return appointmentDetail;
        }

        public bool PutAppointmentDetail(int id, AppointmentDetail appointmentDetail)
        {

            //AppointmentCaching.ClearAppointmentsCache();
            int slot = appointmentDetail.ScheduleTimeSlot;

            int hoursToAdd = 9 + (slot * 30 / 60);
            int minutesToAdd = (slot * 30) % 60;

            // if it is the lunch break theen   
            if (slot >= 8)
            {
                hoursToAdd += 1;
            }

            appointmentDetail.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);

            if (slot < 0 || slot > 17)
            {
                return false;
            }



            if (id != appointmentDetail.AppointmentID)
            {
                return false;
            }

            var appointmentObj = db.AppointmentDetails.Include(a=>a.PetIssues).Include(a => a.Report).Where(ap=>ap.AppointmentID==id).FirstOrDefault();

            appointmentObj.DoctorID = appointmentDetail.DoctorID;
            appointmentObj.PetID= appointmentDetail.PetID;
            appointmentObj.OwnerID = appointmentDetail.OwnerID;
            appointmentObj.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);
            appointmentObj.ScheduleTimeSlot = appointmentDetail.ScheduleTimeSlot;
            appointmentObj.BookingDate = appointmentDetail.BookingDate;
            appointmentObj.ReasonForVisit = appointmentDetail.ReasonForVisit;
            appointmentObj.Status = appointmentDetail.Status;
            appointmentObj.Report= appointmentDetail.Report;
            appointmentObj.PetIssues= appointmentDetail.PetIssues;

            try
            {
                db.Entry(appointmentObj).State = EntityState.Modified;

            
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AppointmentDetailExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public bool PostAppointmentDetail(AppointmentDetail appointmentDetail)
        {
            //AppointmentCaching.ClearAppointmentsCache();

            int slot = appointmentDetail.ScheduleTimeSlot;

            int hoursToAdd = 9 + (slot * 30 / 60);
            int minutesToAdd = (slot * 30) % 60;

            // if it is the lunch break theen
            if (slot >= 8)
            {
                hoursToAdd += 1;
            }

            appointmentDetail.ScheduleDate = appointmentDetail.ScheduleDate.Date.AddHours(hoursToAdd).AddMinutes(minutesToAdd);

            if (slot < 0 || slot > 17)
            {
                return false;
            }
            
               
                db.AppointmentDetails.Add(appointmentDetail);
                db.SaveChanges();
            
          

            return true;
        }

        public bool DeleteAppointmentDetail(int id)
        {
            //AppointmentCaching.ClearAppointmentsCache();

            AppointmentDetail appointmentDetail = db.AppointmentDetails.Find(id);
            if (appointmentDetail == null)
            {
                return false;
            }

            db.AppointmentDetails.Remove(appointmentDetail);
            db.SaveChanges();

            return true;
        }

        public bool AppointmentDetailExists(int id)
        {
            return db.AppointmentDetails.Count(e => e.AppointmentID == id) > 0;
        }

        public IQueryable<GeneralPetIssue> GetAllGeneralPetIssues()
        {
            return db.GeneralPetIssues;
        }

        public bool PostGeneralPetIssue(GeneralPetIssue generalPetIssue)
        {

            db.GeneralPetIssues.Add(generalPetIssue);
            db.SaveChanges();

            //return CreatedAtRoute("DefaultApi", new { id = petIssue.PetIssueID }, petIssue);
            return true;
        }

        public List<AppointmentDetail> GetAppointmentsOfDocOnDate(string doctorId, DateTime date)
        {

            var dateOnly = date.Date;

            return db.AppointmentDetails
                .Where(a => a.DoctorID == doctorId && DbFunctions.TruncateTime(a.ScheduleDate) == dateOnly)
                .ToList();
        }

        public bool PatchAppointmentStatus(int id, Status status)
        {
            //AppointmentCaching.ClearAppointmentsCache();

            var appointment = db.AppointmentDetails.Find(id);
            if (appointment == null)
            {
                return false;
            }

            appointment.Status = status;
            if (status == Status.Closed)
            {
                Prescription prescription = new Prescription();
                Report report = new Report();
                prescription.PrescribedMedicines = new List<PrescribedMedicine>();
                
                db.Prescriptions.Add(prescription);
                report.Prescription = prescription;
                db.SaveChanges();
                db.Reports.Add(report);
                
                appointment.Report = report;
                

               
            }
            try
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AppointmentDetailExists(id))
                {

                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public List<bool> GetScheduledTimeSlotsBasedOnDocIDandDate(string doctorId, DateTime date)
        {

            List<bool> schedules = new List<bool>(18);
            for (int i = 0; i < 18; i++)
            {

                schedules.Add(false);
            }

            var dateOnly = date.Date;

            // Ensuring the date comparison includes only the date part, not the time part
            var scheuledTimeSlots = db.AppointmentDetails
               .Where(a => a.DoctorID == doctorId && DbFunctions.TruncateTime(a.ScheduleDate) == dateOnly)
               .Select(a => a.ScheduleTimeSlot);

            foreach (var item in scheuledTimeSlots)
            {
                schedules[item] = true;
            }

            return schedules;
        }


        public List<AppointmentCardDto> GetAllClosedAppointmentsByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).Select(appointment => new AppointmentCardDto
            {
                AppointmentID = appointment.AppointmentID,
                DoctorID = appointment.DoctorID,
                PetID = appointment.PetID,
                ScheduleDate = appointment.ScheduleDate
            }).ToList();
        }

        public List<AppointmentCardDto> GetAllClosedAppointmentsByVetID(string VetID)
        {
            return db.AppointmentDetails.Where(a => a.DoctorID == VetID && a.Status == Status.Closed).Select(appointment => new AppointmentCardDto
            {
                AppointmentID = appointment.AppointmentID,
                DoctorID = appointment.DoctorID,
                PetID = appointment.PetID,
                ScheduleDate = appointment.ScheduleDate
            }).ToList();
        }


        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||



        // get count of appointments in different statuses
        public AppointmentStatusCountsDto AppointmentStatusCounts(IDFiltersDto ids) 
        {
            AppointmentStatusCountsDto dto = new AppointmentStatusCountsDto();
            var allAppointments = db.AppointmentDetails.ToList(); 

            if(ids.OwnerID != null && ids.OwnerID != "")
            {
                allAppointments = allAppointments.Where(a => a.OwnerID == ids.OwnerID).ToList();
            }
            if (ids.DoctorID != null && ids.DoctorID != "")
            {
                allAppointments = allAppointments.Where(a => a.DoctorID == ids.DoctorID).ToList();
            }
            dto.Total = allAppointments.Count;
            dto.Closed = allAppointments.Count(a => a.Status == Domain.Entities.Status.Closed);
            dto.Pending = allAppointments.Count(a => a.Status == Domain.Entities.Status.Pending);
            dto.Cancelled = allAppointments.Count(a => a.Status == Domain.Entities.Status.Cancelled);
            dto.Confirmed = allAppointments.Count(a => a.Status == Domain.Entities.Status.Confirmed);

            return dto;
        }

        public List<AppointmentCardDto> GetAllAppointmentsWithFilters(FilterParamsDto filterParams)
        {
            IQueryable<AppointmentDetail> query = db.AppointmentDetails;

            // Filter by DoctorID if provided
            if (filterParams.DoctorID != null)
            {
                query = query.Where(appointment => appointment.DoctorID == filterParams.DoctorID);
            }

            // If no appointments found for the given doctor, return empty list
            if (!query.Any())
            {
                return new List<AppointmentCardDto>();
            }

            // Filter by ScheduleDate if provided
            if (filterParams.ScheduleDate != default(DateTime))
            {
                query = query.Where(appointment => DbFunctions.TruncateTime(appointment.ScheduleDate) == DbFunctions.TruncateTime(filterParams.ScheduleDate));
            }

            // Filter by Status if provided
            if (filterParams.Status != null)
            {
                query = query.Where(appointment => appointment.Status == filterParams.Status);
            }

            // Execute the query and map to AppointmentCardDto
            List<AppointmentCardDto> filteredAppointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate,
                    Status = appointment.Status.ToString()
                })
                // Sort appointments by status in the specified order
                .OrderBy(appointment => appointment.Status == "Pending" ? 0 :
                                appointment.Status == "Confirmed" ? 1 :
                                appointment.Status == "Cancelled" ? 2 : 3)
                .ToList();

            return filteredAppointments;
        }

        public List<AppointmentCardDto> UpcomingAppointments(IDFiltersDto ids)
        {
            DateTime today = DateTime.Today;

            var upcomingAppointments = db.AppointmentDetails
                .Where(a => a.Status == Status.Confirmed && a.ScheduleDate >= today)
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate,
                    Status = appointment.Status.ToString()
                })
                .ToList();

            if(ids.DoctorID != null && ids.DoctorID != "")
            {
                upcomingAppointments = upcomingAppointments.Where(a => a.DoctorID == ids.DoctorID).ToList();
            }
            if(ids.OwnerID != null && ids.OwnerID != "")
            {
                upcomingAppointments = upcomingAppointments.Where(a => a.OwnerID == ids.OwnerID).ToList();
            }

            return upcomingAppointments;
        }

        // get appointments for pet on a particular date
        public List<AppointmentCardDto> AppointmentByPetIdAndDate(int petId, DateTime date)
        {
            //IQueryable<AppointmentDetail> query = AppointmentCaching.GetAppointmentsFromCache();
            //if (query == null)
            //{
            //    query = db.AppointmentDetails;
            //    AppointmentCaching.CacheAppointments(query);
            //}
            //query = query.Where(appointment => appointment.PetID == petId);
            IQueryable<AppointmentDetail> query = db.AppointmentDetails.Where(appointment => appointment.PetID == petId);

            // If date is provided, filter appointments by date
            if (date != null)
            {
                query = query.Where(appointment => DbFunctions.TruncateTime(appointment.ScheduleDate) == DbFunctions.TruncateTime(date));
            }

            // Fetch appointments
            var appointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate,
                    Status = appointment.Status.ToString()
                })
                .ToList();

            return appointments;
        }

        public List<AppointmentCardDto> GetAppointmentsByOwnerIdWithFilters(FilterParamsDto filterParams, string ownerid)
        {
            // Execute the query to check if appointments exist for the given petid
            IQueryable<AppointmentDetail> query = db.AppointmentDetails.Where(appointment => appointment.OwnerID == ownerid);
            List<AppointmentDetail> appointments = query.ToList();

            // If no appointments found, return empty list
            if (appointments.Count == 0)
            {
                return new List<AppointmentCardDto>();
            }

            // Filter appointments based on filterParams
            query = query.AsQueryable();

            // Filter by ScheduleDate if provided
            if (filterParams.ScheduleDate != default(DateTime))
            {
                query = query.Where(appointment => DbFunctions.TruncateTime(appointment.ScheduleDate) == DbFunctions.TruncateTime(filterParams.ScheduleDate));
            }

            // Filter by Status if provided
            if (filterParams.Status != null)
            {
                query = query.Where(appointment => appointment.Status == filterParams.Status);
            }

            // Map to AppointmentCardDto
            List<AppointmentCardDto> filteredAppointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate,
                    Status = appointment.Status.ToString()
                })
                // Sort appointments by status in the specified order
                .OrderBy(appointment => appointment.Status == "Pending" ? 0 :
                                appointment.Status == "Confirmed" ? 1 :
                                appointment.Status == "Cancelled" ? 2 : 3)
                .ToList();

            return filteredAppointments;
        }


        public List<AppointmentCardDto> GetAppointmentsByVetIdWithFilters(FilterParamsDto filterParams, string vetid)
        {
            IQueryable<AppointmentDetail> query = db.AppointmentDetails.Where(appointment => appointment.DoctorID == vetid);
            List<AppointmentDetail> appointments = query.ToList();

            // If no appointments found, return empty list
            if (appointments.Count == 0)
            {
                return new List<AppointmentCardDto>();
            }

            // Filter appointments based on filterParams
            query = query.AsQueryable();

            // Filter by ScheduleDate if provided
            if (filterParams.ScheduleDate != default(DateTime))
            {
                query = query.Where(appointment => DbFunctions.TruncateTime(appointment.ScheduleDate) == DbFunctions.TruncateTime(filterParams.ScheduleDate));
            }

            // Filter by Status if provided
            if (filterParams.Status != null)
            {
                query = query.Where(appointment => appointment.Status == filterParams.Status);
            }

            // Map to AppointmentCardDto
            List<AppointmentCardDto> filteredAppointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate,
                    Status = appointment.Status.ToString()
                })
                // Sort appointments by status in the specified order
                .OrderBy(appointment => appointment.Status == "Pending" ? 0 :
                                appointment.Status == "Confirmed" ? 1 :
                                appointment.Status == "Cancelled" ? 2 : 3)
                .ToList();

            return filteredAppointments;
        }


        // get all appointments for a pet
        public List<AppointmentCardDto> AppointmentByPetId(int petId)
        {
            IQueryable<AppointmentDetail> query = db.AppointmentDetails.Where(appointment => appointment.PetID == petId);
            var appointments = query
                .Select(appointment => new AppointmentCardDto
                {
                    AppointmentID = appointment.AppointmentID,
                    DoctorID = appointment.DoctorID,
                    PetID = appointment.PetID,
                    ScheduleDate = appointment.ScheduleDate,
                    Status = appointment.Status.ToString()
                })
                .ToList();

            return appointments;
        }

        // add new report
        public void AddReport(Report report)
        {
            db.Reports.Add(report);
            db.SaveChanges();
        }

        // edit contents of a report
        public void EditReport(Report report)
        {
            foreach (PrescribedMedicine medicine in report.Prescription.PrescribedMedicines)
            {
                // edit contents of prescription
                db.Entry(medicine.Medicine).State = System.Data.Entity.EntityState.Modified;
                db.Entry(medicine).State = System.Data.Entity.EntityState.Modified;
            }
            db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            
            db.SaveChanges();
        }

        // get all medicines
        public List<Medicine> GetAllMedicines()
        {
            return db.Medicines.ToList();
        }

        // get all symptoms
        public IEnumerable<Symptom> GetAllSymptoms()
        {
            return db.Symptoms.Distinct();
        }

        // get all tests
        public IEnumerable<Test> GetAllTests()
        {
            return db.Tests.Distinct();
        }

        // get list of top most recent appointment of a pet using petID
        public List<AppointmentDetail> GetRecentAppointmentsByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).OrderByDescending(a => a.ScheduleDate).Take(10).ToList();

        }

        // get report detial by reportID
        public Report GetReportByID(int id)
        {
            return db.Reports.Find(id);
        }

        // get all past prescriptions of a pet using petID
        public List<Prescription> GetHistoryOfPrescriptionsByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).Select(a => a.Report.Prescription).ToList();

        }

        // get the most recent appointment of a pet using petID
        public AppointmentDetail MostRecentAppointmentByPetID(int PetID)
        {
            return db.AppointmentDetails.Where(a => a.PetID == PetID && a.Status == Status.Closed).OrderByDescending(a => a.ScheduleDate).FirstOrDefault();

        }

        // get medicine details by using medicineID
        public Medicine GetMedicineById(int medicineId)
        {
            return db.Medicines.Find(medicineId);
        }

        // get detials of prescribed medicine by using prescribedMedicineID
        public PrescribedMedicine GetPrescribedMedicine(int medicineId)
        {
            return db.PrescribedMedics.Find(medicineId);
        }

        // add a new prescribedMedicine to presription having prescriptionID
        public void AddMedicineToPrescription(int prescriptionId, PrescribedMedicine medicine)
        {
            db.Prescriptions.Find(prescriptionId).PrescribedMedicines.Add(medicine);
            db.SaveChanges();
        }

        // remove prescribedMedicine that has prescribedMedicineID from prescription
        public void RemoveMedicineFromPrescription(int prescriptionId)
        {
            db.PrescribedMedics.Remove(db.PrescribedMedics.Find(prescriptionId));
            db.SaveChanges();
        }

        // add a symptom to report
        public void AddSymptomToReport(int reportID, ReportSymptom reportSymptom)
        {

            db.Reports.Find(reportID).Symptoms.Add(reportSymptom);
            db.SaveChanges();
        }
        
        // remove a symptom from report
        public void DeleteSymptomFromReport(int reportsymptomID){

            db.ReportSymptoms.Remove(db.ReportSymptoms.Find(reportsymptomID));
            db.SaveChanges();

         }

        // add a test to report
        public void AddTestToReport(int reportID, ReportTest reportTest)
        {

            db.Reports.Find(reportID).Tests.Add(reportTest);
            db.SaveChanges();
        }

        // remove a test from a report
        public void DeleteTestFromReport(int reportTestID)
        {

            db.ReportTests.Remove(db.ReportTests.Find(reportTestID));
            db.SaveChanges();
        }

        // add a doctor recommendation
        public void AddDoctorRecommendation(int reportID, RecommendedDoctor recommendedDoctor)
        {
            db.Reports.Find(reportID).RecommendedDoctors.Add(recommendedDoctor);
            db.SaveChanges();
        }

        // remove a doctor recommendation
        public void RemoveDoctorRecommendation(int recommendedDoctorID)
        {
            db.RecommendedDoctors.Remove(db.RecommendedDoctors.Find(recommendedDoctorID));
            db.SaveChanges();
        }

        // update report details
        public void UpdateReportStatus(Report oldReport, Report newReport)
        {
            int id = oldReport.ReportID;
            oldReport.HeartRate = newReport.HeartRate;
            oldReport.Temperature = newReport.Temperature;
            oldReport.OxygenLevel = newReport.OxygenLevel;
            oldReport.Comment = newReport.Comment;

            UpdateSymptoms(id,oldReport.Symptoms, newReport.Symptoms);
            UpdateTests(id, oldReport.Tests, newReport.Tests);
            UpdateRecommendation(id,oldReport.RecommendedDoctors,newReport.RecommendedDoctors);
            db.SaveChanges ();
        }

        // update symptoms in report with reportID
        public void UpdateSymptoms(int id,List<ReportSymptom> oldSymptoms, List<ReportSymptom> newSymptoms)
        {
            // keep track of deleted symptoms
            List<ReportSymptom> deletedSymptoms = new List<ReportSymptom>();
            foreach(ReportSymptom symptom in oldSymptoms)
            {
                // get the symptoms where were removed and add them to list
                if (!newSymptoms.Select(s => s.SymptomID).Contains(symptom.SymptomID))
                {
                    //DeleteSymptomFromReport(oldSymptoms[i].ReportSymptomID);
                    deletedSymptoms.Add(symptom);
                }
            }
            foreach (ReportSymptom symptom in newSymptoms)
            {
                // add the new symptoms into the report
                if (!oldSymptoms.Select(s => s.SymptomID).Contains(symptom.SymptomID))
                {
                    AddSymptomToReport(id, symptom);
                }
            }
            // delete the symptoms in the deleted symptoms list
            foreach(ReportSymptom symptom in deletedSymptoms)
            {
                DeleteSymptomFromReport(symptom.ReportSymptomID);
            }
        }

        // update the tests in the report with reportID
        public void UpdateTests(int id, List<ReportTest> oldTests, List<ReportTest> newTests)
        { 
            // keep track with deleted tests
            List<ReportTest> deletedTests = new List<ReportTest>();
            foreach(ReportTest test in oldTests)
            {
                // add the delted tests into the list
                if (!newTests.Select(t=>t.TestID).Contains(test.TestID))
                {
                    //DeleteTestFromReport(oldTests[i].ReportTestID);
                    deletedTests.Add(test);
                }
            }
            foreach (ReportTest test in newTests)
            {
                // add the new tests into the report
                if (!oldTests.Select(t=>t.TestID).Contains(test.TestID))
                {
                    AddTestToReport(id, test);
                }
            }
            // delete the list of tests from the report
            foreach(ReportTest test in deletedTests)
            {
                DeleteTestFromReport(test.ReportTestID);
            }
        }

        // update the recommended doctors in the report
        public void UpdateRecommendation(int id, List<RecommendedDoctor> oldDoctors, List<RecommendedDoctor> newDoctors)
        {
            // keep track of deleted doctor recommmendations
            List<RecommendedDoctor> deletedDoctors = new List<RecommendedDoctor>();
            foreach(RecommendedDoctor doctor in oldDoctors)
            {
                // add the deleted doctor recommendations to the list
                if (!newDoctors.Select(r => r.DoctorID).Contains(doctor.DoctorID))
                {
                    //DeleteTestFromReport(oldDoctors[i].ID);
                    deletedDoctors.Add(doctor);
                }
            }
            foreach (RecommendedDoctor doctor in newDoctors)
            {
                // add the new doctor recommendations to the report
                if (!oldDoctors.Select(r => r.DoctorID).Contains(doctor.DoctorID))
                {
                    AddDoctorRecommendation(id, doctor);
                }
            }
            // delete the doctor recommendations from the report
            foreach (RecommendedDoctor doctor in deletedDoctors)
            {
                db.RecommendedDoctors.Remove(doctor);
            }
        }

        // get the prescribedmedicine with the prescribedMedicineID
        public PrescribedMedicine GetPrescribed(int id)
        {
            return db.PrescribedMedics.Find(id);
        }

        // update the details of a prescribedmedicine
        public void UpdateMedicine(PrescribedMedicine oldPrescription, PrescribedMedicine newPrescription)
        {
            oldPrescription.MedicineID = newPrescription.MedicineID;
            oldPrescription.NumberOfDays = newPrescription.NumberOfDays;
            oldPrescription.Dosages = newPrescription.Dosages;
            oldPrescription.Consume =  newPrescription.Consume;
            oldPrescription.Comment = newPrescription.Comment;
            db.SaveChanges();
        }



        /////////////////////////////// feedback
        ///
        public List<Feedback> getAllFeedbacks()
        {
            
            return db.Feedbacks.ToList();
        }
        public Feedback getFeedbackByAppointmrntId(int id)
        {
           
            Feedback feedback = db.Feedbacks.Where(f => f.AppointmentId == id).FirstOrDefault();
            return feedback;
        }
        public bool Addfeedback(Feedback feedback)
        {
            db.Feedbacks.Add(feedback);
            db.SaveChanges();
            return true;
        }

        //change!
        public List<AppointmentCardDto> GetAllAppointments()
        {
            var appointments = db.AppointmentDetails.ToList();

            var appointmentDtos = appointments.Select(appointment => new AppointmentCardDto
            {
                AppointmentID = appointment.AppointmentID,
                DoctorID = appointment.DoctorID,
                PetID = appointment.PetID,
                ScheduleDate = appointment.ScheduleDate,
                Status = appointment.Status.ToString()
            }).ToList();

            return appointmentDtos;

        }
        public List<AppointmentCardDto> GetAppointmentsByOwnerId(string ownerid)
        {
            var appointments = db.AppointmentDetails.Where(a => a.OwnerID == ownerid);

            var appointmentDtos = appointments.Select(appointment => new AppointmentCardDto
            {
                AppointmentID = appointment.AppointmentID,
                DoctorID = appointment.DoctorID,
                PetID = appointment.PetID,
                ScheduleDate = appointment.ScheduleDate,
                Status = appointment.Status.ToString()
            }).ToList();

            return appointmentDtos;
        }
        public List<AppointmentCardDto> GetAppointmentsByVetId(string vetid)
        {
            var appointments = db.AppointmentDetails.Where(a => a.DoctorID == vetid);

            var appointmentDtos = appointments.Select(appointment => new AppointmentCardDto
            {
                AppointmentID = appointment.AppointmentID,
                DoctorID = appointment.DoctorID,
                PetID = appointment.PetID,
                ScheduleDate = appointment.ScheduleDate,
                Status = appointment.Status.ToString()
            }).ToList();

            return appointmentDtos;
        }


        List<AppointmentDetail> IAppointmentRepository.GetAppointmentsOfDoctor(string docId)
        {
            return db.AppointmentDetails.Where(a=>a.DoctorID==docId).ToList();
        }


        public List<FeedbackQuestion> getfeedbackquestion()
        {
            var questions = db.FeedbackQuestions.ToList();
            return questions;
        }
        public FeedbackQuestion getfeedbackquestionbyid(int id)
        {
            FeedbackQuestion feedbackQuestion = db.FeedbackQuestions.Find(id);
            return feedbackQuestion;
        }
        public void updatefeedbackquestion(int id, FeedbackQuestion feedbackQuestion)
        {
            db.Entry(feedbackQuestion).State = EntityState.Modified;
            db.SaveChanges();

        }
        public void deletefeedbackquestion(int id)
        {
            FeedbackQuestion feedbackQuestion = db.FeedbackQuestions.Find(id);
            db.FeedbackQuestions.Remove(feedbackQuestion);
            db.SaveChanges();
        }
        public void Addfeedbackquestion(FeedbackQuestion feedbackQuestion)
        {
            db.FeedbackQuestions.Add(feedbackQuestion);
            db.SaveChanges();
        }
        public bool checkfeedbackquestion(int id)
        {
            return db.FeedbackQuestions.Count(e => e.FeedbackQuestionId == id) > 0;
        }
        public List<int> GetAllPetIDByVetId(string vetId)
        {
          // var appointments= db.AppointmentDetails.Where(a=>a.DoctorID== vetId).ToList();
            return
                db.AppointmentDetails
               .Where(a => a.DoctorID == vetId)
               .Select(a => a.PetID)
               .ToList();

        }

    }
}
