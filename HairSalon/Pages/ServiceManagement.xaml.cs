using HairSalon_BusinessObject.Models;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ServiceManagement.xaml
    /// </summary>
    public partial class ServiceManagement : Page
    {
        private int? RoleID = 0;
        private IServiceService serviceService;
        public ServiceManagement()
        {
            serviceService = new ServiceService();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
        private void loadDataInit()
        {
            txtServiceName.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtImage.Text = "";
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Service service = new Service();

            if (int.TryParse(txtServiceId.Text, out int serviceId))
            {
                service.ServiceId = serviceId;
            }
            else
            {
                MessageBox.Show("Invalid Service ID");
                return;
            }

            if (decimal.TryParse(txtPrice.Text, out decimal price))
            {
                service.Price = price;
            }
            else
            {
                MessageBox.Show("Invalid Price");
                return;
            }

            service.ServiceName = txtServiceName.Text;
            service.Description = txtDescription.Text;

            try
            {
                if (!string.IsNullOrEmpty(txtImage.Text))
                {
                    service.Image = File.ReadAllBytes(txtImage.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load image.");
                return;
            }

            if (serviceService.AddService(service))
            {
                MessageBox.Show("Add Successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Service service = new Service();

            if (int.TryParse(txtServiceId.Text, out int serviceId))
            {
                service.ServiceId = serviceId;
            }
            else
            {
                MessageBox.Show("Invalid Service ID");
                return;
            }

            if (decimal.TryParse(txtPrice.Text, out decimal price))
            {
                service.Price = price;
            }
            else
            {
                MessageBox.Show("Invalid Price");
                return;
            }

            service.ServiceName = txtServiceName.Text;
            service.Description = txtDescription.Text;

            try
            {
                if (!string.IsNullOrEmpty(txtImage.Text))
                {
                    service.Image = File.ReadAllBytes(txtImage.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load image.");
                return;
            }

            if (serviceService.UpdateService(service))
            {
                MessageBox.Show("Update Successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Failed");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtServiceId.Text, out int serviceId))
            {
                if (serviceService.DeleteService(serviceId))
                {
                    MessageBox.Show("Delete successful");
                    loadDataInit();
                }
                else
                {
                    MessageBox.Show("Something's wrong!");
                }
            }
            else
            {
                MessageBox.Show("Invalid Service ID");
            }
        }

    }
}
