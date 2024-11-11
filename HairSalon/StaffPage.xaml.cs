using HairSalon.Pages;
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
    /// Interaction logic for StaffPage.xaml
    /// </summary>
    public partial class StaffPage : Window
    {
        public StaffPage()
        {
            InitializeComponent();
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the selected ListBoxItem
            var selectedItem = sidebar.SelectedItem as ListBoxItem;

            // Check if the selected item is not null
            if (selectedItem != null)
            {
                // Find the StackPanel within the ListBoxItem
                var stackPanel = selectedItem.Content as StackPanel;

                // Check if the StackPanel is not null
                if (stackPanel != null)
                {
                    // Get the NavButton from the StackPanel
                    var navButton = stackPanel.Children[0] as NavButton;

                    // Check if navButton is not null and navigate
                    if (navButton != null)
                    {
                        navframe.Navigate(navButton.Navlink);
                    }
                }
            }
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var HomePage = new CustomerHomePage();
            navframe.Navigate(HomePage);
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string navigationPath = button.Tag as string;
                if (!string.IsNullOrEmpty(navigationPath))
                {
                    try
                    {
                        // Lấy Frame chứa Page hiện tại
                        var frame = Window.GetWindow(this)?.Content as Frame;
                        if (frame != null)
                        {
                            frame.Navigate(new Uri(navigationPath, UriKind.Relative));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Navigation error: {ex.Message}");
                    }
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
