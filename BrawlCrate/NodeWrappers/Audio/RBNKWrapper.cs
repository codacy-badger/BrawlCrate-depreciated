using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RBNK)]
    internal class RBNKWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RBNK;
    }
}
