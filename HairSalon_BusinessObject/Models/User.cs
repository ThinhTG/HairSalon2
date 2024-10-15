﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HairSalon_BusinessObject.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int RoleId { get; set; }

    public string PhoneNumber { get; set; }

    public virtual ICollection<AvailableSlot> AvailableSlot { get; set; } = new List<AvailableSlot>();

    public virtual ICollection<Booking> Booking { get; set; } = new List<Booking>();

    public virtual ICollection<Earning> Earning { get; set; } = new List<Earning>();

    public virtual Role Role { get; set; }
}