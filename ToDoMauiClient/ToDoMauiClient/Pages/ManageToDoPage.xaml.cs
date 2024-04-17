using ToDoMauiClient.DataServices;
using ToDoMauiClient.Models;

namespace ToDoMauiClient.Pages;

[QueryProperty(nameof(Models.ToDo), nameof(ManageToDoPage.ToDo))]
public partial class ManageToDoPage : ContentPage
{
    private readonly IRestDataService _restDataService;
    private ToDo _toDo;
    private bool _isNew;

    public ToDo ToDo
    {
        get => _toDo;
        set
        {
            _isNew = IsNew(value);
            _toDo = value;
            OnPropertyChanged();
        }
    }

    public ManageToDoPage(IRestDataService restDataService)
	{
		InitializeComponent();

		_restDataService = restDataService;
        BindingContext = this;
    }

    private bool IsNew(ToDo toDo)
    {
        if (toDo.Id == 0)
        {
            CreateToDoButtons.IsVisible = true;
            UpdateToDoButtons.IsVisible = false;
            return true;
        }

        CreateToDoButtons.IsVisible = false;
        UpdateToDoButtons.IsVisible = true;
        return false;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if(_isNew)
        {
            await _restDataService.AddToDoAsync(ToDo);
        }
        else
        {
            await _restDataService.UpdateToDoAsync(ToDo);
        }

        NavigateBack();
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        await _restDataService.DeleteToDoAsync(ToDo.Id);
        NavigateBack();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        NavigateBack();
    }

    private async void NavigateBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}