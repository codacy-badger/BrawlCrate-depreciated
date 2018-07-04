using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class GEG1Node : ResourceNode
    {
        internal GEG1* Header { get { return (GEG1*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.GEG1; } }


        [Category("GEG1")]
        [DisplayName("Enemy Count")]
        public int count { get { return Header->_count; } }

        const int _entrySize = 0x84;    // The constant size of a child entry

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
            {
                DataSource source;
                if (i == Header->_count - 1)
                { source = new DataSource((*Header)[i], WorkingUncompressed.Address + WorkingUncompressed.Length - (*Header)[i]); }
                else { source = new DataSource((*Header)[i], (*Header)[i + 1] - (*Header)[i]); }
                new GEG1EntryNode().Initialize(this, source);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x08 + (Children.Count * 4) + (Children.Count * _entrySize);
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEG1* header = (GEG1*)address;
            *header = new GEG1(Children.Count);
            uint offset = (uint)(0x08 + (Children.Count * 4));
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*)((VoidPtr)address + 0x08 + i * 4) = offset;
                r.Rebuild((VoidPtr)address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "GEG1";
            return Header->_count > 0;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((GEG1*)source.Address)->_tag == GEG1.Tag ? new GEG1Node() : null; }
    }

    public enum EnemyList
    {
        Goomba = 0,
        Poppant = 1,
        Feyesh = 2,
        Jyk = 3,
        Auroros = 4,
        Cymul = 5,
        Roturret = 6,
        Borboras = 7,
        GiantGoomba = 8,
        Buckot = 9,
        Bucculus = 10,
        Greap = 11,
        Armight = 12,
        BulletBill = 13,
        Roader = 14,
        Spaak = 15,
        Mite = 16,
        Ticken = 17,
        Towtow = 18,
        HammerBro = 19,
        Bytan = 20,
        Floow = 21,
        Puppit = 22,
        Primid = 23,
        Shellpod = 24,
        Koopa = 25,
        Shaydas = 26,
        Bombed = 27,
        MetalPrimid = 28,
        Nagagog = 29,
        Trowlon = 30,
        BigPrimid = 31,
        BoomerangPrimid = 32,
        FirePrimid = 33,
        ScopePrimid = 34,
        SwordPrimid = 35,
        Gamyga = 36,
        ROBBlaster = 37,
        ROBDistance = 38,
        ROBLauncher = 39,
        ROBSentry = 40,
        Autolance = 41,
        Armank = 42,
        Glire = 43,
        Glice = 44,
        Glunder = 45,
        PeteyPiranha = 46,
        GamygaBase = 47,
        Galleom = 51,
        Ridley = 52,
        Rayquaza = 53,
        Duon = 54,
        Porky = 55,
        MetaRidley = 56,
        FalconFlyer = 57,
        Tabuu = 58,
        MasterHand = 59,
        CrazyHand = 60
    }

    public unsafe class GEG1EntryNode : ResourceNode
    {
        internal GEG1Entry* Header { get { return (GEG1Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ENEMY; } }

        // Headers are known
        public uint _header1;
        public uint _header2;
        public byte _unknown8;   // 0x08
        public byte _unknown9;   // 0x09
        public byte _unknown10;  // 0x0A
        public byte _unknown11;  // 0x0B
        public byte _unknown12;  // 0x0C
        public byte _unknown13;  // 0x0D
        public byte _unknown14;  // 0x0E
        public byte _unknown15;  // 0x0F
        public byte _unknown16;  // 0x10
        public byte _unknown17;  // 0x11
        public byte _unknown18;  // 0x12
        public byte _unknown19;  // 0x13
        public byte _unknown20;  // 0x14
        public byte _unknown21;  // 0x15
        public byte _unknown22;  // 0x16
        public byte _unknown23;  // 0x17
        public byte _unknown24;  // 0x18
        public byte _unknown25;  // 0x19
        public byte _unknown26;  // 0x1A
        public byte _unknown27;  // 0x1B
        public byte _unknown28;  // 0x1C
        // EnemyID is known
        public byte _enemyID;   // 0x1D
        public byte _unknown30;  // 0x1E
        public byte _unknown31;  // 0x1F
        public byte _unknown32;  // 0x20
        public byte _unknown33;  // 0x21
        public byte _unknown34;  // 0x22
        public byte _unknown35;  // 0x23
        public byte _unknown36;  // 0x24
        public byte _unknown37;  // 0x25
        public byte _unknown38;  // 0x26
        public byte _unknown39;  // 0x27
        // Spawn Position is known
        public Vector2 _spawnPos;
        public byte _unknown48;  // 0x30
        public byte _unknown49;  // 0x31
        public byte _unknown50;  // 0x32
        public byte _unknown51;  // 0x33
        public byte _unknown52;  // 0x34
        public byte _unknown53;  // 0x35
        public byte _unknown54;  // 0x36
        public byte _unknown55;  // 0x37
        public byte _unknown56;  // 0x38
        public byte _unknown57;  // 0x39
        public byte _unknown58;  // 0x3A
        public byte _unknown59;  // 0x3B
        public byte _unknown60;  // 0x3C
        public byte _unknown61;  // 0x3D
        public byte _unknown62;  // 0x3E
        public byte _unknown63;  // 0x3F
        public byte _unknown64;  // 0x40
        public byte _unknown65;  // 0x41
        public byte _unknown66;  // 0x42
        public byte _unknown67;  // 0x43
        public byte _unknown68;  // 0x44
        public byte _unknown69;  // 0x45
        public byte _unknown70;  // 0x46
        public byte _unknown71;  // 0x47
        public byte _unknown72;  // 0x48
        public byte _unknown73;  // 0x49
        public byte _unknown74;  // 0x4A
        public byte _unknown75;  // 0x4B
        public byte _unknown76;  // 0x4C
        public byte _unknown77;  // 0x4D
        public byte _unknown78;  // 0x4E
        public byte _unknown79;  // 0x4F
        public byte _unknown80;  // 0x50
        public byte _unknown81;  // 0x51
        public byte _unknown82;  // 0x52
        public byte _unknown83;  // 0x53
        public byte _unknown84;  // 0x54
        public byte _unknown85;  // 0x55
        public byte _unknown86;  // 0x56
        public byte _unknown87;  // 0x57
        public byte _unknown88;  // 0x58
        public byte _unknown89;  // 0x59
        public byte _unknown90;  // 0x5A
        public byte _unknown91;  // 0x5B
        public byte _unknown92;  // 0x5C
        public byte _unknown93;  // 0x5D
        public byte _unknown94;  // 0x5E
        public byte _unknown95;  // 0x5F
        public byte _unknown96;  // 0x60
        public byte _unknown97;  // 0x61
        public byte _unknown98;  // 0x62
        public byte _unknown99;  // 0x63
        public byte _unknown100; // 0x64
        public byte _unknown101; // 0x65
        public byte _unknown102; // 0x66
        public byte _unknown103; // 0x67
        public byte _unknown104; // 0x68
        public byte _unknown105; // 0x69
        public byte _unknown106; // 0x6A
        public byte _unknown107; // 0x6B
        public byte _unknown108; // 0x6C
        public byte _unknown109; // 0x6D
        public byte _unknown110; // 0x6E
        public byte _unknown111; // 0x6F
        public byte _unknown112; // 0x70
        public byte _unknown113; // 0x71
        public byte _unknown114; // 0x72
        public byte _unknown115; // 0x73
        public byte _unknown116; // 0x74
        public byte _unknown117; // 0x75
        public byte _unknown118; // 0x76
        public byte _unknown119; // 0x77
        public byte _unknown120; // 0x78
        public byte _unknown121; // 0x79
        public byte _unknown122; // 0x7A
        public byte _unknown123; // 0x7B
        public byte _unknown124; // 0x7C
        public byte _unknown125; // 0x7D
        public byte _unknown126; // 0x7E
        public byte _unknown127; // 0x7F
        public byte _unknown128; // 0x80
        public byte _unknown129; // 0x81
        public byte _unknown130; // 0x82
        public byte _unknown131; // 0x83

        [Browsable(true)]
        [Category("Enemy Info")]
        [DisplayName("Enemy Type")]
        public EnemyList EnemyName
        {
            get
            {
                return (EnemyList)_enemyID;
            }
            set
            {
                _enemyID = (byte)value;
                Name = EnemyNameList();
                SignalPropertyChange();
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy ID")]
        public byte EnemyID
        {
            get
            {
                return _enemyID;
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy ARC ID")]
        public int EnemyArcID
        {
            get
            {
                return _enemyID * 2;
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy BRRES ARC ID")]
        public int EnemyBrresID
        {
            get
            {
                return (_enemyID * 2) + 1;
            }
        }

        [Browsable(true)]
        [Category("Enemy Info")]
        [DisplayName("Spawn Position"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 EnemySpawnPos
        {
            get
            {
                return _spawnPos;
            }
            set
            {
                _spawnPos = value;
                SignalPropertyChange();
            }
        }


        public override bool OnInitialize()
        {
            _header1 = Header->_header1;
            _header2 = Header->_header2;
            _unknown8 = Header->_unknown8;
            _unknown9 = Header->_unknown9;
            _unknown10 = Header->_unknown10;
            _unknown11 = Header->_unknown11;
            _unknown12 = Header->_unknown12;
            _unknown13 = Header->_unknown13;
            _unknown14 = Header->_unknown14;
            _unknown15 = Header->_unknown15;
            _unknown16 = Header->_unknown16;
            _unknown17 = Header->_unknown17;
            _unknown18 = Header->_unknown18;
            _unknown19 = Header->_unknown19;
            _unknown20 = Header->_unknown20;
            _unknown21 = Header->_unknown21;
            _unknown22 = Header->_unknown22;
            _unknown23 = Header->_unknown23;
            _unknown24 = Header->_unknown24;
            _unknown25 = Header->_unknown25;
            _unknown26 = Header->_unknown26;
            _unknown27 = Header->_unknown27;
            _unknown28 = Header->_unknown28;
            _enemyID = Header->_enemyID;
            _unknown30 = Header->_unknown30;
            _unknown31 = Header->_unknown31;
            _unknown32 = Header->_unknown32;
            _unknown33 = Header->_unknown33;
            _unknown34 = Header->_unknown34;
            _unknown35 = Header->_unknown35;
            _unknown36 = Header->_unknown36;
            _unknown37 = Header->_unknown37;
            _unknown38 = Header->_unknown38;
            _unknown39 = Header->_unknown39;
            _spawnPos._x = Header->_spawnX;
            _spawnPos._y = Header->_spawnY;
            _unknown48 = Header->_unknown48;
            _unknown49 = Header->_unknown49;
            _unknown50 = Header->_unknown50;
            _unknown51 = Header->_unknown51;
            _unknown52 = Header->_unknown52;
            _unknown53 = Header->_unknown53;
            _unknown54 = Header->_unknown54;
            _unknown55 = Header->_unknown55;
            _unknown56 = Header->_unknown56;
            _unknown57 = Header->_unknown57;
            _unknown58 = Header->_unknown58;
            _unknown59 = Header->_unknown59;
            _unknown60 = Header->_unknown60;
            _unknown61 = Header->_unknown61;
            _unknown62 = Header->_unknown62;
            _unknown63 = Header->_unknown63;
            _unknown64 = Header->_unknown64;
            _unknown65 = Header->_unknown65;
            _unknown66 = Header->_unknown66;
            _unknown67 = Header->_unknown67;
            _unknown68 = Header->_unknown68;
            _unknown69 = Header->_unknown69;
            _unknown70 = Header->_unknown70;
            _unknown71 = Header->_unknown71;
            _unknown72 = Header->_unknown72;
            _unknown73 = Header->_unknown73;
            _unknown74 = Header->_unknown74;
            _unknown75 = Header->_unknown75;
            _unknown76 = Header->_unknown76;
            _unknown77 = Header->_unknown77;
            _unknown78 = Header->_unknown78;
            _unknown79 = Header->_unknown79;
            _unknown80 = Header->_unknown80;
            _unknown81 = Header->_unknown81;
            _unknown82 = Header->_unknown82;
            _unknown83 = Header->_unknown83;
            _unknown84 = Header->_unknown84;
            _unknown85 = Header->_unknown85;
            _unknown86 = Header->_unknown86;
            _unknown87 = Header->_unknown87;
            _unknown88 = Header->_unknown88;
            _unknown89 = Header->_unknown89;
            _unknown90 = Header->_unknown90;
            _unknown91 = Header->_unknown91;
            _unknown92 = Header->_unknown92;
            _unknown93 = Header->_unknown93;
            _unknown94 = Header->_unknown94;
            _unknown95 = Header->_unknown95;
            _unknown96 = Header->_unknown96;
            _unknown97 = Header->_unknown97;
            _unknown98 = Header->_unknown98;
            _unknown99 = Header->_unknown99;
            _unknown100 = Header->_unknown100;
            _unknown101 = Header->_unknown101;
            _unknown102 = Header->_unknown102;
            _unknown103 = Header->_unknown103;
            _unknown104 = Header->_unknown104;
            _unknown105 = Header->_unknown105;
            _unknown106 = Header->_unknown106;
            _unknown107 = Header->_unknown107;
            _unknown108 = Header->_unknown108;
            _unknown109 = Header->_unknown109;
            _unknown110 = Header->_unknown110;
            _unknown111 = Header->_unknown111;
            _unknown112 = Header->_unknown112;
            _unknown113 = Header->_unknown113;
            _unknown114 = Header->_unknown114;
            _unknown115 = Header->_unknown115;
            _unknown116 = Header->_unknown116;
            _unknown117 = Header->_unknown117;
            _unknown118 = Header->_unknown118;
            _unknown119 = Header->_unknown119;
            _unknown120 = Header->_unknown120;
            _unknown121 = Header->_unknown121;
            _unknown122 = Header->_unknown122;
            _unknown123 = Header->_unknown123;
            _unknown124 = Header->_unknown124;
            _unknown125 = Header->_unknown125;
            _unknown126 = Header->_unknown126;
            _unknown127 = Header->_unknown127;
            _unknown128 = Header->_unknown128;
            _unknown129 = Header->_unknown129;
            _unknown130 = Header->_unknown130;
            _unknown131 = Header->_unknown131;
            if (_name == null)
                _name = EnemyNameList();
            return false;
        }

        public override int OnCalculateSize(bool force)
        {
            return 0x84;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEG1Entry* hdr = (GEG1Entry*)address;
            hdr->_header1 = _header1;
            hdr->_header2 = _header2;
            hdr->_unknown8 = _unknown8;
            hdr->_unknown9 = _unknown9;
            hdr->_unknown10 = _unknown10;
            hdr->_unknown11 = _unknown11;
            hdr->_unknown12 = _unknown12;
            hdr->_unknown13 = _unknown13;
            hdr->_unknown14 = _unknown14;
            hdr->_unknown15 = _unknown15;
            hdr->_unknown16 = _unknown16;
            hdr->_unknown17 = _unknown17;
            hdr->_unknown18 = _unknown18;
            hdr->_unknown19 = _unknown19;
            hdr->_unknown20 = _unknown20;
            hdr->_unknown21 = _unknown21;
            hdr->_unknown22 = _unknown22;
            hdr->_unknown23 = _unknown23;
            hdr->_unknown24 = _unknown24;
            hdr->_unknown25 = _unknown25;
            hdr->_unknown26 = _unknown26;
            hdr->_unknown27 = _unknown27;
            hdr->_unknown28 = _unknown28;
            hdr->_enemyID = _enemyID;
            hdr->_unknown30 = _unknown30;
            hdr->_unknown31 = _unknown31;
            hdr->_unknown32 = _unknown32;
            hdr->_unknown33 = _unknown33;
            hdr->_unknown34 = _unknown34;
            hdr->_unknown35 = _unknown35;
            hdr->_unknown36 = _unknown36;
            hdr->_unknown37 = _unknown37;
            hdr->_unknown38 = _unknown38;
            hdr->_unknown39 = _unknown39;
            hdr->_spawnX = _spawnPos._x;
            hdr->_spawnY = _spawnPos._y;
            hdr->_unknown48 = _unknown48;
            hdr->_unknown49 = _unknown49;
            hdr->_unknown50 = _unknown50;
            hdr->_unknown51 = _unknown51;
            hdr->_unknown52 = _unknown52;
            hdr->_unknown53 = _unknown53;
            hdr->_unknown54 = _unknown54;
            hdr->_unknown55 = _unknown55;
            hdr->_unknown56 = _unknown56;
            hdr->_unknown57 = _unknown57;
            hdr->_unknown58 = _unknown58;
            hdr->_unknown59 = _unknown59;
            hdr->_unknown60 = _unknown60;
            hdr->_unknown61 = _unknown61;
            hdr->_unknown62 = _unknown62;
            hdr->_unknown63 = _unknown63;
            hdr->_unknown64 = _unknown64;
            hdr->_unknown65 = _unknown65;
            hdr->_unknown66 = _unknown66;
            hdr->_unknown67 = _unknown67;
            hdr->_unknown68 = _unknown68;
            hdr->_unknown69 = _unknown69;
            hdr->_unknown70 = _unknown70;
            hdr->_unknown71 = _unknown71;
            hdr->_unknown72 = _unknown72;
            hdr->_unknown73 = _unknown73;
            hdr->_unknown74 = _unknown74;
            hdr->_unknown75 = _unknown75;
            hdr->_unknown76 = _unknown76;
            hdr->_unknown77 = _unknown77;
            hdr->_unknown78 = _unknown78;
            hdr->_unknown79 = _unknown79;
            hdr->_unknown80 = _unknown80;
            hdr->_unknown81 = _unknown81;
            hdr->_unknown82 = _unknown82;
            hdr->_unknown83 = _unknown83;
            hdr->_unknown84 = _unknown84;
            hdr->_unknown85 = _unknown85;
            hdr->_unknown86 = _unknown86;
            hdr->_unknown87 = _unknown87;
            hdr->_unknown88 = _unknown88;
            hdr->_unknown89 = _unknown89;
            hdr->_unknown90 = _unknown90;
            hdr->_unknown91 = _unknown91;
            hdr->_unknown92 = _unknown92;
            hdr->_unknown93 = _unknown93;
            hdr->_unknown94 = _unknown94;
            hdr->_unknown95 = _unknown95;
            hdr->_unknown96 = _unknown96;
            hdr->_unknown97 = _unknown97;
            hdr->_unknown98 = _unknown98;
            hdr->_unknown99 = _unknown99;
            hdr->_unknown100 = _unknown100;
            hdr->_unknown101 = _unknown101;
            hdr->_unknown102 = _unknown102;
            hdr->_unknown103 = _unknown103;
            hdr->_unknown104 = _unknown104;
            hdr->_unknown105 = _unknown105;
            hdr->_unknown106 = _unknown106;
            hdr->_unknown107 = _unknown107;
            hdr->_unknown108 = _unknown108;
            hdr->_unknown109 = _unknown109;
            hdr->_unknown110 = _unknown110;
            hdr->_unknown111 = _unknown111;
            hdr->_unknown112 = _unknown112;
            hdr->_unknown113 = _unknown113;
            hdr->_unknown114 = _unknown114;
            hdr->_unknown115 = _unknown115;
            hdr->_unknown116 = _unknown116;
            hdr->_unknown117 = _unknown117;
            hdr->_unknown118 = _unknown118;
            hdr->_unknown119 = _unknown119;
            hdr->_unknown120 = _unknown120;
            hdr->_unknown121 = _unknown121;
            hdr->_unknown122 = _unknown122;
            hdr->_unknown123 = _unknown123;
            hdr->_unknown124 = _unknown124;
            hdr->_unknown125 = _unknown125;
            hdr->_unknown126 = _unknown126;
            hdr->_unknown127 = _unknown127;
            hdr->_unknown128 = _unknown128;
            hdr->_unknown129 = _unknown129;
            hdr->_unknown130 = _unknown130;
            hdr->_unknown131 = _unknown131;
        }

        private string EnemyNameList()
        {
            if (Enum.IsDefined(typeof(EnemyList), Index))
            {
                switch (_enemyID)
                {
                    case 0:
                        return "Goomba [" + Index + "]";
                    case 1:
                        return "Poppant [" + Index + "]";
                    case 2:
                        return "Feyesh [" + Index + "]";
                    case 3:
                        return "Jyk [" + Index + "]";
                    case 4:
                        return "Auroros [" + Index + "]";
                    case 5:
                        return "Cymul [" + Index + "]";
                    case 6:
                        return "Roturret [" + Index + "]";
                    case 7:
                        return "Borboras [" + Index + "]";
                    case 8:
                        return "Giant Goomba [" + Index + "]";
                    case 9:
                        return "Buckot [" + Index + "]";
                    case 10:
                        return "Bucculus [" + Index + "]";
                    case 11:
                        return "Greap [" + Index + "]";
                    case 12:
                        return "Armight [" + Index + "]";
                    case 13:
                        return "Bullet Bill [" + Index + "]";
                    case 14:
                        return "Roader [" + Index + "]";
                    case 15:
                        return "Spaak [" + Index + "]";
                    case 16:
                        return "Mite [" + Index + "]";
                    case 17:
                        return "Ticken [" + Index + "]";
                    case 18:
                        return "Towtow [" + Index + "]";
                    case 19:
                        return "Hammer Bro [" + Index + "]";
                    case 20:
                        return "Bytan [" + Index + "]";
                    case 21:
                        return "Floow [" + Index + "]";
                    case 22:
                        return "Puppit [" + Index + "]";
                    case 23:
                        return "Primid [" + Index + "]";
                    case 24:
                        return "Shellpod [" + Index + "]";
                    case 25:
                        return "Koopa [" + Index + "]";
                    case 26:
                        return "Shaydas [" + Index + "]";
                    case 27:
                        return "Bombed [" + Index + "]";
                    case 28:
                        return "Metal Primid [" + Index + "]";
                    case 29:
                        return "Nagagog [" + Index + "]";
                    case 30:
                        return "Trowlon [" + Index + "]";
                    case 31:
                        return "Big Primid [" + Index + "]";
                    case 32:
                        return "Boomerang Primid [" + Index + "]";
                    case 33:
                        return "Fire Primid [" + Index + "]";
                    case 34:
                        return "Scope Primid [" + Index + "]";
                    case 35:
                        return "Sword Primid [" + Index + "]";
                    case 36:
                        return "Gamyga [" + Index + "]";
                    case 37:
                        return "R.O.B. Blaster [" + Index + "]";
                    case 38:
                        return "R.O.B. Distance (?) [" + Index + "]";
                    case 39:
                        return "R.O.B. Launcher [" + Index + "]";
                    case 40:
                        return "R.O.B. Sentry [" + Index + "]";
                    case 41:
                        return "Autolance [" + Index + "]";
                    case 42:
                        return "Armank [" + Index + "]";
                    case 43:
                        return "Glire [" + Index + "]";
                    case 44:
                        return "Glice [" + Index + "]";
                    case 45:
                        return "Glunder [" + Index + "]";
                    case 46:
                        return "Petey Piranha [" + Index + "]";
                    case 47:
                        return "Gamyga Base [" + Index + "]";
                    case 51:
                        return "Galleom [" + Index + "]";
                    case 52:
                        return "Ridley [" + Index + "]";
                    case 53:
                        return "Rayquaza [" + Index + "]";
                    case 54:
                        return "Duon [" + Index + "]";
                    case 55:
                        return "Porky [" + Index + "]";
                    case 56:
                        return "Meta Ridley [" + Index + "]";
                    case 57:
                        return "Falcon Flyer [" + Index + "]";
                    case 58:
                        return "Tabuu [" + Index + "]";
                    case 59:
                        return "Master Hand [" + Index + "]";
                    case 60:
                        return "Crazy Hand [" + Index + "]";
                    default:
                        return "Unknown Enemy [" + Index + "]";
                }
            }
            return "Unknown Enemy [" + Index + "]";
        }
    }
}