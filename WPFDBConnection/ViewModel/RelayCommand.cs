using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFDBConnection.ViewModel
{
    public class RelayCommand : ICommand //This the first line Rlaycommand ICommand with the StudentViewModels ICommand.
    {
        private readonly Action<object> _execute;
     //Action is the method/function in the backend to be executed
     //Whenever the Relay Command is passed a Click-evet from the front-end

        private readonly Predicate<object> _canExecute;
        //This is just the mode if the relay command can actually execute the method
        //to be called
        //By default this is "true" in most cases

        //Relay command's constructor which we call from other classes

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        //Relay command's other constructor which we call from other classes
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        //usually set to ture because in most cases the RelayCommand must be
        //enabled to execute the backend functions it calls
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
