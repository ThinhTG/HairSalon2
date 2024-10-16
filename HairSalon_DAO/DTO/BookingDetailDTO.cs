using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DTO
{
    public class BookingDetailDTO
    {
        public string ServiceName { get; set; }
        public string UserName { get; set; }
        public DateTime ScheduledWorkingDay { get; set; }
        public TimeOnly? StartTime { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
    }


}
