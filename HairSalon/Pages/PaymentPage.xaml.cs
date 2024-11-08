using HairSalon.ViewModel;
using HairSalon_BusinessObject.Models;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private System.Timers.Timer? _timer;
        private static readonly object _lockObject = new object();
        int bookingID;
        IBookingService iBookingService;
        IBookingDetailService iBookingDetailService;
        IUserService iUserService;
        IPaymentService iPaymentService;
        IAvailableSlotService iAvailableSlotService;
        ISlotService slotService;
        public PaymentPage()
        {
            InitializeComponent();
            iBookingService = null!;
            iBookingDetailService = null!;
            this.DataContext = new AvailableSlotViewModel();
        }

        public PaymentPage(int bookingId)
        {
            bookingID = bookingId;
            InitializeComponent();
            iBookingService = new BookingService();
            iBookingDetailService = new BookingDetailService();
            iUserService = new UserService();
            this.DataContext = new AvailableSlotViewModel();
            iPaymentService = new PaymentService();
            iAvailableSlotService = new AvailableSlotService();
            slotService = new SlotService();
            StartCancellationTimer();
            LoadData();

        }

        private void NavigateToCustomerPage()
        {
            // Ensure the NavigationService is not null
            if (this.NavigationService != null)
            {
                // Navigate to CustomerPage
                this.NavigationService.Navigate(new CustomerPage());
            }
            else
            {
                MessageBox.Show("NavigationService is not available.");
            }
        }

        private void grdAppointmentDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StartCancellationTimer()
        {
            _timer = new System.Timers.Timer(0.01 * 60 * 1000); // Every 2 minutes
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            await _semaphore.WaitAsync();

            try
            {
                Booking booking = await iBookingService.GetBookingByIdAsync(bookingID);

                if (booking.Status.Equals("Pending") && booking.BookingDate.HasValue && (DateTime.Now - booking.BookingDate.Value).TotalMinutes >= 0.5)
                {
                    iBookingService.UpdateBookingStatus(booking.BookingId, "Cancelled");
                    MessageBox.Show($"Booking was cancelled because of non-payment");
                    List<BookingDetail> bookingDetails = iBookingDetailService.GetBookingDetailByBookingId(booking.BookingId);

                    foreach (var bookingDetail in bookingDetails)
                    {
                        MessageBox.Show($"Booking was cancelled because of non-payment");
                        iAvailableSlotService.UpdateSlotStatus(bookingDetail.AvailableSlotId, "Unbooked");
                        iBookingDetailService.UpdateBookingDetailStatus(bookingDetail.BookingDetailId, "Cancelled");
                    }
                    Console.WriteLine($"Booking {booking.BookingId} has been canceled due to non-payment.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during booking cancellation: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

        }

        private async void LoadData()
        {
            List<BookingDetail> bookingDetails = iBookingDetailService.GetBookingDetailByBookingId(bookingID);

            var booking = await Task.Run(() => iBookingService.GetBookingById(bookingID));

            if (booking == null)
            {
                MessageBox.Show("Booking not found");
                return;
            }

            var user = await Task.Run(() => iUserService.GetUserById(booking.UserId));
            if (user == null)
            {
                MessageBox.Show("User not found");
                return;
            }

            this.DataContext = new
            {
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                CustomerEmail = user.Email,
                Subtotal = booking.Amount ?? bookingDetails.Sum(detail => detail.Price ?? 0)
            };

            grdAppointmentDetail.ItemsSource = null;



            var bookingSummary = bookingDetails.Select(detail => new
            {
                Service = detail.Service?.ServiceName ?? "Unknown Service",
                Stylist = iUserService.GetUserNameByUserId(detail.AvailableSlot.UserId),
                Date = detail.ScheduledWorkingDay?.ToShortDateString() ?? "No Date",
                Slot = slotService.GetSlotById(detail.AvailableSlot.SlotId).StartTime,
                Price = detail.Service?.Price,
            }).ToList();

            foreach (var detail in bookingDetails)
            {
                Console.WriteLine($"Service: {detail.Service?.ServiceName}, Stylist: {detail.AvailableSlot?.User?.UserName}, Slot Start Time: {detail.AvailableSlot?.Slot?.StartTime}");
            }


            grdAppointmentDetail.ItemsSource = bookingSummary;

        }

        public decimal Subtotal { get; set; }

        private async void Button_PayNow_Click(object sender, RoutedEventArgs e)
        {
            await _semaphore.WaitAsync();
            try
            {
                Payment payment = new Payment();

                Booking booking = await iBookingService.GetBookingByIdAsync(bookingID);
                List<BookingDetail> bookingDetails = iBookingDetailService.GetBookingDetailByBookingId(bookingID);

                if (booking.Status.Equals("Paid") || booking.Status.Equals("Cancelled"))
                {
                    MessageBox.Show("This booking has been paid or cancelled");
                    return;
                }

                payment.BookingId = bookingID;
                payment.TransactionDate = DateTime.Now;
                payment.TransactionType = "Cash";
                payment.Status = "Paid";
                payment.Amount = booking.Amount ?? bookingDetails.Sum(detail => detail.Price ?? 0);

                bool isSuccess = iPaymentService.AddPayment(payment);

                if (isSuccess)
                {
                    MessageBox.Show("Payment successful");
                    booking.Status = "Paid";
                    iBookingService.UpdateBookingStatus(booking.BookingId, "Paid");
                    User user = iUserService.GetUserById(booking.UserId);
                    CustomerPage customerPage = new CustomerPage(user.UserId, user.UserName);
                    customerPage.Show();
                    Window.GetWindow(this).Close();
                }
                else
                {
                    MessageBox.Show("Payment failed");
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
