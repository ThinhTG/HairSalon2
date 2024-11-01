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
        private static StylistDAO instance;

        public StylistManagementDAO()
        {
            dbContext = new HairSalonServiceContext();
        }
        public static StylistDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StylistDAO();
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
            return dbContext.StylistProfile.SingleOrDefault(s => s.StylistProfileId == stylistId);
        }

        public List<StylistProfile> GetStylistList()
        {
            return dbContext.StylistProfile.ToList();
        }

        public bool AddStylist(StylistProfile Stylist)
        {
            bool isSuccess = false;
            try
            {
                StylistProfile stylists = GetStylistByUserId(Stylist.UserId);
                if (stylists == null)
                {
                    using (var context = new HairSalonServiceContext())
                    {
                        context.StylistProfile.Add(Stylist);
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Stylist is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool UpdateStylist(StylistProfile Stylist)
        {
            bool isSuccess = false;
            try
            {
                StylistProfile stylists = GetStylistByUserId(Stylist.UserId);
                if (stylists != null)
                {
                    using (var context = new HairSalonServiceContext())
                    {
                        context.Entry(Stylist).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception("The Stylist does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }

        public bool DeleteStylist(StylistProfile Stylist)
        {
            bool isSuccess = false;
            try
            {
                StylistProfile stylists = GetStylistByUserId(Stylist.UserId);
                if (stylists != null)
                {
                    var context = new HairSalonServiceContext();
                    context.StylistProfile.Remove(Stylist);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The Stylist does not already exist.");
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
