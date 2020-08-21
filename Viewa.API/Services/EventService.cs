using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viewa.Db;
using Viewa.Models;

namespace Viewa.Services
{
    public class EventService : IEventService
    {

        private readonly ViewaContext _context;
        private readonly IMapper _mapper;

        public EventService(ViewaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EventsResponse> GetEvents(DateTime? startDate, DateTime? endDate, string eventType, string gender, string deviceType)
        {
            var events = _context.SampleData.Where(x => true);
            if (startDate.HasValue)
            {
                events = events.Where(x => x.EventDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                events = events.Where(x => x.EventDate <= endDate.Value);
            }
            if (!string.IsNullOrEmpty(eventType))
            {
                events = events.Where(x => x.EventType.ToLower() == eventType.ToLower());
            }
            if (!string.IsNullOrEmpty(gender))
            {
                events = events.Where(x => x.AppUserGender.ToLower() == gender.ToLower());
            }
            if (!string.IsNullOrEmpty(deviceType))
            {
                events = events.Where(x => x.AppDeviceType.ToLower() == deviceType.ToLower());
            }

            var sampleData = await events.ToListAsync();
            var charts = new List<Chart>();

            var deviceTypeGroupedData = sampleData.GroupBy(x => x.AppDeviceType,
                    (typeOfDevice, groupedEvents) => new Item
                    {
                        Key = typeOfDevice,
                        Count = groupedEvents.Count()
                    }).ToList();
            charts.Add(GetChart("deviceType", deviceTypeGroupedData));

            var genderGroupedData = sampleData.GroupBy(x => x.AppUserGender,
                    (sex, groupedEvents) => new Item
                    {
                        Key = sex,
                        Count = groupedEvents.Count()
                    }).ToList();
            charts.Add(GetChart("gender", genderGroupedData));

            var eventDateGroup = sampleData.GroupBy(x => x.EventDate.Value.ToString("dd-MM"),
                    (date, groupedEvents) => new Item
                    {
                        Key = date,
                        Count = groupedEvents.Count()
                    }).ToList();
            charts.Add(GetChart("eventDate", eventDateGroup));

            var eventsResponse = new EventsResponse()
            {
                Data = _mapper.Map<List<EventItem>>(sampleData),
                Charts = charts
            };
            return eventsResponse;
        }

        private Chart GetChart(string name, List<Item> items)
        {
            var chart = new Chart
            {
                Name = name,
                Names = new List<string>(),
                Values = new List<int>()
            };
            foreach (var item in items)
            {
                chart.Names.Add(item.Key);
                chart.Values.Add(item.Count);
            }
            return chart;
        }
    }
}
