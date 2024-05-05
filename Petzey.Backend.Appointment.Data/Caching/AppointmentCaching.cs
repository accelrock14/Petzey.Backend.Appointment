using Petzey.Backend.Appointment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Backend.Appointment.Data.Caching
{
    public class AppointmentCaching
    {
        private static readonly ObjectCache cache = MemoryCache.Default;
        private const string AppointmentsCacheKey = "Appointments";

        public static IQueryable<AppointmentDetail> GetAppointmentsFromCache()
        {
            return cache.Get(AppointmentsCacheKey) as IQueryable<AppointmentDetail>;
        }

        public static void CacheAppointments(IQueryable<AppointmentDetail> appointments)
        {
            cache.Set(AppointmentsCacheKey, appointments, DateTimeOffset.Now.AddMinutes(10));
        }

        public static void ClearAppointmentsCache()
        {
            cache.Remove(AppointmentsCacheKey);
        }
    }
}
