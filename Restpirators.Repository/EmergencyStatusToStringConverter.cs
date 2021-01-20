using Restpirators.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restpirators.Repository
{
    public static class EmergencyStatusToStringConverter
    {
        public static string ConvertTo(EmergencyStatus status)
        {
            switch (status)
            {
                case EmergencyStatus.New:
                    return "Nowe";
                case EmergencyStatus.TeamSent:
                    return "Ekipa ratunkowa w drodze";
                case EmergencyStatus.Done:
                    return "Zakończone";
                case EmergencyStatus.Rejected:
                    return "Odrzucone";
            }
            return "";
        }
    }
}
