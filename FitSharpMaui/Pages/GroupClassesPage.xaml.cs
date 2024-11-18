using FitSharpMaui.Models.Dtos;
using FitSharpMaui.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FitSharpMaui.Pages;

public partial class GroupClassesPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly ApiService _apiService;

    public GroupClassesPage(ApiService apiService)
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

        try
        {
            var token = Preferences.Get("AuthToken", string.Empty);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/GroupClasses/Available");
            if (response.IsSuccessStatusCode)
            {
                var groupClasses = await response.Content.ReadFromJsonAsync<List<GroupClassDto>>();

                ClassesStackLayout.Children.Clear();
                foreach (var groupClass in groupClasses)
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
                                new Label { Text = groupClass.Title, FontSize = 18, FontAttributes = FontAttributes.Bold },
                                new Label { Text = $"Gym: {groupClass.Gym}", FontSize = 14 },
                                new Label { Text = $"Instructor: {groupClass.Instructor}", FontSize = 14 },
                                new Label { Text = $"Duration: {groupClass.Start} - {groupClass.End}", FontSize = 14 },
                                new Label { Text = $"Rating: {groupClass.InstructorScore}/5", FontSize = 14 },

                                new Button
                                {
                                    Text = "Enroll",
                                    BackgroundColor = Color.FromArgb("#B70D00"),
                                    TextColor = Colors.White,
                                    Command = new Command(async () => await EnrollInClass(groupClass.Id))
                                }
                            }
                        }
                    };

                    ClassesStackLayout.Children.Add(classFrame);
                }
            }
            else
            {
                await DisplayAlert("Error", "Failed to load group classes.", "OK");
            }
        }
        catch (Exception ex)
        {
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

        var response = await _httpClient.PostAsJsonAsync("api/GroupClasses/Enroll", classId);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Success", "Successfully enrolled in the class!", "OK");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Enrollment Failed", errorMessage, "OK");
        }
    }
}