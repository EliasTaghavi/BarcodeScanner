using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace BarcodeScanner
{
    public partial class MainPage : CarouselPage
    {

        public ObservableCollection<Barcode> CodeCollection { get; set; }
        public Settings Settings { get; set; }
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            Settings = new Settings();
            CodeCollection = new ObservableCollection<Barcode>();
            CodeList.ItemsSource = CodeCollection;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Settings = new Settings();
            ScanView.IsScanning = true;
            var s = App.Database.GetItemsAsync();
            s.GetAwaiter().OnCompleted(() =>
            {
                if (s.Result.Any() && s.IsCompletedSuccessfully)
                {
                    s.Result.ForEach(x => { CodeCollection.Add(x); });
                }
            });
            //var cameraPer
        }
        public void ScanView_OnScanResult(Result result)
        {
            if (Settings.Beep)
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   DependencyService.Get<IAudio>().PlayAudioFile("beep.mp3");
               });
            }
            if (Settings.Vibrate)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var duration = TimeSpan.FromSeconds(1);
                    Vibration.Vibrate(duration);
                });
            }
            if (Settings.Clipboard)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Clipboard.SetTextAsync(result.Text);
                });
            }

            if (Uri.TryCreate(result.Text, UriKind.Absolute, out Uri uriResult) && Settings.Bulk)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var x = await DisplayAlert("Web Link", $"Do you want to open:\r\n{result.Text}", "Yes", "No");
                    if (x)
                    {
                        await Browser.OpenAsync(uriResult, BrowserLaunchMode.SystemPreferred);
                    }
                });
            }
            else
            {
                if (Settings.Bulk)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Scanned result", "The barcode's text is " + result.Text + ". The barcode's format is " + result.BarcodeFormat, "OK");
                    });
                }
                else
                {
                    //toast
                }

            }
            var code = new Barcode { Text = result.Text, Time = DateTime.UtcNow, Type = result.BarcodeFormat.ToString() };
            if (Settings.Duplicate)
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Database.SaveItemAsync(code);
                });

                CodeCollection.Add(code);
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var barcode = await App.Database.GetItemByCodeAsync(code.Text);
                    if (barcode?.Text == code.Text)
                    {
                        barcode.Time = code.Time;
                        await App.Database.SaveItemAsync(barcode);
                        var x = CodeCollection.FirstOrDefault(x => x.Text == code.Text);
                        CodeCollection.Remove(x);
                        CodeCollection.Add(barcode);
                    }
                    else
                    {
                        await App.Database.SaveItemAsync(code);


                        CodeCollection.Add(code);
                    }
                });

            }
        }

        private void CarouselPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if (((CarouselPage)sender).CurrentPage == CameraPage)
            {

            }
        }

        private void FlashButton_Clicked(object sender, EventArgs e)
        {
            if (ScanView.HasTorch)
            {
                ScanView.ToggleTorch();
                if (ScanView.IsTorchOn)
                {
                    FlashButton.Source = FileImageSource.FromFile("flash_on.png");
                }
                else
                {
                    FlashButton.Source = FileImageSource.FromFile("flash_off.png");
                }
            }
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {

            //App.Current.MainPage = new NavigationPage(this);
            await App.Current.MainPage.Navigation.PushAsync(new SettingsPage());
            //App.Current.MainPage.Navigation.PushAsync(Page2);
        }
    }
}
