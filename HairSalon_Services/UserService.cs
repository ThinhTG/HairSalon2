using HairSalon_BusinessObject.Models;
using HairSalon_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services
{
    public class UserService
    {
        private UserRepository UserRepository;

        public UserService() {
            UserRepository = new UserRepository();
        }
        public User GetUserByEmail(string email) {
            return UserRepository.GetUserByEmail(email);
        }

        public User GetUserByName(string username)
        {
            return UserRepository.GetUserByName(username);
        }

        public List<User> GetUsers()
        {
            return UserRepository.GetUsers();
        }

        public bool AddUser(User user)
        {
            return UserRepository.AddUser(user);
        }

    }
}
