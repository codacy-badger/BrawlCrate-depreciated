using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using System.BrawlEx;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class FCFGNode : ResourceNode
    {
        internal FCFG* Header { get { return (FCFG*)WorkingUncompressed.Address; } }
        //public override ResourceType ResourceType { get { return ResourceType.FCFG; } }

        public uint _tag;                               // 0x00 - Uneditable; FCFG (Or FITC)
        public uint _size;                              // 0x04 - Uneditable; Should be "100"
        public uint _version;                           // 0x08 - Version; Only parses "2" currently
        public byte _editFlag1;                         // 0x0C - Unused?
        public byte _editFlag2;                         // 0x0D - Unused?
        public byte _editFlag3;                         // 0X0E - Unused?
        public byte _editFlag4;                         // 0X0F - Unused?
        public byte _kirbyCopyColorFlags;               // 0x10 - 0 = No file; 1 = Per Color files; 2: One Kirby file
        public byte _entryColorFlags;                   // 0x11
        public byte _resultColorFlags;                  // 0x12
        public CharacterLoadFlags _characterLoadFlags;  // 0x13 - http://opensa.dantarion.com/wiki/Load_Flags
        public byte _finalSmashColorFlags;              // 0x14 - 0x00 = no file; 0x01 = per color files; 
        public byte _unknown0x15;                       // 0x15
        public CostumeLoadFlags _colorFlags;            // 0x16
        public uint _entryArticleFlag;                  // 0x18
        public uint _soundbank;                         // 0x1C - Parse as list
        public uint _kirbySoundbank;                    // 0x20
        public uint _unknown0x24;                       // 0x24
        public uint _unknown0x28;                       // 0x28
        public uint _unknown0x2C;                       // 0x2C

        // Name stuff. Name should be no more than 16 characters (technical max for filenames is 20 but internal name lowers the limit)
        public byte[] _pacNameArray = new byte[48];     // 0x30 - 48 characters; Auto generate from name: (name-lower)/Fit(name).pac
        public byte[] _kirbyPacNameArray = new byte[48];// 0x60 - 48 characters; Auto generate from name: kirby/FitKirby(name).pac
        public byte[] _moduleNameArray = new byte[32];  // 0x90 - 32 characters; Auto generate from name: ft_(name-lower).rel
        public byte[] _internalNameArray = new byte[16];// 0xB0 - 16 characters; Auto generate from name: (name-upper)

        // Strings
        public string _fighterName;
        public string _pacName;
        public string _kirbyPacName;
        public string _moduleName;
        public string _internalName;

        // IC Constants
        public int _jabFlag;                            // 0xC0 - Usage unknown
        public int _jabCount;                           // 0xC4 - Number of jabs in combo
        public int _hasRapidJab;                        // 0xC8 - Whether the fighter has a rapid jab
        public int _canTilt;                            // 0xCC - Whether the fighter can angle their forward tilt attack
        public int _fSmashCount;                        // 0xD0 - Number of attacks in Fsmash chain
        public int _airJumpCount;                       // 0xD4 - Number of mid-air jumps the fighter has
        public int _canGlide;                           // 0xD8 - Whether the fighter can glide
        public int _canCrawl;                           // 0xDC - Whether the fighter can crawl
        public int _dashAttackIntoCrouch;               // 0xE0 - Whether the fighter's dash attack puts them in a crouching position
        public int _canWallJump;                        // 0xE4 - Whether the fighter can jump off walls
        public int _canCling;                           // 0xE8 - Whether the fighter can cling to walls
        public int _canZAir;                            // 0xEC - Whether the fighter can use an aerial tether
        public uint _u12Flag;                            // 0xF0 - Usage unknown
        public uint _u13Flag;                            // 0xF4 - Usage unknown

        public uint _textureLoad;                        // 0xF8 - 0/1/2/3/4/5
        public uint _aiController;                      // 0xFC

        [Browsable(false)]
        [Flags]
        public enum CharacterLoadFlags : byte
        {
            None = 0x00,
            WorkManageFlag = 0x01,
            FinalSmashFilesFlag = 0x02,
            FinalSmashMusicOffFlag = 0x04,
            IkPhysicsFlag = 0x08,
            MergeMotionEtcFlag = 0x10
        }

        [Browsable(false)]
        [Flags]
        public enum CostumeLoadFlags : ushort
        {
            None = 0x0000,
            CostumeFlag00 = 0x0001,
            CostumeFlag01 = 0x0002,
            CostumeFlag02 = 0x0004,
            CostumeFlag03 = 0x0008,
            CostumeFlag04 = 0x0010,
            CostumeFlag05 = 0x0020,
            CostumeFlag06 = 0x0040,
            CostumeFlag07 = 0x0080,
            CostumeFlag08 = 0x0100,
            CostumeFlag09 = 0x0200,
            CostumeFlag10 = 0x0400,
            CostumeFlag11 = 0x0800,
            UnknownFlag_A = 0x1000,
            UnknownFlag_B = 0x2000,
            // Theoretical Flags
            UnknownFlag_C = 0x4000,
            UnknownFlag_D = 0x8000
        }

        [Category("\t\tFighter")]
        [DisplayName("Fighter Name")]
        public string FighterName
        {
            get
            {
                return _fighterName;
            }
            set
            {
                if (value.Length > 0)
                {
                    if (value.Length <= 16)
                        _fighterName = value;
                    else
                        _fighterName = value.Substring(0, 16);
                    _pacName = PacName;
                    _kirbyPacName = KirbyPacName;
                    _moduleName = ModuleName;
                    _internalName = InternalFighterName;
                    SignalPropertyChange();
                }
            }
        }

        [Category("\t\tFighter")]
        [DisplayName("Pac File Name")]
        public string PacName
        {
            get { return _fighterName.ToLower() + "/Fit" + _fighterName + ".pac"; }
        }

        [Category("\t\tFighter")]
        [DisplayName("Kirby Pac File Name")]
        public string KirbyPacName
        {
            get { return "kirby/FitKirby" + _fighterName + ".pac"; }
        }

        [Category("\t\tFighter")]
        [DisplayName("Module File Name")]
        public string ModuleName
        {
            get { return "ft_" + _fighterName.ToLower() + ".rel"; }
        }

        [Category("\t\tFighter")]
        [DisplayName("Internal Fighter Name")]
        public string InternalFighterName
        {
            get { return _fighterName.ToUpper(); }
        }

        [Category("\tAbilities"), Description("Specifies whether the fighter can crawl.")]
        [DisplayName("Can Crawl")]
        public bool CanCrawl
        {
            get
            {
                return Convert.ToBoolean(_canCrawl);
            }
            set
            {
                _canCrawl = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tAbilities"), Description("Specifies whether the fighter can angle their forward tilt attack.")]
        [DisplayName("Can Angle Forward Tilt")]
        public bool CanFTilt
        {
            get
            {
                return Convert.ToBoolean(_canTilt);
            }
            set
            {
                _canTilt = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tAbilities"), Description("Specifies whether the fighter can glide.")]
        [DisplayName("Can Glide")]
        public bool CanGlide
        {
            get
            {
                return Convert.ToBoolean(_canGlide);
            }
            set
            {
                _canGlide = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tAbilities"), Description("Specifies whether the fighter can cling to walls.")]
        [DisplayName("Can Wall Cling")]
        public bool CanWallCling
        {
            get
            {
                return Convert.ToBoolean(_canCling);
            }
            set
            {
                _canCling = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tAbilities"), Description("Specifies whether the fighter can wall jump.")]
        [DisplayName("Can Wall Jump")]
        public bool CanWallJump
        {
            get
            {
                return Convert.ToBoolean(_canWallJump);
            }
            set
            {
                _canWallJump = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tAbilities"), Description("Specifies whether the fighter can execute an aerial tether using Z.")]
        [DisplayName("Can Z-Air")]
        public bool CanZAir
        {
            get
            {
                return Convert.ToBoolean(_canZAir);
            }
            set
            {
                _canZAir = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tCharacteristics"), Description("Specifies the number of mid-air jumps for the fighter.")]
        [DisplayName("Air Jump Count")]
        public int AirJumpCount
        {
            get
            {
                return _airJumpCount;
            }
            set
            {
                _airJumpCount = value;
                SignalPropertyChange();
            }
        }
        
        [Category("\tCharacteristics"), Description("Specifies whether the fighter will enter the crouch position after a dash attack.")]
        [DisplayName("Dash Attack Into Crouch")]
        public bool DAIntoCrouch
        {
            get
            {
                return Convert.ToBoolean(_dashAttackIntoCrouch);
            }
            set
            {
                _dashAttackIntoCrouch = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }
        
        [Category("\tCharacteristics"), Description("Specifies how many times the fighter can chain their forward smash.")]
        [DisplayName("Forward Smash Count")]
        public int FSmashCount
        {
            get
            {
                return _fSmashCount;
            }
            set
            {
                _fSmashCount = value;
                SignalPropertyChange();
            }
        }
        
        [Category("\tCharacteristics"), Description("Specifies whether the fighter will use a rapid jab at the end of their jab combo.")]
        [DisplayName("Has Rapid Jab")]
        public bool HasRapidJab
        {
            get
            {
                return Convert.ToBoolean(_hasRapidJab);
            }
            set
            {
                _hasRapidJab = Convert.ToInt32(value);
                SignalPropertyChange();
            }
        }

        
        [Category("\tCharacteristics"), Description("Specifies the number of hits for the fighter's jab combo.")]
        [DisplayName("Jab Count")]
        public int JabCount
        {
            get
            {
                return _jabCount;
            }
            set
            {
                _jabCount = value;
                SignalPropertyChange();
            }
        }
        
        [Category("\tCharacteristics"), Description("Usage Unknown.")]
        [DisplayName("Jab Flag")]
        public string JabFlag
        {
            get
            {
                return "0x" + _jabFlag.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _jabFlag = Convert.ToInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume00")]
        public bool HasCostume00
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag00) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag00) | (value ? CostumeLoadFlags.CostumeFlag00 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume01")]
        public bool HasCostume01
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag01) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag01) | (value ? CostumeLoadFlags.CostumeFlag01 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume02")]
        public bool HasCostume02
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag02) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag02) | (value ? CostumeLoadFlags.CostumeFlag02 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume03")]
        public bool HasCostume03
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag03) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag03) | (value ? CostumeLoadFlags.CostumeFlag03 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume04")]
        public bool HasCostume04
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag04) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag04) | (value ? CostumeLoadFlags.CostumeFlag04 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume05")]
        public bool HasCostume05
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag05) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag05) | (value ? CostumeLoadFlags.CostumeFlag05 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume06")]
        public bool HasCostume06
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag06) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag06) | (value ? CostumeLoadFlags.CostumeFlag06 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume07")]
        public bool HasCostume07
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag07) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag07) | (value ? CostumeLoadFlags.CostumeFlag07 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume08")]
        public bool HasCostume08
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag08) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag08) | (value ? CostumeLoadFlags.CostumeFlag08 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume09")]
        public bool HasCostume09
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag09) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag09) | (value ? CostumeLoadFlags.CostumeFlag09 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume10")]
        public bool HasCostume10
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag10) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag10) | (value ? CostumeLoadFlags.CostumeFlag10 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes")]
        [DisplayName("Has Costume11")]
        public bool HasCostume11
        {
            get { return (_colorFlags & CostumeLoadFlags.CostumeFlag11) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.CostumeFlag11) | (value ? CostumeLoadFlags.CostumeFlag11 : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes"), Description("Normally set to True for every character except Mr. Game & Watch.")]
        [DisplayName("Unknown Flag A")]
        public bool UnknownFlagA
        {
            get { return (_colorFlags & CostumeLoadFlags.UnknownFlag_A) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_A) | (value ? CostumeLoadFlags.UnknownFlag_A : 0); SignalPropertyChange(); }
        }

        [Category("\tCostumes"), Description("Usage unknown.")]
        [DisplayName("Unknown Flag B")]
        public bool UnknownFlagB
        {
            get { return (_colorFlags & CostumeLoadFlags.UnknownFlag_B) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_B) | (value ? CostumeLoadFlags.UnknownFlag_B : 0); SignalPropertyChange(); }
        }

        [Browsable(false)]
        [Category("\tCostumes"), Description("Usage unknown.")]
        [DisplayName("Unknown Flag C")]
        public bool UnknownFlagC
        {
            get { return (_colorFlags & CostumeLoadFlags.UnknownFlag_C) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_C) | (value ? CostumeLoadFlags.UnknownFlag_C : 0); SignalPropertyChange(); }
        }

        [Browsable(false)]
        [Category("\tCostumes"), Description("Usage unknown.")]
        [DisplayName("Unknown Flag D")]
        public bool UnknownFlagD
        {
            get { return (_colorFlags & CostumeLoadFlags.UnknownFlag_C) != 0; }
            set { _colorFlags = (_colorFlags & ~CostumeLoadFlags.UnknownFlag_C) | (value ? CostumeLoadFlags.UnknownFlag_C : 0); SignalPropertyChange(); }
        }

        [Category("\tSound"), Description("Normally set to false for characters whose Final Smash is accompanied by music.")]
        [DisplayName("Final Smash Music Flag")]
        public bool FinalSmashMusic
        {
            get { return (_characterLoadFlags & CharacterLoadFlags.FinalSmashMusicOffFlag) != 0; }
            set { _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.FinalSmashMusicOffFlag) | (value ? CharacterLoadFlags.FinalSmashMusicOffFlag : 0); SignalPropertyChange(); }
        }

        [Category("\tSound"), Description("Specifies the sound bank that is loaded for the fighter.")]
        [DisplayName("Sound Bank")]
        public string SoundBank
        {
            get
            {
                return "0x" + _soundbank.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _soundbank = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("\tSound"), Description("Specifies the sound bank that is loaded for the fighter.")]
        [DisplayName("Kirby Sound Bank")]
        public string KirbySoundBank
        {
            get
            {
                return "0x" + _kirbySoundbank.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _kirbySoundbank = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("Misc")]
        [TypeConverter(typeof(DropDownListBrawlExAIControllers))]
        [DisplayName("AI Controller")]
        public uint AIController
        {
            get
            {
                return _aiController;
            }
            set
            {
                _aiController = value;
                SignalPropertyChange();
            }
        }

        [Category("Misc"), Description("Usage Unknown.")]
        [DisplayName("Entry Flag")]
        public string EntryFlag
        {
            get
            {
                return "0x" + _entryArticleFlag.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _entryArticleFlag = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("Misc"), Description("Normally set to True for Ice Climbers and Olimar")]
        [DisplayName("Ik Physics Flag")]
        public bool IkPhysics
        {
            get { return (_characterLoadFlags & CharacterLoadFlags.IkPhysicsFlag) != 0; }
            set { _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.IkPhysicsFlag) | (value ? CharacterLoadFlags.IkPhysicsFlag : 0); SignalPropertyChange(); }
        }

        [Category("Misc"), Description("Specifies what Texture Loader to use when loading the fighter's entry articles.")]
        [DisplayName("Texture Loader")]
        public string TextureLoader
        {
            get
            {
                return "0x" + _textureLoad.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _textureLoad = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("Misc"), Description("Usage Unknown.")]
        [DisplayName("U12 Flag")]
        public string U12Flag
        {
            get
            {
                return "0x" + _u12Flag.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _u12Flag = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("Misc"), Description("Usage Unknown.")]
        [DisplayName("U13 Flag")]
        public string U13Flag
        {
            get
            {
                return "0x" + _u13Flag.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                _u13Flag = Convert.ToUInt32(field0, fromBase);
                SignalPropertyChange();
            }
        }

        [Category("Misc"), Description("Normally set to False for Mario, Luigi, Popo, Nana, and Pit")]
        [DisplayName("Work Manage Flag")]
        public bool WorkManage
        {
            get { return (_characterLoadFlags & CharacterLoadFlags.WorkManageFlag) != 0; }
            set { _characterLoadFlags = (_characterLoadFlags & ~CharacterLoadFlags.WorkManageFlag) | (value ? CharacterLoadFlags.WorkManageFlag : 0); SignalPropertyChange(); }
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
            _kirbyCopyColorFlags = Header->_kirbyCopyColorFlags;
            _entryColorFlags = Header->_entryColorFlags;
            _resultColorFlags = Header->_resultColorFlags;
            _characterLoadFlags = (CharacterLoadFlags)Header->_characterLoadFlags;
            _finalSmashColorFlags = Header->_finalSmashColorFlags;
            _unknown0x15 = Header->_unknown0x15;
            _colorFlags = (CostumeLoadFlags)((ushort)Header->_colorFlags);
            _entryArticleFlag = (uint)Header->_entryArticleFlag;
            _soundbank = (uint)Header->_soundbank;
            _kirbySoundbank = (uint)Header->_kirbySoundbank;
            _unknown0x24 = Header->_unknown0x24;
            _unknown0x28 = Header->_unknown0x28;
            _unknown0x2C = Header->_unknown0x2C;

            for (int i = 0; i < _pacNameArray.Length; i++)
                _pacNameArray[i] = Header->_pacNameArray[i];

            for (int i = 0; i < _kirbyPacNameArray.Length; i++)
                _kirbyPacNameArray[i] = Header->_kirbyPacNameArray[i];

            for (int i = 0; i < _moduleNameArray.Length; i++)
                _moduleNameArray[i] = Header->_moduleNameArray[i];

            for (int i = 0; i < _internalNameArray.Length; i++)
                _internalNameArray[i] = Header->_internalNameArray[i];

            _pacName = System.Text.Encoding.UTF8.GetString(_pacNameArray).TrimEnd(new char[] { '\0' });
            _kirbyPacName = System.Text.Encoding.UTF8.GetString(_kirbyPacNameArray).Substring(0, System.Text.Encoding.UTF8.GetString(_kirbyPacNameArray).IndexOf('\0'));
            _moduleName = System.Text.Encoding.UTF8.GetString(_moduleNameArray).TrimEnd(new char[] { '\0' });
            _internalName = System.Text.Encoding.UTF8.GetString(_internalNameArray).TrimEnd(new char[] { '\0' });
            _fighterName = _kirbyPacName.Substring(14, (_kirbyPacName.Length - 4) - 14);

            _jabFlag = Header->_jabFlag;
            _jabCount = Header->_jabCount;
            _hasRapidJab = Header->_hasRapidJab;
            _canTilt = Header->_canTilt;
            _fSmashCount = Header->_fSmashCount;
            _airJumpCount = Header->_airJumpCount;
            _canGlide = Header->_canGlide;
            _canCrawl = Header->_canCrawl;
            _dashAttackIntoCrouch = Header->_dashAttackIntoCrouch;
            _canWallJump = Header->_canWallJump;
            _canCling = Header->_canCling;
            _canZAir = Header->_canZAir;
            _u12Flag = (uint)Header->_u12Flag;
            _u13Flag = (uint)Header->_u13Flag;

            _textureLoad = (uint)Header->_textureLoad;
            _aiController = (uint)Header->_aiController;
            
            if ((_name == null) && (_origPath != null))
                _name = Path.GetFileNameWithoutExtension(_origPath);

            return true;
        }

        internal static ResourceNode TryParse(DataSource source) { return (((FCFG*)source.Address)->_tag == FCFG.Tag1 || ((FCFG*)source.Address)->_tag == FCFG.Tag2) ? new FCFGNode() : null; }
    }
}
