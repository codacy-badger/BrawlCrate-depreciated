using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RWSD)]
    internal class RWSDWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RWSD;
    }
}
