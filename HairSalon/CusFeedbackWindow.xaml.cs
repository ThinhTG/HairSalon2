using HairSalon_BusinessObject.Models;
using HairSalon_Services.SERVICE;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HairSalon
{
    public partial class CusFeedbackWindow : Window
    {
        private readonly FeedbackService _feedbackService;
        private readonly BookingDetailService _bookingDetailService;
        private int _id;
        private string _interest = string.Empty;
        private Button _lastSelectedButton; // Store reference to last selected button
        public Action FeedbackSavedCallback { get; set; }

        public CusFeedbackWindow()
        {
            InitializeComponent();
            _feedbackService = new FeedbackService();
            _bookingDetailService = new BookingDetailService();
        }

        public CusFeedbackWindow(int bookingdetailid)
        {
            InitializeComponent();
            _id = bookingdetailid;
            _feedbackService = new FeedbackService();
            _bookingDetailService = new BookingDetailService();
        }

        private void SaveFeedbackToDatabase(string interest, string description)
        {
            var feedback = new Feedback
            {
                Interest = interest,
                Description = description,
                BookingdetailId = _id
            };

            bool isSaved = _feedbackService.addFeedback(feedback);

            if (isSaved)
            {
                MessageBox.Show("Phản hồi của bạn đã được lưu thành công!");
                // Trigger the FeedbackSavedCallback action if it’s set
                _bookingDetailService.UpdateBookingDetailStatus(_id, "feedbacked");
                // Store reference to current button as last selected
                FeedbackSavedCallback?.Invoke();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu phản hồi của bạn.");
            }
        }

        private void OnEmojiButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Update _interest based on the selected emoji
                switch (button.Content.ToString())
                {
                    case "😢":
                        _interest = "Rất không hài lòng";
                        break;
                    case "🙁":
                        _interest = "Không hài lòng";
                        break;
                    case "😐":
                        _interest = "Bình thường";
                        break;
                    case "🙂":
                        _interest = "Hài lòng";
                        break;
                    case "😁":
                        _interest = "Rất hài lòng";
                        break;
                }

                // Reset appearance of last selected button
                if (_lastSelectedButton != null)
                {
                    _lastSelectedButton.ClearValue(Button.BackgroundProperty);
                    _lastSelectedButton.FontWeight = FontWeights.Normal;
                }

                // Update appearance of current selected button
                button.Background = Brushes.LightBlue;  // Highlight the selected button
                button.FontWeight = FontWeights.Bold;

                
                _lastSelectedButton = button;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string description = txtDescription.Text;
            SaveFeedbackToDatabase(_interest, description);
            
            this.Close();
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            // Show placeholder if the TextBox is empty; otherwise, hide it
            PlaceholderText.Visibility = string.IsNullOrEmpty(txtDescription.Text) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
