using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TestApp.Samples
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IsHostedSample : Page
    {
        public IsHostedSample()
        {
            this.InitializeComponent();
        }

        public void SetShareText(string text)
        {
            SharedTextResult.Text = $"The shared text is: {text}";
        }

        private void ShareButton_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += (transferManager, args) =>
            {
                var request = args.Request;

                request.Data.Properties.Title = "Shared from Windows State Trigger sample";
                request.Data.Properties.Description = ShareTextBox.Text;
                request.Data.SetText(ShareTextBox.Text);
            };

            DataTransferManager.ShowShareUI();
        }
    }
}
