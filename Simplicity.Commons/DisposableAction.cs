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

    /// <summary>
    /// Defines action that will be executed when object is disposed.
    /// </summary>
    /// <typeparam name="T">Type of the argument</typeparam>
    public class DisposableAction<T> : IDisposable
    {
        private readonly Action<T> _action;
        private readonly T _arg;

        public DisposableAction(Action<T> action, T arg)
        {
            _action = action;               
            _arg = arg;
        }

        public void Dispose() => _action(_arg);
    }
}