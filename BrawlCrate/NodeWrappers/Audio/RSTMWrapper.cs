using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.RSTM)]
    internal class RSTMWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.RSTM;
    }
}