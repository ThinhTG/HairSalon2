using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        public string GetServiceNameById(int serviceId)
        {
            var service = dbContext.Service.FirstOrDefault(s => s.ServiceId == serviceId);
            return service != null ? service.ServiceName : "Unknown Service";
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
                    dbContext.Service.Add(service);
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("The Service is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the service: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

        public bool UpdateService(Service service)
        {
            bool isSuccess = false;
            try
            {
                Service services = GetServiceById(service.ServiceId);
                if (services != null)
                {
                    var existingEntity = dbContext.Service.Local
                              .FirstOrDefault(e => e.ServiceId == service.ServiceId);

                    if (existingEntity != null)
                    {
                        dbContext.Entry(existingEntity).State = EntityState.Detached;
                    }
                    dbContext.Entry(service).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
                else
                {
                    throw new Exception("The Service does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the service: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

        public bool DeleteService(int serviceId)
        {
            bool isSuccess = false;
            Service services = this.GetServiceById(serviceId);
            try
            {
                if (services != null)
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
                throw new Exception("An error occurred while adding the stylist: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }



    }
}