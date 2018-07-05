using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GFG1Node : ResourceNode
    {
        internal GFG1* Header { get { return (GFG1*)WorkingUncompressed.Address; } }
        //public override ResourceType ResourceType { get { return ResourceType.GFG1; } }

        [Category("GFG1")]
        [DisplayName("Entries")]
        public int count { get { return Header->_count; } }
        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GFG1EntryNode().Initialize(this, source);
            }
        }
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "GFG1";
            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((GFG1*)source.Address)->_tag == GFG1.Tag ? new GFG1Node() : null; }
    }

    public unsafe class GFG1EntryNode : ResourceNode
    {
        internal GFG1Entry* Header { get { return (GFG1Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ENEMY; } }
        
        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Entry [" + Index + ']';
            return false;
        }
    }
}