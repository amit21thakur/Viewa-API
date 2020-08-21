using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viewa.Models
{
    public class EventItem
    {
        public decimal? Id { get; set; }
        public string CampaignName { get; set; }
        public string EventType { get; set; }
        public string AppUserId { get; set; }
        public string AppUserGender { get; set; }
        public DateTime? EventDate { get; set; }
        public string AppDeviceType { get; set; }
    }
}
