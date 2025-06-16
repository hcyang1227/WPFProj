using System.Windows.Input;

namespace App001_HelloWorld
{
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        public RelayCommand(Action execute) => this.execute = execute;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => execute();
    }

}
