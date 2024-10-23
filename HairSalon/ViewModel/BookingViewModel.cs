using HairSalon.ViewModel;
using HairSalon_BusinessObject.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
                        UserName = user.UserName,         // Lấy UserName từ bảng User
                        PhoneNumber = user.PhoneNumber    // Lấy PhoneNumber từ bảng User
                    }).ToList();

        BookingUserList = new ObservableCollection<BookingUserViewModel>(data);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
