using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Restpirators.Auth
{
    public class User
    {
        public static readonly string DocumentName = "users";

        public Guid Id { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }

        public void SetPassword(string password, Models.IEncryptor encryptor)
        {
            Salt = encryptor.GetSalt(password);
            Password = encryptor.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, Models.IEncryptor encryptor)
        {
            var isValid = Password.Equals(encryptor.GetHash(password, Salt));
            return isValid;
        }
    }
}
