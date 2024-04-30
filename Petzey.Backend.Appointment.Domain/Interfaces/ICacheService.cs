using System;

namespace Petzey.Backend.Appointment.Domain.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan expiry);
    }
}
