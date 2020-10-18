using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.DataGenerator.Enums;

namespace Tests.DataGenerator.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public TeamStatus Status { get; set; }
    }
}
