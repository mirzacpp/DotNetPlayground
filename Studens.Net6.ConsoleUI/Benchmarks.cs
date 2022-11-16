using BenchmarkDotNet.Attributes;

namespace Studens.Net6.ConsoleUI
{
	internal class VectorClass
	{
		public int X { get; set; }
		public int Y { get; set; }
	}

	internal struct VectorStruct
	{
		public int X { get; set; }
		public int Y { get; set; }
	}

	public class Benchmarks
	{
		private const int ITEMS = 100_000;
		
		[Benchmark]
		public void WithClass()
		{
			VectorClass[] vectors = new VectorClass[ITEMS];
			for (int i = 0; i < ITEMS; i++)
			{
				vectors[i] = new VectorClass();
				vectors[i].X = 5;
				vectors[i].Y = 10;
			}
		}

		[Benchmark]
		public void WithStruct()
		{
			VectorStruct[] vectors = new VectorStruct[ITEMS];
			// At this point all the vectors instances are already allocated with default values
			for (int i = 0; i < ITEMS; i++)
			{
				vectors[i].X = 5;
				vectors[i].Y = 10;
			}
		}
	}
}