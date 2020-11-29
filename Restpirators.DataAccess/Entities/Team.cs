using System.Collections.Generic;

namespace Restpirators.DataAccess.Entities
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public virtual ICollection<Emergency> AssignedEmergencies { get; set; }
    }
}
