using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
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
                                .Where(bd => bd.Status == "completed" && bd.AvailableSlot.UserId == userId);
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
                PriceReceive = bd.Price.HasValue ? bd.Price.Value * 0.1m : (decimal?)null,
                ScheduledWorkingDay = bd.ScheduledWorkingDay,
                Status = bd.Status
            })
            .ToList();
        }


        public IEnumerable<DailySalaryOfStylist> GetStylistDailySalaryByUserId(int userId, DateTime? selectedDate)
        {
            // Query to filter by stylist and date
            var query = _context.DailySalaryOfStylist
                                .Where(ds => ds.StylistProfile.UserId == userId);

            // If a specific date is selected, add a filter for the date
            if (selectedDate.HasValue)
            {
                query = query.Where(ds => ds.Date.Date == selectedDate.Value.Date);
            }

            // Select the required data
            return query.Select(ds => new DailySalaryOfStylist
            {
                DailySalary = ds.DailySalary,
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
                    .Where(bd => bd.Status == "completed"
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
                        Date = selectedDate.Value, // Convert DateTime to DateOnly
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




        public List<StylistSalaryDTO> GetStylistsSalary(int userId, int month, int year)
        {
            return (from u in _context.User 
                    join sp in _context.StylistProfile on u.UserId equals sp.UserId
                    join ds in _context.DailySalaryOfStylist on sp.StylistProfileId equals ds.StylistProfileId
                    where u.UserId == userId &&
                          ds.Date.Month == month && 
                          ds.Date.Year == year
                    select new StylistSalaryDTO
                    {
                        UserName = u.UserName,
                        Date = ds.Date,
                        DailySalary = ds.DailySalary ?? 0m
                    }).ToList();
        }


        public int GetTotalServices(int userId, int month, int year)
        {
            return (from bd in _context.BookingDetail // Bỏ AsNoTracking
                    join av in _context.AvailableSlot on bd.AvailableSlotId equals av.AvailableSlotId
                    join sp in _context.StylistProfile on av.UserId equals sp.UserId
                    join u in _context.User on sp.UserId equals u.UserId
                    where u.UserId == userId &&
                          bd.ScheduledWorkingDay.HasValue &&
                          bd.ScheduledWorkingDay.Value.Month == month &&
                          bd.ScheduledWorkingDay.Value.Year == year
                    select bd.ServiceId).Count();
        }

        public decimal GetTotalDailySalary(int userId, int month, int year)
        {
            return (from u in _context.User
                    join sp in _context.StylistProfile on u.UserId equals sp.UserId
                    join ds in _context.DailySalaryOfStylist on sp.StylistProfileId equals ds.StylistProfileId
                    where u.UserId == userId &&
                          ds.Date.Month == month &&
                          ds.Date.Year == year
                    select ds.DailySalary).Sum() ?? 0m; 
        }





        public bool CheckIfSalaryExists(DateTime? selectedDate, int userId)
        {
            // Lấy StylistProfileId dựa trên UserId
            var stylistProfileId = _context.StylistProfile
                                            .Where(sp => sp.UserId == userId)
                                            .Select(sp => sp.StylistProfileId)
                                            .FirstOrDefault();

            // Kiểm tra xem có bản ghi nào tồn tại trong DailySalaryOfStylist không
            return _context.DailySalaryOfStylist
                           .Any(d => d.Date == selectedDate && d.StylistProfileId == stylistProfileId);
        }

    }
}
