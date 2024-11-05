using HairSalon_BusinessObject.Models;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class ServiceService : IServiceService
    {
        private ServiceRepo servicelistRepo;
        public ServiceService()
        {
            servicelistRepo = new ServiceRepo();
        }
        public Service GetServiceById(int serviceId)
        {
            return servicelistRepo.GetServiceById(serviceId);
        }

        public List<Service> GetServiceList()
        {
            return servicelistRepo.GetServiceList();
        }

        public string GetServiceNameById(int serviceId)
        {
            return servicelistRepo.GetServiceNameById(serviceId);
        }
    }
}
