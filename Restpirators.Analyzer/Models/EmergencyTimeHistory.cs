using Restpirators.Analyzer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Analyzer.Models
{
    public class EmergencyTimeHistory
    {
        public int EmergencyId { get; set; }
        public string EmergencyType { get; set; }
        public int EmergencyTypeId { get; set; }
        public EmergencyStatus Status { get; set; }
        public DateTime ModDate { get; set; }
    }
}
