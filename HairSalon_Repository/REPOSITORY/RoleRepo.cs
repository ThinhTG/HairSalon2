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
    public class RoleRepo : IRoleRepo
    {
        public Role GetRoleById(int roleId) => RoleDAO.Instance.GetRoleID(roleId);
        public List<Role> GetRoles() => RoleDAO.Instance.GetRoles();
    }
}
