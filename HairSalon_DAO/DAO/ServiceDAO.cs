using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class ServiceDAO
    {
        private HairSalonContext dbContext;
        private static ServiceDAO instance;


        public ServiceDAO()
        {
            dbContext = new HairSalonContext();
        }

        public static ServiceDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceDAO();
                }
                return instance;
            }
        }

        public Service GetServiceById(int serviceId)
        {
            return dbContext.Service.SingleOrDefault(m => m.ServiceId == serviceId);
        }

        public List<Service> GetServiceList()
        {
            return dbContext.Service.ToList();
        }
    }
}
