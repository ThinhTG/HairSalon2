using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.ViewModel
{
    public class BookingDetailViewModel
    {
        public int BookingDetailId { get; set; }
        public int BookingId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ScheduledWorkingDay { get; set; }
        public int AvailableSlotId { get; set; }
        public decimal? Price { get; set; }
        public string Status { get; set; }

        public string UserName { get; set; }
        public TimeOnly? StartTime { get; set; }

        // Additional properties for User and Slot
        public UserViewModel User { get; set; }
        public SlotViewModel Slot { get; set; }

        public BookingDetailViewModel()
        {
            User = new UserViewModel();
            Slot = new SlotViewModel();
        }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
    }

    public class SlotViewModel
    {
        public TimeOnly? StartTime { get; set; }
    }
 
}




