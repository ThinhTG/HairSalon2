using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using HairSalon_DAO.DAO;
using HairSalon_DAO.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository
{
    public class StylistRepository : IStylistRepository
    {
        public bool CheckIfSalaryExists(DateTime? selectedDate, int userId) => StylistDAO.Instance.CheckIfSalaryExists(selectedDate, userId);

        public List<User> GetAllStylist() => StylistDAO.Instance.GetAllStylists();

        public IEnumerable<DailySalaryOfStylist> GetStylistDailySalaryByUserId(int userId, DateTime? selectedDate) => StylistDAO.Instance.GetStylistDailySalaryByUserId(userId, selectedDate);

        public decimal? GetSalaryByUserId(int userId) => StylistDAO.Instance.GetSalaryByUserId((int)userId);


        public IEnumerable<dynamic> GetStylistServicesByUserId(int userId, DateTime? selectedDate) => StylistDAO.Instance.GetStylistServicesByUserId(userId, selectedDate);

        public bool InsertDailySalaryOfStylist(DateTime? selectedDate, int userId) => StylistDAO.Instance.InsertDailySalaryOfStylist((DateTime?)selectedDate, userId);  

        public bool UpdateSalaryByUserId(int userId, decimal newSalary) => StylistDAO.Instance.UpdateSalaryByUserId(((int)userId), newSalary);

        public List<StylistSalaryDTO> GetStylistsSalary(int userId, int month, int year) => StylistDAO.Instance.GetStylistsSalary((int)userId, month, year);

        public int GetTotalServices(int userId, int month, int year)=> StylistDAO.Instance.GetTotalServices((int)userId, month, year);

        public decimal GetTotalDailySalary(int userId, int month, int year) => StylistDAO.Instance.GetTotalDailySalary((int)userId, month, year);
    }
}
