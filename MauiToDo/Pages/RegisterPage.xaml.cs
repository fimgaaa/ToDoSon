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
            ErrorLabel.Text = "Kayýt baþarýsýz.";
            ErrorLabel.IsVisible = true;
            return;
        }

        await DisplayAlert("Baþarýlý", "Kayýt tamamlandý. Giriþ yapabilirsiniz.", "Tamam");
        await Navigation.PopAsync(); // Login sayfasýna dön
    }
}
