using HairSalon.Pages;
using HairSalon.ViewModel;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CustomerPage : Window
    {
        private int userId;

        private string userName;
        public CustomerPage()
        {
            InitializeComponent();
        }

        public CustomerPage(int id, string name)
        {
            this.DataContext = new CustomerViewModel { Name = name };
            InitializeComponent();
        }



        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = sidebar.SelectedItem as NavButton;

            navframe.Navigate(selected.Navlink);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var HomePage = new CustomerHomePage();
            navframe.Navigate(HomePage);
        }
    }
}