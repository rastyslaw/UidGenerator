using DAWG.Compact;

namespace DAWG.Benchmark
{
    public class DawgCompactBenchmark : global::DAWG.Benchmark.Benchmark
    {
        public override string Name => "Dawg.Compact";

        protected override IPrefixMatcher Build(string dictionaryFile)
        {
            using (var dictionarySource = new WordDictionary(dictionaryFile))
            {
                return new DawgBuilder()
                    .WithOrderedWords(dictionarySource)
                    .BuildCompactDawg();
            }
        }
    }
}
