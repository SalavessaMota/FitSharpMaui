using FitSharpMaui.Models.Dtos;
using FitSharpMaui.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FitSharpMaui.Pages;

public partial class PersonalClassesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly ApiService _apiService;

    public PersonalClassesPage(ApiService apiService)
    {
        InitializeComponent();

        // Configura o HttpClient
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://fitsharp.azurewebsites.net/") // URL base da API
        };
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.Title = "Personal Classes";

        try
        {
            // Obtém o token de autenticação
            var token = Preferences.Get("AuthToken", string.Empty);

            // Configura o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response;

            // Seleciona o endpoint correto com base no token
            if (string.IsNullOrEmpty(token))
            {
                response = await _httpClient.GetAsync("api/PersonalClasses/Available");
            }
            else
            {
                response = await _httpClient.GetAsync("api/PersonalClasses/Upcoming");
            }

            // Verifica se a resposta foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                var personalClasses = await response.Content.ReadFromJsonAsync<List<PersonalClassDto>>();

                // Limpa os elementos existentes no StackLayout
                PersonalClassesStackLayout.Children.Clear();

                // Renderiza as informações das aulas
                foreach (var personalClass in personalClasses)
                {
                    var classFrame = new Frame
                    {
                        BorderColor = Colors.Gray,
                        CornerRadius = 8,
                        Padding = 10,
                        Margin = new Thickness(5),
                        Content = new StackLayout
                        {
                            Spacing = 10,
                            Children =
                            {
                                new Label
                                {
                                    Text = personalClass.Title,
                                    FontSize = 18,
                                    FontAttributes = FontAttributes.Bold
                                },
                                new Label
                                {
                                    Text = $"Gym: {personalClass.Gym}",
                                    FontSize = 14
                                },
                                new Label
                                {
                                    Text = $"Instructor: {personalClass.Instructor}",
                                    FontSize = 14
                                },
                                new Label
                                {
                                    Text = $"Duration: {personalClass.Start} - {personalClass.End}",
                                    FontSize = 14
                                },
                                new Label
                                {
                                    Text = $"Instructor Rating: {personalClass.InstructorScore}/5",
                                    FontSize = 14
                                },
                                new Button
                                {
                                    Text = "Enroll",
                                    BackgroundColor = Color.FromArgb("#B70D00"),
                                    TextColor = Colors.White,
                                    IsVisible = true,
                                    Command = new Command(async () => await EnrollInClass(personalClass.Id))
                                }
                            }
                        }
                    };

                    PersonalClassesStackLayout.Children.Add(classFrame);
                }
            }
            else
            {
                // Exibe mensagem de erro em caso de falha
                await DisplayAlert("Error", $"Failed to load personal classes. ({response.ReasonPhrase})", "OK");
            }
        }
        catch (Exception ex)
        {
            // Tratamento de exceção genérica
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    private async Task EnrollInClass(int classId)
    {
        var token = Preferences.Get("AuthToken", string.Empty);

        if (string.IsNullOrEmpty(token))
        {
            bool goToLogin = await DisplayAlert("Error", "You are not logged in. Please log in to enroll in a class.", "Go to Login", "Cancel");
            if (goToLogin)
            {
                await Navigation.PushAsync(new LoginPage(_apiService));
            }
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.PostAsJsonAsync("api/PersonalClasses/Enroll", classId);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Success", "You have successfully enrolled in the class.", "OK");

            OnAppearing();
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Error", errorMessage, "OK");
        }
    }
}