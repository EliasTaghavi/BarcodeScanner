using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarcodeScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BeepCheck.IsChecked = Preferences.Get("Beep", false);
            VibrateCheck.IsChecked = Preferences.Get("Vibrate", false);
            ClipboardCheck.IsChecked = Preferences.Get("Clipboard", false);
            DuplicateCheck.IsChecked = Preferences.Get("Duplicate", false);
            HistoryCheck.IsChecked = Preferences.Get("History", false);
            BulkCheck.IsChecked = Preferences.Get("Bulk", false);
        }

        private void BeepCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("Beep", e.Value);
        }

        private void VibrateCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("Vibrate", e.Value);
        }

        private void ClipboardCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("Clipboard", e.Value);
        }

        private void DuplicateCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("Duplicate", e.Value);
        }
        private void HistoryCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("History", e.Value);
        }
        private void BulkCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Preferences.Set("Bulk", e.Value);
        }
    }
}