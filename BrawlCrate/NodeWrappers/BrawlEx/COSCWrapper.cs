using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.COSC)]
    class COSCWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return FileFilters.COSC; } }
    }
}