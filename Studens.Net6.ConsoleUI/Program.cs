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

var coord = new GeoCord("20", "21");
var coord2 = new GeoCord("20", "20");

Console.WriteLine(coord.Equals(coord2));

Console.WriteLine("Press any key to terminate...");
Console.ReadKey();

internal struct GeoCord : IEquatable<GeoCord>
{
	private readonly string _latitude;
	private readonly string _longitude;

	public GeoCord(string latitude, string longitude)
	{
		_latitude = latitude;
		_longitude = longitude;
	}

	public (string lat, string lng) GetPosition() => (_latitude, _longitude);

	public bool Equals(GeoCord other)
	{
		return _latitude == other._latitude && _longitude == other._longitude;
	}
}