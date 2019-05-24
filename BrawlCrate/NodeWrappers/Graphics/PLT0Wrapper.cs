﻿using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.PLT0)]
    internal class PLT0Wrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.PLT0;
    }
}
