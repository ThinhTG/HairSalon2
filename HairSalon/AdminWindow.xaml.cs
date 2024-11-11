using HairSalon.Pages;
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
using System.Windows.Shapes;

namespace HairSalon
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btnServiceManagement_Click(object sender, RoutedEventArgs e)
        {
            //ServiceManagement servicePage = new ServiceManagement();
            //MainFrame.Navigate(servicePage);
            ServiceManagementWindow serviceManagementWindow = new ServiceManagementWindow();
            serviceManagementWindow.Show();
            this.Close();
        }

        private void btnStylistManagement_Click(object sender, RoutedEventArgs e)
        {
            //StylistManagement stylistPage = new StylistManagement();
            //MainFrame.Navigate(stylistPage);
            StylistManagementWindow stylistManagementWindow = new StylistManagementWindow();
            stylistManagementWindow.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow userManagementWindow = new UserManagementWindow();
            userManagementWindow.Show();
            this.Close();
        }
    }
}
//using System.Windows;

//namespace HairSalon
//{
//    public partial class AdminWindow : Window
//    {
//        public AdminWindow()
//        {
//            InitializeComponent();
//        }

//        private void btnServiceManagement_Click(object sender, RoutedEventArgs e)
//        {
//            servicePage.Visibility = Visibility.Visible;
//            stylistPage.Visibility = Visibility.Collapsed;
//        }

//        private void btnStylistManagement_Click(object sender, RoutedEventArgs e)
//        {
//            servicePage.Visibility = Visibility.Collapsed;
//            stylistPage.Visibility = Visibility.Visible;
//        }

//        private void btnLogout_Click(object sender, RoutedEventArgs e)
//        {
//            // Logout logic here
//        }
//    }
//}
