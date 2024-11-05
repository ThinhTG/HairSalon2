using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_Repository.REPOSITORY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.INTERFACE
{
    public interface IAvailableSlotService
    {
        AvailableSlot GetAvailableSlotById(int id);
        List<AvailableSlot> GetAvailableSlotsByDate(DateTime date);
        List<AvailableSlot> GetAvailableSlotsByStylist(int stylistId, DateTime date);
        List<User> GetAvailableStylistsBySlotAndDate(int slotId, DateTime date);
        void UpdateSlotStatus(int availableSlotId, string status);
        void SaveChanges();
        List<User> GetStylists();

        List<Slot> GetSlots();
        (string userName, TimeOnly? startTime) GetUserAndSlotInfoByAvailableSlotId(int availableSlotId);
    }
}
