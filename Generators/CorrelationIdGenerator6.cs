using System;
using System.Threading;

namespace UidGenerator.Generators
{
	internal static class CorrelationIdGenerator6
	{
		private static readonly string _encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
		
		private static readonly ThreadLocal<char[]> _buffer = new ThreadLocal<char[]>(() => new char[13]); 
		
		private static long _lastId = DateTime.UtcNow.Ticks;

		public static string GetNextId() => GenerateId(Interlocked.Increment(ref _lastId));
			
		private static string GenerateId(long id)
		{
			var buffer = _buffer.Value;
				
			buffer[0] = _encode32Chars[(int) (id >> 60) & 31];
			buffer[1] = _encode32Chars[(int) (id >> 55) & 31];
			buffer[2] = _encode32Chars[(int) (id >> 50) & 31];
			buffer[3] = _encode32Chars[(int) (id >> 45) & 31];
			buffer[4] = _encode32Chars[(int) (id >> 40) & 31];
			buffer[5] = _encode32Chars[(int) (id >> 35) & 31];
			buffer[6] = _encode32Chars[(int) (id >> 30) & 31];
			buffer[7] = _encode32Chars[(int) (id >> 25) & 31];
			buffer[8] = _encode32Chars[(int) (id >> 20) & 31];
			buffer[9] = _encode32Chars[(int) (id >> 15) & 31];
			buffer[10] = _encode32Chars[(int) (id >> 10) & 31];
			buffer[11] = _encode32Chars[(int) (id >> 5) & 31];
			buffer[12] = _encode32Chars[(int) id & 31];

			return new string(buffer, 0, 13);
		}
	}
}