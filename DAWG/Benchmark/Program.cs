using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DAWG.Compact;

namespace DAWG.Benchmark
{
	class Program
	{
        private static Benchmark[] _benchmarks = new Benchmark[] 
        {
            new DawgCompactBenchmark(),
            new DawgNodeBenchmark(),
            new DawgJaySilkBenchmark(),
            new DawgVtortolaBenchmark()
        };

        private const string _dictionaryFile = "alphanumeric-english-words.txt";

        private static IList<string> _testData;
        
		static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            Console.WriteLine("Preparing test data...");
            _testData = PrepareTestData(_dictionaryFile);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"Preparing test data... done");

            foreach (var benchmark in _benchmarks)
            {
                benchmark.Run(_testData, _dictionaryFile);
                Cleanup();
            }

            Console.WriteLine("Combined results: dictionary build time");
            foreach (var benchmark in _benchmarks)
            {
                Console.WriteLine($"{benchmark.Name.PadLeft(35)}|{benchmark.DictionaryBuildTime.ToString().PadRight(20)}");
            }

            Console.WriteLine();

            Console.WriteLine("Combined results: prefix completion suggestions test time");
            foreach (var benchmark in _benchmarks)
            {
                PrintOptionalTimeTestResultRow(benchmark.Name, benchmark.PrefixCompletionSuggestionsTestTime);
            }

            Console.WriteLine();

            Console.WriteLine("Combined results: word existence test time");
            foreach (var benchmark in _benchmarks)
            {
                PrintOptionalTimeTestResultRow(benchmark.Name, benchmark.WordExistenceTestTime);
            }

            Console.WriteLine();

            Console.WriteLine("Combined results: prefix existence test time");
            foreach (var benchmark in _benchmarks)
            {
                PrintOptionalTimeTestResultRow(benchmark.Name, benchmark.PrefixExistenceTestTime);
            }
            
            // Matches();
        }

        private static void PrintOptionalTimeTestResultRow(string benchmarkName, TimeSpan? result)
        {
            var resultString = result.HasValue ? result.ToString().PadRight(20) : "not supported";
            Console.WriteLine($"{benchmarkName.PadLeft(35)}|{result}");
        }

        private static IList<string> PrepareTestData(string fileName)
        {
            const int prefixesToTestCount = 10000000;
            const int minPrefixLength = 1;
            const int maxPrefixLength = 10;
            char[] alphabet = null;
            _testData = new List<string>(prefixesToTestCount);
            using (var dictionarySource = new WordDictionary(fileName))
            {
                //sample alphabet from source
                alphabet = dictionarySource.Take(1000)
                    .SelectMany(word => word.ToCharArray())
                    .Distinct()
                    .ToArray();
            }

            var pseudoRandom = new Random(12345);
            var prefix = new List<char>(maxPrefixLength);
            for (int i = 0; i < prefixesToTestCount; i++)
            {
                prefix.Clear();
                var prefixLength = pseudoRandom.Next(minPrefixLength, maxPrefixLength + 1);
                for (int prefixIndex = 0; prefixIndex < prefixLength; prefixIndex++)
                {
                    int letterIndex = pseudoRandom.Next(alphabet.Length);
                    prefix.Add(alphabet[letterIndex]);
                }
                _testData.Add(String.Concat(prefix));
            }

            return _testData;
        }

        private static void Cleanup()
        {
            Console.WriteLine("Cleanup...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"Cleanup... done");
        }
        
		public static void Matches()
		{
			Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            
            Console.WriteLine("Building dictionary...");

            var matcher = new DawgBuilder()
                .WithOrderedWordsFromFile("alphanumeric-english-words.txt")
                .BuildCompactDawg();

            var query = "";

            while (true)
            {
                Console.CursorVisible = false;
                Console.Clear();

                var prompt = "Type to see completions> ";
                Console.WriteLine(prompt + query + "_");
                var indent = new string(' ', prompt.Length);
                if (query.Length > 0)
                {
                    var matches = matcher.GetWordsByPrefix(query).Take(10);
                    if (!matches.Any())
                    {
                        Console.WriteLine(indent + "<No matches>");
                    }
                    else
                    {
                        foreach (var match in matches)
                        {
                            Console.WriteLine(indent + match);
                        }
                    }
                }

                Console.WriteLine("\nPress esc to exit.");

                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Backspace && query.Length > 0)
                {
                    query = query.Substring(0, query.Length - 1);
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else
                {
                    query += key.KeyChar;
                }
            }
		}
	}
}