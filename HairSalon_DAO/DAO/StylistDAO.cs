using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class StylistDAO
    {
        private HairSalonServiceContext _context;
        private static StylistDAO instance = null;

        public static StylistDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StylistDAO();
                }
                return instance;
            }
        }
        public StylistDAO()
        {
            _context = new HairSalonServiceContext();
        }



        public List<User> GetAllStylists()
        {
            return _context.User
                .Where(u => u.RoleId == 2)
                .Select(u => new User
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                })
                .ToList();
        }



        public decimal? GetSalaryByUserId(int userId)
        {
            return _context.StylistProfile
                           .Where(profile => profile.UserId == userId)
                           .Select(profile => profile.Salary)
                           .FirstOrDefault();
        }




        public bool UpdateSalaryByUserId(int userId, decimal newSalary)
        {
            bool isSuccess = false;
            try
            {
                // Tìm kiếm StylistProfile dựa trên UserId
                var stylistProfile = _context.StylistProfile
                    .FirstOrDefault(sp => sp.UserId == userId);
                // Nếu tìm thấy, cập nhật Salary
                if (stylistProfile != null)
                {
                    stylistProfile.Salary = newSalary;
                    _context.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý ngoại lệ
                throw new Exception("An error occurred while updating the salary: " + ex.Message);
            }
            return isSuccess;
        }



        public IEnumerable<dynamic> GetStylistServicesByUserId(int userId, DateTime? selectedDate)
        {
            // Lọc theo stylist và status
            var query = _context.BookingDetail
                                .Where(bd => bd.Status == "confirmed" && bd.AvailableSlot.UserId == userId);
            // Nếu có ngày đã chọn, thêm điều kiện lọc theo ngày
            if (selectedDate.HasValue)
            {
                query = query.Where(bd => bd.ScheduledWorkingDay.HasValue &&
                                           bd.ScheduledWorkingDay.Value.Date == selectedDate.Value.Date);
            }
            return query.Select(bd => new
            {
                StylistName = bd.AvailableSlot.User.UserName,
                Service = bd.Service.ServiceName,
                Price = bd.Price,
                ScheduledWorkingDay = bd.ScheduledWorkingDay,
                Status = bd.Status
            })
            .ToList();
        }




        public bool InsertDailySalaryOfStylist(DateTime? selectedDate, int userId)
        {
            bool isSuccess = false;
            try
            {
                if (!selectedDate.HasValue)
                {
                    throw new ArgumentNullException(nameof(selectedDate), "Selected date must not be null.");
                }

                // Check if the selected date is in the past
                if (selectedDate.Value.Date < DateTime.Now.Date)
                {
                    // Do not allow inserting for past dates
                    return false;
                }

                // Retrieve the corresponding StylistProfileId for the given userId
                var stylistProfileId = _context.StylistProfile
                    .Where(sp => sp.UserId == userId)
                    .Select(sp => sp.StylistProfileId)
                    .FirstOrDefault();

                // Calculate the daily salary for the stylist based on the selected date and userId
                var dailySalary = _context.BookingDetail
                    .Where(bd => bd.Status == "confirmed"
                              && bd.ScheduledWorkingDay == selectedDate.Value.Date
                              && bd.AvailableSlot.UserId == userId)
                    .Sum(bd => bd.Price * 0.05m) + _context.StylistProfile
                    .Where(sp => sp.UserId == userId)
                    .Select(sp => sp.Salary)
                    .FirstOrDefault();

                // Insert the calculated daily salary into DailySalaryOfStylist
                if (stylistProfileId != 0 && dailySalary > 0) // Ensure StylistProfileId exists (not 0) and dailySalary is valid
                {
                    var dailySalaryRecord = new DailySalaryOfStylist
                    {
                        Date = DateOnly.FromDateTime(selectedDate.Value), // Convert DateTime to DateOnly
                        DailySalary = dailySalary,
                        StylistProfileId = stylistProfileId, // Use the retrieved StylistProfileId directly
                    };

                    _context.DailySalaryOfStylist.Add(dailySalaryRecord);
                    _context.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as necessary
                throw new Exception("An error occurred while inserting daily salary: " + ex.Message);
            }
            return isSuccess;
        }



        public bool CheckIfSalaryExists(DateTime date, int userId)
        {
            // Lấy StylistProfileId dựa trên UserId
            var stylistProfileId = _context.StylistProfile
                                            .Where(sp => sp.UserId == userId)
                                            .Select(sp => sp.StylistProfileId)
                                            .FirstOrDefault();

            // Kiểm tra xem có bản ghi nào tồn tại trong DailySalaryOfStylist không
            return _context.DailySalaryOfStylist
                           .Any(d => d.Date == DateOnly.FromDateTime(date) && d.StylistProfileId == stylistProfileId);
        }
    }
}
