using System.Text;
using System.Text.Json;
using MauiToDo.Services;

namespace MauiToDo.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService = new();

    public RegisterPage()
    {
        InitializeComponent();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        var dto = new
        {
            Username = UsernameEntry.Text,
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text
        };

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _apiService.PostAsync("auth/register", content);

        if (!response.IsSuccessStatusCode)
        {
            ErrorLabel.Text = "Kay�t ba�ar�s�z.";
            ErrorLabel.IsVisible = true;
            return;
        }

        await DisplayAlert("Ba�ar�l�", "Kay�t tamamland�. Giri� yapabilirsiniz.", "Tamam");
        await Navigation.PopAsync(); // Login sayfas�na d�n
    }
}
