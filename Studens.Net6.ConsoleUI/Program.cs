
Console.WriteLine("Press any key to terminate...");
Console.ReadKey();

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