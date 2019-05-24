﻿using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    //Alot of this was reused from STPM
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BLOC
    {
        public const uint Tag = 0x434F4C42;
        public const int Size = 0x10;

        public uint _tag;
        public bint _count;
        public bint _version;
        public int _extParam;

        public VoidPtr this[int index] => (byte*)Address + Offsets(index);
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x10 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this) { return ptr; } } }
    }
}