using HairSalon_BusinessObject.Models;
using HairSalon_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository iuserRepository;

        public UserService() {
            iuserRepository = new UserRepository();
        }
        public User GetUserByEmail(string email) {
            return iuserRepository.GetUserByEmail(email);
        }

        public User GetUserByName(string username)
        {
            return iuserRepository.GetUserByName(username);
        }

        public List<User> GetUsers()
        {
            return iuserRepository.GetUsers();
        }

        public bool AddUser(User user)
        {
            return iuserRepository.AddUser(user);
        }

    }
}
