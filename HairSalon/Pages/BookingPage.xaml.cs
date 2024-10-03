using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICES;
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
    /// Interaction logic for BookingPage.xaml
    /// </summary>
    public partial class BookingPage : Page
    {
        private IStylistService stylistService;
        private ISalonServiceService serviceService;
        public BookingPage()
        {
            InitializeComponent();
            stylistService = new StylistService();
            serviceService = new SalonServiceService();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataInit();
        }

        private void LoadDataInit()
        {
            this.stylistComboBox.ItemsSource = stylistService.GetStylists();
            this.stylistComboBox.DisplayMemberPath = "FullName";
            this.stylistComboBox.SelectedValuePath = "StylistId";

            this.serviceComboBox.ItemsSource = serviceService.GetServiceList();
            this.serviceComboBox.DisplayMemberPath = "ServiceName";
            this.serviceComboBox.SelectedValuePath = "ServiceId";
        }
    }
}
