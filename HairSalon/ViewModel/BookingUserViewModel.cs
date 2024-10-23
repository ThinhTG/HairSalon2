using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.ViewModel
{
    public class BookingUserViewModel
    {
        public int BookingId { get; set; }
        public string Status { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public int? CreatedBy { get; set; }

        // Thông tin từ bảng User
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }

}
