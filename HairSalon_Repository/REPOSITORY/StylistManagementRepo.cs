using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class StylistManagementRepo
    {
        public StylistProfile GetStylistByUserId(int stylistId)
            => StylistManagementDAO.Instance.GetStylistByUserId(stylistId);

        public List<StylistProfile> GetStylistList()
            => StylistManagementDAO.Instance.GetStylistList();

        public bool AddStylist(StylistProfile stylist)
            => StylistManagementDAO.Instance.AddStylist(stylist);

        public bool UpdateStylist(StylistProfile stylist)
            => ServiceDAO.Instance.UpdateStylist(stylist);

        public bool DeleteStylist(int stylistId)
            => StylistManagementDAO.Instance.DeleteService(stylistId);

    }
}
