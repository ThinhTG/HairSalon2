using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.ViewModel
{
    public class BookingViewModel
    {
        public DateTime? BookingDate { get; set; }
        public decimal? Amount { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public decimal? Discount { get; set; }
        public int BookingId { get; set; }
        public bool IsBookingActive => Status != "Completed" &&
                                          Status != "Cancelled";
                                        
    }

}


