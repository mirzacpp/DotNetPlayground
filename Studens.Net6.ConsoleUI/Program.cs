//using System.Text.Json;
//using System.Text.Json.Serialization;

//var timeStamp = Guid.NewGuid().ToString();

//var dbProviders = new List<DatabaseProvider> {
//	new DatabaseProvider("SqlServer", 1000, true, timeStamp),
//	new DatabaseProvider("PostgreSql", 2000, true, timeStamp),
//	new DatabaseProvider("SqlLite", 3000, false, timeStamp)
//};

//JsonSerializerOptions jOptions = new()
//{
//	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
//	WriteIndented = true,
//};

//var jsonString = JsonSerializer.Serialize(dbProviders, jOptions);

//File.WriteAllText("my-file.json", jsonString);

////var jsonString = File.ReadAllText("dbproviders.json", Encoding.UTF8);
////var dbProviders = JsonSerializer.Deserialize<List<DatabaseProvider>>(jsonString, jOptions) ?? Enumerable.Empty<DatabaseProvider>();

////Console.WriteLine(jsonString);
////Console.WriteLine(dbProviders.Count());

Console.WriteLine(sizeof(ushort));

Console.WriteLine("Press any key to terminate...");
Console.ReadKey();

internal struct EmptyPoint
{
}

internal struct Point
{
	private int _x;
	private int _y;

	public Point(int x, int y)
	{
		_x = x;
		_y = y;
	}

	public (int x, int y) Position => (_x, _y);

	public override string ToString() => $"x:{_x}, y: {_y}";
}

internal class Point3D
{
	private int _x;
	private int _y;
	private int _z;

	public Point3D(int x, int y, int z)
	{
		_x = x;
		_y = y;
		_z = z;
	}

	public override string ToString() => $"x:{_x}, y: {_y}, z: {_z}";
}

//internal record DatabaseProvider(string name, int timeout, bool encrypt, string timeStamp);