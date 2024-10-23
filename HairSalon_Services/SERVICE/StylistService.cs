using HairSalon_BusinessObject.Models;
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

        public bool CheckIfSalaryExists(DateTime date, int userId)
        {
            return _stylistRepository.CheckIfSalaryExists(date, userId);
        }

        public List<User> GetAllStylist()
        {
            return _stylistRepository.GetAllStylist();
        }


        public decimal? GetSalaryByUserId(int stylistId)
        {
            return _stylistRepository.GetSalaryByUserId(stylistId);
        }



        public IEnumerable<dynamic> GetStylistServicesByUserId(int userId, DateTime? selectedDate)
        {
            return _stylistRepository.GetStylistServicesByUserId(userId, selectedDate);
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
