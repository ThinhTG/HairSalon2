using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class ServiceDAO
    {
        private HairSalonServiceContext dbContext;
        private static ServiceDAO instance;


        public ServiceDAO()
        {
            dbContext = new HairSalonServiceContext();
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

        public string GetServiceNameById(int serviceId)
        {
            var service = dbContext.Service.FirstOrDefault(s => s.ServiceId == serviceId);
            return service != null ? service.ServiceName : "Unknown Service";
        }


        public List<Service> GetServiceList()
        {
            return dbContext.Service.ToList();
        }
    }
}
