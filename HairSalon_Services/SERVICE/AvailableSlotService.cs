using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;

namespace HairSalon_Services.SERVICE
{
    public class AvailableSlotService : IAvailableSlotService
    {
        private IAvailableSlotRepo _availableSlotRepo;
        public AvailableSlotService()
        {
            _availableSlotRepo = new AvailableSlotRepo();
        }

        public AvailableSlot GetAvailableSlotById(int availableSlotId)
        {
            return _availableSlotRepo.GetAvailableSlotById(availableSlotId);
        }

        public List<AvailableSlot> GetAvailableSlotsByDate(DateTime date)
        {
            return _availableSlotRepo.GetAvailableSlotsByDate(date);
        }

        public List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date)
        {
            return _availableSlotRepo.GetAvailableSlotsByStylist(stylistId, date);
        }

        public List<User> GetStylists()
        {
            return _availableSlotRepo.GetStylists();
        }

        public List<Slot> GetSlots()
        {
            return _availableSlotRepo.GetSlots();
        }

        public void UpdateSlotStatus(int availableSlotId, bool status)
        {
            _availableSlotRepo.UpdateSlotStatus(availableSlotId, status);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _availableSlotRepo.SaveChanges();
        }

        public List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date)
        {
            return _availableSlotRepo.GetAvailableStylistsBySlotAndDate(slotId, date);
        }
    }
}
