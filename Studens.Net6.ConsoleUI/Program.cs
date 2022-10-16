using Microsoft.Extensions.DependencyInjection;
using Studens.Commons.DependencyInjection;

var sc = new ServiceCollection();
sc.AutoRegisterMarkedDependencies(typeof(Program));
//sc.AddSingleton(typeof(IMessage<int>), typeof(Worker));
//sc.Add(new ServiceDescriptor(typeof(IWorkerGeneric<>), typeof(WorkerD<>), ServiceLifetime.Singleton));

var sp = sc.BuildServiceProvider();
//var worker = sp.GetRequiredService<IMessage<int>>();
//worker.Handle(2);

foreach (var item in sc)
{
	Console.WriteLine($"{item.ServiceType} / {item.ImplementationType} / {item.Lifetime}");
}

Console.WriteLine("Press any key to terminate...");
Console.ReadKey();

public class Worker : IOrderable, ISingletonDependency, IWorker, IWorkerG<int>, IMessage<int>, IMessage<string>
{
	public void Handle(string message)
	{
		Console.WriteLine($"Message {message} handled.");
	}

	public void Handle(int message)
	{
		Console.WriteLine($"Message {message} handled.");
	}
}

public interface IOrderable {

}

public interface IWorker {

}

public interface IWorkerG<T> {

}

public interface IMessage<T>
{
	void Handle(T message);
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