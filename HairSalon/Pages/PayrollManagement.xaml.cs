
using HairSalon_BusinessObject.Models;
using HairSalon_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HairSalon.Pages
{
    public partial class PayrollManagement : Page
    {
        private IStylistService _stylistService;

        public PayrollManagement()
        {
            InitializeComponent();
            _stylistService = new StylistService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataInit();
        }

        private void LoadDataInit()
        {
            // Cập nhật cmbMonth
            cmbMonth.Items.Clear();
            var months = Enumerable.Range(1, 12).Select(i => new DateTime(1, i, 1).ToString("MMMM")).ToList();
            foreach (var month in months)
            {
                cmbMonth.Items.Add(month);
            }
            cmbMonth.SelectedIndex = 0;


            // Cập nhật cmbStylist
            cmbStylist.Items.Clear();
            var stylists = _stylistService.GetAllStylist();
            foreach (var stylist in stylists)
            {
                cmbStylist.Items.Add(stylist);
            }
            cmbStylist.DisplayMemberPath = "UserName";
            cmbStylist.SelectedValuePath = "UserId";
            cmbStylist.SelectedIndex = 0;

            // Gắn lại sự kiện SelectionChanged
            cmbMonth.SelectionChanged += cmbMonth_SelectionChanged;
            cmbStylist.SelectionChanged += cmbStylist_SelectionChanged;

            // Lấy dữ liệu ban đầu cho stylist đầu tiên
            LoadStylistServices();
        }

        private void LoadStylistServices()
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                DateTime? selectedDate = datePicker.SelectedDate; // Lấy ngày đã chọn từ DatePicker

                IEnumerable<dynamic> stylistServices;

                // Trường hợp 1: Không chọn date, chỉ lấy dữ liệu theo userId
                if (!selectedDate.HasValue)
                {
                    stylistServices = _stylistService.GetStylistServicesByUserId(userId, null); // Truyền null cho ngày
                }
                // Trường hợp 2: Đã chọn cả date và userId
                else
                {
                    stylistServices = _stylistService.GetStylistServicesByUserId(userId, selectedDate);
                }

                StylistDataGrid.ItemsSource = stylistServices.ToList(); // Cập nhật ItemSource
            }
        }

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbStylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                decimal? salary = _stylistService.GetSalaryByUserId(userId);
                Console.WriteLine($"Fetched Salary: {salary}");
                txtSalary.Text = salary.HasValue ? string.Format("{0:N0}", salary.Value) : "0";
            }
            LoadStylistServices(); // Cập nhật dữ liệu khi stylist được chọn
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStylistServices(); // Cập nhật dữ liệu khi ngày được chọn
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                try
                {
                    if (decimal.TryParse(txtSalary.Text, out decimal newSalary))
                    {
                        bool isUpdated = _stylistService.UpdateSalaryByUserId(userId, newSalary);

                        if (isUpdated)
                        {
                            MessageBox.Show("Update Successful!");
                            LoadDataInit(); // Tải lại dữ liệu
                        }
                        else
                        {
                            MessageBox.Show("Update Failed!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid salary input. Please enter a valid number.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a stylist to update.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
        private void btnUpdateSalary_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn stylist hay chưa
            if (cmbStylist.SelectedValue is int userId)
            {
                // Lấy ngày được chọn từ DatePicker
                DateTime? selectedDate = datePicker.SelectedDate;

                // Kiểm tra xem người dùng đã chọn ngày hay chưa
                if (selectedDate.HasValue)
                {
                    // Kiểm tra xem lương đã được cập nhật cho stylist vào ngày này hay chưa
                    bool isSalaryUpdated = _stylistService.CheckIfSalaryExists(selectedDate.Value, userId);

                    if (isSalaryUpdated)
                    {
                        MessageBox.Show("Salary for this date has already been recorded. No further updates are necessary.");
                    }
                    else
                    {
                        // Gọi phương thức InsertDailySalaryOfStylist
                        bool isInserted = _stylistService.InsertDailySalaryOfStylist(selectedDate, userId);

                        // Cung cấp phản hồi dựa trên kết quả của việc chèn
                        if (isInserted)
                        {
                            MessageBox.Show("Daily salary recorded successfully!");
                            LoadDataInit(); // Tải lại dữ liệu để phản ánh thay đổi
                        }
                        else
                        {
                            MessageBox.Show("Failed to record daily salary. Please check the details.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a date to update the salary.");
                }
            }
            else
            {
                MessageBox.Show("Please select a stylist to update their salary.");
            }
        }

    }
}

