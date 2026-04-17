using System.Net.Http.Json;
using Task = AppMobile.Models.Task;
namespace AppMobile;

public partial class NovaTarefaPage : ContentPage
{
	public NovaTarefaPage()
	{
		InitializeComponent();
		
	}
	
    private async void Button_Clicked(object sender, EventArgs e)
    {

		if (string.IsNullOrEmpty(txtDescricao.Text) ||string.IsNullOrEmpty (txtNome.Text))
		{
			await DisplayAlert("Erro ao cadastrar", "Por favor preencha todos os campos", "OK");
			return;
		}


		Task newTask = new Task
		{
			Nome = txtNome.Text,
            Descrição = txtDescricao.Text,
			Data = DateTime.Now
        };


        HttpClient client = new HttpClient();
        string url = "http://10.0.2.2:5123/api/tasks";

		try
		{
			var response = await client.PostAsJsonAsync(url, newTask);
			if (response.IsSuccessStatusCode)
			{
				await DisplayAlert("Sucesso", "Cadastro com sucesso", "OK");
				await Navigation.PushModalAsync(new HomePage());
			}
			else await DisplayAlert("Erro", "Erro no cadastro", "OK");
		}
		catch (Exception ex) { 
			await DisplayAlert("Erro", "Erro de comunicação" +ex.Message, "OK");
		}
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new HomePage()); 
    }
}