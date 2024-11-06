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


        public bool UpdateSlotStatus(int availableSlotId, string newStatus)
        {
            bool isSuccess = false;
            try
            {
                var slot = dbContext.AvailableSlot.FirstOrDefault(s => s.AvailableSlotId == availableSlotId);

                if (slot != null)
                {
                    slot.Status = newStatus;
                    dbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSuccess;
        }
        public (string userName, TimeOnly? startTime) GetUserAndSlotInfoByAvailableSlotId(int availableSlotId)
        {
            var availableSlot = dbContext.AvailableSlot
                .Include(a => a.User)
                .Include(a => a.Slot)
                .SingleOrDefault(a => a.AvailableSlotId == availableSlotId);

            if (availableSlot == null)
            {
                throw new Exception("AvailableSlot not found for the given availableSlotId.");
            }

            var userName = availableSlot.User?.UserName ?? "Unknown User";
            var startTime = availableSlot.Slot?.StartTime;

            return (userName, startTime);
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