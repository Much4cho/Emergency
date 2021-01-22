using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restpirators.Auth.DataAccess
{
    public interface IUserRepository
    {
        User GetUser(string userName);
        void InsertUser(User user);
        IEnumerable<User> GetUsers();
    }
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _db;

        public UserRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public User GetUser(string username)
        {
            var col = _db.GetCollection<User>(User.DocumentName);
            var user = col.Find(u => u.Username == username).FirstOrDefault();
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            var col = _db.GetCollection<User>(User.DocumentName);
            return col.Find(x => true).ToList();
        }

        public void InsertUser(User user)
        {
            var col = _db.GetCollection<User>(User.DocumentName);
            col.InsertOne(user);
        }
    }
}
