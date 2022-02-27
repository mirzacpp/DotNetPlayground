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
var testCases = new[]
{
    "(){",
    "()[]{}",
    "(]",
    "{[(]]}",
    "{[(n]]}",
    "{[()]}",
    "[[[]",
    "()))"
};

bool IsValid(string s)
{
    var length = s.Length;

    // Some base validation
    if (length < 2) return false;
    if (length % 2 != 0) return false;

    var brackets = new Dictionary<char, char>
    {
        { '(', ')' },
        { '[', ']' },
        { '{', '}' },
    };

    // validate last and first chars
    if (!brackets.ContainsKey(s[0]) || !brackets.ContainsValue(s[length - 1]))
    {
        return false;
    }

    var stack = new Stack<char>();
    stack.Push(s[0]);

    for (int i = 1; i < s.Length; i++)
    {
        // Is it a open bracket ?
        if (brackets.ContainsKey(s[i]))
        {
            stack.Push(s[i]);
        }
        else if (stack.Count > 0)
        {
            var lastOpeningBracket = stack.Pop();

            if (brackets[lastOpeningBracket] != s[i])
            {
                return false;
            }
        }
    }

    return stack.Count == 0;
}

foreach (var test in testCases)
{
    var result = IsValid(test);
    if (result)
    {
        Console.WriteLine($"Test result for '{test}' is valid.");
    }
    else
    {
        Console.WriteLine($"Test result for '{test}' is not valid because");
    }
}

Console.ReadLine();