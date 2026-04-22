using AppMobile.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;

namespace AppMobile;

public partial class EditarTarefa : ContentPage
{
    private Models.Task _tarefaSelecionada; 
    ObservableCollection<SubTask> _listSubTasks = new ObservableCollection<SubTask>();

    public EditarTarefa(Models.Task task)
    {
        InitializeComponent();
        _tarefaSelecionada = task;

        this.BindingContext = _tarefaSelecionada;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSubTasks(); 
    }

    private async void LoadSubTasks()
    {
        try { 
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"http://10.0.2.2:5123/api/subtasks?taskId={_tarefaSelecionada.Id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var listaDeSub = JsonSerializer.Deserialize<List<SubTask>>(content, options);

                if (listaDeSub != null)
                {
                    _listSubTasks.Clear();
                    foreach (var item in listaDeSub) _listSubTasks.Add(item);
                    colSubTasks.ItemsSource = _listSubTasks;
                }

                if(_tarefaSelecionada.Concluida.Value == true)
                {
                    lblStatus.Text = "Concluído";
                }
                else
                {
                    lblStatus.Text = "Pendente";
                }
            }
            else
            {
                await DisplayAlert("Erro API", $"Servidor retornou erro: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro de Conexão", $"Detalhes: {ex.Message}", "OK");
        }
    }

    private async void Alterar_Clicked(object sender, EventArgs e)
    {
       
        if(lblStatus.Text == "Concluído")
        {
            lblStatus.Text = "Pendente";
            _tarefaSelecionada.Concluida = false;
        }
        else
        {
            lblStatus.Text = "Concluído";
            _tarefaSelecionada.Concluida = true;

        }

        HttpClient client = new HttpClient();
        string url = $"http://10.0.2.2:5123/api/tasks/status/{_tarefaSelecionada.Id}";

        try
        {

            var response = await client.PutAsJsonAsync(url, _tarefaSelecionada);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Sucesso", "As alterações foram salvas", "OK");
            }
            else
            {
                string erroDetalhado = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Erro da API", $"Status: {response.StatusCode}\nDetalhes: {erroDetalhado}", "OK");
            }
        }


        catch (Exception ex)
        {
            await DisplayAlert("Erro", "erro ao salvar", "OK");
        }
    }


    private async void Salvar_Clicked(object sender, EventArgs e)   
    {
        if (!string.IsNullOrEmpty(txtNome.Text)){
            _tarefaSelecionada.Nome = txtNome.Text;
        }
        if (!string.IsNullOrEmpty(txtDescricao.Text))
            _tarefaSelecionada.Descrição = txtDescricao.Text;
        HttpClient client = new HttpClient();
        string url = $"http://10.0.2.2:5123/api/tasks/{_tarefaSelecionada.Id}";

        try
        {

            var response = await client.PutAsJsonAsync(url, _tarefaSelecionada);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Sucesso", "As alterações foram salvas", "OK");
            }
            else
            {
                string erroDetalhado = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Erro da API", $"Status: {response.StatusCode}\nDetalhes: {erroDetalhado}", "OK");
            }
        }
        catch (Exception ex) {
            await DisplayAlert("Erro", "erro ao salvar", "OK");
        }
    }
}