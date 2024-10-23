using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_Repository.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class UserRepository : IUserRepository
    {
        public User GetUserByEmail(string email) => UserDAO.Instance.GetUserByEmail(email);

        public User GetUserByName(string username) => UserDAO.Instance.GetUserByName(username);

        public List<User> GetUsers() => UserDAO.Instance.GetUsers();

        public bool AddUser(User user) => UserDAO.Instance.AddUser(user);

        public User GetUserByUserId(int userId) => UserDAO.Instance.GetUserByUserId(userId);
       
    }
}
