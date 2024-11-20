using CommunityToolkit.Maui.Views;
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

    private async void OnGroupClassesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GroupClassesPage(_apiService));
    }

    private async void OnPersonalClassesClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new PersonalClassesPage(_apiService));
    }

    private async void OnGymsClicked(object sender, EventArgs e)
    {
       //await Navigation.PushAsync(new GymsPage(_apiService));
    }

    private async void OnInstructorsClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new InstructorsPage(_apiService));
    }
}