using ChannelIntegration.EfoodAPI.Schedules.Days.WeekDays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelIntegration.EfoodAPI.Schedules
{
    public class Schedule
    {
        public Monday[] Monday { get; set; }
        public Tuesday[] Tuesday { get; set; }
        public Wednesday[] Wednesday { get; set; }
        public Thursday[] Thursday { get; set; }
        public Friday[] Friday { get; set; }
        public Saturday[] Saturday { get; set; }
        public Sunday[] Sunday { get; set; }
    }
}
