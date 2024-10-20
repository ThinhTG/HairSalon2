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

        public BookingManagement()
        {
            InitializeComponent();
            bookingService = new BookingService();
        }

        private void loadDataInit()
        {
            this.dtgBooking.ItemsSource = bookingService.GetBookings();
        }


        

        private void WindowLoad(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
