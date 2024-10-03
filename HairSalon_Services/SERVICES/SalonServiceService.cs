using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICES
{
    public class SalonServiceService : ISalonServiceService
    {
        private ISalonServiceRepo servicelistRepo;
        public SalonServiceService()
        {
            servicelistRepo = new SalonServiceRepo();
        }
        public Service GetServiceById(string serviceId)
        {
            return servicelistRepo.GetServiceById(serviceId);
        }

        public List<Service> GetServiceList()
        {
            return servicelistRepo.GetServiceList();
        }
    }
}
