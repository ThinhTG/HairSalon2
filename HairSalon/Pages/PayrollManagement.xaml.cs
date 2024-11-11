
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
            List<string> months = Enumerable.Range(1, 12)
                                            .Select(i => new DateTime(1, i, 1).ToString("MMMM"))
                                            .ToList();
            foreach (var month in months)
            {
                cmbMonth.Items.Add(month);
            }
            cmbMonth.SelectedItem = months[DateTime.Now.Month - 1];


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
            LoadStylistServicesAndSalary();
            LoadStylistSalaryMonth();
        }




        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbMonth?.SelectedIndex >= 0 && cmbStylist1?.SelectedValue is int userId)
            {
                LoadStylistSalaryMonth();
            }
        }




        private void LoadStylistSalaryMonth()
        {
            if (cmbStylist1.SelectedValue is int userId)
            {
                int selectedMonth = cmbMonth.SelectedIndex + 1;
                int selectedYear = DateTime.Now.Year;

                List<StylistSalaryDTO> stylistSalaries = _stylistService.GetStylistsSalary(userId, selectedMonth, selectedYear);
                var totalService = _stylistService.GetTotalServices(userId, selectedMonth, selectedYear);
                decimal? totalSalary = _stylistService.GetTotalDailySalary(userId, selectedMonth, selectedYear);

                if (stylistSalaries != null)
                {
                    StylistDataGridMonth.ItemsSource = stylistSalaries.ToList();
                    txtTotalService.Text = $"Total Service: {totalService}";
                    txtTotalSalary.Text = totalSalary.HasValue ? $"Total Salary: {totalSalary.Value:N0}" : "Total Salary: Not available";
                    txtYear.Text = $"Year :{selectedYear.ToString()}";
                    return;
                }
                StylistDataGridMonth.ItemsSource = null;
            }
        }


        private void LoadStylistServicesAndSalary()
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                DateTime? selectedDate = datePicker.SelectedDate;
                var stylistServices = _stylistService.GetStylistServicesByUserId(userId, selectedDate);

                if (selectedDate.HasValue)
                {
                    var dailySalaryInfo = _stylistService.GetStylistDailySalaryByUserId(userId, selectedDate);
                    var dailySalary = dailySalaryInfo.FirstOrDefault()?.DailySalary;
                    txtDailySalary.Text = dailySalary.HasValue ? $"Daily Salary: {dailySalary.Value:N0}" : "Daily Salary: Not available";
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
            LoadStylistServicesAndSalary();
        }


        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadStylistServicesAndSalary();
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                try
                {
                    if (decimal.TryParse(txtSalary.Text, out var newSalary) && newSalary > 0)
                    {
                        if (_stylistService.UpdateSalaryByUserId(userId, newSalary))
                        {
                            MessageBox.Show("Update Salary Successful!");
                            LoadDataInit();
                        }
                        else
                        {
                            MessageBox.Show("Update Salary Failed!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid salary input. Please enter a positive number.");
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



        private void btnUpdateSalary_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStylist.SelectedValue is int userId)
            {
                DateTime? selectedDate = datePicker.SelectedDate;
                if (selectedDate.HasValue)
                {
                    if (_stylistService.CheckIfSalaryExists(selectedDate, userId))
                    {
                        MessageBox.Show("Salary for this date has already been updated. Can't update more time.");
                    }
                    else if (_stylistService.InsertDailySalaryOfStylist(selectedDate, userId))
                    {
                        MessageBox.Show("Daily salary update successfully!");
                        LoadDataInit();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update daily salary. Please check details.");
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


        private void cmbStylist1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbStylist1 == null || cmbMonth == null || txtYear == null)
            {
                return;
            }

            if (cmbStylist1.SelectedValue is int userId)
            {
                if (int.TryParse(txtYear.Text, out int year))
                {
                    int month = cmbMonth.SelectedValue is int selectedMonth ? selectedMonth : DateTime.Now.Month;
                    var stylistSalaries = _stylistService.GetStylistsSalary(userId, month, year);
                    StylistDataGridMonth.ItemsSource = stylistSalaries?.ToList() ?? null;
                    LoadStylistSalaryMonth();
                }
            }

        }
        
    }
}


