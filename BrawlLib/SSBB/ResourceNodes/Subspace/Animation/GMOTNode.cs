﻿using BrawlLib.SSBBTypes;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GMOTNode : ResourceNode
    {
        internal GMOT* Header => (GMOT*) WorkingUncompressed.Address;
        public override ResourceType ResourceType => ResourceType.GMOT;

        [Category("GMOT")]
        [DisplayName("Entries")]
        public int count => Header->_count;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                {
                    source = new DataSource((*Header)[i],
                        WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]);
                }
                else
                {
                    source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]);
                }

                new GMOTEntryNode().Initialize(this, source);
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Animated Objects";
            }

            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((GMOT*) source.Address)->_tag == GMOT.Tag ? new GMOTNode() : null;
        }
    }

    public unsafe class GMOTEntryNode : ResourceNode
    {
        internal GMOTEntry* Header => (GMOTEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceType => ResourceType.Unknown;

        [Category("Animated Object")]
        [DisplayName("Model Index")]
        public int MID => *(byte*) (WorkingUncompressed.Address + 0x3C);

        [Category("Animated Object")]
        [DisplayName("Collision Index")]
        public int CID
        {
            get
            {
                int CID = *(byte*) (WorkingUncompressed.Address + 0x3D);
                if (CID == 0xFF)
                {
                    return -1;
                }
                else
                {
                    return CID;
                }
            }
        }

        [Category("Sound")]
        [DisplayName("Info Index")]
        public bint InfoIndex => *(bint*) (WorkingUncompressed.Address + 0x118);

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
            {
                _name = "Object[" + Index + ']';
            }

            return false;
        }
    }
}