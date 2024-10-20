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

        public string Navlink { get; set; } = string.Empty; // Initialize with a default value


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public event Action<int>? OnNavigateToPage;

        public void RaiseOnNavigateToPage(int userId)
        {
            OnNavigateToPage?.Invoke(userId);
        }

        public NavButton2()
        {
            this.MouseLeftButtonUp += NavButton_MouseLeftButtonUp;
        }

        private void NavButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow is CustomerPage customerPage)
            {
                OnNavigateToPage?.Invoke(customerPage.UserId);
            }
        }
    }
}
