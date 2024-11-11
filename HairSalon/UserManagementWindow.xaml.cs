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
        private IRoleService roleService;
        public UserManagementWindow()
        {
            InitializeComponent();
            userManagementService = new UserManagementService();
            roleService = new RoleService();
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                cmbRoleId.SelectedValue == null)
            {
                MessageBox.Show("Please fill all the blanks.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User user = new User
            {
                CreatedAt = DateTime.Now, // Set the current date and time
                RoleId = Convert.ToInt32(cmbRoleId.SelectedValue), // Assign selected RoleId
                UserName = txtUserName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                PhoneNumber = txtPhoneNumber.Text
            };

            if (userManagementService.AddUser(user))
            {
                MessageBox.Show("Added successfully");
                loadDataInit();
            }
            else
            {
                MessageBox.Show("Add failed");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ||
                cmbRoleId.SelectedValue == null)
            {
                MessageBox.Show("Please fill all the blanks.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User user = new User();
            int useridText = int.Parse(txtUserId.Text);
            user.UserId = useridText;
            user.RoleId = int.Parse(cmbRoleId.SelectedValue.ToString());
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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
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
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
        }

        private void dtgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtgUser.SelectedItem != null)
            {
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnAdd.IsEnabled = false;
            }
            else
            {
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
                btnAdd.IsEnabled = true;
            }

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
                    cmbRoleId.SelectedValue = user.RoleId;
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
            this.cmbRoleId.ItemsSource = roleService.GetRoles();
            this.cmbRoleId.DisplayMemberPath = "RoleName";
            this.cmbRoleId.SelectedValuePath = "RoleId";


            txtUserId.Text = "";
            txtUserName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtDateCreated.Text = "";
            txtPhoneNumber.Text = "";
            cmbRoleId.SelectedValue = null;
        }
    }
}
