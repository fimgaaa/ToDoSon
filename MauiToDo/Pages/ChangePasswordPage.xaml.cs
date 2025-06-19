using System.Text;
using System.Text.Json;
using MauiToDo.Services;

namespace MauiToDo.Pages;

public partial class ChangePasswordPage : ContentPage
{
    private readonly ApiService _apiService;

    public ChangePasswordPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        var dto = new
        {           
            CurrentPassword = CurrentPasswordEntry.Text,
            NewPassword = NewPasswordEntry.Text
        };

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _apiService.PostAsync("auth/change-password", content);

        if (!response.IsSuccessStatusCode)
        {
            ErrorLabel.Text = "Þifre deðiþtirilemedi.";
            ErrorLabel.IsVisible = true;
            return;
        }

        await DisplayAlert("Baþarýlý", "Þifreniz deðiþtirildi.", "Tamam");
        await Navigation.PopAsync();
    }
}
