using HairSalon.ViewModel;
using HairSalon_BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.ViewModel
{

    public class BookingViewModel : INotifyPropertyChanged
    {
        private HairSalonServiceContext _context;

        private ObservableCollection<BookingUserViewModel> _bookingUserList;
        public ObservableCollection<BookingUserViewModel> BookingUserList
        {
            get { return _bookingUserList; }
            set
            {
                _bookingUserList = value;
                OnPropertyChanged(nameof(BookingUserList));
            }
        }

        public BookingViewModel(HairSalonServiceContext context)
        {
            _context = context;
            LoadData();
        }

        private void LoadData()
        {
            var data = (from booking in _context.Booking.ToList()
                        join user in _context.User.ToList()
                        on booking.UserId equals user.UserId
                        select new BookingUserViewModel
                        {
                            BookingId = booking.BookingId,
                            Status = booking.Status,
                            BookingDate = booking.BookingDate,
                            Amount = booking.Amount,
                            Discount = booking.Discount,
                            CreatedBy = booking.CreateBy,
                            UserName = user.UserName,         // L?y UserName t? b?ng User
                            PhoneNumber = user.PhoneNumber    // L?y PhoneNumber t? b?ng User
                        }).ToList();

            BookingUserList = new ObservableCollection<BookingUserViewModel>(data);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}