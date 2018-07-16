using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.InteropServices;
using BrawlLib.SSBB.Types;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ClassicStageBlockNode : ResourceNode
    {
        private ClassicStageBlockStageData data;

        public override ResourceType ResourceType => ResourceType.Container;

        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID1 { get { return data._stageID1; } set { data._stageID1 = (ushort)value; SignalPropertyChange(); } }
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID2 { get { return data._stageID2; } set { data._stageID2 = (ushort)value; SignalPropertyChange(); } }
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID3 { get { return data._stageID3; } set { data._stageID3 = (ushort)value; SignalPropertyChange(); } }
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID4 { get { return data._stageID4; } set { data._stageID4 = (ushort)value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicStageBlock))
                throw new Exception("Wrong size for ClassicStageBlockNode");

            // Copy the data from the address
            data = ((ClassicStageBlock*)WorkingUncompressed.Address)->_stages;

            List<string> stageList = new List<string>();
            foreach (int stageID in new int[] { StageID1, StageID2, StageID3, StageID4 })
            {
                if (stageID == 255) continue;
                Stage found = Stage.Stages.FirstOrDefault(s => s.ID == stageID);
                stageList.Add(found == null ? stageID.ToString() : found.PacBasename);
            }

            _name = "Classic Stage Block (" + string.Join(", ", stageList) + ")";

            return true;
        }

        public override void OnPopulate()
        {
            ClassicFighterData* ptr = &((ClassicStageBlock*)WorkingUncompressed.Address)->_opponent1;
            for (int i = 0; i < 3; i++)
            {
                DataSource source = new DataSource(ptr, sizeof(ClassicFighterData));
                new ClassicFighterNode().Initialize(this, source);
                ptr++;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            ClassicStageBlock* dataPtr = (ClassicStageBlock*)address;
            dataPtr->_stages._unknown00 = data._unknown00;
            dataPtr->_stages._stageID1 = data._stageID1;
            dataPtr->_stages._stageID2 = data._stageID2;
            dataPtr->_stages._stageID3 = data._stageID3;
            dataPtr->_stages._stageID4 = data._stageID4;

            // Rebuild children using new address
            ClassicFighterData* ptr = &((ClassicStageBlock*)address)->_opponent1;
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicFighterData), true);
                ptr++;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            // Constant size (260 bytes)
            return sizeof(ClassicStageBlock);
        }
    }

    public unsafe class ClassicDifficultyNode : ResourceNode
    {
        private ClassicDifficultyData data;

        public byte Unknown00 { get { return data._unknown00; } set { data._unknown00 = value; SignalPropertyChange(); } }
        public byte Unknown01 { get { return data._unknown01; } set { data._unknown01 = value; SignalPropertyChange(); } }
        public byte Unknown02 { get { return data._unknown02; } set { data._unknown02 = value; SignalPropertyChange(); } }
        public byte Unknown03 { get { return data._unknown03; } set { data._unknown03 = value; SignalPropertyChange(); } }

        public short OffenseRatio { get { return data._offenseRatio; } set { data._offenseRatio = value; SignalPropertyChange(); } }
        public short DefenseRatio { get { return data._defenseRatio; } set { data._defenseRatio = value; SignalPropertyChange(); } }

        public byte Unknown08 { get { return data._unknown08; } set { data._unknown08 = value; SignalPropertyChange(); } }
        public byte Color { get { return data._color; } set { data._color = value; SignalPropertyChange(); } }
        public byte Stock { get { return data._stock; } set { data._stock = value; SignalPropertyChange(); } }
        public byte Unknown0b { get { return data._unknown0b; } set { data._unknown0b = value; SignalPropertyChange(); } }

        public short Unknown0c { get { return data._unknown0c; } set { data._unknown0c = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicDifficultyData))
                throw new Exception("Wrong size for ClassicDifficultyNode");

            // Copy the data from the address
            data = *(ClassicDifficultyData*)WorkingUncompressed.Address;

            return false;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(ClassicDifficultyData*)address = data;
        }
        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicDifficultyData);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ClassicTblHeader
    {
        public const int Size = 0x50;

        public byte _fighterID;
        public byte _unknown01;
        public byte _unknown02;
        public byte _unknown03;
        public float _unknown04;
        public byte _isAlly;

        private VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }
    }

        public unsafe class ClassicFighterNode : ResourceNode
    {
        private ClassicTblHeader _header;

        [TypeConverter(typeof(DropDownListFighterIDs))]
        public byte FighterID { get { return _header._fighterID; } set { _header._fighterID = value; SignalPropertyChange(); } }
        public byte Unknown01 { get { return _header._unknown01; } set { _header._unknown01 = value; SignalPropertyChange(); } }
        public byte Unknown02 { get { return _header._unknown02; } set { _header._unknown02 = value; SignalPropertyChange(); } }
        public byte Unknown03 { get { return _header._unknown03; } set { _header._unknown03 = value; SignalPropertyChange(); } }
        public float Unknown04 { get { return _header._unknown04; } set { _header._unknown04 = value; SignalPropertyChange(); } }
        //public byte IsAlly { get { return _isally; } set { _isally = value; SignalPropertyChange(); } }
        public bool IsAlly
        {
            get
            {
                return (_header._isAlly) != 0;
            }
            set
            {
                SignalPropertyChange();
                if (value)
                    _header._isAlly = 1;
                else
                    _header._isAlly = 0;
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(ClassicFighterData))
                throw new Exception("Wrong size for ClassicFighterNode");

            // Copy the data from the address
            ClassicFighterData* ptr = (ClassicFighterData*)WorkingUncompressed.Address;
            _header._fighterID = ptr->_fighterID;
            //_unknown01 = ptr->_unknown01;
            //_unknown02 = ptr->_unknown02;
            //_unknown03 = ptr->_unknown03;
            _header._unknown04 = ptr->_unknown04;
            //_unknown05 = ptr->_unknown05;
            //_unknown06 = ptr->_unknown06;
            //_unknown07 = ptr->_unknown07;
            _header._isAlly = ptr->_isAlly;

            if (_name == null)
            {
                var fighter = Fighter.Fighters.Where(s => s.ID == FighterID).FirstOrDefault();
                _name = "Fighter: 0x" + FighterID.ToString("X2") + (fighter == null ? "" : (" - " + fighter.Name));
            }

            return true;
        }

        public override void OnPopulate()
        {
            VoidPtr ptr = WorkingUncompressed.Address + 8;
            foreach (string s in new string[] { "Easy", "Normal", "Hard", "Very Hard", "Intense" })
            {
                DataSource source = new DataSource(ptr, sizeof(ClassicDifficultyData));
                var node = new ClassicDifficultyNode();
                node.Initialize(this, source);
                node.Name = s;
                node.IsDirty = false;
                ptr += sizeof(ClassicDifficultyData);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            ClassicFighterData* header_ptr = (ClassicFighterData*)address;
            header_ptr->_fighterID = _header._fighterID;
            //header_ptr->_unknown01 = _unknown01;
            //header_ptr->_unknown02 = _unknown02;
            //header_ptr->_unknown03 = _unknown03;
            header_ptr->_unknown04 = _header._unknown04;
            //header_ptr->_unknown05 = _unknown05;
            //header_ptr->_unknown06 = _unknown06;
            //header_ptr->_unknown07 = _unknown07;
            header_ptr->_isAlly = _header._isAlly;

            // Rebuild children using new address
            VoidPtr ptr = address + 9;
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicDifficultyData), true);
                ptr += sizeof(ClassicDifficultyData);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicFighterData);
        }
    }

    public unsafe class ClassicStageTblNode : ResourceNode
    {
        public override ResourceType ResourceType => ResourceType.ClassicStageTbl;

        private List<int> _padding;

        public string Padding { get { return string.Join(", ", _padding); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            VoidPtr ptr = WorkingUncompressed.Address;
            int numEntries = WorkingUncompressed.Length / sizeof(ClassicStageBlock);
            for (int i = 0; i < numEntries; i++) ptr += sizeof(ClassicStageBlock);

            _padding = new List<int>();
            bint* ptr2 = (bint*)ptr;
            while (ptr2 < WorkingUncompressed.Address + WorkingUncompressed.Length)
            {
                _padding.Add(*(ptr2++));
            }

            return true;
        }

        public override void OnPopulate()
        {
            int numEntries = WorkingUncompressed.Length / sizeof(ClassicStageBlock);

            ClassicStageBlock* ptr = (ClassicStageBlock*)WorkingUncompressed.Address;
            for (int i = 0; i < numEntries; i++)
            {
                DataSource source = new DataSource(ptr, sizeof(ClassicStageBlock));
                new ClassicStageBlockNode().Initialize(this, source);
                ptr++;
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Rebuild children using new address
            ClassicStageBlock* ptr = (ClassicStageBlock*)address;
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(ClassicStageBlock), true);
                ptr++;
            }

            bint* ptr2 = (bint*)ptr;
            foreach (int pad in Padding)
            {
                *(ptr2++) = pad;
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return sizeof(ClassicStageBlock) * Children.Count + Padding.Length * sizeof(bint);
        }

        public void CreateEntry()
        {
            FileMap tempFile = FileMap.FromTempFile(sizeof(ClassicStageBlock));
            // Is this the right way to add a new child node?
            var node = new ClassicStageBlockNode();
            node.Initialize(this, tempFile);
            AddChild(node, true);
        }
    }

    public class ClassicStageTblSizeTblNode : RawDataNode { }
}