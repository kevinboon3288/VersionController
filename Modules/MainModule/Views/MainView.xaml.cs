using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainModule.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            MainViewModel vm = (MainViewModel)this.DataContext;
            vm.LogReceived += OnLogReceived;
        }

        private void OnLogReceived(object? sender, EventArgs e)
        {
            if (e is LogEventArgs eventArgs)
            {
                try
                {
                    LogMessageBox.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        string appendMessage = eventArgs.LogMessage;

                        LogMessageBox.AppendText(appendMessage += Environment.NewLine);
                        LogMessageBox.ScrollToEnd();
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
