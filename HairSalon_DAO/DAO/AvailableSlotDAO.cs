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
        private HairSalonContext dbContext;
        private static AvailableSlotDAO instance;


        public AvailableSlotDAO()
        {
            dbContext = new HairSalonContext();
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
            // Kiểm tra trạng thái không phải là null và không booked
            return dbContext.AvailableSlot
                            .Where(a => a.Date == date && (!a.Status.HasValue || !a.Status.Value))
                            .ToList();
        }
        public List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date)
        {
            return dbContext.AvailableSlot
                            .Include(a => a.Slot) // Include bảng Slot để lấy thông tin Slot
                            .Where(a => a.UserId == stylistId && a.Date == date && (!a.Status.HasValue || a.Status == false))
                            .ToList();
        }
        public List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date)
        {
            return dbContext.AvailableSlot
                .Where(a => a.SlotId == slotId && a.Date == date && (!a.Status.HasValue || !a.Status.Value))
                .Select(a => a.User) // Lấy thông tin stylist
                .Distinct()
                .ToList();
        }

        public void UpdateSlotStatus(int availableSlotId, bool status)
        {
            var slot = GetAvailableSlotById(availableSlotId);
            if (slot != null)
            {
                slot.Status = status; // Update status to true/false
                SaveChanges(); // Lưu lại thay đổi
            }
            else
            {
                // Có thể log hoặc xử lý khi slot không tồn tại
                throw new Exception("AvailableSlot not found.");
            }
        }


        public List<User> GetStylists()
        {
            // Giả sử roleId = 1 đại diện cho stylist
            return dbContext.User.Where(u => u.RoleId == 2).ToList();
        }

        public List<Slot> GetSlots()
        {
            return dbContext.Slot.ToList();
        }


        public void SaveChanges()
        {
            dbContext.SaveChanges(); // Lưu tất cả thay đổi vào cơ sở dữ liệu
        }
    }

}