﻿using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using System.BrawlEx;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CSSCNode : ResourceNode
    {
        internal CSSC* Header { get { return (CSSC*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.CSSC; } }

        public uint _tag;                   // 0x00 - Uneditable; CSSC
        public uint _size;                  // 0x04 - Uneditable; Should be "40"
        public uint _version;               // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1;             // 0x0C - Unused?
        public byte _editFlag2;             // 0x0D - Unused?
        public byte _editFlag3;             // 0X0E - Unused?
        public byte _editFlag4;             // 0x0F - 0x01 = Set Primary Character/Secondary Character; 0x02 = Set Cosmetic Slot; 0x03 = Set both; Use Booleans for both?
        public byte _primaryCharSlot;       // 0x10 - Primary Character Slot: Only used when set in _editFlag4
        public byte _secondaryCharSlot;     // 0x11 - Secondary Character Slot: Only used when set in _editFlag4
        public byte _recordSlot;            // 0x12 - Record Bank
        public byte _cosmeticSlot;          // 0x13 - Only used when set in _editFlag4
        public uint _wiimoteSFX;             // 0x14
        public uint _unknown0x18;           // 0x18 - Seemingly padding
        public uint _status;                 // 0x1C - I have no idea what this is
        

        private bool _primarySecondarySet;
        [Category("Character")]
        [DisplayName("Set Primary/Secondary")]
        public bool SetPrimarySecondary
        {
            get
            {
                return _primarySecondarySet;
            }
            set
            {
                _primarySecondarySet = value;
                SignalPropertyChange();
            }
        }

        [Category("Character")]
        [TypeConverter(typeof(DropDownListBrawlExSlotIDs))]
        [DisplayName("Primary Character Slot")]
        public byte CharSlot1
        {
            get
            {
                return _primaryCharSlot;
            }
            set
            {
                _primaryCharSlot = value;
                SignalPropertyChange();
            }
        }

        [Category("Character")]
        [TypeConverter(typeof(DropDownListBrawlExSlotIDs))]
        [DisplayName("Secondary Character Slot")]
        public byte CharSlot2
        {
            get
            {
                return _secondaryCharSlot;
            }
            set
            {
                _secondaryCharSlot = value;
                SignalPropertyChange();
            }
        }


        [Category("Character")]
        [TypeConverter(typeof(DropDownListBrawlExRecordIDs))]
        [DisplayName("Record Bank")]
        public byte Records
        {
            get
            {
                return _recordSlot;
            }
            set
            {
                _recordSlot = value;
                SignalPropertyChange();
            }
        }

        private bool _cosmeticSlotSet;
        [Category("Cosmetics")]
        [DisplayName("Set Cosmetic Slot")]
        public bool SetCosmeticSlot
        {
            get
            {
                return _cosmeticSlotSet;
            }
            set
            {
                _cosmeticSlotSet = value;
                SignalPropertyChange();
            }
        }

        [Category("Cosmetics")]
        [TypeConverter(typeof(DropDownListBrawlExCosmeticIDs))]
        [DisplayName("Cosmetic Slot")]
        public byte CosmeticSlot
        {
            get
            {
                return _cosmeticSlot;
            }
            set
            {
                _cosmeticSlot = value;
                foreach(CSSCEntryNode c in Children)
                    c.regenName();
                SignalPropertyChange();
            }
        }

        [Category("Cosmetics")]
        [DisplayName("Wiimote SFX")]
        public string WiimoteSFX
        {
            get
            {
                return "0x" + _wiimoteSFX.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _wiimoteSFX = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [DisplayName("Status")]
        public string Status
        {
            get
            {
                return "0x" + _status.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _status = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        public override void OnPopulate()
        {
            CSSCEntryNode end = new CSSCEntryNode(true);
            for (int i = 0; i < ((_size - 0x20) / 2); i++)
            {
                new CSSCEntryNode().Initialize(this, new DataSource((*Header)[i], 2));
                CSSCEntryNode c = (CSSCEntryNode)Children[Children.Count - 1];
                if(c.Color == end.Color && c.CostumeID == end.CostumeID)
                {
                    RemoveChild(c);
                    _changed = false;
                    break;
                }
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CSSC* hdr = (CSSC*)address;
            *hdr = new CSSC();
            hdr->_tag = _tag;
            hdr->_size = (buint)(OnCalculateSize(true));
            hdr->_version = _version;
            hdr->_editFlag1 = _editFlag1;
            hdr->_editFlag2 = _editFlag2;
            hdr->_editFlag3 = _editFlag3;
            _editFlag4 = 0;
            if (_primarySecondarySet)
                _editFlag4 += 0x01;
            if (_cosmeticSlotSet)
                _editFlag4 += 0x02;
            hdr->_editFlag4 = _editFlag4;
            hdr->_primaryCharSlot = _primaryCharSlot;
            hdr->_secondaryCharSlot = _secondaryCharSlot;
            hdr->_recordSlot = _recordSlot;
            hdr->_cosmeticSlot = _cosmeticSlot;
            hdr->_wiimoteSFX = _wiimoteSFX;
            hdr->_unknown0x18 = _unknown0x18;
            hdr->_status = _status;
            uint offset = (uint)(0x20);
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                r.Rebuild((VoidPtr)address + offset, 2, true);
                offset += 2;
            }
            CSSCEntryNode end = new CSSCEntryNode(true);
            end.Rebuild((VoidPtr)address + offset, 2, true);
            offset += 2;
            while(offset < hdr->_size)
            {
                CSSCEntryNode blank = new CSSCEntryNode(false);
                blank.Rebuild((VoidPtr)address + offset, 2, true);
                offset += 2;
            }
            /*for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*)((byte*)address + 0x20 + (i * 2)) = offset;
                r.Rebuild((VoidPtr)address + offset, 0x02, true);
                offset += 0x02;
                index += 2;
            }
            if(index < 32)
            {
                hdr->_cosmetics[index] = 0x0C;
                for (int j = index + 1; j < 32; j++)
                    hdr->_cosmetics[j] = 0x0;
            }*/
        }
        
        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            int extraBytes = 0;
            while ((0x20 + ((Children.Count + 1) * 2) + extraBytes) % 16 != 0)
                extraBytes++;
            return (0x20 + ((Children.Count + 1) * 2) + extraBytes) < CSSC.Size ? CSSC.Size : (0x20 + ((Children.Count + 1) * 2) + extraBytes);
        }

        public override bool OnInitialize()
        {
            _tag = Header->_tag;
            _size = Header->_size;
            _version = Header->_version;
            _editFlag1 = Header->_editFlag1;
            _editFlag2 = Header->_editFlag2;
            _editFlag3 = Header->_editFlag3;
            _editFlag4 = Header->_editFlag4;
            _primaryCharSlot = Header->_primaryCharSlot;
            _secondaryCharSlot = Header->_secondaryCharSlot;
            _recordSlot = Header->_recordSlot;
            _cosmeticSlot = Header->_cosmeticSlot;
            _wiimoteSFX = Header->_wiimoteSFX;
            _unknown0x18 = Header->_unknown0x18;
            _status = Header->_status;
            _cosmeticSlotSet = ((_editFlag4 & 0x02) != 0);
            _primarySecondarySet = ((_editFlag4 & 0x01) != 0);
            if ((_name == null) && (_origPath != null))
                _name = Path.GetFileNameWithoutExtension(_origPath);
            return true;
        }
        
        internal static ResourceNode TryParse(DataSource source) { return ((CSSC*)source.Address)->_tag == CSSC.Tag ? new CSSCNode() : null; }
    }

    public unsafe class CSSCEntryNode : ResourceNode
    {
        internal CSSCEntry* Header { get { return (CSSCEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public byte _colorID;
        public byte _costumeID;
        
        public CSSCEntryNode()
        {
            _colorID = 0x00;
            _costumeID = 0x00;
        }

        // Defaults to the costume end marker
        public CSSCEntryNode(bool end)
        {
            _colorID = (byte)(end ? 0x0C : 0x00);
            _costumeID = 0x00;
        }

        [Category("Costume")]
        [DisplayName("Costume ID")]
        public byte CostumeID
        {
            get
            {
                return _costumeID;
            }
            set
            {
                _costumeID = value;
                regenName();
                SignalPropertyChange();
            }
        }

        [Category("Costume")]
        [TypeConverter(typeof(DropDownListBrawlExColorIDs))]
        [DisplayName("Color")]
        public byte Color
        {
            get
            {
                return _colorID;
            }
            set
            {
                _colorID = value;
                regenName();
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            return CSSCEntry.Size;
        }

        public override bool OnInitialize()
        {
            _colorID = Header->_colorID;
            _costumeID = Header->_costumeID;
            if (_name == null)
                _name = "Fit" + BrawlLib.BrawlCrate.FighterNameGenerators.InternalNameFromID(((CSSCNode)Parent)._cosmeticSlot, BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDIndex, "+S") + _costumeID.ToString("00") + (BrawlExColorID.Colors.Length > _colorID ? " - " + BrawlExColorID.Colors[_colorID].Name : "");
            return false;
        }

        public void regenName()
        {
            Name = "Fit" + BrawlLib.BrawlCrate.FighterNameGenerators.InternalNameFromID(((CSSCNode)Parent)._cosmeticSlot, BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDIndex, "+S") + _costumeID.ToString("00") + (BrawlExColorID.Colors.Length > _colorID ? " - " + BrawlExColorID.Colors[_colorID].Name : "");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CSSCEntry* hdr = (CSSCEntry*)address;
            hdr->_colorID = _colorID;
            hdr->_costumeID = _costumeID;
        }
    }
}
