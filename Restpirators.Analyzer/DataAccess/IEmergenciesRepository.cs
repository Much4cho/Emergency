using Restpirators.Analyzer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restpirators.Analyzer.DataAccess
{
    public interface IEmergenciesRepository
    {
        IEnumerable<DtoQuantityStatistic> GetEmergencyQuantityStatistics(int? year, int? month);
        IEnumerable<DtoTimeStatistic> GetEmergencyTimeStatistics();
        void InsertEmergencyHistory(EmergencyHistory emergencyHistory);
    }
}
