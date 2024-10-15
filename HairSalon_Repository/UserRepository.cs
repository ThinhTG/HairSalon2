using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository
{
    public class UserRepository
    {
        public User GetUserByEmail(string email) => UserDAO.Instance.GetUserByEmail(email);

        public User GetUserByName(string username) => UserDAO.Instance.GetUserByName(username);

        public List<User> GetUsers() => UserDAO.Instance.GetUsers();

        public bool AddUser(User user) => UserDAO.Instance.AddUser(user);
    }
}
