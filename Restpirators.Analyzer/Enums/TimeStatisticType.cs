using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Analyzer.Enums
{
    public enum TimeStatisticType
    {
        AcceptedToNew = 1,
        TeamSentToNew = 2,
        TeamSentToAccepted = 3,
        DoneToNew = 4,
        DoneToAccepted = 5,
        DoneToTeamSent = 6
    }
}
