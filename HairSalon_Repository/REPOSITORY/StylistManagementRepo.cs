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
    public class StylistManagementRepo : IStylistManagementRepo
    {
        public StylistProfile GetStylistById(int stylistId)
            => StylistManagementDAO.Instance.GetStylistById(stylistId);

        public StylistProfile GetStylistByUserId(int userId)
            => StylistManagementDAO.Instance.GetStylistByUserId(userId);
        public List<StylistProfile> GetStylistList()
            => StylistManagementDAO.Instance.GetStylistList();

        public bool AddStylist(StylistProfile stylist)
            => StylistManagementDAO.Instance.AddStylist(stylist);

        public bool UpdateStylist(StylistProfile stylist)
            => StylistManagementDAO.Instance.UpdateStylist(stylist);

        public bool DeleteStylist(int stylistId)
            => StylistManagementDAO.Instance.DeleteStylist(stylistId);

    }
}
