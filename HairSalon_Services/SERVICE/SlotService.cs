using HairSalon_BusinessObject.Models;
using HairSalon_Repository.INTERFACE;
using HairSalon_Repository.REPOSITORY;
using HairSalon_Services.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Services.SERVICE
{
    public class SlotService : ISlotService
    {
        private ISlotRepo ISlotRepo;

        public SlotService()
        {
            ISlotRepo = new SlotRepo();
        }

        public bool AddSlot(Slot slot)
        {
            return ISlotRepo.AddSlot(slot);
        }

        public bool DeleteSlot(int slotId)
        {
            return ISlotRepo.DeleteSlot(slotId);
        }

        public Slot GetSlotById(int slotId)
        {
            return ISlotRepo.GetSlotById(slotId);
        }

        public List<Slot> GetSlots()
        {
            return ISlotRepo.GetSlots();
        }

        public bool UpdateSlot(Slot slot)
        {
            return ISlotRepo.UpdateSlot(slot);
        }




    }
}
