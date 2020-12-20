using Xamarin.Essentials;

namespace BarcodeScanner
{
    public class Settings
    {
        public bool Beep { get; set; }
        public bool Vibrate { get; set; }
        public bool Clipboard { get; set; }
        public bool Duplicate { get; set; }
        public bool History { get; set; }
        public bool Bulk { get; set; }
        public Settings()
        {
            Beep = Preferences.Get("Beep", false);
            Vibrate = Preferences.Get("Vibrate", false);
            Clipboard = Preferences.Get("Clipboard", false);
            Duplicate = Preferences.Get("Duplicate", false);
            History = Preferences.Get("History", false);
            Bulk = Preferences.Get("Bulk", false);
        }
    }
}
