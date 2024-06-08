namespace PackageModule.Models;

public class Package : BindableBase
{
    private string _name; 
    private string? _version;
    private bool _isChecked;

    public string Name
    {
        get => _name;
        set { SetProperty(ref _name, value); }
    }

    public string? Version
    {
        get => _version;
        set { SetProperty(ref _version, value); }
    }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            SetProperty(ref _isChecked, value);
        }
    }

    public Package() 
    {
        _name = string.Empty;
    }

    public Package(string name, string? version)
    {
        _name = name;
        _version = $"[{version}]";
        _isChecked = false;
    }
}
