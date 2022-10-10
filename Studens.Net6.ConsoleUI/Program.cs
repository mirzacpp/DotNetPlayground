Task task1 = CreateTask(5000);
Task task2 = CreateTask(6000);
Task task3 = CreateTask(7000);

await Task.WhenAll(task1, task2, task3);

int worker = 0;
int io = 0;
ThreadPool.GetAvailableThreads(out worker, out io);

Console.WriteLine("Thread pool threads available at startup: ");
Console.WriteLine("   Worker threads: {0:N0}", worker);
Console.WriteLine("   Asynchronous I/O threads: {0:N0}", io);
Console.WriteLine("Press any key to terminate...");
Console.ReadKey();

Task CreateTask(int delay)
{
	return Task.Run(async () =>
	{
		Console.WriteLine("I will now delay for " + delay);
		await Task.Delay(delay);
		Console.WriteLine("I have delayed for " + delay);
	});
}

internal sealed class Test
{
	private const string Const = "Vlado";
	private static string Static = "Statico";
	public event EventHandler<EventArgs> Event;
}

internal struct Data : IEquatable<Data>
{
	private int _value;

	public Data(int value) => _value = value;

	public void Change(int value) => _value = value;

	public override bool Equals(object? obj)
	{
		return obj is Data data && Equals(data);
	}

	public bool Equals(Data other)
	{
		return _value == other._value;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_value);
	}

	public override string ToString() => $"Data is: {_value}";

	public static bool operator ==(Data left, Data right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Data left, Data right)
	{
		return !(left == right);
	}
}