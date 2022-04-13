using System.Windows;
using System.Windows.Input;
using Winsomnia.Command;
using Winsomnia.Utility;

namespace Winsomnia.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public NotifyIconViewModel NotifyIconViewModel { get; set; }

        /// <summary>
        /// ABorts and closes the settings window.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => true,
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow.Close();
                    }
                };
            }
        }

        public MainWindowViewModel(NotifyIconViewModel notifyIconViewModel)
        {
            NotifyIconViewModel = notifyIconViewModel;
        }
    }
}