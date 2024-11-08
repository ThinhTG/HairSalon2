using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DAO;
using HairSalon_Repository.INTERFACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_Repository.REPOSITORY
{
    public class SlotRepo : ISlotRepo
    {
       

        public bool AddSlot(Slot slot) => SlotDAO.Instance.AddSlot(slot);

        public bool DeleteSlot(int slotId) => SlotDAO.Instance.DeleteSlot(slotId);

        public Slot GetSlotById(int slotId) => SlotDAO.Instance.GetSlotById(slotId);

        public List<Slot> GetSlots() => SlotDAO.Instance.GetSlots();

        public bool UpdateSlot(Slot slot) => SlotDAO.Instance.UpdateSlot(slot);
    }
}
