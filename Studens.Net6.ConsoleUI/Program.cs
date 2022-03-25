//int RomanToInt(string s)
//{
//    var dicto = new Dictionary<string, int>
//    {
//        { "I", 1 },
//        { "V", 5 },
//        { "X", 10 },
//        { "L", 50 },
//        { "C", 100 },
//        { "D", 500 },
//        { "M", 1000 },
//        { "IV", 4 },
//        { "IX", 9 },
//        { "XL", 40 },
//        { "XC", 90 },
//        { "CD", 400 },
//        { "CM", 900 },
//    };

//    var ss = s.ToCharArray();
//    var sum = 0;

//    for (int i = 0; i < s.Length; i++)
//    {
//    }

//    return sum;
//}
//Console.WriteLine($"{RomanToInt("III")} == 3");
//Console.WriteLine($"{RomanToInt("LVIII")} == 58");
//Console.WriteLine($"{RomanToInt("MCMXCIV")} == 1994");

//LeetCode.RunShuffleStringProblem();

Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
var task = new Task(() => Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is printing message."));
task.Start();

Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

Console.ReadLine();

void Print()
{
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is printing message.");
}

async void AwaitVoid()
{
    await Task.CompletedTask;
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is awaiting void.");
}

async Task AwaitTask()
{
    await Task.CompletedTask;
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is awaiting Task.");
}