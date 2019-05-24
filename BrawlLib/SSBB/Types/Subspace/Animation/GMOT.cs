﻿using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GMOT
    {
        public const uint Tag = 0x544F4D47;
        public const int Size = 484;
        public uint _tag;
        public bint _count;
        public bint _DataOffset;

        public VoidPtr this[int index] => (byte*)Address + Offsets(index);
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x08 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this) { return ptr; } } }
    }
    public unsafe struct GMOTEntry
    {
        private VoidPtr Address { get { fixed (void* ptr = &this) { return ptr; } } }
    }
}