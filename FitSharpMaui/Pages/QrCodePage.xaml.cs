using FitSharpMaui.Services;
using System.Net.Http.Headers;

namespace FitSharpMaui.Pages;

public partial class QrCodePage : ContentPage
{
    private readonly HttpClient _httpClient;

    public QrCodePage()
    {
        InitializeComponent();

        // Configura o HttpClient com a URL base
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://fitsharp.azurewebsites.net/") // URL base da API
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Obtém o token de autenticação
            var token = Preferences.Get("AuthToken", string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                await DisplayAlert("Error", "You are not logged in. Please log in to view your QR Code.", "OK");
                await Navigation.PopAsync();
                return;
            }

            // Configura o cabeçalho de autorização
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Faz a chamada ao endpoint da API para obter o QR Code
            var response = await _httpClient.GetAsync("api/Account/GenerateQRCode");

            if (response.IsSuccessStatusCode)
            {
                // Carrega o QR Code como uma imagem
                var qrCodeStream = await response.Content.ReadAsStreamAsync();
                QrCodeImage.Source = ImageSource.FromStream(() => qrCodeStream);

                // Atualiza a interface
                LoadingIndicator.IsVisible = false;
                QrCodeLayout.IsVisible = true;
            }
            else
            {
                // Exibe uma mensagem de erro se falhar
                var errorMessage = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", errorMessage, "OK");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            await Navigation.PopAsync();
        }
    }
}
