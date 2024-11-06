using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.INTERFACE
{
    public interface IBookingDetailService
    {
        bool AddBookingDetail(BookingDetail bookingDetail);
        bool UpdateBookingDetailStatus(int bookingDetailId, string newStatus);
        List<BookingDetail> GetPendingBookingDetails();
        List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId);
        public BookingDetail GetBookingDetailById(int bookingDetailId);
        bool SaveChanges();
    }
}
