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
        public AvailableSlot GetAvailableSlotById(int id) 
            => AvailableSlotDAO.Instance.GetAvailableSlotById(id);


        public List<AvailableSlot> GetAvailableSlotsByDate(DateTime date) 
            => AvailableSlotDAO.Instance.GetAvailableSlotsByDate(date);
       

        public void UpdateSlotStatus(int availableSlotId, bool status)
            => AvailableSlotDAO.Instance.UpdateSlotStatus(availableSlotId, status);  
       

        public void SaveChanges() 
            => AvailableSlotDAO.Instance.SaveChanges();
      

        public List<User> GetStylists() 
            => AvailableSlotDAO.Instance.GetStylists();
       

        public List<Slot> GetSlots()
            => AvailableSlotDAO.Instance.GetSlots();


        public List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date) 
            => AvailableSlotDAO.Instance.GetAvailableSlotsByStylist(stylistId, date);
       

        public List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date)
            => AvailableSlotDAO.Instance.GetAvailableStylistsBySlotAndDate(slotId, date);      
    }
}
