using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class SalonServiceDAO
    {
        private HairSalonContext dbContext;
        private static SalonServiceDAO instance;


        public SalonServiceDAO()
        {
            dbContext = new HairSalonContext();
        }

        public static SalonServiceDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SalonServiceDAO();
                }
                return instance;
            }
        }

        public Service GetServiceById(string serviceId)
        {
            return dbContext.Service.SingleOrDefault(m => m.ServiceId.Equals(serviceId));
        }

        public List<Service> GetServiceList()
        {
            return dbContext.Service.ToList();
        }
    }
}

