﻿using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class ServiceDAO
    {
        private HairSalonServiceContext dbContext;
        private static ServiceDAO instance;


        public ServiceDAO()
        {
            dbContext = new HairSalonServiceContext();
        }

        public static ServiceDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceDAO();
                }
                return instance;
            }
        }

        public Service GetServiceById(int serviceId)
        {
            return dbContext.Service.SingleOrDefault(m => m.ServiceId == serviceId);
        }

        public List<Service> GetServiceList()
        {
            return dbContext.Service.ToList();
        }

        public bool AddService(Service service)
        {
            bool isSuccess = false;
            try
            {
                Service services = GetServiceById(service.ServiceId);
                if (services == null)
                {
                    using (var context = new HairSalonServiceContext())
                    {
                        context.Service.Add(service);
                        context.SaveChanges();
                        isSuccess = true;
                    }
                }
                else
                {
                    throw new Exception("The Service is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool UpdateService(Service service)
        {
            bool isSuccess = false;
            try
            {
                Service services = GetServiceById(service.ServiceId);
                if (services == null)
                {
                    using (var context = new HairSalonServiceContext())
                    {
                        context.Entry(service).State = EntityState.Modified;
                        context.SaveChanges();
                        isSuccess = true;
                    }
                }
                else
                {
                    throw new Exception("The Service does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool DeleteService(int serviceId)
        {
            bool isSuccess = false;
            Service services = this.GetServiceById(serviceId);
            try
            {
                if (services == null)
                {
                    dbContext.Service.Remove(services);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("The Service does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }
    }
}