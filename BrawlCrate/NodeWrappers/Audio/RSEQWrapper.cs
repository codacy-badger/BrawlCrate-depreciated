using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSEQ)]
    class RSEQWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.RSEQ; } }
    }
}
