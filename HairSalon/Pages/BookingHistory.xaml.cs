﻿using HairSalon.Pages;
using HairSalon.ViewModel;
using HairSalon_Services.SERVICE;
using System.Windows.Controls;
using System.Windows;
using HairSalon_BusinessObject.Models;
using System.Timers;
namespace HairSalon.Pages
{
    public partial class BookingHistory : Page
    {
        private BookingService bookingService;
        private BookingDetailService bookingDetailService;
        private AvailableSlotService availableSlotService;
        private ServiceService serviceService;
        private UserService userService;
        private int UserId;
        private System.Timers.Timer _timer;

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
            userService = new UserService();
            this.UserId = userId;
            LoadBookingHistory();
            var pendingBookings = bookingService.GetPendingBookingsByUserId(userId);

        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingHistory();
        }

        private void LoadBookingHistory()
        {
            var bookings = bookingService.GetBookingsByUserId(UserId)
                                         .OrderByDescending(booking => booking.BookingId)
                                         .ToList();

            var bookingViewModels = bookings.Select(booking => new BookingHistoryViewModel
            {
                BookingDate = booking.BookingDate,
                Amount = booking.Amount,
                UserName = userService.GetUserNameByUserId(booking.UserId) ?? "Unknown User",
                Status = booking.Status,
                Discount = booking.Discount,
                BookingId = booking.BookingId
            }).ToList();

            BookingHistoryDataGrid.ItemsSource = bookingViewModels;
        }

        private void LoadBookingDetailHistory(int bookingId)
        {
            try
            {
                var bookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId);
                BookingDetailDataGrid.ItemsSource = bookingDetails;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading booking details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BookingHistoryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (BookingHistoryDataGrid.SelectedItem == null) return;

            var selectedBooking = (BookingHistoryViewModel)BookingHistoryDataGrid.SelectedItem;
            int bookingId = selectedBooking.BookingId;

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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? fromDate = FromDatePicker.SelectedDate;
            DateTime? toDate = ToDatePicker.SelectedDate;

            if (fromDate.HasValue && toDate.HasValue)
            {
                var bookings = bookingService.SearchBookingByDate(UserId, fromDate.Value.Date, toDate.Value.Date.AddDays(1));
                if (bookings != null && bookings.Any())
                {
                    var bookingViewModels = bookings.Select(booking => new BookingHistoryViewModel
                    {
                        BookingDate = booking.BookingDate,
                        Amount = booking.Amount,
                        UserName = userService.GetUserNameByUserId(booking.UserId),
                        Status = booking.Status,
                        Discount = booking.Discount,
                        BookingId = booking.BookingId
                    }).ToList();

                    BookingHistoryDataGrid.ItemsSource = bookingViewModels;
                    BookingDetailDataGrid.ItemsSource = null;
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

        private void CancelBookingButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int bookingId)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to cancel this booking and all associated details?",
                    "Confirm Cancellation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {

                    var cancelResult = bookingService.CancelBookingAndDetails(bookingId);

                    if (cancelResult)
                    {
                        var bookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId);
                        foreach (var detail in bookingDetails)
                        {
                            availableSlotService.UpdateSlotStatus(detail.AvailableSlotId, "Unbooked");
                        }

                        MessageBox.Show("Booking and all associated details canceled successfully.", "Cancel Booking", MessageBoxButton.OK, MessageBoxImage.Information);
                        Window_Loaded(sender, e);
                        BookingDetailDataGrid.ItemsSource = bookingDetailService.GetBookingDetailsByBookingId(bookingId);
                    }
                    else
                    {
                        MessageBox.Show("Failed to cancel booking.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void CancelBookingDetailButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int bookingDetailId)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to cancel this booking detail?",
                    "Confirm Cancellation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var cancelResult = bookingDetailService.UpdateBookingDetailStatus(bookingDetailId, "Cancelled");

                    if (cancelResult)
                    {
                        var bookingDetail = bookingDetailService.GetBookingDetailById(bookingDetailId);
                        int bookingId = bookingDetail.BookingId;

                        availableSlotService.UpdateSlotStatus(bookingDetail.AvailableSlotId, "Unbooked");

                        MessageBox.Show("Booking detail canceled successfully.", "Cancel Booking Detail", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadBookingDetailHistory(bookingId);

                        var allBookingDetails = bookingDetailService.GetBookingDetailsByBookingId(bookingId);
                        int canceledCount = allBookingDetails.Count(detail => detail.Status == "Cancelled");

                        if (canceledCount == allBookingDetails.Count)
                        {
                            var bookingCanceled = bookingService.UpdateBookingStatus(bookingId, "Cancelled");
                            if (bookingCanceled)
                            {
                                MessageBox.Show("All booking details are canceled. Booking status set to 'Canceled'.", "Update Booking Status", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        LoadBookingHistory();
                    }
                    else
                    {
                        MessageBox.Show("Failed to cancel booking detail.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int bookingDetailId)
            {
                var feedbackPage = new BookingFeedback(bookingDetailId);
                this.NavigationService.Navigate(feedbackPage);
            }
        }

        private void PaymentButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is int bookinngId)
            {
                Booking booking = bookingService.GetBookingById(bookinngId);

                if (booking.Status.Equals("Cancelled"))
                {
                    
                    LoadBookingHistory();
                    MessageBox.Show("Booking deleted ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    PaymentPage paymentPage = new PaymentPage(booking.BookingId);
                    this.NavigationService.Navigate(paymentPage);

                }
            }
            
        }
    

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        FromDatePicker.SelectedDate = null;
        ToDatePicker.SelectedDate = null;
        LoadBookingHistory();
    }

}
}




