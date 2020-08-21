using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Viewa.Models;

namespace Viewa.Models
{
    public class EventsResponse
    {
        public List<EventItem> Data { get; set; }
        public List<Chart> Charts { get; set; }
    }

    public class Chart
    {
        public string Name { get; set; }
        public List<string> Names { get; set; }
        public List<int> Values { get; set; }
    }
    public class Item
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }
}
