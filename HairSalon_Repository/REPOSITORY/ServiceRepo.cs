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
    public class ServiceRepo : IServiceRepo
    {
        public Service GetServiceById(int serviceId)
            => ServiceDAO.Instance.GetServiceById(serviceId);

        public List<Service> GetServiceList()
            => ServiceDAO.Instance.GetServiceList();

        public bool AddService(Service service)
            => ServiceDAO.Instance.AddService(service);

        public bool UpdateService(Service service)
            => ServiceDAO.Instance.UpdateService(service);

        public bool DeleteService(int serviceId)
            => ServiceDAO.Instance.DeleteService(serviceId);

    }
}
