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
    public class UserManagementRepo : IUserManagementRepo
    {
        public User GetUserById(int userId)
            => UserManagementDAO.Instance.GetUserById(userId);

        public List<User> GetUserList()
            => UserManagementDAO.Instance.GetUserList();

        public bool AddUser(User user)
            => UserManagementDAO.Instance.AddUser(user);

        public bool UpdateUser(User user)
            => UserManagementDAO.Instance.UpdateUser(user);

        public bool DeleteUser(int userId)
            => UserManagementDAO.Instance.DeleteUser(userId);
    }
}
