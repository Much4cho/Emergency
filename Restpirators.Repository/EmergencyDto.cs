using System;
using System.Collections.Generic;
using System.Text;

namespace Restpirators.Repository
{
    public class EmergencyDto
    {
        public string Status { get; set; }
        public string TeamLocation { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public string ReportDate { get; set; }
        public string EmergencyType { get; set; }
        public string EmergencyLocation { get; set; }
    }
}
