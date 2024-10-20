using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface IAvailableSlotRepo
    {
        AvailableSlot GetAvailableSlotById(int id);
        List<AvailableSlot> GetAvailableSlotsByDate(DateTime date);
        List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date);
        List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date);
        void UpdateSlotStatus(int availableSlotId, string status);
        void SaveChanges();
        List<User> GetStylists();
        List<Slot> GetSlots();

    }

}
