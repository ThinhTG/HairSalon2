using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
using HairSalon_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services
{
    public class StylistService : IStylistService
    {

        private StylistRepository _stylistRepository;

        public StylistService()
        {
            _stylistRepository = new StylistRepository();
        }

        public bool CheckIfSalaryExists(DateTime? selectedDate, int userId)
        {
            return _stylistRepository.CheckIfSalaryExists(selectedDate, userId);
        }

        public List<User> GetAllStylist()
        {
            return _stylistRepository.GetAllStylist();
        }


        public decimal? GetSalaryByUserId(int stylistId)
        {
            return _stylistRepository.GetSalaryByUserId(stylistId);
        }

        public IEnumerable<DailySalaryOfStylist> GetStylistDailySalaryByUserId(int userId, DateTime? selectedDate)
        {
           return _stylistRepository.GetStylistDailySalaryByUserId(userId, selectedDate);
        }

        public IEnumerable<dynamic> GetStylistServicesByUserId(int userId, DateTime? selectedDate)
        {
            return _stylistRepository.GetStylistServicesByUserId(userId, selectedDate);
        }

        public List<StylistSalaryDTO> GetStylistsSalary(int userId, int month, int year)
        {
            return _stylistRepository.GetStylistsSalary(userId, month, year);
        }

        public decimal GetTotalDailySalary(int userId, int month, int year)
        {
            return _stylistRepository.GetTotalDailySalary(userId,month, year);
        }

        public int GetTotalServices(int userId, int month, int year)
        {
           return _stylistRepository.GetTotalServices(userId, month, year);
        }

        public bool InsertDailySalaryOfStylist(DateTime? selectedDate, int userId)
        {
           return _stylistRepository.InsertDailySalaryOfStylist(selectedDate, userId);
        }

        public bool UpdateSalaryByUserId(int userId, decimal newSalary)
        {
            return _stylistRepository.UpdateSalaryByUserId(userId, newSalary);
        }
    }
}
