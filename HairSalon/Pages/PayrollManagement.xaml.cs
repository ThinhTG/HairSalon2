
using HairSalon_BusinessObject.Models;
using HairSalon_DAO.DTO;
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
            cmbMonth.Items.Clear();
            int currentMonth = DateTime.Now.Month;
            List<string> months = (from i in Enumerable.Range(1, 12)
                                   select new DateTime(1, i, 1).ToString("MMMM")).ToList();
            foreach (string month in months)
            {
                cmbMonth.Items.Add(month);
            }
            cmbMonth.SelectedIndex = currentMonth - 1;


            cmbStylist.Items.Clear();
            List<User> stylists = _stylistService.GetAllStylist();
            foreach (User stylist2 in stylists)
            {
                cmbStylist.Items.Add(stylist2);
            }
            cmbStylist.DisplayMemberPath = "UserName";
            cmbStylist.SelectedValuePath = "UserId";
            cmbStylist.SelectedIndex = 0;

            cmbStylist1.Items.Clear();
            List<User> stylist3 = _stylistService.GetAllStylist();
            foreach (User stylist in stylist3)
            {
                cmbStylist1.Items.Add(stylist);
            }
            cmbStylist1.DisplayMemberPath = "UserName";
            cmbStylist1.SelectedValuePath = "UserId";
            cmbStylist1.SelectedIndex = 0;

            cmbMonth.SelectionChanged += cmbMonth_SelectionChanged;
            cmbStylist.SelectionChanged += cmbStylist_SelectionChanged;
            LoadStylistServices();
            LoadYears();
            LoadStylistByMonth();
        }

        private void LoadYears()
        {
            int currentYear = DateTime.Now.Year;
            cmbYear.Items.Clear(); // Xóa các mục hiện tại trong ComboBox

            // Thêm năm hiện tại vào ComboBox và chọn nó
            var currentYearItem = new ComboBoxItem
            {
                Content = currentYear.ToString()
            };
            cmbYear.Items.Add(currentYearItem);
            cmbYear.SelectedItem = currentYearItem; // Chọn mục vừa thêm
        }


        private void CmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedItem is ComboBoxItem selectedItem)
            {
                int selectedYear = int.Parse(selectedItem.Content.ToString());
            }
        }

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbMonth == null || cmbYear == null || cmbStylist1 == null)
            {
                return;
            }
            int month = cmbMonth.SelectedIndex + 1;
            int year = cmbYear.SelectedValue is int selectedYear ? selectedYear : DateTime.Now.Year;
            if (cmbStylist1.SelectedValue is int userId)
            {
                LoadStylistByMonth();
            }
        }



        private void LoadStylistByMonth()
        {
            if (cmbStylist1.SelectedValue is int userId)
            {
                int selectedMonth = cmbMonth.SelectedIndex + 1;
                int selectedYear = DateTime.Now.Year;
                if (cmbYear.SelectedItem is ComboBoxItem selectedYearItem)
                {
                    selectedYear = int.Parse(selectedYearItem.Content.ToString());
                }
                List<StylistSalaryDTO> stylistSalaries = _stylistService.GetStylistsSalary(userId, selectedMonth, selectedYear);
                var totalService = _stylistService.GetTotalServices(userId, selectedMonth, selectedYear);
                decimal? totalSalary = _stylistService.GetTotalDailySalary(userId, selectedMonth, selectedYear);

                if (stylistSalaries != null && stylistSalaries.Any())
                {
                    StylistDataGridMonth.ItemsSource = stylistSalaries.ToList();
                    txtTotalService.Text = $"Total Service: {totalService}";
                    txtTotalSalary.Text = totalSalary.HasValue ? $"Total Salary: {totalSalary.Value:N0}" : "Total Salary: Not available";
                    return;
                }
                StylistDataGridMonth.ItemsSource = null;
            }
            else
            {
                MessageBox.Show("Please select a stylist.");
            }
        }

        private void LoadStylistServices()
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                DateTime? selectedDate = datePicker.SelectedDate;
                var stylistServices = _stylistService.GetStylistServicesByUserId(userId, selectedDate);

                if (selectedDate.HasValue)
                {
                    var dailySalaryInfo = _stylistService.GetStylistDailySalaryByUserId(userId, selectedDate);
                    var dailySalary = dailySalaryInfo.FirstOrDefault()?.DailySalary;
                    txtDailySalary.Text = dailySalary.HasValue
                        ? $"Daily Salary: {dailySalary.Value:N0}"
                        : "Daily Salary: Not available";
                }
                else
                {
                    txtDailySalary.Text = "";
                }
                StylistDataGrid.ItemsSource = stylistServices.ToList();
            }
        }


        private void cmbStylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                decimal? salary = _stylistService.GetSalaryByUserId(userId);
                txtSalary.Text = (salary.HasValue ? $"{salary.Value:N0}" : "0");
            }
            LoadStylistServices();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStylistServices();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                try
                {
                    if (decimal.TryParse(txtSalary.Text, out var newSalary))
                    {
                        if (_stylistService.UpdateSalaryByUserId(userId, newSalary))
                        {
                            MessageBox.Show("Update Successful!");
                            LoadDataInit();
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
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    return;
                }
            }
            MessageBox.Show("Please select a stylist to update.");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }

        private void btnUpdateSalary_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                DateTime? selectedDate = datePicker.SelectedDate;
                if (selectedDate.HasValue)
                {
                    if (_stylistService.CheckIfSalaryExists(selectedDate, userId))
                    {
                        MessageBox.Show("Salary for this date has already been recorded. No further updates are necessary.");
                    }
                    else if (_stylistService.InsertDailySalaryOfStylist(selectedDate, userId))
                    {
                        MessageBox.Show("Daily salary recorded successfully!");
                        LoadDataInit();
                    }
                    else
                    {
                        MessageBox.Show("Failed to record daily salary. Please check the details.");
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

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbYear.SelectedItem is ComboBoxItem selectedItem)
            {
                int selectedYear = int.Parse(selectedItem.Content.ToString());
            }
        }

        private void cmbStylist1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (cmbStylist1 == null || cmbMonth == null || cmbYear == null)
            {
                return; 
            }
          
            if (cmbStylist1.SelectedValue is int userId)
            {
               
                int month = cmbMonth.SelectedValue is int selectedMonth ? selectedMonth : DateTime.Now.Month;
                int year = cmbYear.SelectedValue is int selectedYear ? selectedYear : DateTime.Now.Year;

                var stylistSalaries = _stylistService.GetStylistsSalary(userId, month, year);

                StylistDataGridMonth.ItemsSource = stylistSalaries?.ToList() ?? null; // Directly use null-coalescing operator
                LoadStylistByMonth();
            }
        }
    }
}

