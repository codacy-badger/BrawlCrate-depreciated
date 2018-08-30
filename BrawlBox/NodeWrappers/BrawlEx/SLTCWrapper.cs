using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.SLTC)]
    class SLTCWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.SLTC; } }
    }
}