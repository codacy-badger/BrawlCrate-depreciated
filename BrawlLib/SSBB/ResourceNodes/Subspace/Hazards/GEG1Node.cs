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

    public unsafe class GEG1EntryNode : ResourceNode
    {
        internal GEG1Entry* Header { get { return (GEG1Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ENEMY; } }

        [Browsable(true), TypeConverter(typeof(DropDownListEnemies))]
        [Category("Enemy Info")]
        [DisplayName("Enemy Type")]
        public string Enemy
        {
            get
            {
                int Enemy = *(byte*)(WorkingUncompressed.Address + 0x1D); //enemy id is half of their param brres id
                switch (Enemy)
                {
                    case 0: return "Goomba";
                    case 1: return "Poppant";
                    case 2: return "Feyesh";
                    case 3: return "Jyk";
                    case 4: return "Auroros";
                    case 5: return "Cymul";
                    case 6: return "Roturret";
                    case 7: return "Borboras";
                    case 8: return "Giant Goomba";
                    case 9: return "Buckot";
                    case 10: return "Bucculus";
                    case 11: return "Greap";
                    case 12: return "Armight";
                    case 13: return "Bullet Bill";
                    case 14: return "Roader";
                    case 15: return "Spaak";
                    case 16: return "Mite";
                    case 17: return "Ticken";
                    case 18: return "Towtow";
                    case 19: return "Hammer Bro";
                    case 20: return "Bytan";
                    case 21: return "Floow";
                    case 22: return "Puppit";
                    case 23: return "Primid";
                    case 24: return "Shellpod";
                    case 25: return "Koopa";
                    case 26: return "Shaydas";
                    case 27: return "Bombed";
                    case 28: return "Metal Primid";
                    case 29: return "Nagagog";
                    case 30: return "Trowlon";
                    case 31: return "Big Primid";
                    case 32: return "Boomerang Primid";
                    case 33: return "Fire Primid";
                    case 34: return "Scope Primid";
                    case 35: return "Sword Primid";
                    case 36: return "Gamyga";
                    case 37: return "R.O.B. Blaster";
                    case 38: return "R.O.B. Distance (?)";
                    case 39: return "R.O.B. Launcher";
                    case 40: return "R.O.B. Sentry";
                    case 41: return "Autolance";
                    case 42: return "Armank";
                    case 43: return "Glire";
                    case 44: return "Glice";
                    case 45: return "Glunder";
                    case 46: return "Petey Piranha";
                    case 47: return "Gamyga Base";
                    case 51: return "Galleom";
                    case 52: return "Ridley";
                    case 53: return "Rayquaza";
                    case 54: return "Duon";
                    case 55: return "Porky";
                    case 56: return "Meta Ridley";
                    case 57: return "Falcon Flyer";
                    case 58: return "Tabuu";
                    case 59: return "Master Hand";
                    case 60: return "Crazy Hand";
                    default: return "Unknown";
                }
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy ID")]
        public int EnemyID
        {
            get
            {
                return *(byte*)(WorkingUncompressed.Address + 0x1D);
            }
        }

        [Category("Enemy Info")]
        [DisplayName("Enemy Brres ID")]
        public int EnemyBID
        {
            get
            {
                return *(byte*)(WorkingUncompressed.Address + 0x1D)*2;
            }
        }


        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_name == null)
                _name = "Enemy[" + Index + ']';
            return false;
        }
    }
}