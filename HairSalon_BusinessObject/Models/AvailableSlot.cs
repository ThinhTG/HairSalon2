﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HairSalon_BusinessObject.Models;

public partial class AvailableSlot
{
    public int AvailableSlotId { get; set; }

    public int SlotId { get; set; }

    public DateTime? Date { get; set; }

    public int UserId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetail { get; set; } = new List<BookingDetail>();

    public virtual Slot Slot { get; set; }

    public virtual User User { get; set; }
}