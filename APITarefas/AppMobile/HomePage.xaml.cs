using AppMobile.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;
using Task = AppMobile.Models.Task;

namespace AppMobile;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        
    }
    ObservableCollection<Task> _listTasks = new ObservableCollection<Task>();

    List<Task> _listaIntermediaria = new List<Task>();
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
    }

    private async void AdicionarTarefa_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NovaTarefaPage());
    }

    private async void LoadData()
    {
         
        HttpClient client = new HttpClient();
        var response = await client.GetAsync("http://10.0.2.2:5123/api/tasks");
        var content = await response.Content.ReadAsStringAsync();

        if (content == "" || content == null)
            return;
     
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var listaDeApi = JsonSerializer.Deserialize<List<Task>>(content, options);

        if (listaDeApi != null)
        {
            _listTasks.Clear();
            foreach (var item in listaDeApi)
            {
                _listTasks.Add(item);
            }
            colTasks.ItemsSource = _listTasks;

            _listaIntermediaria = listaDeApi;
        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var question = await DisplayAlert("Alert - Tasks", "Deseja realmente remover a tarefa?", "Sim", "Não");

        if (question)
        {
            var task = (sender as ImageButton).BindingContext as Task;


            HttpClient client = new HttpClient();
            var response = await client.DeleteAsync($"http://10.0.2.2:5123/api/tasks/{task.Id}");
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sucesso", "Deletado com sucesso", "OK");
                }
                else await DisplayAlert("Erro", "Erro ao deletar", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Erro de comunicação" + ex.Message, "OK");
            }

            //DisplayAlert("Information - Tasks", "Tarefa removida com sucesso", "Ok");

            _listTasks.Remove(task);

        }
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        Task task = (sender as ImageButton).BindingContext as Task;
        await Navigation.PushModalAsync(new EditarTarefa(task));
    }

    private void entSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var newList = _listaIntermediaria
               .Where(b => string.IsNullOrWhiteSpace(entSearchBar.Text) || b.Nome.ToLower().Contains(entSearchBar.Text.ToLower()))
               .ToList();

        _listTasks.Clear();

        foreach (var item in newList)
        {
            _listTasks.Add(item);
        }
    }
}