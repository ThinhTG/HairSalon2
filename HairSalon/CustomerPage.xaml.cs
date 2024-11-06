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

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string userName;
        public CustomerPage()
        {
            InitializeComponent();
            this.Loaded += CustomerPage_Loaded;
        }

        public CustomerPage(int id, string name)
        {
            this.userId = id;
            this.userName = name;
            this.DataContext = new CustomerViewModel { Name = name };
            InitializeComponent();

        }


        private void AddResourceIfNotExists(string key, string uri)
        {
            if (!this.Resources.Contains(key))
            {
                this.Resources.Add(key, new BitmapImage(new Uri(uri)));
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var HomePage = new CustomerHomePage();
            navframe.Navigate(HomePage);
        }


        /* private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             var selected = sidebar.SelectedItem as NavButton;

             navframe.Navigate(selected.Navlink);
         }*/
        private void CustomerPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Điều hướng mặc định đến trang CustomerHomePage
            navframe.Navigate(new Uri("/Pages/CustomerHomePage.xaml", UriKind.Relative));
        }
        private void CustomerHomePage_Click(object sender, MouseButtonEventArgs e)
        {
            navframe.Navigate(new Uri("/Pages/CustomerHomePage.xaml", UriKind.Relative));
        }

        private void BookingPage_Click(object sender, MouseButtonEventArgs e)
        {
            var selected = sender as NavButton2;
            if (selected != null)
            {
                selected.OnNavigateToPage += userId =>
                {
                    BookingPage bookingPage = new BookingPage(userId);
                    navframe.Navigate(bookingPage);
                };
                selected.RaiseOnNavigateToPage(userId);
            }
        }

        private void FeedbackPage_Click(object sender, MouseButtonEventArgs e)
        {
            navframe.Navigate(new Uri("/Pages/FeedbackPage.xaml", UriKind.Relative));
        }

        private void BookingHistory_Click(object sender, MouseButtonEventArgs e)
        {
            var selected = sender as NavButton2;
            if (selected != null)
            {
                selected.OnNavigateToPage += userId =>
                {
                    BookingHistory bookingHistory = new BookingHistory(userId);
                    navframe.Navigate(bookingHistory);
                };
                selected.RaiseOnNavigateToPage(userId);
            }
        }
    }
}