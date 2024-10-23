using HairSalon_BusinessObject.Models;
using HairSalon_Services;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HairSalon.Pages
{
    /// <summary>
    /// Interaction logic for BookingManagement.xaml
    /// </summary>
    public partial class BookingManagement : Page
    {
        private IBookingService bookingService;
        private BookingViewModel bookingViewModel;
        private IUserService userService;
        

        public BookingManagement()
        {
            InitializeComponent();
            bookingService = new BookingService();
            userService = new UserService();
        }

        private void loadDataInit()
        {
            HairSalonServiceContext context = new HairSalonServiceContext();
            bookingViewModel = new BookingViewModel(context);
            var confirmedBookings = (from booking in bookingService.GetBookings()
                                     join user in userService.GetUsers()
                                     on booking.UserId equals user.UserId
                                     where booking.Status == "Confirmed"
                                     select new
                                     {
                                         booking.BookingId,
                                         booking.Status,
                                         booking.BookingDate,
                                         booking.Amount,
                                         booking.Discount,
                                         booking.CreateBy,
                                         user.UserName,
                                         user.PhoneNumber
                                     }).ToList();

            dtgBooking.ItemsSource = confirmedBookings;
            this.DataContext = bookingViewModel;

        }


        

        private void WindowLoad(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newPage = new CustomerHomePage();
            NavigationService.Navigate(newPage);
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            // Lấy giá trị từ TextBox
            string userName = txtUserName.Text.Trim();
            string phoneNumber = txtPhoneNumber.Text.Trim();
            DateTime? selectedDate = txtDatePicker.SelectedDate;

            // Lấy toàn bộ danh sách bookings
            var filteredBookings = (from booking in bookingService.GetBookings()
                                    join user in userService.GetUsers()
                                    on booking.UserId equals user.UserId
                                    where booking.Status == "Confirmed"
                                    select new
                                    {
                                        booking.BookingId,
                                        booking.Status,
                                        booking.BookingDate,
                                        booking.Amount,
                                        booking.Discount,
                                        booking.CreateBy,
                                        user.UserName,
                                        user.PhoneNumber
                                    }).AsQueryable();

            // Lọc theo UserName nếu có nhập
            if (!string.IsNullOrEmpty(userName))
            {
                filteredBookings = filteredBookings.Where(b => b.UserName.ToLower().Contains(userName.ToLower()));
            }

            // Lọc theo PhoneNumber nếu có nhập
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                filteredBookings = filteredBookings.Where(b => b.PhoneNumber.Contains(phoneNumber));
            }

            // Lọc theo BookingDate nếu có chọn ngày
            if (selectedDate.HasValue)
            {
                filteredBookings = filteredBookings.Where(b => b.BookingDate == selectedDate.Value.Date);
            }

            // Nếu không nhập gì, thì vẫn trả về toàn bộ danh sách
            dtgBooking.ItemsSource = filteredBookings.ToList();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            // Xóa nội dung trong các ô TextBox và DatePicker
            txtUserName.Clear();
            txtPhoneNumber.Clear();
            txtDatePicker.SelectedDate = null;

            // Hiển thị lại toàn bộ danh sách bookings
            var allBookings = (from booking in bookingService.GetBookings()
                               join user in userService.GetUsers()
                               on booking.UserId equals user.UserId
                               where booking.Status == "Confirmed"
                               select new
                               {
                                   booking.BookingId,
                                   booking.Status,
                                   booking.BookingDate,
                                   booking.Amount,
                                   booking.Discount,
                                   booking.CreateBy,
                                   user.UserName,
                                   user.PhoneNumber
                               }).ToList();

            // Gán danh sách này vào ItemsSource của DataGrid
            dtgBooking.ItemsSource = allBookings;
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy BookingId từ thuộc tính Tag của nút
            Button btn = sender as Button;
            if (btn != null)
            {
                int bookingId =(int) btn.Tag;
                if ((bookingId) != null)
                {
                    var booking = bookingService.GetBookings().FirstOrDefault(b => b.BookingId == bookingId);
                    if (booking != null)
                    {

                        bookingService.UpdateBookingStatus(bookingId, "In Progress");
                    }
                }


                MessageBox.Show($"Check In for Booking ID: {bookingId} Successful");

               
            }
        }

    }
}
