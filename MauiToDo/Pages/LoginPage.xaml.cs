using MauiToDo.Services;
using MauiToDo.Models;
using System.Text;
using System.Text.Json;



namespace MauiToDo.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginDto = new
        {
            Username = UsernameEntry.Text,
            Password = PasswordEntry.Text
        };
        Console.WriteLine($"Kullan�c� ad�: {UsernameEntry.Text}");
        Console.WriteLine($"�ifre: {PasswordEntry.Text}");


        var json = JsonSerializer.Serialize(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _apiService.PostAsync("auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            ErrorLabel.Text = "Giri� ba�ar�s�z.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var jsonResult = await response.Content.ReadAsStringAsync();
        var tokenObj = JsonSerializer.Deserialize<TokenResponse>(jsonResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _apiService.Token = tokenObj.Token;

        // Uygulama i�inde servisi payla�mak istersen DI/Singleton kullanabilirsin
        await Navigation.PushAsync(new MainPage(_apiService)); // Token'� MainPage'e g�nderiyoruz
    }
    private async void OnRegisterNavigateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

}

public class TokenResponse
{
    public string Token { get; set; }
}
