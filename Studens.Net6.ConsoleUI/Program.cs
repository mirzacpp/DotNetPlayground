var cts = new CancellationTokenSource();

//var tasks = new[] { 2000, 1000, 3000, 4000, 5000 }
//.Select(t => DoSomeWorkAsync(t));

Console.WriteLine("=====> Application started");
Console.WriteLine("=====> Press ENTER key to cancel ...");

Task cancelTask = Task.Run(() =>
{
	while (Console.ReadKey().Key != ConsoleKey.Enter)
	{
		Console.WriteLine("=====> Press ENTER key to cancel ...");
	}

	Console.WriteLine("\n\n\n=====> ENTER pressed, cancelling ...");
	cts.Cancel();	
});

await Task.WhenAny(new[] { cancelTask, DoSomeWorkAsync() });

Console.WriteLine("=====> Application stopped");

async Task DoSomeWorkAsync()
{
	foreach (var delay in new[] { 2000, 1000, 3000, 4000, 5000, 10_000 })
	{
		Console.WriteLine($"I will delay for {delay}");
		await Task.Delay(delay, cts!.Token);
		Console.WriteLine($"I have completed after delay of {delay}ms");
	}
}