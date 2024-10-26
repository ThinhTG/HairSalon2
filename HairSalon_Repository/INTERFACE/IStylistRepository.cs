using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository
{
    public interface IStylistRepository
    {
        public List<User> GetAllStylist();

        public decimal? GetSalaryByUserId(int stylistId);

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
