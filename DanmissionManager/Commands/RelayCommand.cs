using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanmissionManager.Commands
{
    class RelayCommand
    {
        private readonly Predicate<object> canExecutePredicate;

        private readonly Action<object> executeAction;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<object> executeAction, Predicate<object> canExecutePredicate = null)
        {
            this.executeAction = executeAction;
            this.canExecutePredicate = canExecutePredicate;
        }

        public RelayCommand(Action<object> executeAction)
        {
            this.executeAction = executeAction;
        }
        public bool CanExecute(object parameter) => this.canExecutePredicate?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => this.executeAction?.Invoke(parameter);

        public void NotifyCanExecuteChanged() => this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
