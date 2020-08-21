using System;
using System.Collections.Generic;

namespace Viewa.Db
{
    public partial class SampleData
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
