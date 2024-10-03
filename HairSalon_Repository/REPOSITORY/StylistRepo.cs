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
    public class StylistRepo : IStylistRepo
    {
        public Stylist GetStylistById(string stylistId)
            => StylistDAO.Instance.GetStylistById(stylistId);

        public List<Stylist> GetStylists()
            => StylistDAO.Instance.GetStylists();
    }
}
