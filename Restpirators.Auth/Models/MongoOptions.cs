using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Auth.Models
{
    public class MongoOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
