//namespace MauiToDo
//{
//    public partial class MainPage : ContentPage
//    {
//        int count = 0;

//        public MainPage()
//        {
//            InitializeComponent();
//        }

//        private void OnCounterClicked(object sender, EventArgs e)
//        {
//            count++;

//            if (count == 1)
//                CounterBtn.Text = $"Clicked {count} time";
//            else
//                CounterBtn.Text = $"Clicked {count} times";

//            SemanticScreenReader.Announce(CounterBtn.Text);
//        }
//    }
//---------------------------------------------------------
////}
//using MauiToDo.Models;
//using MauiToDo.Services;

//namespace MauiToDo;

//public partial class MainPage : ContentPage
//{
//    private readonly ApiService _apiService = new();

//    public MainPage()
//    {
//        InitializeComponent();
//        LoadToDos();
//    }

//    private async void LoadToDos()
//    {
//        _apiService.Token = "BURAYA_LOGIN_TOKEN"; // Test amaçlı şimdilik token’ı buraya elle yaz
//        var todos = await _apiService.GetToDosAsync();
//        ToDoListView.ItemsSource = todos;
//    }
//    private async void OnAddClicked(object sender, EventArgs e)
//    {
//        await Navigation.PushAsync(new ToDoFormPage(_apiService));
//    }

//}
using MauiToDo.Models;
using MauiToDo.Services;
using MauiToDo.Pages;

namespace MauiToDo;

public partial class MainPage : ContentPage
{
    private readonly ApiService _apiService;

    public MainPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        LoadToDos();
    }

    private async void LoadToDos()
    {
        var todos = await _apiService.GetToDosAsync();
        ToDoListView.ItemsSource = todos;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ToDoFormPage(_apiService));
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.CommandParameter as ToDoItem;

        if (item != null)
            await Navigation.PushAsync(new ToDoFormPage(_apiService, item));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var item = button?.CommandParameter as ToDoItem;

        if (item == null)
            return;

        bool confirm = await DisplayAlert("Sil", "Bu görevi silmek istiyor musunuz?", "Evet", "Hayır");
        if (!confirm)
            return;

        var result = await _apiService.DeleteAsync($"todo/{item.Id}");

        if (result.IsSuccessStatusCode)
            LoadToDos();
        else
            await DisplayAlert("Hata", "Silme işlemi başarısız.", "Tamam");
    }
    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePasswordPage(_apiService));
    }


}
