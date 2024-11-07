using HairSalon_BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.ViewModel
{
    public class ServiceViewModel
    {
        public ObservableCollection<BookingDetail> BookingDetails { get; set; }

        public ServiceViewModel()
        {
            BookingDetails = new ObservableCollection<BookingDetail>();
            LoadServices();
        }

        private void LoadServices()
        {
            using (var context = new HairSalonServiceContext())
            {
                var slots = context.BookingDetail
                    .Include(a => a.Service)  // Include User information
                   
                    .ToList();

                foreach (var slot in slots)
                {
                    BookingDetails.Add(slot);
                }
            }
        }
    }
}
