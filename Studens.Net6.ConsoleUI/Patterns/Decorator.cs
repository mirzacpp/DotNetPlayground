using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Studens.Net6.ConsoleUI.Patterns
{
	internal static partial class Patterns
	{
		internal class Decorator
		{
			internal interface ITextWriter
			{
				public void Write(string text);
			}

			internal class TextWriter : ITextWriter
			{
				public void Write(string text)
				{
					Console.WriteLine(text);
				}
			}

			internal class TextWriterDecorator : ITextWriter
			{
				private readonly ITextWriter _writer;

				public TextWriterDecorator(ITextWriter writer)
				{
					_writer = writer;
				}

				public void Write(string text)
				{
					Console.WriteLine($"I wrote {text} before.");
					_writer.Write(text);	
				}
			}
		}
	}
}
