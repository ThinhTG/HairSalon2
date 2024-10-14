using HairSalon_BusinessObject.Models;
using HairSalon_Services;
using Microsoft.IdentityModel.Tokens;
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
using System.Text.RegularExpressions;

namespace HairSalon.Pages
{
    /// <summary>
    /// Interaction logic for RegistorPage.xaml
    /// </summary>
    public partial class RegistorPage : Page
    {

        private UserService userService;
        public RegistorPage()
        {
            InitializeComponent();
            userService = new UserService();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTextBox.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(EmailTextBox.Text) || string.IsNullOrEmpty(PhoneNumberTextBox.Text) || string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Please fill out all the fields.");
                return;
            }


            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string email = EmailTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;

            // Validate phone number to contain only digits
            if (!phoneNumber.All(char.IsDigit) || phoneNumber.Length == 9)
            {
                MessageBox.Show("Please enter a valid phone number with digits only.");
                return;
            }

            int roleId = 1;
            DateTime createdAt = DateTime.Now;
            User user = new User();
            user.UserName = username;
            user.Email = email;
            user.Password = password;
            user.PhoneNumber = phoneNumber;
            user.RoleId = roleId;
            user.CreatedAt = createdAt;

            if (userService.AddUser(user) && password.Equals(confirmPassword))
            {
                MessageBox.Show($"Username: {username}\nEmail: {email}\nPhone Number: {phoneNumber}", "Registration Successful");
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();  // This opens the LoginWindow as a separate window

            }
            else
            {
                MessageBox.Show("Contact 113!");
            }




        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();   // Mở lại LoginWindow
            Window.GetWindow(this).Close();  // Đóng cửa sổ chứa RegistorPage (hoặc cửa sổ hiện tại)
        }



    }
}

