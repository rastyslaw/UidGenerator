using System.Collections.Generic;
using DAWG.Compact;
using DAWG.Vtortola;

namespace DAWG.Benchmark
{
    public class DawgVtortolaBenchmark : Benchmark
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
                return _dawg.WithPrefix(prefix);
            }

            public bool HasWord(string prefix)
            {
                return _dawg.Contains(prefix);
            }

            public bool HasPrefix(string prefix)
            {
                throw new System.NotImplementedException();
            }
        }

        public override string Name => "DAWG Vtortola";

        protected override IPrefixMatcher Build(string dictionaryFile)
        {
            using (var dictionarySource = new WordDictionary(dictionaryFile))
            {
                var dawgBuilder = Dawg.CreateBuilder(dictionarySource);
                var dawg = dawgBuilder.Build();
                
                return new PrefixMatcher(dawg);
            }
        }
    }
}
