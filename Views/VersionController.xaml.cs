using System;
using System.Windows;
using VersionController.Services.Events;
using VersionController.ViewModels;

namespace VersionController
{
    /// <summary>
    /// Interaction logic for VersionController.xaml
    /// </summary>
    public partial class VersionControllerWindow : Window
    {
        public VersionControllerWindow()
        {
            InitializeComponent();

            VersionControllerViewModel vm = (VersionControllerViewModel)this.DataContext;
            vm.LogReceived += OnLogReceived;
        }

        private void OnLogReceived(object sender, EventArgs e) 
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
