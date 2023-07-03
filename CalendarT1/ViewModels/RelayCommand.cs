using System.Windows.Input;

public class RelayCommand : ICommand
{
	private readonly Action _execute;
	private readonly Func<bool> _canExecute;
	private event EventHandler canExecuteChanged;

	public RelayCommand(Action execute, Func<bool> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	public bool CanExecute(object parameter)
	{
		return _canExecute == null || _canExecute();
	}

	public void Execute(object parameter)
	{
		_execute();
	}

	public event EventHandler CanExecuteChanged
	{
		add { canExecuteChanged += value; }
		remove { canExecuteChanged -= value; }
	}

	public void RaiseCanExecuteChanged()
	{
		canExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}

public class RelayCommand<T> : ICommand
{
	private readonly Action<T> _execute;
	private readonly Predicate<T> _canExecute;
	private event EventHandler canExecuteChanged;

	public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
	{
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	public bool CanExecute(object parameter)
	{
		return _canExecute == null || _canExecute((T)parameter);
	}

	public void Execute(object parameter)
	{
		_execute((T)parameter);
	}

	public event EventHandler CanExecuteChanged
	{
		add { canExecuteChanged += value; }
		remove { canExecuteChanged -= value; }
	}

	public void RaiseCanExecuteChanged()
	{
		canExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
