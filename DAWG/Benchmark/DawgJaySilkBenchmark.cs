using System.Collections.Generic;
using DAWG.Compact;
using DAWG.JaySilk;

namespace DAWG.Benchmark
{
    public class DawgJaySilkBenchmark : Benchmark
    {
        private class PrefixMatcher : IPrefixMatcher
        {
            private readonly Dawg _dawg;

            public PrefixMatcher(Dawg dawg)
            {
                _dawg = dawg;
            }

            public IEnumerable<string> GetWordsByPrefix(string prefix)
            {
                throw new System.NotImplementedException();
            }

            public bool HasWord(string prefix)
            {
                return _dawg.Exists(prefix);
            }

            public bool HasPrefix(string prefix)
            {
                throw new System.NotImplementedException();
            }
        }

        public override string Name => "DAWG JaySilk";

        protected override IPrefixMatcher Build(string dictionaryFile)
        {
            using (var dictionarySource = new WordDictionary(dictionaryFile))
            {
                var dawg = new Dawg();
                foreach (var word in dictionarySource)
                {
                    dawg.Insert(word.ToUpper());
                }

                return new PrefixMatcher(dawg);
            }
        }
    }
}
