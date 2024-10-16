using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class BookingDetailDAO
    {
        private HairSalonContext dbContext;

        private static BookingDetailDAO instance = null;

        public static BookingDetailDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookingDetailDAO();
                }
                return instance;
            }
        }

        public BookingDetailDAO()
        {
            dbContext = new HairSalonContext();
        }

        public List<BookingDetail> GetBookingDetailsByBookingId(int bookingId)
        {
            return dbContext.BookingDetail.Where(b => b.BookingId == bookingId).ToList();
        }
        public BookingDetail GetBookingDetailById(int bookingDetailId)
        {
            return dbContext.BookingDetail.SingleOrDefault(b => b.BookingDetailId == bookingDetailId);
        }

        public bool AddBookingDetail(BookingDetail bookingDetail)
        {
            bool isSuccess = false;
            try
            {
                if (bookingDetail != null)
                {
                    dbContext.BookingDetail.Add(bookingDetail);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database update error: " + dbEx.Message);
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + dbEx.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error while adding BookingDetail: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
            }
            return isSuccess;
        }

        public bool UpdateBookingDetailStatus(int bookingDetailId, string newStatus)
        {
            bool isSuccess = false;
            try
            {
                var bookingdetail = dbContext.BookingDetail.SingleOrDefault(b => b.BookingDetailId == bookingDetailId);

                if (bookingdetail != null)
                {
                    bookingdetail.Status = newStatus;
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

        public List<BookingDetail> GetPendingBookingDetails()
        {
            return dbContext.BookingDetail
                             .Where(b => b.Status.Equals("Pending"))
                             .ToList();
        }

        public bool SaveChanges()
        {
            try
            {
                dbContext.SaveChanges(); 
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes: " + ex.Message);
            }
        }
    }
}
