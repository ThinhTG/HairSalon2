using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HairSalon
{
    public class NavButton2 : ListBoxItem
    {
        public static readonly DependencyProperty NavlinkProperty =
             DependencyProperty.Register("Navlink", typeof(string), typeof(NavButton2), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconProperty =
             DependencyProperty.Register("Icon", typeof(ImageSource), typeof(NavButton2), new PropertyMetadata(null));

        public string Navlink
        {
            get { return (string)GetValue(NavlinkProperty); }
            set { SetValue(NavlinkProperty, value); }
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public event Action<int> OnNavigateToPage;

        public NavButton2()
        {
            this.MouseLeftButtonUp += NavButton_MouseLeftButtonUp;
        }

        private void NavButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Gọi sự kiện OnNavigateToPage khi nhấn vào NavButton
            if (Application.Current.MainWindow is CustomerPage customerPage)
            {
                OnNavigateToPage?.Invoke(customerPage.UserId);
            }
        }
    }
}