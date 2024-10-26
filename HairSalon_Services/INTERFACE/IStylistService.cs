using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using HairSalon_DAO.DTO;
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

        public bool CheckIfSalaryExists(DateTime? selectedDate, int userId);

        public IEnumerable<DailySalaryOfStylist> GetStylistDailySalaryByUserId(int userId, DateTime? selectedDate);

        public List<StylistSalaryDTO> GetStylistsSalary(int userId, int month, int year);

        public int GetTotalServices(int userId, int month, int year);

        public decimal GetTotalDailySalary(int userId, int month, int year);
    }
}
