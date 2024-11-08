using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class UserManagementService : IUserManagementService
    {
        private IUserManagementRepo userlistRepo;
        public UserManagementService()
        {
            userlistRepo = new UserManagementRepo();
        }

        public bool AddUser(User user)
        {
            return userlistRepo.AddUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return userlistRepo.DeleteUser(userId);
        }

        public User GetUserById(int userId)
        {
            return userlistRepo.GetUserById(userId);
        }

        public List<User> GetUserList()
        {
            return userlistRepo.GetUserList();
        }

        public bool UpdateUser(User user)
        {
            return userlistRepo.UpdateUser(user);
        }
    }
}
