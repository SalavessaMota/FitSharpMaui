using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FitSharpMaui.Models.Dtos;
using FitSharpMaui.Services;

namespace FitSharpMaui.Pages
{
    public partial class MembershipPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public MembershipPage(ApiService apiService)
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
            this.Title = "Your Membership";

            await LoadMembershipAsync();
        }

        private async Task LoadMembershipAsync()
        {
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

                // Chama o endpoint para obter os detalhes da membership
                var response = await _httpClient.GetAsync("api/gyms/mymembership");

                if (response.IsSuccessStatusCode)
                {
                    var membership = await response.Content.ReadFromJsonAsync<CustomerMembershipDto>();

                    // Atualiza a interface com os detalhes da membership
                    MembershipDetailsStackLayout.Children.Clear();

                    MembershipDetailsStackLayout.Children.Add(new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Label
                            {
                                Text = "Name:",
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 16
                            },
                            new Label
                            {
                                Text = membership.Name,
                                FontSize = 16
                            }
                        }
                    });

                    MembershipDetailsStackLayout.Children.Add(new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Label
                            {
                                Text = "Classes Remaining:",
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 16
                            },
                            new Label
                            {
                                Text = membership.ClassesRemaining.ToString(),
                                FontSize = 16
                            }
                        }
                    });

                    MembershipDetailsStackLayout.Children.Add(new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Label
                            {
                                Text = "Membership Period:",
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 16
                            },
                            new Label
                            {
                                Text = $"{membership.MembershipBeginDate:dd MMM yyyy} - {membership.MembershipEndDate:dd MMM yyyy}",
                                FontSize = 16
                            }
                        }
                    });

                    MembershipDetailsStackLayout.Children.Add(new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Label
                            {
                                Text = "Description:",
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 16
                            },
                            new Label
                            {
                                Text = membership.Description,
                                FontSize = 16
                            }
                        }
                    });

                }
                else
                {
                    await DisplayAlert("Error", "Failed to load your membership details.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}