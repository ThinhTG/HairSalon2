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
    public class SalonServiceRepo : ISalonServiceRepo
    {
        public Service GetServiceById(string serviceId)
            => SalonServiceDAO.Instance.GetServiceById(serviceId);

        public List<Service> GetServiceList()
            => SalonServiceDAO.Instance.GetServiceList();
    }
}
