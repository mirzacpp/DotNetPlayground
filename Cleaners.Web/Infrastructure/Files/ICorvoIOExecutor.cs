using System;

namespace Cleaners.Web.Infrastructure.Files
{
    /// <summary>
    /// Handles all <see cref="System.IO"/> operations
    /// </summary>
    public interface ICorvoIOExecutor
    {
        CorvoFileResult Execute<T>(Func<T> operations);

        void Execute(Action operation);
    }
}