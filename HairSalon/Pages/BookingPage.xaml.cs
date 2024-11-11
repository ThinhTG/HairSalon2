
using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;


namespace HairSalon.Pages
{
    public partial class BookingPage : Page
    {
        private System.Timers.Timer? _timer;
        private static readonly object _lockObject = new object();

        private int userId;
        private readonly AvailableSlotService _availableSlotService;
        private readonly BookingDetailService _bookingDetailService;
        private readonly BookingService _bookingService;
        private readonly ServiceService _serviceService;
        private ObservableCollection<BookingDetail> tempBookingDetails = new ObservableCollection<BookingDetail>();

        public BookingPage()
        {
            InitializeComponent();
            _availableSlotService = new AvailableSlotService();
            _bookingDetailService = new BookingDetailService();
            _bookingService = new BookingService();
            _serviceService = new ServiceService();
            bookingSummaryDataGrid.ItemsSource = tempBookingDetails;
            datePicker.DisplayDateStart = DateTime.Now;
            datePicker.DisplayDateEnd = DateTime.Now.AddDays(31);

        }

        public BookingPage(int id)
        {
            InitializeComponent();
            this.userId = id;
            _availableSlotService = new AvailableSlotService();
            _bookingDetailService = new BookingDetailService();
            _bookingService = new BookingService();
            _serviceService = new ServiceService();
            bookingSummaryDataGrid.ItemsSource = tempBookingDetails;
            datePicker.DisplayDateStart = DateTime.Now;
            datePicker.DisplayDateEnd = DateTime.Now.AddDays(31);

        }

        private void LoadServices()
        {
            serviceComboBox.ItemsSource = _serviceService.GetServiceList();
            serviceComboBox.DisplayMemberPath = "ServiceName";
            serviceComboBox.SelectedValuePath = "ServiceId";
        }

        private void LoadStylists()
        {
            var stylists = _availableSlotService.GetStylists();
            stylistComboBox.ItemsSource = stylists;
            stylistComboBox.DisplayMemberPath = "UserName";
            stylistComboBox.SelectedValuePath = "UserId";
        }

