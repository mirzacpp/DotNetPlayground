//LeetCode.RunLengthOfLastWordProblem();

await foreach (var item in YieldAsync())
{
    Console.WriteLine($"Captured {item}");
}

IEnumerable<int> YieldSync()
{
    for (int i = 1; i <= 3; i++)
    {
        Console.WriteLine($"Return {i}");
        yield return i;
        Console.WriteLine("Move on.");
    }
}

async IAsyncEnumerable<int> YieldAsync()
{
    for (int i = 1; i <= 3; i++)
    {
        Console.WriteLine($"Return {i}");
        yield return i;
        Console.WriteLine($"Wait for {1_000 * i}ms");
        await Task.Delay(1_000 * i);        
    }
}

//var cts = new CancellationTokenSource();

//ThreadPool.QueueUserWorkItem(_ => Count(cts.Token, 1000));
//Console.WriteLine("Press enter to cancel the operation.");
//Console.ReadKey();
//cts.Cancel();

////LeetCode.RunShuffleStringProblem();

//Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
//var task = new Task(() => Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is printing message."));
//task.Start();

//Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

//Console.ReadLine();

//void Print()
//{
//    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is printing message.");
//}

//async void AwaitVoid()
//{
//    await Task.CompletedTask;
//    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is awaiting void.");
//}

//async Task AwaitTask()
//{
//    await Task.CompletedTask;
//    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is awaiting Task.");
//}