using DAWG.Vtortola.Contracts;

namespace DAWG.Vtortola.UIntDawg
{
    internal sealed class UIntDawgStateProvider : IDawgStateProvider
    {
        public IDawgStateWriter CreateWriter(in int nodeCount, in int symbolCount)
            => new UIntDawgStateWriter(nodeCount, symbolCount);

        public IDawgState CreateState()
            => new UIntDawgState();
    }
}