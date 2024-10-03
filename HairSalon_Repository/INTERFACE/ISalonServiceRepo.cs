using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface ISalonServiceRepo
    {
        public Service GetServiceById(string serviceId);
        public List<Service> GetServiceList();
        
    }
}
