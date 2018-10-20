using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.SLTC)]
    class SLTCWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.SLTC; } }
    }
}