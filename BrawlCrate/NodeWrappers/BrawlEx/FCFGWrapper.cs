using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.FCFG)]
    class FCFGWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.FCFG; } }
    }
}