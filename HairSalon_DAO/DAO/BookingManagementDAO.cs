using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class BookingManagementDAO
    {
        private HairSalonServiceContext context;
        private static BookingManagementDAO instance;

        public BookingManagementDAO()
        {
            context = new HairSalonServiceContext();
        }

        public static BookingManagementDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookingManagementDAO();
                }
                return instance;
            }
        }

    }
}
