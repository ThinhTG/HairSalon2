﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HairSalon_BusinessObject.Models;

public partial class DailySalaryOfStylist
{
    public DateTime Date { get; set; }

    public decimal? DailySalary { get; set; }

    public int StylistProfileId { get; set; }

    public int DailySalaryOfStylistId { get; set; }

    public virtual StylistProfile StylistProfile { get; set; }
}