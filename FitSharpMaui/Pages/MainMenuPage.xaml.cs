using FitSharpMaui.Models;
using FitSharpMaui.Services;

namespace FitSharpMaui.Pages;

public partial class MainMenuPage : ContentPage
{
    private readonly ApiService _apiService;

    public MainMenuPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.Title = "Main Menu";

        var token = Preferences.Get("AuthToken", string.Empty);
        QRCode.IsVisible = !string.IsNullOrEmpty(token);
        Login.IsVisible = string.IsNullOrEmpty(token);
    }

    private async void OnGroupClassesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GroupClassesPage(_apiService));
    }

    private async void OnPersonalClassesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PersonalClassesPage(_apiService));
    }

    private async void OnGymsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GymsPage(_apiService));
    }

    private async void OnInstructorsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InstructorsPage(_apiService));
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QrCodePage());
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage(_apiService));
    }
}