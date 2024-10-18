using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class ServiceRepo
    {
        public Service GetServiceById(int serviceId)
            => ServiceDAO.Instance.GetServiceById(serviceId);

        public List<Service> GetServiceList()
            => ServiceDAO.Instance.GetServiceList();
    }
}
