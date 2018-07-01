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

        internal byte _enemy;
        internal Vector2 spawnPos;

        [Browsable(true)]
        [Category("Enemy Info")]
        [DisplayName("Enemy Type")]
        public EnemyList EnemyName
        {
            get
            {
                return (EnemyList)_enemy;
            }
            set
            {
                _enemy = (byte)value;
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
                return _enemy;
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy ARC ID")]
        public int EnemyArcID
        {
            get
            {
                return _enemy * 2;
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy BRRES ARC ID")]
        public int EnemyBrresID
        {
            get
            {
                return (_enemy * 2) + 1;
            }
        }

        [Browsable(true)]
        [Category("Enemy Info")]
        [DisplayName("Spawn Position"), TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 EnemySpawnPos
        {
            get
            {
                return spawnPos;
            }
            set
            {
                spawnPos._x = value._x;
                spawnPos._y = value._y;
                SignalPropertyChange();
            }
        }


        public override bool OnInitialize()
        {
            base.OnInitialize();
            _enemy = *(byte*)(WorkingUncompressed.Address + 0x1D);
            spawnPos._x = *(bfloat*)(WorkingUncompressed.Address + 0x28);
            spawnPos._y = *(bfloat*)(WorkingUncompressed.Address + 0x2C);
            if (_name == null)
                _name = EnemyNameList();
            return false;
        }

        /*public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEG1Entry* header = (GEG1Entry*)address;
            
        }*/

        private string EnemyNameList()
        {
            if (Enum.IsDefined(typeof(EnemyList), Index))
            {
                switch (_enemy)
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