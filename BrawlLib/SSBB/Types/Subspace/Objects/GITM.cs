﻿using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GITM
    {
        public const uint Tag = 0x4D544947;
        public const int Size = 127;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        public GITM(int count)
        {
            _tag = Tag;
            _count = count;
            _DataOffset = count * 4;
        }

        public VoidPtr this[int index] => (byte*) Address + Offsets(index);

        public uint Offsets(int index)
        {
            return *(buint*) ((byte*) Address + 0x08 + index * 4);
        }

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GITMEntry
    {
        public bfloat _xpos;
        public bfloat _ypos;
        public byte _unknown0;
        public byte _unknown1;
        public byte _unknown2;
        public byte _unknown3;
        public byte _unknown4;
        public byte _unknown5;
        public byte _unknown6;
        public byte _unknown7;
        public byte _unknown0x0A;
        public byte _unknown0x0B;
        public byte _unknown0x0C;
        public byte _unknown0x0D;
        public byte _modeldataid2; // 0x0E
        public byte _unknown0x0F;
        public byte _unknown0x10;
        public byte _unknown0x11;
        public byte _unknown0x12;
        public byte _unknown0x13;
        public byte _unknown0x14;
        public byte _flag0x15;
        public byte _unknown0x16;
        public byte _unknown0x17;
        public byte _unknown0x18;
        public byte _unknown0x19;
        public byte _unknown0x1A;
        public byte _unknown0x1B;
        public byte _unknown0x1C;
        public byte _unknown0x1D;
        public byte _unknown0x1E;
        public byte _unknown0x1F;
        public byte _unknown0x20;
        public byte _unknown0x21;
        public byte _unknown0x22;
        public byte _unknown0x23;
        public byte _unknown0x24;
        public byte _unknown0x25;
        public byte _unknown0x26;
        public byte _unknown0x27;
        public byte _unknown0x28;
        public byte _unknown0x29;
        public byte _unknown0x2A;
        public byte _unknown0x2B;
        public byte _unknown0x2C;
        public byte _unknown0x2D;
        public byte _unknown0x2E;
        public byte _unknown0x2F;
        public byte _unknown0x30;
        public byte _unknown0x31;
        public byte _unknown0x32;
        public byte _unknown0x33;
        public byte _unknown0x34;
        public byte _unknown0x35;
        public byte _unknown0x36;
        public byte _unknown0x37;

        private VoidPtr Address
        {
            get
            {
                fixed (void* ptr = &this)
                {
                    return ptr;
                }
            }
        }
    }
}