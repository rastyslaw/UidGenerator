using System;
using System.Runtime;
using System.Threading;

namespace UidGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var isServerGC = GCSettings.IsServerGC;
			Console.WriteLine($"IsServerGC: {isServerGC}");
			
			// new ParallelTest();

			new UidGenerator();
		}
	}
}