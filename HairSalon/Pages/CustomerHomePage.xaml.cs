using HairSalon_BusinessObject.Models;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HairSalon.Pages
{
    /// <summary>
    /// Interaction logic for CustomerHomePage.xaml
    /// </summary>
    public partial class CustomerHomePage : Page
    {
        private IServiceService _serviceService;

        public CustomerHomePage()
        {
            InitializeComponent();
            _serviceService = new ServiceService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowService(); 
        }

        private void ShowService()
        {
            try
            {
                List<Service> services = _serviceService.GetServiceList(); 
                ServiceItemsControl.ItemsSource = services; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading services: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
