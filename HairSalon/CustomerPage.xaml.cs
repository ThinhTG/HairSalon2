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
        }

        public CustomerPage(int id, string name)
        {
            this.userId = id;
            this.userName = name;
            this.DataContext = new CustomerViewModel { Name = name };
            InitializeComponent();

            //AddResourceIfNotExists("closeIcon", "pack://application:,,,/HairSalon;component/image/close.png");
            //AddResourceIfNotExists("passwordIcon", "pack://application:,,,/HairSalon;component/image/password.png");
            //AddResourceIfNotExists("qrCodeIcon", "pack://application:,,,/HairSalon;component/image/qrCode.png");

        }


        //private void AddResourceIfNotExists(string key, string uri)
        //{
        //    if (!this.Resources.Contains(key))
        //    {
        //        this.Resources.Add(key, new BitmapImage(new Uri(uri)));
        //    }
        //}


        /* private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             var selected = sidebar.SelectedItem as NavButton;

             navframe.Navigate(selected.Navlink);
         }*/

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = sidebar.SelectedItem as NavButton2;

            if (selected != null)
            {

                selected.OnNavigateToPage += userId =>
                {

                    BookingPage bookingPage = new BookingPage(userId);
                    navframe.Navigate(bookingPage);
                };


                navframe.Navigate(new Uri(selected.Navlink, UriKind.Relative));
            }
        }

    }
}