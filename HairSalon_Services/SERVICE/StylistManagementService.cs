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
    public class StylistManagementService : IStylistManagementService
    {
        private IStylistManagementRepo stylistlistRepo;
        public StylistManagementService()
        {
            stylistlistRepo = new StylistManagementRepo();
        }
        public StylistProfile GetStylistById(int stylistId)
        {
            return stylistlistRepo.GetStylistById(stylistId);
        }
        public StylistProfile GetStylistByUserId(int userId)
        {
            return stylistlistRepo.GetStylistByUserId(userId);
        }
        public List<StylistProfile> GetStylistList()
        {
            return stylistlistRepo.GetStylistList();
        }
        public bool AddStylist(StylistProfile stylist)
        {
            return stylistlistRepo.AddStylist(stylist);
        }
        public bool UpdateStylist(StylistProfile stylist)
        {
            return stylistlistRepo.UpdateStylist(stylist);
        }
        public bool DeleteStylist(int stylistId)
        {
            return stylistlistRepo.DeleteStylist(stylistId);
        }
    }
}
