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
    public class BookingDetailDAO
    {
        private HairSalonServiceContext dbContext;

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
            dbContext = new HairSalonServiceContext();
        }

        public List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId)
        {
            var bookingDetails = (from bd in dbContext.BookingDetail
                                  join b in dbContext.Booking on bd.BookingId equals b.BookingId
                                  join avs in dbContext.AvailableSlot on bd.AvailableSlotId equals avs.AvailableSlotId
                                  join s in dbContext.Slot on avs.SlotId equals s.SlotId
                                  join ser in dbContext.Service on bd.ServiceId equals ser.ServiceId
                                  join u in dbContext.User on avs.UserId equals u.UserId
                                  where bd.BookingId == bookingId
                                  select new BookingDetailDTO
                                  {
                                      BookingDetailId = bd.BookingDetailId,
                                      UserName = u.UserName,
                                      ScheduledWorkingDay = bd.ScheduledWorkingDay ?? DateTime.MinValue,
                                      StartTime = s.StartTime,
                                      ServiceName = ser.ServiceName,
                                      Price = bd.Price ?? 0m,
                                      Status = bd.Status
                                  }).ToList();

            return bookingDetails;
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
                    var existingService = dbContext.Service.Find(bookingDetail.ServiceId);
                    var existingSlot = dbContext.AvailableSlot.Find(bookingDetail.AvailableSlotId);

                    if (existingService == null || existingSlot == null)
                    {
                        throw new InvalidOperationException("One or more related entities do not exist.");
                    }
                    bookingDetail.Service = existingService;
                    bookingDetail.AvailableSlot = existingSlot;

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
