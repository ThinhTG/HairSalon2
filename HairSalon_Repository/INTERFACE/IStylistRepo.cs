﻿using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IStylistRepo
    {
        public Stylist GetStylistById(string stylistId);

        public List<Stylist> GetStylists();

    }
}

