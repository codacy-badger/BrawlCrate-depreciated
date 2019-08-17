﻿using BrawlLib.SSBBTypes;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ADSJNode : ARCEntryNode
    {
        internal ADSJ* Header => (ADSJ*) WorkingUncompressed.Address;
        public override ResourceType ResourceType => ResourceType.ADSJ;

        private int _count;

        [Category("ADSJ")]
        [DisplayName("Entries")]
        public int count => _count;

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

                new ADSJEntryNode().Initialize(this, source);
            }
        }

        public override bool OnInitialize()
        {
            ARCFileHeader* header = (ARCFileHeader*) (WorkingUncompressed.Address - 0x20);
            int index = header->_index;

            _count = Header->_count;

            if (_name == null)
            {
                _name = string.Format("Stepjumps[{0}]", index);
            }

            return Header->_count > 0;
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            int size = ADSJ.Size + Children.Count * 4;
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(force);
            }

            return size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ADSJ* header = (ADSJ*) address;
            *header = new ADSJ(_children.Count);
            uint offset = (uint) (0x10 + Children.Count * 4);
            for (int i = 0; i < Children.Count; i++)
            {
                if (i > 0)
                {
                    offset += (uint) Children[i - 1].CalculateSize(false);
                }

                *(buint*) (address + 0x10 + i * 4) = offset;
                _children[i].Rebuild(address + offset, _children[i].CalculateSize(false), true);
            }
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((ADSJ*) source.Address)->_tag == ADSJ.Tag ? new ADSJNode() : null;
        }
    }

    public unsafe class ADSJEntryNode : ResourceNode
    {
        internal ADSJEntry* Header => (ADSJEntry*) WorkingUncompressed.Address;
        public override ResourceType ResourceType => ResourceType.Unknown;

        private string _doorID;

        [Category("Jump Info")]
        [DisplayName("Corrosponding GDOR")]
        public string DoorID
        {
            get => _doorID;
            set
            {
                _doorID = value;
                SignalPropertyChange();
            }
        }

        private string _sendingID;

        [Category("Jump Info")]
        [DisplayName("File ID (hex)")]
        public string SendingID
        {
            get => _sendingID;
            set
            {
                _sendingID = value;
                SignalPropertyChange();
            }
        }

        private string _jumpBone;

        [Category("Jump Info")]
        [DisplayName("Jump Bone?")]
        public string JumpBone
        {
            get => _jumpBone;
            set
            {
                _jumpBone = value;
                Name = value;
                SignalPropertyChange();
            }
        }

        private byte _flag0;

        [Category("Jump Flags")]
        public byte Flag0
        {
            get => _flag0;
            set
            {
                _flag0 = value;
                SignalPropertyChange();
            }
        }

        private byte _flag1;

        [Category("Jump Flags")]
        public byte Flag1
        {
            get => _flag1;
            set
            {
                _flag1 = value;
                SignalPropertyChange();
            }
        }

        private byte _flag2;

        [Category("Jump Flags")]
        public byte Flag2
        {
            get => _flag2;
            set
            {
                _flag2 = value;
                SignalPropertyChange();
            }
        }

        private byte _flag3;

        [Category("Jump Flags")]
        public byte Flag3
        {
            get => _flag3;
            set
            {
                _flag3 = value;
                SignalPropertyChange();
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            _doorID = Header->DoorID;
            _sendingID = Header->SendStage;
            _jumpBone = Header->JumpBone;
            _flag0 = Header->_unk0;
            _flag1 = Header->_unk1;
            _flag2 = Header->_unk2;
            _flag3 = Header->_unk3;

            if (_name == null)
            {
                _name = JumpBone;
            }

            return false;
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            return ADSJEntry.Size;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ADSJEntry* header = (ADSJEntry*) address;
            *header = new ADSJEntry();
            header->_unk0 = _flag0;
            header->_unk1 = _flag1;
            header->_unk2 = _flag2;
            header->_unk3 = _flag3;
            header->JumpBone = _jumpBone;
            header->DoorID = _doorID;
            header->SendStage = _sendingID;
        }
    }
}