using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ToDoMauiClient.Models;

namespace ToDoMauiClient.DataServices;

public class RestDataService : IRestDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseAddress;
    private readonly string _url;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public RestDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5259" : "https://localhost:7288";
        _url = $"{_baseAddress}/api";

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<List<ToDo>> GetAllToDosAsync()
    {
        var toDos = new List<ToDo>();

        if (!CheckInternetConnection())
            return toDos;

        try
        {
            var response = await _httpClient.GetAsync($"{_url}/todo");

            if (!CheckHttpRequestSuccess(response, nameof(GetAllToDosAsync)))
                return toDos;

            var content = await response.Content.ReadAsStringAsync();
            toDos = JsonSerializer.Deserialize<List<ToDo>>(content, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return toDos;
    }

    public async Task AddToDoAsync(ToDo toDo)
    {
        if (!CheckInternetConnection())
            return;

        try
        {
            var jsonToDo = JsonSerializer.Serialize(toDo, _jsonSerializerOptions);
            var content = new StringContent(jsonToDo, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_url}/todo", content);

            if (!CheckHttpRequestSuccess(response, nameof(AddToDoAsync)))
                return;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return;

    }

    public async Task UpdateToDoAsync(ToDo toDo)
    {
        if (!CheckInternetConnection())
            return;

        try
        {
            var jsonToDo = JsonSerializer.Serialize(toDo, _jsonSerializerOptions);
            var content = new StringContent(jsonToDo, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_url}/todo/{toDo.Id}", content);

            if (!CheckHttpRequestSuccess(response, nameof(UpdateToDoAsync)))
                return;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return;
    }

    public async Task DeleteToDoAsync(int id)
    {
        if (!CheckInternetConnection())
            return;

        try
        {
            var response = await _httpClient.DeleteAsync($"{_url}/todo/{id}");

            if (!CheckHttpRequestSuccess(response, nameof(DeleteToDoAsync)))
                return;
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return;
    }

    private bool CheckInternetConnection()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Debug.WriteLine("---> No internet access.");
            return false;
        }

        return true;
    }

    private bool CheckHttpRequestSuccess(HttpResponseMessage response, string methodName)
    {
        if (response.IsSuccessStatusCode)
        {
            Debug.WriteLine($"---> {methodName} success.");
            return true;
        }
        else
        {
            Debug.WriteLine($"---> {methodName} error: {response.StatusCode}.");
            return false;
        }
    }

    private void HandleException(Exception ex)
    {
        Debug.WriteLine($"---> Exception occured: {ex.Message}.");
    }
}
