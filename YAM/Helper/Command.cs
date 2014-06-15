using System;
using System.Windows.Input;

namespace YAM
{
    public class Command : ICommand
    {
        public Command(Action<object> executeAction, Func<object, bool> canExecute)
            : this(executeAction)
        {
            if (canExecute != null)
                this.CanExecute += new CanExecuteEventHandler((s, e) => e.CanExecute = canExecute(e.Parameter));
        }

        public Command(Action<object> executeAction)
            : this()
        {
            if (executeAction != null)
                this.Executed += new ExecutedEventHandler((s, e) => executeAction(e.Parameter));
        }

        public Command(Action executeAction)
            : this()
        {
            if (executeAction != null)
                this.Executed += new ExecutedEventHandler((s, e) => executeAction());
        }

        public Command(Action executeAction, Func<bool> canExecute)
            : this(executeAction)
        {
            if (canExecute != null)
                this.CanExecute += new CanExecuteEventHandler((s, e) => e.CanExecute = canExecute());
        }

        public Command()
        {
        }

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add
            {
                canExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                canExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        private event EventHandler canExecuteChanged;

        public void RequerySuggested()
        {
            if (this.canExecuteChanged != null)
                this.canExecuteChanged(this, EventArgs.Empty);
        }

        public event CanExecuteEventHandler CanExecute;

        bool ICommand.CanExecute(object parameter)
        {
            var e = new CanExecuteEventArgs(this, parameter);

            if (CanExecute != null)
                CanExecute(this, e);

            return e.CanExecute;
        }

        public event ExecutedEventHandler Executed;

        void ICommand.Execute(object parameter)
        {
            if (Executed != null)
                Executed(this, new ExecutedEventArgs(this, parameter));
        }

        #endregion
    }

    public delegate void CanExecuteEventHandler(object sender, CanExecuteEventArgs e);

    /// <summary>
    /// Provides data for the DialogCommand.CanExecute events.
    /// </summary>
    public class CanExecuteEventArgs
    {
        /// <summary>
        /// Gets the command that was invoked.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the Command
        /// associated with this event can be executed on the command target.
        /// </summary>
        public bool CanExecute { get; set; }

        /// <summary>
        /// Gets the command specific data.
        /// </summary>
        public object Parameter { get; private set; }

        public CanExecuteEventArgs(ICommand command, object parameter)
        {
            this.Command = command;
            this.Parameter = parameter;
            this.CanExecute = true;
        }
    }

    public delegate void ExecutedEventHandler(object sender, ExecutedEventArgs e);

    /// <summary>
    /// Provides data for the DialogCommand.Executed events.
    /// </summary>
    public class ExecutedEventArgs
    {
        /// <summary>
        /// Gets the command that was invoked.
        /// </summary>
        public ICommand Command { get; private set; }


        /// <summary>
        /// Gets the command specific data.
        /// </summary>
        public object Parameter { get; private set; }

        public ExecutedEventArgs(ICommand command, object parameter)
        {
            this.Command = command;
            this.Parameter = parameter;
        }
		
    }
}
