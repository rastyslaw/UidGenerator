using System;
using System.Diagnostics;
using System.Linq;
using UidGenerator.Generators;

namespace UidGenerator
{
	public class UidGenerator
	{
		public UidGenerator()
		{
			const int ITERATION_COUNT = 1_000_000_000;
			const int THREAD_COUNT = 12;

			var sw = Stopwatch.StartNew();

			ParallelEnumerable
				.Range(1, ITERATION_COUNT)
				.WithDegreeOfParallelism(THREAD_COUNT)
				.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
				.ForAll(_ =>
				{
					string id = CorrelationIdGenerator7.GetNextId();
				});

			sw.Stop();

			using (var process = Process.GetCurrentProcess())
			{
				Console.WriteLine("Execution time: {0}\r\n  - Gen-0: {1}, Gen-1: {2}, Gen-2: {3}",
					sw.Elapsed.ToString(),
					GC.CollectionCount(0),
					GC.CollectionCount(1),
					GC.CollectionCount(2));
			}
		}
	}
}