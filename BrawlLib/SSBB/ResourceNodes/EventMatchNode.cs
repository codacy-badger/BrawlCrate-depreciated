﻿using BrawlLib.SSBB.Types;
using System;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class EventMatchDifficultyNode : ResourceNode
    {
        private EventMatchDifficultyData data;

        public byte CpuLevel { get => data._cpuLevel; set { data._cpuLevel = value; SignalPropertyChange(); } }
        //public byte Unknown01       { get { return data._unknown01; }        set { data._unknown01 = value;        SignalPropertyChange(); } }
        public ushort OffenseRatio { get => data._offenseRatio; set { data._offenseRatio = value; SignalPropertyChange(); } }
        public ushort DefenseRatio { get => data._defenseRatio; set { data._defenseRatio = value; SignalPropertyChange(); } }
        public byte AiType { get => data._aiType; set { data._aiType = value; SignalPropertyChange(); } }
        public byte Costume { get => data._costume; set { data._costume = value; SignalPropertyChange(); } }
        public byte StockCount { get => data._stockCount; set { data._stockCount = value; SignalPropertyChange(); } }
        //public byte Unknown09       { get { return data._unknown09; }        set { data._unknown09 = value;        SignalPropertyChange(); } }
        public short InitialHitPoints { get => data._initialHitPoints; set { data._initialHitPoints = value; SignalPropertyChange(); } }
        public short StartingDamage { get => data._startingDamage; set { data._startingDamage = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(EventMatchDifficultyData))
            {
                throw new Exception("Wrong size for EventMatchDifficultyNode");
            }

            // Copy the data from the address
            data = *(EventMatchDifficultyData*)WorkingUncompressed.Address;

            return false;
        }
        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(EventMatchDifficultyData*)address = data;
        }
        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            // Constant size (14 bytes)
            return sizeof(EventMatchDifficultyData);
        }
    }

    public unsafe class EventMatchFighterNode : ResourceNode
    {
        public enum StatusEnum : byte
        {
            Normal = 0,
            Metal = 1,
            Invisible = 2
        }

        private EventMatchFighterHeader data;

        [TypeConverter(typeof(DropDownListFighterIDs))]
        public byte FighterID { get => data._fighterID; set { data._fighterID = value; Name = BrawlLib.BrawlCrate.FighterNameGenerators.FromID(data._fighterID, BrawlLib.BrawlCrate.FighterNameGenerators.slotIDIndex, "-S"); SignalPropertyChange(); } }

        public StatusEnum Status { get => (StatusEnum)data._status; set { data._status = (byte)value; SignalPropertyChange(); } }
        //public byte Unknown02  { get { return data._unknown02; }          set { data._unknown02 = value;    SignalPropertyChange(); } }
        //public byte Unknown03  { get { return data._unknown03; }          set { data._unknown03 = value;    SignalPropertyChange(); } }
        public float Scale { get => data._scale; set { data._scale = value; SignalPropertyChange(); } }
        public byte Team { get => data._team; set { data._team = value; SignalPropertyChange(); } }
        //public byte Unknown09  { get { return data._unknown09; }          set { data._unknown09 = value;    SignalPropertyChange(); } }
        //public byte Unknown0a  { get { return data._unknown0a; }          set { data._unknown0a = value;    SignalPropertyChange(); } }
        //public byte Unknown0b  { get { return data._unknown0b; }          set { data._unknown0b = value;    SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            if (WorkingUncompressed.Length != sizeof(EventMatchFighterData))
            {
                throw new Exception("Wrong size for EventMatchFighterNode");
            }

            // Copy the data from the address
            data = *(EventMatchFighterHeader*)WorkingUncompressed.Address;

            if (_name == null)
            {
                bool changed = HasChanged;
                UpdateName();
                HasChanged = changed;
            }

            return true;
        }

        public override void OnPopulate()
        {
            VoidPtr ptr = WorkingUncompressed.Address + sizeof(EventMatchFighterHeader);
            foreach (string s in new string[] { "Easy", "Normal", "Hard" })
            {
                DataSource source = new DataSource(ptr, sizeof(EventMatchDifficultyData));
                EventMatchDifficultyNode node = new EventMatchDifficultyNode();
                node.Initialize(this, source);
                node.Name = s;
                node.IsDirty = false;
                ptr += sizeof(EventMatchDifficultyData);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            // Copy the data back to the address
            *(EventMatchFighterHeader*)address = data;

            // Rebuild children using new address
            VoidPtr ptr = address + sizeof(EventMatchFighterHeader);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(EventMatchDifficultyData), true);
                ptr += sizeof(EventMatchDifficultyData);
            }
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            // Constant size (48 bytes)
            return sizeof(EventMatchFighterData);
        }

        public void UpdateName()
        {
            Name = BrawlLib.BrawlCrate.FighterNameGenerators.FromID(FighterID, BrawlLib.BrawlCrate.FighterNameGenerators.slotIDIndex, "-S");
        }
    }

    public unsafe class EventMatchNode : ResourceNode
    {
        public override ResourceType ResourceType => ResourceType.Container;

        public enum ItemLevelEnum : short
        {
            Off = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            Raining = 4
        }

        public enum MatchTypeEnum : byte
        {
            Time = 0,
            Stock = 1,
            Coin = 2
        }

        private EventMatchTblHeader _header;

        [DisplayName("Event Extension")]
        public bint EventExtension => _header._eventExtension;

        //public bint Unknown04 { get { return _header._unknown04; } set { _header._unknown04 = value; } }

        [DisplayName("Match Type")]
        public MatchTypeEnum MatchType { get => (MatchTypeEnum)_header._matchType; set { _header._matchType = (byte)value; SignalPropertyChange(); } }

        //public byte Unknown09 { get { return _header._unknown09; } set { _header._unknown09 = value; SignalPropertyChange(); } }
        //public byte Unknown0a { get { return _header._unknown0a; } set { _header._unknown0a = value; SignalPropertyChange(); } }
        //public byte Unknown0b { get { return _header._unknown0b; } set { _header._unknown0b = value; SignalPropertyChange(); } }

        [DisplayName("Time Limit")]
        public int TimeLimit { get => _header._timeLimit; set { _header._timeLimit = value; SignalPropertyChange(); } }

        [DisplayName("Timer Visible")]
        public bool TimerVisible
        {
            get => (_header._flags10 & 0x20000000) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags10 |= 0x20000000;
                }
                else
                {
                    _header._flags10 &= ~0x20000000;
                }
            }
        }

        [DisplayName("Hide Countdown")]
        public bool HideCountdown
        {
            get => (_header._flags10 & 0x40000000) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags10 |= 0x40000000;
                }
                else
                {
                    _header._flags10 &= ~0x40000000;
                }
            }
        }

        //public bool UnknownFlag_10_80000000
        //{
        //    get
        //    {
        //        return (_header._flags10 & 0x80000000) != 0;
        //    }
        //    set
        //    {
        //        SignalPropertyChange();
        //        if (value)
        //            _header._flags10 |= unchecked((int)0x80000000);
        //        else
        //            _header._flags10 &= ~unchecked((int)0x80000000);
        //    }
        //}

        //public float Unknown14 { get { return _header._unknown14; } set { _header._unknown14 = value; SignalPropertyChange(); } }

        [DisplayName("Hide Damage Values")]
        public bool HideDamageValues
        {
            get => (_header._flags18 & 0x80) != 0;
            set
            {
                SignalPropertyChange();
                if (value)
                {
                    _header._flags18 |= 0x80;
                }
                else
                {
                    _header._flags18 &= unchecked((byte)~0x80);
                }
            }
        }

        [DisplayName("Team Match")]
        public bool IsTeamGame
        {
            get => _header._isTeamGame != 0;
            set
            {
                SignalPropertyChange();
                _header._isTeamGame = (byte)(value ? 1 : 0);
            }
        }

        [DisplayName("Item Level")]
        public ItemLevelEnum ItemLevel { get => (ItemLevelEnum)(short)_header._itemLevel; set { _header._itemLevel = (short)value; SignalPropertyChange(); } }

        //public byte Unknown1c { get { return _header._unknown1c; } set { _header._unknown1c = value; SignalPropertyChange(); } }
        //public byte Unknown1d { get { return _header._unknown1d; } set { _header._unknown1d = value; SignalPropertyChange(); } }

        [DisplayName("Stage")]
        [TypeConverter(typeof(DropDownListStageIDs))]
        public int StageID
        {
            get => _header._stageID;
            set
            {
                _header._stageID = (ushort)value;
                SignalPropertyChange();
            }
        }

        [DisplayName("Players On Screen")]
        public int PlayersOnScreen
        {
            get => (_header._flags20 >> 21) & 7;
            set
            {
                SignalPropertyChange();
                _header._flags20 &= ~0xE00000;
                _header._flags20 |= (value & 7) << 21;
            }
        }

        //public bool UnknownFlag_20_10000000
        //{
        //    get
        //    {
        //        return (_header._flags20 & 0x10000000) != 0;
        //    }
        //    set
        //    {
        //        SignalPropertyChange();
        //        if (value)
        //            _header._flags20 |= unchecked((int)0x10000000);
        //        else
        //            _header._flags20 &= ~unchecked((int)0x10000000);
        //    }
        //}

        //public int Unknown24 { get { return _header._unknown24; } set { _header._unknown24 = value; SignalPropertyChange(); } }
        //public int Unknown28 { get { return _header._unknown28; } set { _header._unknown28 = value; SignalPropertyChange(); } }
        //public int Unknown2c { get { return _header._unknown2c; } set { _header._unknown2c = value; SignalPropertyChange(); } }
        //public int Unknown30 { get { return _header._unknown30; } set { _header._unknown30 = value; SignalPropertyChange(); } }
        //public int Unknown34 { get { return _header._unknown34; } set { _header._unknown34 = value; SignalPropertyChange(); } }

        [DisplayName("Game Speed")]
        public float GameSpeed { get => _header._gameSpeed; set { _header._gameSpeed = value; SignalPropertyChange(); } }

        [DisplayName("Camera Shake Control")]
        public float CameraShakeControl { get => _header._cameraShakeControl; set { _header._cameraShakeControl = value; SignalPropertyChange(); } }

        //public bool UnknownFlag_40_80000000
        //{
        //    get
        //    {
        //        return (_header._flags40 & 0x80000000) != 0;
        //    }
        //    set
        //    {
        //        SignalPropertyChange();
        //        if (value)
        //            _header._flags40 |= unchecked((int)0x80000000);
        //        else
        //            _header._flags40 &= ~unchecked((int)0x80000000);
        //    }
        //}

        [DisplayName("Song ID")]
        public int SongID { get => _header._songID; set { _header._songID = value; SignalPropertyChange(); } }

        [DisplayName("Global Offense Ratio")]
        public short GlobalOffenseRatio { get => _header._globalOffenseRatio; set { _header._globalOffenseRatio = value; SignalPropertyChange(); } }

        [DisplayName("Global Defense Ratio")]
        public short GlobalDefenseRatio { get => _header._globalDefenseRatio; set { _header._globalDefenseRatio = value; SignalPropertyChange(); } }

        //[DisplayName("Unknown")]
        //public bint Unknown4c { get { return _header._unknown4c; } set { _header._unknown4c = value; SignalPropertyChange(); } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            // Copy the data from the address
            EventMatchTblHeader* dataPtr = (EventMatchTblHeader*)WorkingUncompressed.Address;
            _header = *dataPtr;

            return true;
        }

        public override void OnPopulate()
        {
            int numFighters =
                _header._eventExtension == 0 ? 4
                : _header._eventExtension == 1 ? 9
                : _header._eventExtension == 2 ? 38
                : 0;

            VoidPtr ptr = WorkingUncompressed.Address + sizeof(EventMatchTblHeader);
            for (int i = 0; i < numFighters; i++)
            {
                DataSource source = new DataSource(ptr, sizeof(EventMatchFighterData));
                new EventMatchFighterNode().Initialize(this, source);
                ptr += sizeof(EventMatchFighterData);
            }
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            switch (Children.Count)
            {
                case 4:
                    _header._eventExtension = 0;
                    break;
                case 9:
                    _header._eventExtension = 1;
                    break;
                case 38:
                    _header._eventExtension = 2;
                    break;
                default:
                    throw new Exception("Invalid number of children for EventMatchNode (must be 4, 9, or 38)");
            }

            // Copy the data back to the address
            EventMatchTblHeader* dataPtr = (EventMatchTblHeader*)address;
            *dataPtr = _header;

            // Rebuild children using new address
            VoidPtr ptr = address + sizeof(EventMatchTblHeader);
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Rebuild(ptr, sizeof(EventMatchFighterData), true);
                ptr += sizeof(EventMatchFighterData);
            }
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            int size = sizeof(EventMatchTblHeader);
            foreach (ResourceNode node in Children)
            {
                size += node.CalculateSize(true);
            }

            return size;
        }
    }
}
