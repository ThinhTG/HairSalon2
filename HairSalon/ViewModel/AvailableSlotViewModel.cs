using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore; // Add this directive
using System.Collections.ObjectModel;
using System.Linq;

namespace HairSalon.ViewModel
{
    public class AvailableSlotViewModel
    {
        public ObservableCollection<AvailableSlot> AvailableSlots { get; set; }

        public AvailableSlotViewModel()
        {
            AvailableSlots = new ObservableCollection<AvailableSlot>();
            LoadAvailableSlots();
        }

        private void LoadAvailableSlots()
        {
            using (var context = new HairSalonServiceContext())
            {
                var slots = context.AvailableSlot
                    .Include(a => a.Slot)  // Include Slot information
                    .Include(a => a.User)  // Include User information
                    .ToList();

                foreach (var slot in slots)
                {
                    AvailableSlots.Add(slot);
                }
            }
        }
    }
}
