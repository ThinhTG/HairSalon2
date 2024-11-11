using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class RoleDAO
    {
        public HairSalonServiceContext dbContext;
        private static RoleDAO instance = null;
        public static RoleDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RoleDAO();
                }
                return instance;
            }
        }
        public RoleDAO()
        {
            dbContext = new HairSalonServiceContext();
        }
        public Role GetRoleID(int roleId)
        {
            return dbContext.Role.SingleOrDefault(m => m.RoleId.Equals(roleId));
        }
        public List<Role> GetRoles()
        {
            return dbContext.Role.ToList();
        }
    }
}
