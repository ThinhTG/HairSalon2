using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore; // Add this directive
using System.Collections.ObjectModel;
using System.Linq;

namespace HairSalon.ViewModel
{
    public class AvailableSlotViewModel
    {
        public ObservableCollection<AvailableSlot> AvailableSlot { get; set; }

        public AvailableSlotViewModel()
        {
            AvailableSlot = new ObservableCollection<AvailableSlot>();
            LoadAvailableSlots();
        }

        private void LoadAvailableSlots()
        {
            using (var context = new HairSalonServiceContext())
            {
                var slots = context.AvailableSlot
                    .Include(a => a.Slot) 
                    .Include(a => a.User) 
                    .ToList();

                foreach (var slot in slots)
                {
                    AvailableSlot.Add(slot);
                }
            }
        }
    }
}
