using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSEQ)]
    internal class RSEQWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RSEQ;
    }
}
