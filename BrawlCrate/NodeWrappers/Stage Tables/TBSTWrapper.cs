﻿using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.TBST)]
    internal class TBSTWrapper : STDTWrapper
    {
        public override string ExportFilter => FileFilters.TBST;

        public override ResourceNode Duplicate()
        {
            if (_resource._parent == null)
            {
                return null;
            }
            _resource.Rebuild();
            TBSTNode newNode = NodeFactory.FromAddress(null, _resource.WorkingUncompressed.Address, _resource.WorkingUncompressed.Length) as TBSTNode;
            _resource._parent.InsertChild(newNode, true, _resource.Index + 1);
            newNode.Populate();
            newNode.FileType = ((TBSTNode)_resource).FileType;
            newNode.FileIndex = ((TBSTNode)_resource).FileIndex;
            newNode.GroupID = ((TBSTNode)_resource).GroupID;
            newNode.RedirectIndex = ((TBSTNode)_resource).RedirectIndex;
            newNode.Name = _resource.Name;
            return newNode;
        }

        public TBSTWrapper() { }
    }
}
