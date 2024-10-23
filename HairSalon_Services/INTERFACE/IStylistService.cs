using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using HairSalon_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services
{
    public interface IStylistService
    {
        public List<User> GetAllStylist();

        public decimal? GetSalaryByUserId(int userId);

        public bool UpdateSalaryByUserId(int userId, decimal newSalary);

        public IEnumerable<dynamic> GetStylistServicesByUserId(int userId, DateTime? selectedDate);

        public bool InsertDailySalaryOfStylist(DateTime? selectedDate, int userId);

        public bool CheckIfSalaryExists(DateTime date, int userId);
    }
}
