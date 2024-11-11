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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo roleRepo;
        public RoleService()
        {
            roleRepo = new RoleRepo();
        }
        public Role GetRoleById(int roleId)
        {
            return roleRepo.GetRoleById(roleId);
        }

        public List<Role> GetRoles()
        {
            return roleRepo.GetRoles();
        }
    }
}
