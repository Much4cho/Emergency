﻿using Restpirators.Common.Enums;
using System;

namespace Restpirators.DataAccess.Entities
{
    public class Emergency : BaseEntity
    {
        public int EmergencyTypeId { get; set; }
        public EmergencyType EmergencyType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime ReportTime { get; set; }
        public EmergencyStatus Status { get; set; }
        public string ModUser { get; set; }
    }
}