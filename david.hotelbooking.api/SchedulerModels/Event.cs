using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.api.SchedulerModels
{
    public class Event
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Resource { get; set; }
    }
}
