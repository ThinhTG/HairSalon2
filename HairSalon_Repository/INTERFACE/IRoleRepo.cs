using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IRoleRepo
    {
        public Role GetRoleById(int roleId);
        public List<Role> GetRoles();
    }
}
