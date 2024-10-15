using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class BookingDAO
    {
        private HairSalonContext dbContext;

        private static BookingDAO instance = null;

        public static BookingDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookingDAO();
                }
                return instance;
            }
        }

        public BookingDAO()
        {
            dbContext = new HairSalonContext();
        }

        public Booking GetBookingById(int bookingId)
        {
            return dbContext.Booking.SingleOrDefault(b => b.BookingId == bookingId);
        }

        public bool AddBooking(Booking booking)
        {
            bool isSuccess = false;
            try
            {
                if (booking != null)
                {
                    dbContext.Booking.Add(booking);
                    dbContext.SaveChanges(); 
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // Log thông tin chi tiết về lỗi
                Console.WriteLine("Error while adding BookingDetail: " + ex.Message);

                // Nếu có InnerException, log thêm thông tin từ InnerException
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }

                throw; // Ném lại ngoại lệ để gọi ra ngoài có thể tiếp tục bắt lỗi
            }
            return isSuccess;
        }

        public bool UpdateBookingStatus(int bookingId, string newStatus)
        {
            bool isSuccess = false;
            try
            {
                var booking = dbContext.Booking.SingleOrDefault(b => b.BookingId == bookingId);

                if (booking != null)
                {
                    booking.Status = newStatus; 
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {         
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool SaveChanges()
        {
            try
            {
                dbContext.SaveChanges();  // Thực hiện lưu thay đổi vào database
                return true;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                throw new Exception("Error saving changes: " + ex.Message);
            }
        }
    }
}
