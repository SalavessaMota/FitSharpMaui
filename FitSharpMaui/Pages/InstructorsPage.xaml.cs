using FitSharpMaui.Models.Dtos;
using FitSharpMaui.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FitSharpMaui.Pages;

public partial class InstructorsPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly ApiService _apiService;

    public InstructorsPage(ApiService apiService)
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
        this.Title = "Our Instructors";

        try
        {
            // Obtém o token de autenticação (se existir)
            var token = Preferences.Get("AuthToken", string.Empty);

            // Configura o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("Bearer", token);

            // Chama o endpoint para obter a lista de instrutores
            var response = await _httpClient.GetAsync("api/Gyms/AllInstructors");

            // Verifica se a resposta foi bem-sucedida
            if (response.IsSuccessStatusCode)
            {
                var instructors = await response.Content.ReadFromJsonAsync<List<InstructorDto>>();

                // Limpa o layout existente
                InstructorsStackLayout.Children.Clear();

                // Adiciona os instrutores ao layout
                foreach (var instructor in instructors)
                {
                    var instructorFrame = new Frame
                    {
                        BorderColor = Colors.Gray,
                        CornerRadius = 8,
                        Padding = 10,
                        Content = new VerticalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Image
                                {
                                    Source = instructor.ImageFullPath,
                                    HeightRequest = 150,
                                    WidthRequest = 150,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = instructor.FullName,
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = 18,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = $"Speciality: {instructor.Speciality}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = $"Description: {instructor.Description}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = $"Gym: {instructor.GymName}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = $"Group Classes: {instructor.NumberOfGroupClasses}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = $"Personal Classes: {instructor.NumberOfPersonalClasses}",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = $"Rating: {instructor.Rating}/5",
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center
                                }
                            }
                        }
                    };

                    InstructorsStackLayout.Children.Add(instructorFrame);
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to load instructors.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
