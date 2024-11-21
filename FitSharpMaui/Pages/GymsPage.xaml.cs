using FitSharpMaui.Models.Dtos;
using FitSharpMaui.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FitSharpMaui.Pages;

public partial class GymsPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly ApiService _apiService;

    public GymsPage(ApiService apiService)
    {
        InitializeComponent();

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://fitsharp.azurewebsites.net/") // URL base da API
        };
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        this.Title = "Our Gyms";

        try
        {
            // Configurar cabeçalho de autorização, se necessário
            var token = Preferences.Get("AuthToken", string.Empty);
            _httpClient.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("Bearer", token);

            // Fazer chamada ao endpoint da API
            var response = await _httpClient.GetAsync("api/Gyms/All");

            if (response.IsSuccessStatusCode)
            {
                var gyms = await response.Content.ReadFromJsonAsync<List<GymDto>>();

                // Limpar o StackLayout existente
                GymsStackLayout.Children.Clear();

                foreach (var gym in gyms)
                {
                    var gymFrame = new Frame
                    {
                        BorderColor = Colors.Gray,
                        CornerRadius = 8,
                        Padding = 10,
                        Margin = new Thickness(5),
                        Content = new VerticalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Image
                                {
                                    Source = gym.ImageUrl,
                                    HeightRequest = 150,
                                    Aspect = Aspect.AspectFill
                                },
                                new Label
                                {
                                    Text = gym.Name,
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = 18
                                },
                                new Label
                                {
                                    Text = $"Address: {gym.Address}, {gym.City}, {gym.Country}",
                                    FontSize = 14
                                },
                                new Label
                                {
                                    Text = $"Rooms: {gym.NumberOfRooms}",
                                    FontSize = 14
                                },
                                new Label
                                {
                                    Text = $"Equipments: {gym.NumberOfEquipments}",
                                    FontSize = 14
                                },
                                new Label
                                {
                                    Text = $"Rating: {gym.Rating}/5",
                                    FontSize = 14
                                }
                            }
                        }
                    };

                    GymsStackLayout.Children.Add(gymFrame);
                }
            }
            else
            {
                await DisplayAlert("Error", $"Failed to load gyms: {response.ReasonPhrase}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }
}
