using System.Collections.Generic;

namespace DAWG.Compact
{
    public interface IPrefixMatcher
    {
        bool HasPrefix(string prefix);
        bool HasWord(string prefix);
        IEnumerable<string> GetWordsByPrefix(string prefix);
    }
}