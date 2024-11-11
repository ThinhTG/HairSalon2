using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IUserManagementRepo
    {
        public User GetUserById(int userId);

        public List<User> GetUserList();

        public bool AddUser(User user);

        public bool UpdateUser(User user);

        public bool DeleteUser(int userId);
    }
}
