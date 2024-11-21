using FitSharpMaui.Models.Dtos;
using FitSharpMaui.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FitSharpMaui.Pages;

public partial class CustomerPersonalClassesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly ApiService _apiService;

    public CustomerPersonalClassesPage(ApiService apiService)
    {
        InitializeComponent();

        // Configura o HttpClient com a URL base
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://fitsharp.azurewebsites.net/") // URL base da API
        };
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.Title = "Your Personal Classes";

        try
        {
            // Obtém o token de autenticação
            var token = Preferences.Get("AuthToken", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                await DisplayAlert("Error", "You are not logged in.", "OK");
                return;
            }

            // Configura o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Chama o endpoint para obter as aulas inscritas
            var response = await _httpClient.GetAsync("api/PersonalClasses/Enrolled");

            if (response.IsSuccessStatusCode)
            {
                var enrolledClasses = await response.Content.ReadFromJsonAsync<List<PersonalClassDto>>();

                // Atualiza a interface com as aulas inscritas
                CustomerClassesStackLayout.Children.Clear();

                foreach (var personalClass in enrolledClasses)
                {
                    // Cria uma interface dinâmica para cada aula inscrita
                    var classFrame = new Frame
                    {
                        BorderColor = Colors.Gray,
                        CornerRadius = 8,
                        Padding = 10,
                        Content = new VerticalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label
                                {
                                    Text = personalClass.Title,
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = 18,
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                new Label
                                {
                                    Text = $"Gym: {personalClass.Gym}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                new Label
                                {
                                    Text = $"Instructor: {personalClass.Instructor}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                new Label
                                {
                                    Text = $"Duration: {personalClass.Start} - {personalClass.End}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                new Label
                                {
                                    Text = $"Rating: {personalClass.InstructorScore}/5",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Start
                                },
                                new Button
                                {
                                    Text = "Cancel Enrollment",
                                    BackgroundColor = Color.FromArgb("#B70D00"),
                                    TextColor = Colors.White,
                                    IsVisible = DateTime.TryParse(personalClass.Start, out var startDate) && startDate > DateTime.Now,
                                    Command = new Command(async () => await CancelEnrollment(personalClass.Id))
                                }
                            }
                        }
                    };

                    CustomerClassesStackLayout.Children.Add(classFrame);
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to load your personal classes.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task CancelEnrollment(int classId)
    {
        try
        {
            var token = Preferences.Get("AuthToken", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                await DisplayAlert("Error", "You are not logged in.", "OK");
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/PersonalClasses/Unenroll", classId);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "You have successfully unenrolled from the class.", "OK");

                // Atualiza a lista após cancelar a inscrição
                OnAppearing();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", errorMessage, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}