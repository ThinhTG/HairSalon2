using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository
{
    public interface IUserRepository
    {
        public User GetUserByEmail(string email);

        public User GetUserByName(string username);

        public List<User> GetUsers();

        public bool AddUser(User user);
    }
}
