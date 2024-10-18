using HairSalon.Pages;
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

namespace HairSalon
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private IUserService userService;

        public LoginWindow()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }
        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {

                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {

                textPassword.Visibility = Visibility.Visible;
            }
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) && string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter email and password");
                return;
            }
            else
            {
                User user = userService.GetUserByEmail(txtEmail.Text);
                if (user != null && txtPassword.Password.Equals(user.Password))
                {


                    if (user.RoleId == 1)
                    {
                        CustomerPage customerPage = new CustomerPage(user.UserId,user.UserName);
                        customerPage.Show();
                        this.Close();
                    }
                    if (user.RoleId == 3)
                    {
                        
                    }
                    else if (user.RoleId == 2)
                    {
                        StaffPage staffPage = new StaffPage();
                        staffPage.Show();
                    }

                }
                else
                {
                    MessageBox.Show("Invalid email or password");
                }

            }
        }
            private void Image_MouseUp(object sender, MouseButtonEventArgs e)
            {
                Application.Current.Shutdown();
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {

            }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegistorPage registerPage = new RegistorPage();
            this.Content = registerPage;
            MainFrame.Navigate(new RegistorPage());



        }
    }
    }

