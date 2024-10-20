using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HairSalon.Pages
{
    public partial class BookingPage : Page
    {
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
                    MessageBox.Show("❌ Không có slot nào cho stylist và ngày đã chọn.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        MessageBox.Show("❌ Không có slot nào hợp lệ cho stylist và ngày đã chọn.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("⚠️ Vui lòng chọn một dịch vụ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (stylistComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn stylist.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn một ngày.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (slotComboBox.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn một slot.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selectedService = serviceComboBox.SelectedItem as Service;
            var selectedSlot = slotComboBox.SelectedItem as AvailableSlot;

            if (selectedSlot == null)
            {
                MessageBox.Show("⚠️ Slot không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var existingDetail = tempBookingDetails
                .FirstOrDefault(detail => detail.AvailableSlotId == selectedSlot.AvailableSlotId
                                          && detail.ScheduledWorkingDay == datePicker.SelectedDate.Value);

            if (existingDetail != null)
            {
                MessageBox.Show("⚠️ Slot này đã được thêm vào danh sách bên dưới. Vui lòng chọn slot khác.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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

            MessageBox.Show("✅ Đã thêm dịch vụ thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (tempBookingDetails.Count > 0)
            {
                var booking = new Booking
                {
                    BookingDate = DateTime.Now,
                    Amount = CalculateTotalAmount(),
                    Status = "Pending",
                    CreateBy = 1,
                    Discount = 0,
                    UserId = userId
                };

                _bookingService.AddBooking(booking);

                int successfulDetails = 0;
                int failedDetails = 0;

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
                    MessageBox.Show($"✅ Đã thêm thành công {successfulDetails} booking detail(s).", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (failedDetails > 0)
                {
                    MessageBox.Show($"❌ Có {failedDetails} booking detail(s) không thể thêm do dịch vụ hoặc slot không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                serviceComboBox.SelectedItem = null;
                stylistComboBox.SelectedItem = null;
                datePicker.SelectedDate = null;
                slotComboBox.ItemsSource = null;
                slotComboBox.Items.Clear();
            }
            else
            {
                MessageBox.Show("⚠️ Không có thông tin booking detail để xác nhận.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                Image = detail.Service?.Image,
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
                MessageBox.Show("⚠️ Vui lòng chọn một dòng để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var selectedDetail = bookingSummaryDataGrid.SelectedItem as dynamic;

            if (selectedDetail == null)
            {
                MessageBox.Show("⚠️ Không có thông tin để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var selectedService = serviceComboBox.SelectedItem as Service;
            var selectedSlot = slotComboBox.SelectedItem as AvailableSlot;

            if (selectedService == null || selectedSlot == null || datePicker.SelectedDate == null)
            {
                MessageBox.Show("⚠️ Vui lòng điền đầy đủ thông tin để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            var detailToUpdate = tempBookingDetails.FirstOrDefault(detail =>
                detail.Service.ServiceName == selectedDetail.Service &&
                detail.AvailableSlot.User.UserName == selectedDetail.Stylist &&
                detail.ScheduledWorkingDay.Value.ToShortDateString() == selectedDetail.Date &&
                detail.AvailableSlot.Slot.StartTime.ToString() == selectedDetail.Slot);

            if (detailToUpdate != null)
            {
                detailToUpdate.ServiceId = selectedService.ServiceId;
                detailToUpdate.AvailableSlotId = selectedSlot.AvailableSlotId;
                detailToUpdate.ScheduledWorkingDay = datePicker.SelectedDate.Value;
                detailToUpdate.Price = selectedService.Price;
                detailToUpdate.Service = selectedService;
                detailToUpdate.AvailableSlot = selectedSlot;


                LoadBookingSummary();
                MessageBox.Show("✅ Đã cập nhật dịch vụ thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                serviceComboBox.SelectedItem = null;
                stylistComboBox.SelectedItem = null;
                datePicker.SelectedDate = null;
                slotComboBox.ItemsSource = null;
                slotComboBox.Items.Clear();
            }
            else
            {
                MessageBox.Show("❌ Không tìm thấy thông tin để cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookingSummaryDataGrid.SelectedItem == null)
            {
                MessageBox.Show("⚠️ Vui lòng chọn một dòng để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                MessageBox.Show("✅ Đã xóa dịch vụ thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                serviceComboBox.SelectedItem = null;
                stylistComboBox.SelectedItem = null;
                datePicker.SelectedDate = null;
                slotComboBox.ItemsSource = null;
                slotComboBox.Items.Clear();
            }
            else
            {
                MessageBox.Show("⚠️ Không tìm thấy dịch vụ để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
