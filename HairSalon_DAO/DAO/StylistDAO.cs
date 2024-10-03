using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class StylistDAO
    {

        private HairSalonContext dbContext;
        private static StylistDAO instance;


        public StylistDAO()
        {
            dbContext = new HairSalonContext();
        }

        public static StylistDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StylistDAO();
                }
                return instance;
            }
        }

        public Stylist GetStylistById(string stylistId)
        {
            return dbContext.Stylist.SingleOrDefault(m => m.StylistId.Equals(stylistId));
        }

        public List<Stylist> GetStylists()
        {
            return dbContext.Stylist.ToList();
        }
    }
}
