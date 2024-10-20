using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class AvailableSlotDAO
    {
        private HairSalonServiceContext dbContext;
        private static AvailableSlotDAO instance;


        public AvailableSlotDAO()
        {
            dbContext = new HairSalonServiceContext();
        }
        public static AvailableSlotDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AvailableSlotDAO();
                }
                return instance;
            }
        }

        public AvailableSlot GetAvailableSlotById(int id)
        {
            return dbContext.AvailableSlot.SingleOrDefault(a => a.AvailableSlotId == id);
        }

        public List<AvailableSlot> GetAvailableSlotsByDate(DateTime date)
        {
            return dbContext.AvailableSlot
                            .Where(a => a.Date.HasValue && a.Date.Value.Date == date.Date
                                        && (string.IsNullOrEmpty(a.Status) || a.Status == "Unbooked"))
                            .ToList();
        }

        public List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date)
        {
            return dbContext.AvailableSlot
                            .Include(a => a.Slot)
                            .Where(a => a.UserId == stylistId
                                        && a.Date.HasValue && a.Date.Value.Date == date.Date 
                                        && (string.IsNullOrEmpty(a.Status) || a.Status == "Unbooked"))
                            .ToList();
        }

        public List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date)
        {
            return dbContext.AvailableSlot
                .Where(a => a.SlotId == slotId
                            && a.Date.HasValue && a.Date.Value.Date == date.Date
                            && (string.IsNullOrEmpty(a.Status) || a.Status == "Unbooked"))
                .Select(a => a.User)
                .Distinct()
                .ToList();
        }


        public void UpdateSlotStatus(int availableSlotId, string status)
        {
            var slot = GetAvailableSlotById(availableSlotId);
            if (slot != null)
            {
                slot.Status = status; 
                SaveChanges(); 
            }
            else
            {
                throw new Exception("AvailableSlot not found.");
            }
        }


        public List<User> GetStylists()
        {
            return dbContext.User.Where(u => u.RoleId == 2).ToList();
        }

        public List<Slot> GetSlots()
        {
            return dbContext.Slot.ToList();
        }
        public void SaveChanges()
        {
            dbContext.SaveChanges(); 
        }
    }

}