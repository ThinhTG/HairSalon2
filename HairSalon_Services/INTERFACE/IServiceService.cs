﻿using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.INTERFACE
{
    public interface IServiceService
    {
        public Service GetServiceById(int serviceId);
        public List<Service> GetServiceList();
        public string GetServiceNameById(int serviceId);
        public bool AddService(Service service);
        public bool UpdateService(Service service);
        public bool DeleteService(int serviceId);
    }
}