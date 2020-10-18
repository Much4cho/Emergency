using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DataGenerator.Models
{
    public class Emergency_Team
    {
        public int Id { get; set; }
        public int EmergencyId { get; set; }
        public Emergency Emergency { get; set; }
        public Team Team { get; set; }
    }
}
