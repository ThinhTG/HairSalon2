using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DTO
{
    // Tạo thư mục mới nếu cần: Services/DTOs hoặc DTOs
    public class StylistSalaryDTO
    {
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public decimal DailySalary { get; set; }
    }

}
