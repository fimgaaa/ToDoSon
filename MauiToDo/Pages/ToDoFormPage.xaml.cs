using MauiToDo.Models;
using MauiToDo.Services;
using System.Text;
using System.Text.Json;

namespace MauiToDo.Pages;

public partial class ToDoFormPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly ToDoItem _item = new(); // Null atanamaz hatasýný engeller;

    public string PageTitle => _item == null ? "Yeni Görev" : "Görev Güncelle";

    public ToDoFormPage(ApiService apiService, ToDoItem item = null)
    {
        InitializeComponent();
        BindingContext = this;

        _apiService = apiService;
        _item = item;

        if (_item != null)
        {
            TitleEntry.Text = _item.Title;
            DescriptionEditor.Text = _item.Description;
            DueDatePicker.Date = _item.DueDate;
            IsCompletedCheckBox.IsChecked = _item.IsCompleted;
        }
        else
        {
            DueDatePicker.Date = DateTime.Now;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var newItem = new ToDoItem
        {
            Title = TitleEntry.Text,
            Description = DescriptionEditor.Text,
            DueDate = DueDatePicker.Date,
            IsCompleted = IsCompletedCheckBox.IsChecked
        };

        var json = JsonSerializer.Serialize(newItem);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response;

        if (_item == null)
        {
            response = await _apiService.PostAsync("todo", content);
        }
        else
        {
            response = await _apiService.PutAsync($"todo/{_item.Id}", content);
        }

        if (!response.IsSuccessStatusCode)
        {
            ErrorLabel.Text = "Kaydetme iþlemi baþarýsýz.";
            ErrorLabel.IsVisible = true;
            return;
        }

        await Navigation.PopAsync(); // Listeye dön
    }
}
