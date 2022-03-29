//LeetCode.RunLengthOfLastWordProblem();

var cts = new CancellationTokenSource();

ThreadPool.QueueUserWorkItem(_ => Count(cts.Token, 1000));
Console.WriteLine("Press enter to cancel the operation.");
Console.ReadKey();
cts.Cancel();

Console.WriteLine("Main done.");
Console.ReadKey();

static void Count(CancellationToken token, int countTo)
{
    token.Register(() => Console.WriteLine("Tokens register event."));

    for (int i = 0; i < countTo; i++)
    {
        if (token.IsCancellationRequested)
        {
            Console.WriteLine("Count is cancelled");
            break;
        }

        Console.WriteLine(i);
        Thread.Sleep(200);
    }
    Console.WriteLine("COunt is done.");
}