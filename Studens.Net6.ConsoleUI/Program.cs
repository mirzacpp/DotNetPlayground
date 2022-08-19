using Microsoft.Extensions.DependencyInjection;

var sc = new ServiceCollection();
sc.AddSingleton(typeof(ICommand<>), typeof(GenericCommand<>));
var sp = sc.BuildServiceProvider();

var command = sp.GetRequiredService<ICommand<int>>();
Console.WriteLine(command.GetMessage());
Console.ReadKey();

internal interface ICommand<TResponse> where TResponse : struct
{
	string GetMessage();
}

//internal class UserCommand : ICommand<int>
//{
//	public string GetMessage()
//	{
//		return "Hello from User command";
//	}
//}

internal class GenericCommand<TResponse> : ICommand<TResponse> where TResponse : struct
{
	public string GetMessage()
	{
		return "Hello from generic";
	}
}