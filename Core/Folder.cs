using Prism.Mvvm;

namespace VersionController.Core
{
    public class Folder : BindableBase
    {
        private string _name;
        private string _status;

        public string Name
        {
            get => _name;
            set { SetProperty(ref _name, value); }
        }

        public string ImagePath
        {
            get => _status;
            set
            {
                SetProperty(ref _status, value);
            }
        }

        public Folder(string name, string imagePath)
        {
            _name = name;
            _status = imagePath;
        }
    }
}
