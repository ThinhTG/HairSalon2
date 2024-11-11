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
            var selectedItem = sidebar.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                var stackPanel = selectedItem.Content as StackPanel;

                if (stackPanel != null)
                {
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
