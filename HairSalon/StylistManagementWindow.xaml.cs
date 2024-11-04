using HairSalon_BusinessObject.Models;
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
using System.Windows.Shapes;

namespace HairSalon
{
    /// <summary>
    /// Interaction logic for StylistManagementWindow.xaml
    /// </summary>
    public partial class StylistManagementWindow : Window
    {
        private IStylistManagementService stylistManagementService;
        public StylistManagementWindow()
        {
            stylistManagementService = new StylistManagementService();
            InitializeComponent();

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StylistProfile stylist = new StylistProfile();
            //stylist.UserId = int.Parse(txtUserId.Text);
            //stylist.Salary = decimal.Parse(txtSalary.Text);

            if (int.TryParse(txtUserId.Text, out int userId))
            {
                stylist.UserId = userId;
            }

            if (decimal.TryParse(txtSalary.Text, out decimal salary))
            {
                stylist.Salary = salary;
            }
            if (stylistManagementService.AddStylist(stylist))
            {
                MessageBox.Show("Add successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Add Falied");
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            StylistProfile stylist = new StylistProfile();
            int useridText = int.Parse(txtUserId.Text);
            int stylistId = int.Parse(txtStylistId.Text);
            decimal? salary = decimal.Parse(txtSalary.Text);
            stylist.UserId = useridText;
            stylist.StylistProfileId = stylistId;
            stylist.Salary = salary;
            if (stylistManagementService.UpdateStylist(stylist))
            {
                MessageBox.Show("Update successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Update Falied");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int stylistId = int.Parse(txtStylistId.Text);
            if (stylistManagementService.DeleteStylist(stylistId))
            {
                MessageBox.Show("Delete Successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Delete Failed");
            }
        }

        private void dtgStylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
            if (row != null)
            {
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                int IdText = int.Parse(id);
                StylistProfile stylist = stylistManagementService.GetStylistByUserId(IdText);
                if (stylist != null)
                {
                        txtUserId.Text = stylist.UserId.ToString();
                        txtStylistId.Text = stylist.StylistProfileId.ToString();
                        txtSalary.Text = stylist.Salary.HasValue ? stylist.Salary.Value.ToString("F2") : "";
                }
            }
        }

        private void loadDataInit()
        {
            this.dtgStylist.ItemsSource = stylistManagementService.GetStylistList();

            txtUserId.Text = "";
            txtStylistId.Text = "";
            txtSalary.Text = "";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
    }
}
