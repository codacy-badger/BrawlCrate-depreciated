using System;
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

        public byte[] _cosmetics = new byte[32];

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
            for (int i = 0; i < 16; i++)
            {
                if (_cosmetics[i * 2] == 0x0C && _cosmetics[i * 2 + 1] == 0x00)
                    break;
                DataSource source;
                if (i == 15)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new CSSCEntryNode().Initialize(this, source);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CSSC* hdr = (CSSC*)address;
            *hdr = new CSSC();
            hdr->_tag = _tag;
            hdr->_size = _size;
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
            int index = 0;
            for (int i = 0; i < Children.Count; i++)
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
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return CSSC.Size;
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
            _cosmetics[0] = Header->_cosmetics[0];
            _cosmetics[1] = Header->_cosmetics[1];
            _cosmetics[2] = Header->_cosmetics[2];
            _cosmetics[3] = Header->_cosmetics[3];
            _cosmetics[4] = Header->_cosmetics[4];
            _cosmetics[5] = Header->_cosmetics[5];
            _cosmetics[6] = Header->_cosmetics[6];
            _cosmetics[7] = Header->_cosmetics[7];
            _cosmetics[8] = Header->_cosmetics[8];
            _cosmetics[9] = Header->_cosmetics[9];
            _cosmetics[10] = Header->_cosmetics[10];
            _cosmetics[11] = Header->_cosmetics[11];
            _cosmetics[12] = Header->_cosmetics[12];
            _cosmetics[13] = Header->_cosmetics[13];
            _cosmetics[14] = Header->_cosmetics[14];
            _cosmetics[15] = Header->_cosmetics[15];
            _cosmetics[16] = Header->_cosmetics[16];
            _cosmetics[17] = Header->_cosmetics[17];
            _cosmetics[18] = Header->_cosmetics[18];
            _cosmetics[19] = Header->_cosmetics[19];
            _cosmetics[20] = Header->_cosmetics[20];
            _cosmetics[21] = Header->_cosmetics[21];
            _cosmetics[22] = Header->_cosmetics[22];
            _cosmetics[23] = Header->_cosmetics[23];
            _cosmetics[24] = Header->_cosmetics[24];
            _cosmetics[25] = Header->_cosmetics[25];
            _cosmetics[26] = Header->_cosmetics[26];
            _cosmetics[27] = Header->_cosmetics[27];
            _cosmetics[28] = Header->_cosmetics[28];
            _cosmetics[29] = Header->_cosmetics[29];
            _cosmetics[30] = Header->_cosmetics[30];
            _cosmetics[31] = Header->_cosmetics[31];
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
                Name = "Fit" + _costumeID.ToString("00") + " - " + BrawlExColorID.Colors[_colorID].Name;
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
                Name = "Fit" + _costumeID.ToString("00") + " - " + BrawlExColorID.Colors[_colorID].Name;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return CSSCEntry.Size;
        }

        public override bool OnInitialize()
        {
            _colorID = Header->_colorID;
            _costumeID = Header->_costumeID;
            if (_name == null)
                _name = "Fit" + BrawlLib.BrawlCrate.FighterNameGenerators.InternalNameFromID(((CSSCNode)Parent)._cosmeticSlot, BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDIndex, "+S") + _costumeID.ToString("00") + " - " + BrawlExColorID.Colors[_colorID].Name;
            return false;
        }

        public void regenName()
        {
            Name = "Fit" + BrawlLib.BrawlCrate.FighterNameGenerators.InternalNameFromID(((CSSCNode)Parent)._cosmeticSlot, BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDIndex, "+S") + _costumeID.ToString("00") + " - " + BrawlExColorID.Colors[_colorID].Name;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CSSCEntry* hdr = (CSSCEntry*)address;
            hdr->_colorID = _colorID;
            hdr->_costumeID = _costumeID;
        }
    }
}
