using Restpirators.Common.Enums;
using System;

namespace Restpirator.Messaging
{
    public class EmergencyReport
    {
        public int Id { get; set; }
        public int EmergencyTypeId { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime ReportTime { get; set; }
        public EmergencyStatus Status { get; set; }
        public string ModUser { get; set; }
    }
}
