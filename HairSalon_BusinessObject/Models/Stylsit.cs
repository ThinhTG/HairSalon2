﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HairSalon_BusinessObject.Models;

public partial class Stylsit
{
    public string StylistId { get; set; }

    public string FullName { get; set; }

    public string PhoneNumber { get; set; }

    public decimal Salary { get; set; }

    public string Specialty { get; set; }

    public virtual ICollection<BookingDetail> BookingDetail { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Earning> Earning { get; set; } = new List<Earning>();
}