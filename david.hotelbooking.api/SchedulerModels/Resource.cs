using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.api.SchedulerModels
{
    public class Resource  // for Each Room Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool expanded { get; set; } = true;
        public ICollection<Child> Children { get; set; }


    }

    public class Child  // for Each Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
