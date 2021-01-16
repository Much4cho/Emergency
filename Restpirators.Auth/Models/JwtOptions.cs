using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Auth.Models
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public double ExpiryMinutes { get; set; }
    }
}
