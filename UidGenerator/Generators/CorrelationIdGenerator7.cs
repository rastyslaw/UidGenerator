using System;
using System.Buffers;
using System.Threading;

namespace UidGenerator.Generators
{
	internal static class CorrelationIdGenerator7
	{
		private static readonly string _encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
		
		public static string GenerateId(long id) => string.Create(13, id, _writeToStringMemory);

		private static readonly SpanAction<char, long> _writeToStringMemory =  
			// DO NOT convert to method group otherwise will allocate
			(span, id) => WriteToStringMemory(span, id);
		
		private static long _lastId = DateTime.UtcNow.Ticks;

		public static string GetNextId() => GenerateId(Interlocked.Increment(ref _lastId));
			
		private static void WriteToStringMemory(Span<char> span, long id)  
		{
			//обратное заполнение чтобы JIT не тратил время на проверку границ
			span[12] = _encode32Chars[(int) id & 31]; 
			span[11] = _encode32Chars[(int) (id >> 5) & 31];
			span[10] = _encode32Chars[(int) (id >> 10) & 31];
			span[9] = _encode32Chars[(int) (id >> 15) & 31];
			span[8] = _encode32Chars[(int) (id >> 20) & 31];
			span[7] = _encode32Chars[(int) (id >> 25) & 31];
			span[6] = _encode32Chars[(int) (id >> 30) & 31];
			span[5] = _encode32Chars[(int) (id >> 35) & 31];
			span[4] = _encode32Chars[(int) (id >> 40) & 31];
			span[3] = _encode32Chars[(int) (id >> 45) & 31];
			span[2] = _encode32Chars[(int) (id >> 50) & 31];
			span[1] = _encode32Chars[(int) (id >> 55) & 31];
			span[0] = _encode32Chars[(int) (id >> 60) & 31];
		}
	}
}