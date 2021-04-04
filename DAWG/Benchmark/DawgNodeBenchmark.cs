using DAWG.Compact;

namespace DAWG.Benchmark
{
    public class DawgNodeBenchmark : Benchmark
    {
        public override string Name => "Dawg.Compact (before compacting)";

        protected override IPrefixMatcher Build(string dictionaryFile)
        {
            using (var dictionarySource = new WordDictionary(dictionaryFile))
            {
                return new DawgBuilder()
                    .WithOrderedWords(dictionarySource)
                    .BuildNodeDawg();
            }
        }
    }
}
