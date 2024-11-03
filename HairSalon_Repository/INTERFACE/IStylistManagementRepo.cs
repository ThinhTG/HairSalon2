using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IStylistManagementRepo
    {
        public StylistProfile GetStylistById(int stylistId);
        public StylistProfile GetStylistByUserId(int userId);

        public List<StylistProfile> GetStylistList();

        public bool AddStylist(StylistProfile stylist);

        public bool UpdateStylist(StylistProfile stylist);

        public bool DeleteStylist(int stylistId);
    }
}
