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
    /// Interaction logic for UserManagementWindow.xaml
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        private IUserManagementService userManagementService;
        public UserManagementWindow()
        {
            InitializeComponent();
            userManagementService = new UserManagementService();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();

            if (int.TryParse(txtRoleId.Text, out int roleId))
            {
                user.RoleId = roleId;
            }

            DateTime dateCreated = DateTime.Now;
            if (DateTime.TryParse(txtDateCreated.Text, out dateCreated))
            {
                user.CreatedAt = dateCreated;
            }

            txtUserName.Text = user.UserName;
            txtEmail.Text = user.Email;
            txtPassword.Text = user.Password;
            txtPhoneNumber.Text = user.PhoneNumber;

            if (userManagementService.AddUser(user))
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
            User user = new User();
            int useridText = int.Parse(txtUserId.Text);
            int roleIdText = int.Parse(txtRoleId.Text);
            user.UserId = useridText;
            user.RoleId = roleIdText;
            user.UserName = txtUserName.Text;
            user.Email = txtEmail.Text;
            user.Password = txtPassword.Text;
            user.PhoneNumber = txtPhoneNumber.Text;
            user.CreatedAt = DateTime.Parse(txtDateCreated.Text);

            if (userManagementService.UpdateUser(user))
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
            int userId = int.Parse(txtUserId.Text);
            if (userManagementService.DeleteUser(userId))
            {
                MessageBox.Show("Delete Successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Delete Failed");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        private void dtgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
            if (row != null)
            {
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string id = ((TextBlock)RowColumn.Content).Text;
                int IdText = int.Parse(id);
                User user = userManagementService.GetUserById(IdText);
                if (user != null)
                {
                    txtUserId.Text = user.UserId.ToString();
                    txtUserName.Text = user.UserName;
                    txtEmail.Text = user.Email;
                    txtPassword.Text = user.Password;
                    txtDateCreated.Text = user.CreatedAt.ToString();
                    txtRoleId.Text = user.RoleId.ToString();
                    txtPhoneNumber.Text = user.PhoneNumber;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadDataInit();
        }
        private void loadDataInit()
        {
            this.dtgUser.ItemsSource = userManagementService.GetUserList();

            txtUserId.Text = "";
            txtUserName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtDateCreated.Text = "";
            txtRoleId.Text = "";
            txtPhoneNumber.Text = "";
        }
    }
}
