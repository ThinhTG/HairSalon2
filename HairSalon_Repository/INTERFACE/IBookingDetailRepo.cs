﻿using HairSalon_BusinessObject.Models;
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
        public List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId);
        public BookingDetail GetBookingDetailById(int bookingDetailId);
    }

}
