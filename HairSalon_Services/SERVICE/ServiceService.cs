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
    public class ServiceService : IServiceService
    {
        private IServiceRepo servicelistRepo;
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
        public bool AddService(Service service)
        {
            return servicelistRepo.AddService(service);
        }
        public bool UpdateService(Service service)
        {
            return servicelistRepo.UpdateService(service);
        }
        public bool DeleteService(int serviceId)
        {
            return servicelistRepo.DeleteService(serviceId);
        }
    }
}
