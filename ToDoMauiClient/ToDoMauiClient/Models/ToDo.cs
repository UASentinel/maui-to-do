using System.ComponentModel;

namespace ToDoMauiClient.Models;

public class ToDo : INotifyPropertyChanged
{
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            if (_id == value)
                return;

            _id = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
        }
    }

    private string _text;
    public string Text
    {
        get => _text;
        set
        {
            if (_text == value)
                return;

            _text = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
