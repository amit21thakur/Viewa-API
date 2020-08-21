using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viewa.Models;

namespace Viewa.Services
{
    public interface IEventService
    {
        Task<EventsResponse> GetEvents(DateTime? startDate, DateTime? endDate, string eventType, string gender, string deviceType);
    }
}
