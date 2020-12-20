using Xamarin.Forms;

namespace BarcodeScanner
{
    public partial class App : Application
    {
        private static CodeDatabase database;
        public static CodeDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new CodeDatabase();
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
