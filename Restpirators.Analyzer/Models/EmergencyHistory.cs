using Restpirators.Analyzer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Analyzer.Models
{
    public class EmergencyHistory
    {
        public int EmergencyId { get; set; }
        public int EmergencyTypeId { get; set; }
        public EmergencyStatus Status { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime ModDate { get; set; }
    }
}
