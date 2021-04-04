using System.IO;

namespace DAWG.Vtortola.Contracts
{
    internal interface IDawgState
    {
        int Length { get; }
        IDawgReader GetReader();
        void Write(BinaryWriter writer);
        void Read(BinaryReader reader);
    }
}