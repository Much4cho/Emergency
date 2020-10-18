using Restpirators.Analyzer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restpirators.Analyzer.Models
{
    public class DtoTimeStatistic
    {
        public TimeStatisticType StatisticType { get; set; }
        public TimeSpan TimeAverage { get; set; }
        public string EmergencyType { get; set; }
    }
}
