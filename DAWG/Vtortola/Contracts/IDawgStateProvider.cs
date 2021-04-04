namespace DAWG.Vtortola.Contracts
{
    internal interface IDawgStateProvider
    {
        IDawgStateWriter CreateWriter(in int nodeCount, in int symbolCount);
        IDawgState CreateState();
    }
}