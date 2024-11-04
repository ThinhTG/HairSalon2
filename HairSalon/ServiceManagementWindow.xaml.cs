using HairSalon_BusinessObject.Models;
using HairSalon_Services.INTERFACE;
using HairSalon_Services.SERVICE;
using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace HairSalon
{
    /// <summary>
    /// Interaction logic for ServiceManagementWindow.xaml
    /// </summary>
    public partial class ServiceManagementWindow : Window
    {
        private IServiceService serviceService;
        private string currentFilePath;
        public ServiceManagementWindow()
        {
            serviceService = new ServiceService();
            InitializeComponent();
        }

        private void loadDataInit()
        {
            this.dtgService.ItemsSource = serviceService.GetServiceList();

            txtServiceName.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtImage.Text = "";
        }



        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Service service = new Service();
            //string servicePrice = service.Price.ToString();
            //string serviceImage = service.Image.ToString();

            if (decimal.TryParse(txtPrice.Text, out decimal price))
            {
                service.Price = price;
            }
            service.Description = txtDescription.Text;
            service.ServiceName = txtServiceName.Text;
            if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
            {
                service.Image = File.ReadAllBytes(currentFilePath);
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

        //private void btnUpdate_Click(object sender, RoutedEventArgs e)
        //{
        //    Service service = new Service();

        //    string serviceId = service.ServiceId.ToString();
        //    string servicePrice = service.Price.ToString();
        //    //string serviceImage = service.Image.ToString();

        //    serviceId = txtServiceId.Text;
        //    servicePrice = txtPrice.Text;
        //    service.Description = txtDescription.Text;
        //    service.ServiceName = txtServiceName.Text;
        //    if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
        //    {
        //        service.Image = File.ReadAllBytes(currentFilePath);
        //    }

        //    if (serviceService.UpdateService(service))
        //    {
        //        MessageBox.Show("Update Successfully");
        //        loadDataInit();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Failed");
        //    }
        //}

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtServiceId.Text, out int serviceId))
            {
                
                Service service = serviceService.GetServiceById(serviceId);
                if (service != null)
                {
                    
                    service.ServiceName = txtServiceName.Text;
                    service.Description = txtDescription.Text;

                    
                    if (decimal.TryParse(txtPrice.Text, out decimal price))
                    {
                        service.Price = price;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid price.");
                        return;
                    }

                    
                    if (!string.IsNullOrEmpty(currentFilePath) && File.Exists(currentFilePath))
                    {
                        service.Image = File.ReadAllBytes(currentFilePath);
                    }

                    
                    if (serviceService.UpdateService(service))
                    {
                        MessageBox.Show("Update Successfully");
                        loadDataInit();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update.");
                    }
                }
                else
                {
                    MessageBox.Show("The service with the specified ID does not exist.");
                }
            }
            else
            {
                MessageBox.Show("Invalid Service ID.");
            }
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int serviceId = int.Parse(txtServiceId.Text);
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
        }


        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All files (*.*)|*.*",
                Title = "Select a File"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                txtImage.Text = openFileDialog.FileName;
                currentFilePath = openFileDialog.FileName;
            }
        }

        private void txtImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filePath = txtImage.Text;
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                currentFilePath = filePath;
            }
            else
            {
                txtImage.Text = string.Empty;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        private void dtgService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
            if (row != null)
            {
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                int IdText = int.Parse(id);
                Service service = serviceService.GetServiceById(IdText);
                if (service != null)
                {
                    txtServiceId.Text = service.ServiceId.ToString();
                    txtServiceName.Text = service.ServiceName;
                    txtDescription.Text = service.Description;
                    txtPrice.Text = service.Price.ToString();
                    txtImage.Text = service.Image != null && service.Image.Length > 0 ? "Image Available" : "No Image Available";


                    if (service.Image != null && service.Image.Length > 0)
                    {
                        using (var ms = new MemoryStream(service.Image))
                        {
                            BitmapImage bi = new BitmapImage();
                            bi.BeginInit();
                            bi.CacheOption = BitmapCacheOption.OnLoad;
                            bi.StreamSource = ms;
                            bi.EndInit();
                            imgService.Source = bi;
                        }
                    }
                    else
                    {
                        imgService.Source = null;
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
    }
}
