using HairSalon_BusinessObject.Models;
using HairSalon_DAO;
using HairSalon_DAO.DAO;
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
        public bool CheckIfSalaryExists(DateTime date, int userId) => StylistDAO.Instance.CheckIfSalaryExists(date, userId);

        public List<User> GetAllStylist() => StylistDAO.Instance.GetAllStylists();

        public decimal? GetSalaryByUserId(int userId) => StylistDAO.Instance.GetSalaryByUserId((int)userId);


        public IEnumerable<dynamic> GetStylistServicesByUserId(int userId, DateTime? selectedDate) => StylistDAO.Instance.GetStylistServicesByUserId(userId, selectedDate);

        public bool InsertDailySalaryOfStylist(DateTime? selectedDate, int userId) => StylistDAO.Instance.InsertDailySalaryOfStylist((DateTime?)selectedDate, userId);  

        public bool UpdateSalaryByUserId(int userId, decimal newSalary) => StylistDAO.Instance.UpdateSalaryByUserId(((int)userId), newSalary);
    }
}
