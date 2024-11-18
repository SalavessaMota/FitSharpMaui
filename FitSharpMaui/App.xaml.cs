using FitSharpMaui.Pages;
using FitSharpMaui.Services;

namespace FitSharpMaui
{
    public partial class App : Application
    {
        private readonly ApiService _apiService;

        public App(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;

            SetMainPage();
        }

        private void SetMainPage()
        {
            var accessToken = Preferences.Get("AuthToken", string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                MainPage = new NavigationPage(new LoginPage(_apiService))
                {
                    BarBackgroundColor = (Color)Resources["NavigationBarColor"],
                    BarTextColor = (Color)Resources["NavigationBarTextColor"]
                };
            }
            else
            {
                MainPage = new NavigationPage(new AppShell(_apiService))
                {
                    BarBackgroundColor = (Color)Resources["NavigationBarColor"],
                    BarTextColor = (Color)Resources["NavigationBarTextColor"]
                };
            }
        }
    }
}
