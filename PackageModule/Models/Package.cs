using Prism.Mvvm;
using System.Xml.Linq;

namespace VersionController.PackageModule.Models
{
    public class Package : BindableBase
    {
        private string _name;
        private bool _isChecked;

        public string Name
        {
            get => _name;
            set { SetProperty(ref _name, value); }
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

        public Package(string name)
        {
            _name = name;
            _isChecked = false;
        }
    }
}
