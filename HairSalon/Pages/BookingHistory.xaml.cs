using HairSalon.Pages;
using HairSalon.ViewModel;
using HairSalon_Services.SERVICE;
using System.Windows.Controls;
using System.Windows;
using HairSalon_BusinessObject.Models;
namespace HairSalon.Pages
{
    public partial class BookingHistory : Page
    {
        private BookingService bookingService;
        private BookingDetailService bookingDetailService;
        private AvailableSlotService availableSlotService;
        private ServiceService serviceService;
        private int UserId;

        public BookingHistory()
        {
            InitializeComponent();
            bookingService = new BookingService();
            bookingDetailService = new BookingDetailService();
            availableSlotService = new AvailableSlotService();
            serviceService = new ServiceService();
            LoadBookingHistory();
            
        }

        public BookingHistory(int userId)
        {
            InitializeComponent();
            bookingService = new BookingService();
            bookingDetailService = new BookingDetailService();
            availableSlotService = new AvailableSlotService();
            serviceService = new ServiceService();
            this.UserId = userId;
            LoadBookingHistory();

        }

        private void LoadBookingHistory()
        {
            var bookings = bookingService.GetBookingsByUserId(UserId);

            var bookingViewModels = bookings.Select(booking => new BookingViewModel
            {
                BookingDate = booking.BookingDate,
                Amount = booking.Amount,
                UserName = booking.User?.UserName ?? "Unknown User",
                Status = booking.Status,
                Discount = booking.Discount,
                BookingId = booking.BookingId
            }).ToList();

            BookingHistoryDataGrid.ItemsSource = bookingViewModels;
        }
        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadBookingDetailHistory()
        {
            var selectedBooking = (BookingViewModel)BookingHistoryDataGrid.SelectedItem;
            int bookingId = selectedBooking.BookingId;
            if (bookingId != null)
            {
                try
                {
                    var bookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId)
                                    .Select(detail => new BookingDetailViewModel
                                    {
                                        BookingId = detail.BookingId,
                                        BookingDetailId = detail.BookingDetailId,
                                        AvailableSlotId = detail.AvailableSlotId,
                                        Price = detail.Price,
                                        Status = detail.Status,
                                        ServiceName = serviceService.GetServiceNameById(detail.ServiceId),
                                        ScheduledWorkingDay = detail.ScheduledWorkingDay
                                    }).ToList();

                    if (bookingDetails != null && bookingDetails.Any())
                    {
                        foreach (var detail in bookingDetails)
                        {
                            if (detail.AvailableSlotId != 0)
                            {
                                var (userName, startTime) = availableSlotService.GetUserAndSlotInfoByAvailableSlotId(detail.AvailableSlotId);
                                detail.User.UserName = userName;
                                detail.Slot.StartTime = startTime;
                            }
                        }

                        BookingDetailDataGrid.ItemsSource = bookingDetails;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading booking details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            if (fromDate.HasValue && toDate.HasValue)
            {
                var bookings = bookingService.SearchBookingByDate(UserId, fromDate.Value, toDate.Value);
                if (bookings != null && bookings.Any())
                {
                    BookingHistoryDataGrid.ItemsSource = bookings;
                }
                else
                {
                    MessageBox.Show("No bookings found for the selected date range.", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select both 'From Date' and 'To Date'.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BookingHistoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (BookingHistoryDataGrid.SelectedItem == null) return;

            var selectedBooking = (BookingViewModel)BookingHistoryDataGrid.SelectedItem;
            int bookingId = selectedBooking.BookingId;

            var bookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId)
                                .Select(detail => new BookingDetailViewModel
                                {
                                    BookingDetailId = detail.BookingDetailId,
                                    AvailableSlotId = detail.AvailableSlotId,
                                    Price = detail.Price,
                                    Status = detail.Status,
                                    ServiceName = serviceService.GetServiceNameById(detail.ServiceId),
                                    ScheduledWorkingDay = detail.ScheduledWorkingDay
                                }).ToList();

            if (bookingDetails != null && bookingDetails.Any())
            {
                foreach (var detail in bookingDetails)
                {
                    if (detail.AvailableSlotId != 0)
                    {
                        var (userName, startTime) = availableSlotService.GetUserAndSlotInfoByAvailableSlotId(detail.AvailableSlotId);

                        detail.User.UserName = userName;
                        detail.Slot.StartTime = startTime;
                    }
                }

                BookingDetailDataGrid.ItemsSource = bookingDetails;
                
            }
            else
            {
                BookingDetailDataGrid.ItemsSource = null;
                MessageBox.Show("No booking details found for the selected booking.", "View Booking Details", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BookingDetailDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void CancelBookingButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int bookingId)
            {
                var result = bookingService.CancelBookingAndDetails(bookingId);

                if (result)
                {
                    MessageBox.Show("Booking and all associated details canceled successfully.", "Cancel Booking", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reload the booking history to reflect the canceled booking
                    LoadBookingHistory();

                    // Clear the selection to reset the details view
                    var bookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId)
                               .Select(detail => new BookingDetailViewModel
                               {
                                   BookingDetailId = detail.BookingDetailId,
                                   AvailableSlotId = detail.AvailableSlotId,
                                   Price = detail.Price,
                                   Status = detail.Status,
                                   ServiceName = serviceService.GetServiceNameById(detail.ServiceId),
                                   ScheduledWorkingDay = detail.ScheduledWorkingDay
                               }).ToList();

                    if (bookingDetails != null && bookingDetails.Any())
                    {
                        foreach (var detail in bookingDetails)
                        {
                            if (detail.AvailableSlotId != 0)
                            {
                                var (userName, startTime) = availableSlotService.GetUserAndSlotInfoByAvailableSlotId(detail.AvailableSlotId);

                                detail.User.UserName = userName;
                                detail.Slot.StartTime = startTime;
                            }
                        }

                        BookingDetailDataGrid.ItemsSource = bookingDetails;



                    }
                    else
                    {
                        MessageBox.Show("Failed to cancel booking.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int bookingDetailId)
            {
                var feedbackPage = new FeedbackPage(bookingDetailId);
                this.NavigationService.Navigate(feedbackPage);
            }
        }

        private void CancelBookingDetailButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


    }
}