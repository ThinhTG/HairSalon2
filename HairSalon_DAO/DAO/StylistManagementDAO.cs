using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class StylistManagementDAO
    {
        private HairSalonServiceContext dbContext;
        private static StylistManagementDAO instance;

        public StylistManagementDAO()
        {
            dbContext = new HairSalonServiceContext();
        }
        public static StylistManagementDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StylistManagementDAO();
                }
                return instance;
            }
        }

        public StylistProfile GetStylistByUserId(int userId)
        {
            return dbContext.StylistProfile.SingleOrDefault(u => u.UserId == userId);
        }
        public StylistProfile GetStylistById(int stylistId)
        {
            return dbContext.StylistProfile/*.AsNoTracking()*/.SingleOrDefault(s => s.StylistProfileId == stylistId);
        }

        public List<StylistProfile> GetStylistList()
        {
            return dbContext.StylistProfile.Include("User").ToList();
        }

        public bool AddStylist(StylistProfile Stylist)
        {
            bool isSuccess = false;
            try
            {
                StylistProfile stylists = GetStylistByUserId(Stylist.UserId);
                if (stylists == null)
                {
                        dbContext.StylistProfile.Add(Stylist);
                        dbContext.SaveChanges();
                        isSuccess = true;
                }
                else
                {
                    throw new Exception("The Stylist for this user is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the stylist: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

        public bool UpdateStylist(StylistProfile Stylist)
        {
            bool isSuccess = false;
            try
            {
                StylistProfile stylists = GetStylistById(Stylist.StylistProfileId);
                if (stylists != null)
                {
                    var existingEntity = dbContext.StylistProfile.Local
                              .FirstOrDefault(e => e.StylistProfileId == Stylist.StylistProfileId);

                    if (existingEntity != null)
                    {
                        dbContext.Entry(existingEntity).State = EntityState.Detached;
                    }
                    dbContext.Entry(Stylist).State = EntityState.Modified;
                        dbContext.SaveChanges();
                        isSuccess=true;
                }
                else
                {
                    throw new Exception("The Stylist does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the stylist: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

        public bool DeleteStylist(int stylistId)
        {
            bool isSuccess = false;
            try
            {
                StylistProfile stylists = GetStylistById(stylistId);
                if (stylists != null)
                {
                    dbContext.StylistProfile.Remove(stylists);
                    dbContext.SaveChanges();
                    isSuccess=true;
                }
                else
                {
                    throw new Exception("The Stylist does not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the stylist: " + ex.InnerException?.Message, ex);
            }
            return isSuccess;
        }

    }
}
