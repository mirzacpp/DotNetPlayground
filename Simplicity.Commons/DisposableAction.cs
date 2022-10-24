namespace Simplicity.Commons
{
	/// <summary>
	/// Defines action that will be executed when object is disposed.
	/// </summary>
	public class DisposableAction : IDisposable
	{
		private readonly Action _action;

		public DisposableAction(Action action)
		{
			_action = action;
		}

		public void Dispose() => _action();
	}
}