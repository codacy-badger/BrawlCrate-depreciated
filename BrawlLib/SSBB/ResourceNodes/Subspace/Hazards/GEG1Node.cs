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

        internal int _enemy;
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
                _enemy = (int)value;
                generateEnemyName();
                SignalPropertyChange();
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy ID")]
        public int EnemyID
        {
            get
            {
                return _enemy;
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy Brres ID")]
        public int EnemyBID
        {
            get
            {
                return _enemy*2;
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
                generateEnemy_name();
            return false;
        }

        /*public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GEG1Entry* header = (GEG1Entry*)address;
            
        }*/

        private void generateEnemyName()
        {
            if (Index >= 0 && Index <= 255)
            {
                if (Enum.IsDefined(typeof(EnemyList), Index))
                {
                    switch (_enemy)
                    {
                        case 0:
                            Name = "Goomba [" + Index + "]";
                            return;
                        case 1:
                            Name = "Poppant [" + Index + "]";
                            return;
                        case 2:
                            Name = "Feyesh [" + Index + "]";
                            return;
                        case 3:
                            Name = "Jyk [" + Index + "]";
                            return;
                        case 4:
                            Name = "Auroros [" + Index + "]";
                            return;
                        case 5:
                            Name = "Cymul [" + Index + "]";
                            return;
                        case 6:
                            Name = "Roturret [" + Index + "]";
                            return;
                        case 7:
                            Name = "Borboras [" + Index + "]";
                            return;
                        case 8:
                            Name = "Giant Goomba [" + Index + "]";
                            return;
                        case 9:
                            Name = "Buckot [" + Index + "]";
                            return;
                        case 10:
                            Name = "Bucculus [" + Index + "]";
                            return;
                        case 11:
                            Name = "Greap [" + Index + "]";
                            return;
                        case 12:
                            Name = "Armight [" + Index + "]";
                            return;
                        case 13:
                            Name = "Bullet Bill [" + Index + "]";
                            return;
                        case 14:
                            Name = "Roader [" + Index + "]";
                            return;
                        case 15:
                            Name = "Spaak [" + Index + "]";
                            return;
                        case 16:
                            Name = "Mite [" + Index + "]";
                            return;
                        case 17:
                            Name = "Ticken [" + Index + "]";
                            return;
                        case 18:
                            Name = "Towtow [" + Index + "]";
                            return;
                        case 19:
                            Name = "Hammer Bro [" + Index + "]";
                            return;
                        case 20:
                            Name = "Bytan [" + Index + "]";
                            return;
                        case 21:
                            Name = "Floow [" + Index + "]";
                            return;
                        case 22:
                            Name = "Puppit [" + Index + "]";
                            return;
                        case 23:
                            Name = "Primid [" + Index + "]";
                            return;
                        case 24:
                            Name = "Shellpod [" + Index + "]";
                            return;
                        case 25:
                            Name = "Koopa [" + Index + "]";
                            return;
                        case 26:
                            Name = "Shaydas [" + Index + "]";
                            return;
                        case 27:
                            Name = "Bombed [" + Index + "]";
                            return;
                        case 28:
                            Name = "Metal Primid [" + Index + "]";
                            return;
                        case 29:
                            Name = "Nagagog [" + Index + "]";
                            return;
                        case 30:
                            Name = "Trowlon [" + Index + "]";
                            return;
                        case 31:
                            Name = "Big Primid [" + Index + "]";
                            return;
                        case 32:
                            Name = "Boomerang Primid [" + Index + "]";
                            return;
                        case 33:
                            Name = "Fire Primid [" + Index + "]";
                            return;
                        case 34:
                            Name = "Scope Primid [" + Index + "]";
                            return;
                        case 35:
                            Name = "Sword Primid [" + Index + "]";
                            return;
                        case 36:
                            Name = "Gamyga [" + Index + "]";
                            return;
                        case 37:
                            Name = "R.O.B. Blaster [" + Index + "]";
                            return;
                        case 38:
                            Name = "R.O.B. Distance (?) [" + Index + "]";
                            return;
                        case 39:
                            Name = "R.O.B. Launcher [" + Index + "]";
                            return;
                        case 40:
                            Name = "R.O.B. Sentry [" + Index + "]";
                            return;
                        case 41:
                            Name = "Autolance [" + Index + "]";
                            return;
                        case 42:
                            Name = "Armank [" + Index + "]";
                            return;
                        case 43:
                            Name = "Glire [" + Index + "]";
                            return;
                        case 44:
                            Name = "Glice [" + Index + "]";
                            return;
                        case 45:
                            Name = "Glunder [" + Index + "]";
                            return;
                        case 46:
                            Name = "Petey Piranha [" + Index + "]";
                            return;
                        case 47:
                            Name = "Gamyga Base [" + Index + "]";
                            return;
                        case 51:
                            Name = "Galleom [" + Index + "]";
                            return;
                        case 52:
                            Name = "Ridley [" + Index + "]";
                            return;
                        case 53:
                            Name = "Rayquaza [" + Index + "]";
                            return;
                        case 54:
                            Name = "Duon [" + Index + "]";
                            return;
                        case 55:
                            Name = "Porky [" + Index + "]";
                            return;
                        case 56:
                            Name = "Meta Ridley [" + Index + "]";
                            return;
                        case 57:
                            Name = "Falcon Flyer [" + Index + "]";
                            return;
                        case 58:
                            Name = "Tabuu [" + Index + "]";
                            return;
                        case 59:
                            Name = "Master Hand [" + Index + "]";
                            return;
                        case 60:
                            Name = "Crazy Hand [" + Index + "]";
                            return;
                        default:
                            Name = "Unknown Enemy [" + Index + "]";
                            return;
                    }
                }
            }
            Name = "Unknown Enemy [" + Index + "]";
            return;
        }

        private void generateEnemy_name()
        {
            if (Index >= 0 && Index <= 255)
            {
                if (Enum.IsDefined(typeof(EnemyList), Index))
                {
                    switch (_enemy)
                    {
                        case 0:
                            _name = "Goomba [" + Index + "]";
                            return;
                        case 1:
                            _name = "Poppant [" + Index + "]";
                            return;
                        case 2:
                            _name = "Feyesh [" + Index + "]";
                            return;
                        case 3:
                            _name = "Jyk [" + Index + "]";
                            return;
                        case 4:
                            _name = "Auroros [" + Index + "]";
                            return;
                        case 5:
                            _name = "Cymul [" + Index + "]";
                            return;
                        case 6:
                            _name = "Roturret [" + Index + "]";
                            return;
                        case 7:
                            _name = "Borboras [" + Index + "]";
                            return;
                        case 8:
                            _name = "Giant Goomba [" + Index + "]";
                            return;
                        case 9:
                            _name = "Buckot [" + Index + "]";
                            return;
                        case 10:
                            _name = "Bucculus [" + Index + "]";
                            return;
                        case 11:
                            _name = "Greap [" + Index + "]";
                            return;
                        case 12:
                            _name = "Armight [" + Index + "]";
                            return;
                        case 13:
                            _name = "Bullet Bill [" + Index + "]";
                            return;
                        case 14:
                            _name = "Roader [" + Index + "]";
                            return;
                        case 15:
                            _name = "Spaak [" + Index + "]";
                            return;
                        case 16:
                            _name = "Mite [" + Index + "]";
                            return;
                        case 17:
                            _name = "Ticken [" + Index + "]";
                            return;
                        case 18:
                            _name = "Towtow [" + Index + "]";
                            return;
                        case 19:
                            _name = "Hammer Bro [" + Index + "]";
                            return;
                        case 20:
                            _name = "Bytan [" + Index + "]";
                            return;
                        case 21:
                            _name = "Floow [" + Index + "]";
                            return;
                        case 22:
                            _name = "Puppit [" + Index + "]";
                            return;
                        case 23:
                            _name = "Primid [" + Index + "]";
                            return;
                        case 24:
                            _name = "Shellpod [" + Index + "]";
                            return;
                        case 25:
                            _name = "Koopa [" + Index + "]";
                            return;
                        case 26:
                            _name = "Shaydas [" + Index + "]";
                            return;
                        case 27:
                            _name = "Bombed [" + Index + "]";
                            return;
                        case 28:
                            _name = "Metal Primid [" + Index + "]";
                            return;
                        case 29:
                            _name = "Nagagog [" + Index + "]";
                            return;
                        case 30:
                            _name = "Trowlon [" + Index + "]";
                            return;
                        case 31:
                            _name = "Big Primid [" + Index + "]";
                            return;
                        case 32:
                            _name = "Boomerang Primid [" + Index + "]";
                            return;
                        case 33:
                            _name = "Fire Primid [" + Index + "]";
                            return;
                        case 34:
                            _name = "Scope Primid [" + Index + "]";
                            return;
                        case 35:
                            _name = "Sword Primid [" + Index + "]";
                            return;
                        case 36:
                            _name = "Gamyga [" + Index + "]";
                            return;
                        case 37:
                            _name = "R.O.B. Blaster [" + Index + "]";
                            return;
                        case 38:
                            _name = "R.O.B. Distance (?) [" + Index + "]";
                            return;
                        case 39:
                            _name = "R.O.B. Launcher [" + Index + "]";
                            return;
                        case 40:
                            _name = "R.O.B. Sentry [" + Index + "]";
                            return;
                        case 41:
                            _name = "Autolance [" + Index + "]";
                            return;
                        case 42:
                            _name = "Armank [" + Index + "]";
                            return;
                        case 43:
                            _name = "Glire [" + Index + "]";
                            return;
                        case 44:
                            _name = "Glice [" + Index + "]";
                            return;
                        case 45:
                            _name = "Glunder [" + Index + "]";
                            return;
                        case 46:
                            _name = "Petey Piranha [" + Index + "]";
                            return;
                        case 47:
                            _name = "Gamyga Base [" + Index + "]";
                            return;
                        case 51:
                            _name = "Galleom [" + Index + "]";
                            return;
                        case 52:
                            _name = "Ridley [" + Index + "]";
                            return;
                        case 53:
                            _name = "Rayquaza [" + Index + "]";
                            return;
                        case 54:
                            _name = "Duon [" + Index + "]";
                            return;
                        case 55:
                            _name = "Porky [" + Index + "]";
                            return;
                        case 56:
                            _name = "Meta Ridley [" + Index + "]";
                            return;
                        case 57:
                            _name = "Falcon Flyer [" + Index + "]";
                            return;
                        case 58:
                            _name = "Tabuu [" + Index + "]";
                            return;
                        case 59:
                            _name = "Master Hand [" + Index + "]";
                            return;
                        case 60:
                            _name = "Crazy Hand [" + Index + "]";
                            return;
                        default:
                            _name = "Unknown Enemy [" + Index + "]";
                            return;
                    }
                }
            }
            _name = "Unknown Enemy [" + Index + "]";
            return;
        }
    }
}