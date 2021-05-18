using System;
using System.Threading.Tasks;

namespace Cleaners.Core.Tasks
{
    public static class TaskExtensions
    {
        public static async Task TimeoutAfter(this Task task, int millis)
        {
            if (task == await Task.WhenAny(task, Task.Delay(millis)))
            {
                await task;
            }
            else throw new TimeoutException();
        }
    }
}