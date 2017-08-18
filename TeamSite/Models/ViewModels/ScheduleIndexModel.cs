using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamSite.Models.ViewModels
{
    public class ScheduleIndexModel
    {
        public IEnumerable<Schedule> Schedules { get; set; }
        public IEnumerable<Program> Programs { get; set; }
    }
}
