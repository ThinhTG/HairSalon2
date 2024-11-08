using HairSalon.ViewModel;
using HairSalon_BusinessObject.Models;
using HairSalon_Services;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HairSalon.Pages
{
    public partial class BookingFeedback : Page
    {
        private readonly BookingDetailService _bookingDetailService;
        private readonly BookingService _bookingService;
        private readonly UserService _userService;
        private readonly ServiceService _serviceService;
        private readonly StylistService stylistService;
        private readonly SlotService slotService;
        private readonly AvailableSlotService availableSlotService;
        private int userid;

        public BookingFeedback(int id)
        {
            InitializeComponent();
            this.userid = id;
            _bookingDetailService = new BookingDetailService();
            _bookingService = new BookingService();
            _serviceService = new ServiceService();
            _userService = new UserService();
            stylistService = new StylistService();
            slotService = new SlotService();
            availableSlotService = new AvailableSlotService();
            this.DataContext = new ServiceViewModel();
        }




        private async void LoadCompletedBookingDetails()
        {
            List<Booking> bookings =  _bookingService.GetBookingsByUserId(userid).ToList();
            if (bookings == null || !bookings.Any())
            {
                Console.WriteLine("No bookings found.");
                return;
            }

            // Create a list to store BookingDetails with status "Pending"
            List<BookingDetail> completedBookingDetails = new List<BookingDetail>();

            foreach (var booking in bookings)
            {
                var bookingDetails =  _bookingDetailService.GetBookingDetailByBookingId(booking.BookingId);
                if (bookingDetails == null)
                {
                    Console.WriteLine($"No booking details found for BookingId {booking.BookingId}");
                    continue;
                }

                var completedDetails = bookingDetails.Where(bd => bd.Status == "Pending").ToList();
                completedBookingDetails.AddRange(completedDetails);
            }

            var bookingSummary = completedBookingDetails.Select(detail => new
            {

                ServiceName = detail.ServiceId != null ? _serviceService.GetServiceById(detail.ServiceId)?.ServiceName ?? "Unknown Service" : "Unknown Service",
                Stylist = _userService.GetUserNameByUserId(availableSlotService.GetAvailableSlotById(detail.AvailableSlotId).UserId),
                Date = detail.ScheduledWorkingDay?.ToShortDateString() ?? "No Date",
                Slot = slotService.GetSlotById(availableSlotService.GetAvailableSlotById(detail.AvailableSlotId).SlotId).StartTime,
                Price = detail.Service?.Price ?? 0,
                BookingDetailId = detail.BookingDetailId  // Make sure this exists
            }).ToList();


            dtgBookingFeedback.ItemsSource = bookingSummary;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompletedBookingDetails();
            InitializeServiceDropdown();
            InitializeStylistDropdown();
        }

        

        private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                int bookingDetailId = (int)button.Tag;
                CusFeedbackWindow feedbackWindow = new CusFeedbackWindow(bookingDetailId);
                feedbackWindow.FeedbackSavedCallback = LoadCompletedBookingDetails;
                feedbackWindow.ShowDialog();
            }
        }


        private void InitializeServiceDropdown()
        {
            List<Service> services = _serviceService.GetServiceList();
            cmbService.ItemsSource = services;
            cmbService.DisplayMemberPath = "ServiceName";
            cmbService.SelectedValuePath = "ServiceId";
        }

        private void InitializeStylistDropdown()
        {
            // Get the user with UserId = 2, assuming _userService.GetUserById exists
            List<User> stylist = stylistService.GetAllStylist();

            // Check if stylist exists
            if (stylist != null)
            {
                // Create a list with the single stylist to set as ItemsSource
                cmbStylist.ItemsSource = stylist;
                cmbStylist.DisplayMemberPath = "UserName";
                cmbStylist.SelectedValuePath = "UserId";
            }
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = datePicker.SelectedDate;
            int? selectedServiceId = cmbService.SelectedValue as int?;
            int? selectedStylistId = cmbStylist.SelectedValue as int?;

            var filteredBookingDetails = _bookingService.GetBookingsByUserId(userid)
                .SelectMany(b => _bookingDetailService.GetBookingDetailByBookingId(b.BookingId))
                .Where(bd => bd.Status == "Pending")
                .Where(bd => !selectedDate.HasValue || bd.ScheduledWorkingDay == selectedDate)
                .Where(bd => !selectedServiceId.HasValue || bd.ServiceId == selectedServiceId)
                .Where(bd => !selectedStylistId.HasValue || bd.AvailableSlot?.UserId == selectedStylistId)
                .Select(detail => new
                {
                    ServiceName = _serviceService.GetServiceById(detail.ServiceId).ServiceName,
                    Stylist = detail.AvailableSlot?.User?.UserName ?? "Unknown Stylist",
                    Date = detail.ScheduledWorkingDay?.ToShortDateString() ?? "No Date",
                    Slot = detail.AvailableSlot?.Slot?.StartTime.ToString() ?? "No Start Time",
                    BookingDetailId = detail.BookingDetailId
                })
                .ToList();

            dtgBookingFeedback.ItemsSource = filteredBookingDetails;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = null;
            cmbService.SelectedIndex = -1;
            cmbStylist.SelectedIndex = -1;
            LoadCompletedBookingDetails();
        }
    }
}
