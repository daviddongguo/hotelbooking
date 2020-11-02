using System.Collections.Generic;

namespace david.hotelbooking.api.SchedulerModels
{
    public class Resource  // for Each Room Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool expanded { get; set; } = true;
        public ICollection<Child> Children { get; set; }


    }

    public class Child  // for Each Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
