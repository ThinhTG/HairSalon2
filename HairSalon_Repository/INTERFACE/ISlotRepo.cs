using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.INTERFACE
{
    public interface ISlotRepo
    {
        public List<Slot> GetSlots();
        public Slot GetSlotById(int slotId);
        public bool AddSlot(Slot slot);
        public bool UpdateSlot(Slot slot);
        public bool DeleteSlot(int slotId);
    }
}
