using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IStylistRepo
    {
        public Service GetStylistById(int stylistId);

        public List<Service> GetStylistList();

        public Service AddStylist(StylistProfile stylist);

        public Service UpdateService(StylistProfile stylist);

        public Service DeleteService(StylistProfile stylist);
    }
}
