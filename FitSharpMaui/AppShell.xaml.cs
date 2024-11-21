﻿using FitSharpMaui.Pages;
using FitSharpMaui.Services;

namespace FitSharpMaui
{
    public partial class AppShell : Shell
    {
        private readonly ApiService _apiService;

        public AppShell(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;

            ConfigureShell();
        }

        private void ConfigureShell()
        {
            var mainMenuPage = new MainMenuPage(_apiService);
            var profilePage = new ProfilePage(_apiService);

            Items.Add(new TabBar
            {
                Items =
                {
                    new ShellContent { Title = "Home",Icon = "home", Content = mainMenuPage },
                    new ShellContent { Title = "Profile",Icon = "profile", Content = profilePage }
                }
            });
        }
    }
}