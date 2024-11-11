using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IBookingDetailRepo
    {
        bool AddBookingDetail(BookingDetail bookingDetail);
        bool UpdateBookingDetailStatus(int bookingDetailId, string newStatus);
        List<BookingDetail> GetPendingBookingDetails();
        bool SaveChanges();
        bool AreAllBookingDetailsCompleted(int bookingId);






        public List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId);
        public List<BookingDetail> GetBookingDetailByBookingId(int bookingId);
        public BookingDetail GetBookingDetailById(int bookingDetailId);

    }

}
