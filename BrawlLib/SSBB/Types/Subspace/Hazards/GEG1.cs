using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEG1
    {
        public const uint Tag = 0x31474547;
        public const int Size = 132;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        public GEG1(int count)
        {
            _tag = Tag;
            _count = count;
            _DataOffset = count*4;
        }

        //private GDOR* Address { get { fixed (GDOR* ptr = &this)return ptr; } }
        //public byte* Data { get { return (byte*)(Address + _DataOffset); } }

        public VoidPtr this[int index] { get { return (VoidPtr)((byte*)Address + Offsets(index)); } }
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x08 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GEG1Entry
    {


        // I believe these are constant values for the Header
        public const uint Header1 = 0x3F800000; // 0x00
        public const uint Header2 = 0x0001FF00; // 0x04
        // Unknown values. I just assumed byte for all of them for now
        // Headers are known
        public uint _header1;
        public uint _header2;
        public byte  _unknown8;   // 0x08
        public byte  _unknown9;   // 0x09
        public byte  _unknown10;  // 0x0A
        public byte  _unknown11;  // 0x0B
        public byte  _unknown12;  // 0x0C
        public byte  _unknown13;  // 0x0D
        public byte  _unknown14;  // 0x0E
        public byte  _unknown15;  // 0x0F
        public byte  _unknown16;  // 0x10
        public byte  _unknown17;  // 0x11
        public byte  _unknown18;  // 0x12
        public byte  _unknown19;  // 0x13
        public byte  _unknown20;  // 0x14
        public byte  _unknown21;  // 0x15
        public byte  _unknown22;  // 0x16
        public byte  _unknown23;  // 0x17
        public byte  _unknown24;  // 0x18
        public byte  _unknown25;  // 0x19
        public byte  _unknown26;  // 0x1A
        public byte  _unknown27;  // 0x1B
        public byte  _unknown28;  // 0x1C
        // EnemyID is known
        public byte _enemyID;   // 0x1D
        public byte  _unknown30;  // 0x1E
        public byte  _unknown31;  // 0x1F
        public byte  _unknown32;  // 0x20
        public byte  _unknown33;  // 0x21
        public byte  _unknown34;  // 0x22
        public byte  _unknown35;  // 0x23
        public byte  _unknown36;  // 0x24
        public byte  _unknown37;  // 0x25
        public byte  _unknown38;  // 0x26
        public byte  _unknown39;  // 0x27
        // Spawn Position is known
        public bfloat _spawnX;  // 0x28
        public bfloat _spawnY;  // 0x2C
        public byte  _unknown48;  // 0x30
        public byte  _unknown49;  // 0x31
        public byte  _unknown50;  // 0x32
        public byte  _unknown51;  // 0x33
        public byte  _unknown52;  // 0x34
        public byte  _unknown53;  // 0x35
        public byte  _unknown54;  // 0x36
        public byte  _unknown55;  // 0x37
        public byte  _unknown56;  // 0x38
        public byte  _unknown57;  // 0x39
        public byte  _unknown58;  // 0x3A
        public byte  _unknown59;  // 0x3B
        public byte  _unknown60;  // 0x3C
        public byte  _unknown61;  // 0x3D
        public byte  _unknown62;  // 0x3E
        public byte  _unknown63;  // 0x3F
        public byte  _unknown64;  // 0x40
        public byte  _unknown65;  // 0x41
        public byte  _unknown66;  // 0x42
        public byte  _unknown67;  // 0x43
        public byte  _unknown68;  // 0x44
        public byte  _unknown69;  // 0x45
        public byte  _unknown70;  // 0x46
        public byte  _unknown71;  // 0x47
        public byte  _unknown72;  // 0x48
        public byte  _unknown73;  // 0x49
        public byte  _unknown74;  // 0x4A
        public byte  _unknown75;  // 0x4B
        public byte  _unknown76;  // 0x4C
        public byte  _unknown77;  // 0x4D
        public byte  _unknown78;  // 0x4E
        public byte  _unknown79;  // 0x4F
        public byte  _unknown80;  // 0x50
        public byte  _unknown81;  // 0x51
        public byte  _unknown82;  // 0x52
        public byte  _unknown83;  // 0x53
        public byte  _unknown84;  // 0x54
        public byte  _unknown85;  // 0x55
        public byte  _unknown86;  // 0x56
        public byte  _unknown87;  // 0x57
        public byte  _unknown88;  // 0x58
        public byte  _unknown89;  // 0x59
        public byte  _unknown90;  // 0x5A
        public byte  _unknown91;  // 0x5B
        public byte  _unknown92;  // 0x5C
        public byte  _unknown93;  // 0x5D
        public byte  _unknown94;  // 0x5E
        public byte  _unknown95;  // 0x5F
        public byte  _unknown96;  // 0x60
        public byte  _unknown97;  // 0x61
        public byte  _unknown98;  // 0x62
        public byte  _unknown99;  // 0x63
        public byte  _unknown100; // 0x64
        public byte  _unknown101; // 0x65
        public byte  _unknown102; // 0x66
        public byte  _unknown103; // 0x67
        public byte  _unknown104; // 0x68
        public byte  _unknown105; // 0x69
        public byte  _unknown106; // 0x6A
        public byte  _unknown107; // 0x6B
        public byte  _unknown108; // 0x6C
        public byte  _unknown109; // 0x6D
        public byte  _unknown110; // 0x6E
        public byte  _unknown111; // 0x6F
        public byte  _unknown112; // 0x70
        public byte  _unknown113; // 0x71
        public byte  _unknown114; // 0x72
        public byte  _unknown115; // 0x73
        public byte  _unknown116; // 0x74
        public byte  _unknown117; // 0x75
        public byte  _unknown118; // 0x76
        public byte  _unknown119; // 0x77
        public byte  _unknown120; // 0x78
        public byte  _unknown121; // 0x79
        public byte  _unknown122; // 0x7A
        public byte  _unknown123; // 0x7B
        public byte  _unknown124; // 0x7C
        public byte  _unknown125; // 0x7D
        public byte  _unknown126; // 0x7E
        public byte  _unknown127; // 0x7F
        public byte  _unknown128; // 0x80
        public byte  _unknown129; // 0x81
        public byte  _unknown130; // 0x82
        public byte  _unknown131; // 0x83

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        
        public enum EnemyType : short
        {
            Spaak = 0x0F,
            Prim = 0x17,
            BoxerPrim = 0x14,
            BoomPrim = 0x20,
            SwordPrim = 0x23,
        }
        internal static EnemyType[] _KnownEnemies = 
        {
            EnemyType.Spaak,
            EnemyType.Prim,
            EnemyType.BoxerPrim,
            EnemyType.BoomPrim,
            EnemyType.SwordPrim
        };
    }

}
