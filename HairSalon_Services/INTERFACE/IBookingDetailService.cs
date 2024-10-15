﻿using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.INTERFACE
{
    public interface IBookingDetailService
    {
        List<BookingDetail> GetBookingDetailsByBookingId(int bookingId);
        bool AddBookingDetail(BookingDetail bookingDetail);
        bool UpdateBookingDetailStatus(int bookingDetailId, string newStatus);
        List<BookingDetail> GetPendingBookingDetails();

        bool SaveChanges();
    }
}
