namespace Simplicity.Commons.Json
{
	/// <summary>
	/// Json serialization abastractions.
	/// </summary>
	public interface IJsonSerializer
	{
		string Serialize<T>(T value);
	}
}