        private void LoadSlots()
        {
            slotComboBox.ItemsSource = null;
            slotComboBox.Items.Clear();
            if (stylistComboBox.SelectedItem != null && datePicker.SelectedDate != null)
            {
                var selectedStylist = stylistComboBox.SelectedItem as User;
                int stylistId = selectedStylist.UserId;
                var selectedDate = datePicker.SelectedDate.Value;

                var availableSlots = _availableSlotService.GetAvailableSlotsByStylist(stylistId, selectedDate);

                if (availableSlots == null || !availableSlots.Any())
                {
                    MessageBox.Show("❌ There are no slots available for the selected stylist and date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var now = DateTime.Now;
                    var filteredSlots = availableSlots
                        .Where(a => a.Slot != null &&
                                   (selectedDate.Date != now.Date ||
                                   (a.Slot.StartTime.HasValue && a.Slot.StartTime.Value.ToTimeSpan() > now.TimeOfDay)))
                        .ToList();


                    if (!filteredSlots.Any())
                    {
                        MessageBox.Show("❌ There are no slots available for the selected stylist and date.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        slotComboBox.ItemsSource = filteredSlots;
                        slotComboBox.DisplayMemberPath = "Slot.StartTime";
                        slotComboBox.SelectedValuePath = "AvailableSlotId";
                    }
                }
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (serviceComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Please select a service.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (stylistComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Please choose a stylist.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("⚠️ Please choose a date.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (slotComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Please select a slot.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selectedService = serviceComboBox.SelectedItem as Service;
            var selectedSlot = slotComboBox.SelectedItem as AvailableSlot;

            if (selectedSlot == null)
            {
                MessageBox.Show("⚠️ Invalid slot.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var existingDetail = tempBookingDetails
                .FirstOrDefault(detail => detail.AvailableSlotId == selectedSlot.AvailableSlotId
                                          && detail.ScheduledWorkingDay == datePicker.SelectedDate.Value);

            if (existingDetail != null)
            {
                MessageBox.Show("⚠️ This slot has been added to the list below. Please choose another slot.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var bookingDetail = new BookingDetail
            {
                ServiceId = selectedService.ServiceId,
                AvailableSlotId = selectedSlot.AvailableSlotId,
                ScheduledWorkingDay = datePicker.SelectedDate.Value,
                Status = "Pending",
                Price = selectedService.Price,
                Service = selectedService,
                AvailableSlot = selectedSlot
            };

            tempBookingDetails.Add(bookingDetail);
            LoadBookingSummary();

            serviceComboBox.SelectedItem = null;
            stylistComboBox.SelectedItem = null;
            datePicker.SelectedDate = null;
            slotComboBox.ItemsSource = null;
            slotComboBox.Items.Clear();

            MessageBox.Show("✅ Service added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private Booking currentBooking;

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (tempBookingDetails.Count > 0)
            {
                var booking = new Booking
                {
                    BookingDate = DateTime.Now,
                    Amount = CalculateTotalAmount(),
                    Status = "Pending",
                    CreateBy = userId,
                    Discount = 0,
                    UserId = userId
                };

                int successfulDetails = 0;
                int failedDetails = 0;

                var result = MessageBox.Show("Do you want to book more services? \n - If yes, continue booking \n - If no, payment", "Booking Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);


                if (result == MessageBoxResult.Yes)
                {
                    return;
                }
                else if (result == MessageBoxResult.No)
                {
                    _bookingService.AddBooking(booking);

                    foreach (var detail in tempBookingDetails)
                    {
                        var service = _serviceService.GetServiceById(detail.Service.ServiceId);
                        var slot = _availableSlotService.GetAvailableSlotById(detail.AvailableSlot.AvailableSlotId);

                        if (service != null && slot != null)
                        {
                            detail.BookingId = booking.BookingId;
                            bool success = _bookingDetailService.AddBookingDetail(detail);

                            if (success)
                            {
                                _availableSlotService.UpdateSlotStatus(detail.AvailableSlotId, "Booked");
                                successfulDetails++;
                            }
                            else
                            {
                                failedDetails++;
                            }
                        }
                        else
                        {
                            failedDetails++;
                        }
                    }


                    tempBookingDetails.Clear();
                    LoadBookingSummary();

                    if (successfulDetails > 0)
                    {
                        MessageBox.Show($"✅ Successfully added {successfulDetails} order.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if (failedDetails > 0)
                    {
                        MessageBox.Show($"❌ There are {failedDetails} orders that cannot be added because the service or slot does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    serviceComboBox.SelectedItem = null;
                    stylistComboBox.SelectedItem = null;
                    datePicker.SelectedDate = null;
                    slotComboBox.ItemsSource = null;
                    slotComboBox.Items.Clear();
                    PaymentPage paymentPage = new PaymentPage(booking.BookingId);
                    this.NavigationService.Navigate(paymentPage);
                }
            }
            else
            {
                MessageBox.Show("⚠️ There is no order information to confirm.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private void LoadBookingSummary()
        {
            bookingSummaryDataGrid.ItemsSource = null;

            var bookingSummary = tempBookingDetails.Select(detail => new
            {
                Service = detail.Service?.ServiceName ?? "Unknown Service",
                Stylist = detail.AvailableSlot?.User?.UserName ?? "Unknown Stylist",
                Date = detail.ScheduledWorkingDay?.ToShortDateString() ?? "No Date",
                Slot = detail.AvailableSlot?.Slot?.StartTime.ToString() ?? "No Start Time",
                Price = detail.Service?.Price,

            }).ToList();

            bookingSummaryDataGrid.ItemsSource = bookingSummary;
            bookingSummaryDataGrid.Items.Refresh();
        }

        private decimal CalculateTotalAmount()
        {
            return tempBookingDetails.Sum(detail => detail.Price ?? 0);
        }

        private void Window_Load(object sender, RoutedEventArgs e)
        {
            LoadServices();
            LoadStylists();
            LoadSlots();
        }

        private void stylistComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePicker.SelectedDate == null)
            {
                slotComboBox.ItemsSource = null;
                slotComboBox.Items.Clear();
            }
            else
            {
                LoadSlots();
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSlots();
        }

        private void bookingSummaryDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bookingSummaryDataGrid.SelectedItem != null)
            {
                var selectedDetail = bookingSummaryDataGrid.SelectedItem as dynamic;
                if (selectedDetail != null)
                {
                    var detailToEdit = tempBookingDetails.FirstOrDefault(detail =>
                        detail.Service.ServiceName == selectedDetail.Service &&
                        detail.AvailableSlot.User.UserName == selectedDetail.Stylist &&
                        detail.ScheduledWorkingDay.Value.ToShortDateString() == selectedDetail.Date &&
                        detail.AvailableSlot.Slot.StartTime.ToString() == selectedDetail.Slot);

                    if (detailToEdit != null)
                    {
                        serviceComboBox.SelectedValue = detailToEdit.ServiceId;
                        stylistComboBox.SelectedValue = detailToEdit.AvailableSlot.User.UserId;
                        datePicker.SelectedDate = detailToEdit.ScheduledWorkingDay;
                        slotComboBox.SelectedValue = detailToEdit.AvailableSlotId;
                    }
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookingSummaryDataGrid.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Please select a line to update.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var selectedDetail = bookingSummaryDataGrid.SelectedItem as dynamic;

            if (selectedDetail == null)
            {
                MessageBox.Show("⚠️ There is no information to update.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selectedService = serviceComboBox.SelectedItem as Service;
            var selectedSlot = slotComboBox.SelectedItem as AvailableSlot;

            if (selectedService == null || selectedSlot == null || datePicker.SelectedDate == null)
            {
                MessageBox.Show("⚠️ Please fill in all information to update.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }


            var detailToUpdate = tempBookingDetails.FirstOrDefault(detail =>
                detail.Service.ServiceName == selectedDetail.Service &&
                detail.AvailableSlot.User.UserName == selectedDetail.Stylist &&
                detail.ScheduledWorkingDay.Value.ToShortDateString() == selectedDetail.Date &&
                detail.AvailableSlot.Slot.StartTime.ToString() == selectedDetail.Slot);

            var existingDetail = tempBookingDetails
            .FirstOrDefault(detail => detail.AvailableSlotId == selectedSlot.AvailableSlotId
                                  && detail.ScheduledWorkingDay == datePicker.SelectedDate.Value
                                  && detail != detailToUpdate);

            if (existingDetail != null)
            {
                MessageBox.Show("⚠️ This slot has been added to the list below. Please choose another slot.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (detailToUpdate != null)
            {
                detailToUpdate.ServiceId = selectedService.ServiceId;
                detailToUpdate.AvailableSlotId = selectedSlot.AvailableSlotId;
                detailToUpdate.ScheduledWorkingDay = datePicker.SelectedDate.Value;
                detailToUpdate.Price = selectedService.Price;
                detailToUpdate.Service = selectedService;
                detailToUpdate.AvailableSlot = selectedSlot;


                LoadBookingSummary();
                MessageBox.Show("✅ Service updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                serviceComboBox.SelectedItem = null;
                stylistComboBox.SelectedItem = null;
                datePicker.SelectedDate = null;
                slotComboBox.ItemsSource = null;
                slotComboBox.Items.Clear();
            }
            else
            {
                MessageBox.Show("❌ No information found to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookingSummaryDataGrid.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Please select a line to delete.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var selectedDetail = bookingSummaryDataGrid.SelectedItem as dynamic;
            var detailToDelete = tempBookingDetails.FirstOrDefault(detail =>
                detail.Service.ServiceName == selectedDetail.Service &&
                detail.AvailableSlot.User.UserName == selectedDetail.Stylist &&
                detail.ScheduledWorkingDay.Value.ToShortDateString() == selectedDetail.Date &&
                detail.AvailableSlot.Slot.StartTime.ToString() == selectedDetail.Slot);

            if (detailToDelete != null)
            {
                tempBookingDetails.Remove(detailToDelete);
                LoadBookingSummary();
                MessageBox.Show("✅Service deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                serviceComboBox.SelectedItem = null;
                stylistComboBox.SelectedItem = null;
                datePicker.SelectedDate = null;
                slotComboBox.ItemsSource = null;
                slotComboBox.Items.Clear();
            }
            else
            {
                MessageBox.Show("⚠️ No service found to delete.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}


