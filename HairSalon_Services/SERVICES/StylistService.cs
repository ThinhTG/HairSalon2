using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICES
{
    public class StylistService : IStylistService
    {
        private IStylistRepo stylelistRepo;

        public StylistService()
        {
            stylelistRepo = new StylistRepo();
        }
        public Stylist GetStylistById(string stylistId)
        {
            return stylelistRepo.GetStylistById(stylistId);
        }

        public List<Stylist> GetStylists()
        {
            return stylelistRepo.GetStylists();
        }

    }
}
