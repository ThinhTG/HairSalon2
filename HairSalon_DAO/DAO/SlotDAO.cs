using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon_DAO.DAO
{
    public class SlotDAO
    {
        private HairSalonServiceContext _context;
        private static SlotDAO _instance = null;
        public SlotDAO()
        {
            _context = new HairSalonServiceContext();
        }

        public static SlotDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SlotDAO();
                }
                return _instance;
            }
        }

        public List<Slot> GetSlots()
        {
            return _context.Slot.ToList();
        }

        public Slot GetSlotById(int slotId)
        {
            return _context.Slot.FirstOrDefault(s => s.SlotId == slotId);
        }

        public bool AddSlot(Slot slot)
        {
            _context.Slot.Add(slot);
            return SaveChanges();
        }

        public bool UpdateSlot(Slot slot)
        {
            _context.Slot.Update(slot);
            return SaveChanges();
        }

        public bool DeleteSlot(int slotId)
        {
            Slot slot = GetSlotById(slotId);
            _context.Slot.Remove(slot);
            return SaveChanges();
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
