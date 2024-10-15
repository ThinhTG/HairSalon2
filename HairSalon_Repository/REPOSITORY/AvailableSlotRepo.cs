using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_Repository.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HairSalon_Repository.REPOSITORY
{
    public class AvailableSlotRepo : IAvailableSlotRepo
    {
        private readonly AvailableSlotDAO _availableSlotDAO;

        public AvailableSlotRepo()
        {
            _availableSlotDAO = AvailableSlotDAO.Instance;
        }

        public AvailableSlot GetAvailableSlotById(int id)
        {
            return _availableSlotDAO.GetAvailableSlotById(id);
        }

        public List<AvailableSlot> GetAvailableSlotsByDate(DateTime date)
        {
            return _availableSlotDAO.GetAvailableSlotsByDate(date);
        }

        public void UpdateSlotStatus(int availableSlotId, bool status)
        {
            _availableSlotDAO.UpdateSlotStatus(availableSlotId, status);
        }

        public void SaveChanges()
        {
            _availableSlotDAO.SaveChanges();
        }

        // Cập nhật để lấy danh sách stylist (User với RoleId = 1)
        public List<User> GetStylists()
        {
            // Gọi đến DAO để lấy danh sách stylist
            return _availableSlotDAO.GetStylists();
        }

        // Cập nhật để lấy danh sách slot
        public List<Slot> GetSlots()
        {
            // Gọi đến DAO để lấy danh sách slot
            return _availableSlotDAO.GetSlots();
        }

        public List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date)
        {
            return _availableSlotDAO.GetAvailableSlotsByStylist(stylistId, date);
        }

        public List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date)
        {
            return _availableSlotDAO.GetAvailableStylistsBySlotAndDate((int)slotId, date);
        }
    }
}
