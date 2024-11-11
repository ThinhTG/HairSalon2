using HairSalon.ViewModel;
using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
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
        private BookingDetailService bookingDetailService;
       private int viewStateBookingId = 0;
        

        public BookingManagement()
        {
            InitializeComponent();
            bookingService = new BookingService();
            userService = new UserService();
            bookingDetailService = new BookingDetailService();
        }

        private void loadDataInit()
        {
            HairSalonServiceContext context = new HairSalonServiceContext();
            bookingViewModel = new BookingViewModel(context);
            var confirmedBookings = (from booking in bookingService.GetBookings()
                                     join user in userService.GetUsers()
                                     on booking.UserId equals user.UserId
                                     where booking.Status == "Paid"
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
            string userName = txtUserName.Text.Trim();
            string phoneNumber = txtPhoneNumber.Text.Trim();
            DateTime? selectedDate = txtDatePicker.SelectedDate;

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

            if (!string.IsNullOrEmpty(userName))
            {
                filteredBookings = filteredBookings.Where(b => b.UserName.ToLower().Contains(userName.ToLower()));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                filteredBookings = filteredBookings.Where(b => b.PhoneNumber.Contains(phoneNumber));
            }

            if (selectedDate.HasValue)
            {
                filteredBookings = filteredBookings.Where(b => b.BookingDate == selectedDate.Value.Date);
            }

            dtgBooking.ItemsSource = filteredBookings.ToList();
        }


        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            txtUserName.Clear();
            txtPhoneNumber.Clear();
            txtDatePicker.SelectedDate = null;

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

            dtgBooking.ItemsSource = allBookings;
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int BookingDetailId))
            {
                bookingDetailService.UpdateBookingDetailStatus(BookingDetailId, "Checked In");
                UpdateBookingDetailList(viewStateBookingId);
                MessageBox.Show("Khách hàng đã check-in thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            else
            {
                MessageBox.Show("Lỗi: Không thể lấy ID chi tiết đặt chỗ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag != null && int.TryParse(button.Tag.ToString(), out int BookingDetailId))
            {
                bookingDetailService.UpdateBookingDetailStatus(BookingDetailId, "Completed");
                UpdateBookingDetailList(viewStateBookingId);
                if (bookingDetailService.AreAllBookingDetailsCompleted(viewStateBookingId))
                {
                    // Cập nhật trạng thái của Booking thành Completed
                    bookingService.UpdateBookingStatus(viewStateBookingId, "Completed");
                    loadDataInit();
                    BookingDetailDataGrid.ItemsSource = null;


                }

                MessageBox.Show("Khách hàng đã checkout thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Lỗi: Không thể lấy ID chi tiết đặt chỗ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        



        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int bookingId = (int)btn.Tag;
                viewStateBookingId = bookingId;

                var bookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId);

                if (bookingDetails != null && bookingDetails.Count != 0)
                {
                    BookingDetailDataGrid.ItemsSource = bookingDetails;
                }
                else
                {
                    BookingDetailDataGrid.ItemsSource = null;
                    MessageBox.Show("No booking details found for the selected booking.", "View Booking Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void UpdateBookingDetailList(int bookingId)
        {
            // Giả sử bạn có phương thức để lấy danh sách BookingDetail theo BookingId
          List<BookingDetailDTO> BookingDetailList = bookingDetailService.GetBookingDetailsByBookingId(bookingId);
            BookingDetailDataGrid.ItemsSource = BookingDetailList;
            BookingDetailDataGrid.Items.Refresh();
        }



    }
}
