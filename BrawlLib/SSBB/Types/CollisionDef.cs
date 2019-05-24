﻿using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CollisionHeader
    {
        public const int Size = 0x28;

        public bshort _numPoints;
        public bshort _numPlanes;
        public bshort _numObjects;
        public bshort _unk1;
        public bint _pointOffset;
        public bint _planeOffset;
        public bint _objectOffset;
        internal fixed int _pad[5];

        public CollisionHeader(int numPoints, int numPlanes, int numObjects, int unk1)
        {
            _numPoints = (short)numPoints;
            _numPlanes = (short)numPlanes;
            _numObjects = (short)numObjects;
            _unk1 = (short)unk1;
            _pointOffset = 0x28;
            _planeOffset = 0x28 + (numPoints * 8);
            _objectOffset = 0x28 + (numPoints * 8) + (numPlanes * ColPlane.Size);

            fixed (int* p = _pad)
            {
                for (int i = 0; i < 5; i++)
                {
                    p[i] = 0;
                }
            }
        }

        private VoidPtr Address { get { fixed (void* p = &this) { return p; } } }

        public BVec2* Points => (BVec2*)(Address + _pointOffset);
        public ColPlane* Planes => (ColPlane*)(Address + _planeOffset);
        public ColObject* Objects => (ColObject*)(Address + _objectOffset);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColPlane
    {
        public const int Size = 0x10;

        public bshort _point1;
        public bshort _point2;
        public bshort _link1;
        public bshort _link2;
        public bint _magic; //-1
        public bushort _type;
        public CollisionPlaneFlags _flags;
        public byte _material;

        public ColPlane(int pInd1, int pInd2, int pLink1, int pLink2, CollisionPlaneType type, CollisionPlaneFlags2 flags2, CollisionPlaneFlags flags, byte material)
        {
            _point1 = (short)pInd1;
            _point2 = (short)pInd2;
            _link1 = (short)pLink1;
            _link2 = (short)pLink2;
            _magic = -1;
            _type = (ushort)((int)flags2 | (int)type);
            _flags = flags;
            _material = material;
        }

        public CollisionPlaneType Type { get => (CollisionPlaneType)(_type & 0xF); set => _type = (ushort)(_type & 0xFFF0 | (int)value); }
        public CollisionPlaneFlags2 Flags2 { get => (CollisionPlaneFlags2)(_type & 0xFFF0); set => _type = (ushort)(_type & 0x000F | (int)value); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColObject
    {
        public const int Size = 0x6C;

        public bshort _planeIndex;
        public bshort _planeCount;
        public bint _unk1; //0
        public bint _unk2; //0
        public bint _unk3; //0
        public bushort _flags; //2
        public bshort _unk5; //0
        public BVec2 _boxMin;
        public BVec2 _boxMax;
        public bshort _pointOffset;
        public bshort _pointCount;
        public bshort _unk6; //0
        public bshort _boneIndex;
        public fixed byte _modelName[32];
        public fixed byte _boneName[32];

        public ColObject(int planeIndex, int planeCount, int pointOffset, int pointCount, Vector2 boxMin, Vector2 boxMax, string modelName, string boneName,
            int unk1, int unk2, int unk3, int flags, int unk5, int unk6, int boneIndex)
        {
            _planeIndex = (short)planeIndex;
            _planeCount = (short)planeCount;
            _unk1 = unk1;
            _unk2 = unk2;
            _unk3 = unk3;
            _flags = (ushort)flags;
            _unk5 = (short)unk5;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _pointOffset = (short)pointOffset;
            _pointCount = (short)pointCount;
            _unk6 = (short)unk6;
            _boneIndex = (short)boneIndex;

            fixed (byte* p = _modelName)
            {
                SetStr(p, modelName);
            }

            fixed (byte* p = _boneName)
            {
                SetStr(p, boneName);
            }
        }

        public void Set(int planeIndex, int planeCount, Vector2 boxMin, Vector2 boxMax, string modelName, string boneName)
        {
            _planeIndex = (short)planeIndex;
            _planeCount = (short)planeCount;
            _unk1 = 0;
            _unk2 = 0;
            _unk3 = 0;
            _flags = 0;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _unk5 = 0;
            _unk6 = 0;

            ModelName = modelName;
            BoneName = boneName;
        }

        private VoidPtr Address { get { fixed (void* p = &this) { return p; } } }

        public string ModelName
        {
            get => new string((sbyte*)Address + 0x2C);
            set => SetStr((byte*)Address + 0x2C, value);
        }
        public string BoneName
        {
            get => new string((sbyte*)Address + 0x4C);
            set => SetStr((byte*)Address + 0x4C, value);
        }

        private static void SetStr(byte* dPtr, string str)
        {
            int index = 0;
            if (str != null)
            {
                //Fill string
                int len = Math.Min(str.Length, 31);
                fixed (char* p = str)
                {
                    while (index < len)
                    {
                        *dPtr++ = (byte)p[index++];
                    }
                }
            }

            //Fill remaining
            while (index++ < 32)
            {
                *dPtr++ = 0;
            }
        }
    }

    public class CollisionTerrain
    {
        public byte ID { get; private set; }

        public string Name { get; private set; }

        public bool Valid { get; private set; }

        // Pikmin pluck variables (as strings for ease of display)
        public string PluckSpeed { get; private set; }
        public string RedPluckPercent { get; private set; }
        public string BluePluckPercent { get; private set; }
        public string YellowPluckPercent { get; private set; }
        public string PurplePluckPercent { get; private set; }
        public string WhitePluckPercent { get; private set; }

        // SCLA Variables. Can be changed.
        public float Traction { get; private set; }
        public uint HitDataSet { get; private set; }

        // SCLA Subentry: WalkRun
        public bool WalkCreatesDust { get; private set; }
        public byte WalkUnknown2 { get; private set; }
        public ushort WalkUnknown3 { get; private set; }
        public uint WalkGFXFlag { get; private set; }
        public int WalkSFXFlag { get; private set; }

        // SCLA Subentry: JumpLand
        public bool JumpCreatesDust { get; private set; }
        public byte JumpUnknown2 { get; private set; }
        public ushort JumpUnknown3 { get; private set; }
        public uint JumpGFXFlag { get; private set; }
        public int JumpSFXFlag { get; private set; }

        // SCLA Subentry: TumbleLand
        public bool TumbleCreatesDust { get; private set; }
        public byte TumbleUnknown2 { get; private set; }
        public ushort TumbleUnknown3 { get; private set; }
        public uint TumbleGFXFlag { get; private set; }
        public int TumbleSFXFlag { get; private set; }

        public CollisionTerrain(byte identification, string n)
        {
            ID = identification;
            Name = n;
            PluckSpeed = RedPluckPercent = BluePluckPercent = YellowPluckPercent = WhitePluckPercent = PurplePluckPercent = "?";
            Valid = true; // Assume valid unless otherwise defined
        }

        public CollisionTerrain(byte identification, string n, SCLANode s)
        {
            ID = identification;
            Name = n;
            LoadSCLA(s);
        }

        public CollisionTerrain(byte identification, string n, string pl, double r, double y, double b, double p, double w)
        {
            ID = identification;
            Name = n;
            Valid = true; // Assume valid unless otherwise defined
            PluckSpeed = pl;
            RedPluckPercent = r.ToString();
            BluePluckPercent = b.ToString();
            YellowPluckPercent = y.ToString();
            PurplePluckPercent = p.ToString();
            WhitePluckPercent = w.ToString();
        }

        public CollisionTerrain(byte identification, string n, string pl, double r, double y, double b, double p, double w, SCLANode s)
        {
            ID = identification;
            Name = n;
            PluckSpeed = pl;
            RedPluckPercent = r.ToString();
            BluePluckPercent = b.ToString();
            YellowPluckPercent = y.ToString();
            PurplePluckPercent = p.ToString();
            WhitePluckPercent = w.ToString();
            LoadSCLA(s);
        }

        public void LoadSCLA(SCLANode s)
        {
            SCLAEntryNode e = s.FindSCLAEntry(ID);
            if (e != null)
            {
                Valid = true;
                Traction = e.Traction;
                HitDataSet = e.HitDataSet;
                // WalkRun
                WalkCreatesDust = (e.WalkRun.CreatesDust == 1);
                WalkUnknown2 = e.WalkRun.Unknown2;
                WalkUnknown3 = e.WalkRun.Unknown3;
                WalkGFXFlag = e.WalkRun.GFXFlag;
                WalkSFXFlag = e.WalkRun.SFXFlag;
                // JumpLand
                JumpCreatesDust = (e.JumpLand.CreatesDust == 1);
                JumpUnknown2 = e.JumpLand.Unknown2;
                JumpUnknown3 = e.JumpLand.Unknown3;
                JumpGFXFlag = e.JumpLand.GFXFlag;
                JumpSFXFlag = e.JumpLand.SFXFlag;
                // TumbleLand
                TumbleCreatesDust = (e.TumbleLand.CreatesDust == 1);
                TumbleUnknown2 = e.TumbleLand.Unknown2;
                TumbleUnknown3 = e.TumbleLand.Unknown3;
                TumbleGFXFlag = e.TumbleLand.GFXFlag;
                TumbleSFXFlag = e.TumbleLand.SFXFlag;
            }
            else
            {
                Valid = false;
            }
        }

        public override string ToString() { return Name; }

        public static readonly CollisionTerrain[] Terrains = new CollisionTerrain[] {
            //                   ID    Display Name                                                 Pluck Speed     R       Y       B       P       W
            new CollisionTerrain(0x00, BrawlLib.Properties.Resources.Collision0x00,                 "100%",         1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x01, BrawlLib.Properties.Resources.Collision0x01,                 "40%",          1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x02, BrawlLib.Properties.Resources.Collision0x02,                 "100%",         1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x03, BrawlLib.Properties.Resources.Collision0x03,                 "75%",          1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x04, BrawlLib.Properties.Resources.Collision0x04,                 "100%",         1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x05, BrawlLib.Properties.Resources.Collision0x05,                 "80%",          1,      2,      1,      0.8,    0.5),
            new CollisionTerrain(0x06, BrawlLib.Properties.Resources.Collision0x06,                 "80%",          1,      2,      1,      0.8,    0.5),
            new CollisionTerrain(0x07, BrawlLib.Properties.Resources.Collision0x07,                 "120%",         2,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x08, BrawlLib.Properties.Resources.Collision0x08,                 "80%",          1,      1,      2.5,    0.4,    0.5),
            new CollisionTerrain(0x09, BrawlLib.Properties.Resources.Collision0x09,                 "100%",         1,      0.5,    1,      0.2,    1),
            new CollisionTerrain(0x0A, BrawlLib.Properties.Resources.Collision0x0A,                 "100%",         0,      1,      4,      0.4,    0.5),
            new CollisionTerrain(0x0B, BrawlLib.Properties.Resources.Collision0x0B,                 "100%",         1,      1,      1,      0.2,    0.5),
            new CollisionTerrain(0x0C, BrawlLib.Properties.Resources.Collision0x0C,                 "100%",         1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x0D, BrawlLib.Properties.Resources.Collision0x0D,                 "100%",         0.5,    0.5,    1,      0.2,    1),
            new CollisionTerrain(0x0E, BrawlLib.Properties.Resources.Collision0x0E,                 "75%",          0.5,    0.5,    1,      0.2,    0.5),
            new CollisionTerrain(0x0F, BrawlLib.Properties.Resources.Collision0x0F,                 "100%",         0.5,    0.5,    0.5,    1,      0.5),
            new CollisionTerrain(0x10, BrawlLib.Properties.Resources.Collision0x10,                 "80%",          0,      0,      0,      1,      0.5),
            new CollisionTerrain(0x11, BrawlLib.Properties.Resources.Collision0x11,                 "100%",         1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x12, BrawlLib.Properties.Resources.Collision0x12,                 "70%",          1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x13, BrawlLib.Properties.Resources.Collision0x13,                 "70%",          1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x14, BrawlLib.Properties.Resources.Collision0x14,                 "70%",          1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x15, BrawlLib.Properties.Resources.Collision0x15,                 "100%",         1,      1,      1,      0.1,    0.1),
            new CollisionTerrain(0x16, BrawlLib.Properties.Resources.Collision0x16,                 "100%",         1,      1,      2,      0.4,    1),
            new CollisionTerrain(0x17, BrawlLib.Properties.Resources.Collision0x17,                 "100%",         0.3,    0.3,    0.3,    1,      1),
            new CollisionTerrain(0x18, BrawlLib.Properties.Resources.Collision0x18,                 "100%",         1,      1,      0.5,    0.6,    0.5),
            new CollisionTerrain(0x19, BrawlLib.Properties.Resources.Collision0x19,                 "100%",         1,      1,      1,      0.4,    0.5),
            new CollisionTerrain(0x1A, BrawlLib.Properties.Resources.Collision0x1A,                 "100%",         0.6,    0.6,    1,      0.3,    0.4),
            new CollisionTerrain(0x1B, BrawlLib.Properties.Resources.Collision0x1B,                 "100%",         1,      1,      1,      1,      0.5),
            new CollisionTerrain(0x1C, BrawlLib.Properties.Resources.Collision0x1C,                 "100%",         1.5,    1.5,    0.5,    0.5,    0.2),
            new CollisionTerrain(0x1D, BrawlLib.Properties.Resources.Collision0x1D,                 "100%",         0,      0,      0,      1,      0),
            new CollisionTerrain(0x1E, BrawlLib.Properties.Resources.Collision0x1E),
            new CollisionTerrain(0x1F, BrawlLib.Properties.Resources.Collision0x1F),
            new CollisionTerrain(0x20, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x20")),
            new CollisionTerrain(0x21, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x21")),
            new CollisionTerrain(0x22, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x22")),
            new CollisionTerrain(0x23, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x23")),
            new CollisionTerrain(0x24, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x24")),
            new CollisionTerrain(0x25, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x25")),
            new CollisionTerrain(0x26, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x26")),
            new CollisionTerrain(0x27, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x27")),
            new CollisionTerrain(0x28, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x28")),
            new CollisionTerrain(0x29, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x29")),
            new CollisionTerrain(0x2A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x2A")),
            new CollisionTerrain(0x2B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x2B")),
            new CollisionTerrain(0x2C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x2C")),
            new CollisionTerrain(0x2D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x2D")),
            new CollisionTerrain(0x2E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x2E")),
            new CollisionTerrain(0x2F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x2F")),
            new CollisionTerrain(0x30, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x30")),
            new CollisionTerrain(0x31, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x31")),
            new CollisionTerrain(0x32, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x32")),
            new CollisionTerrain(0x33, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x33")),
            new CollisionTerrain(0x34, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x34")),
            new CollisionTerrain(0x35, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x35")),
            new CollisionTerrain(0x36, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x36")),
            new CollisionTerrain(0x37, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x37")),
            new CollisionTerrain(0x38, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x38")),
            new CollisionTerrain(0x39, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x39")),
            new CollisionTerrain(0x3A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x3A")),
            new CollisionTerrain(0x3B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x3B")),
            new CollisionTerrain(0x3C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x3C")),
            new CollisionTerrain(0x3D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x3D")),
            new CollisionTerrain(0x3E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x3E")),
            new CollisionTerrain(0x3F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x3F")),
            new CollisionTerrain(0x40, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x40")),
            new CollisionTerrain(0x41, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x41")),
            new CollisionTerrain(0x42, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x42")),
            new CollisionTerrain(0x43, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x43")),
            new CollisionTerrain(0x44, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x44")),
            new CollisionTerrain(0x45, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x45")),
            new CollisionTerrain(0x46, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x46")),
            new CollisionTerrain(0x47, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x47")),
            new CollisionTerrain(0x48, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x48")),
            new CollisionTerrain(0x49, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x49")),
            new CollisionTerrain(0x4A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x4A")),
            new CollisionTerrain(0x4B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x4B")),
            new CollisionTerrain(0x4C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x4C")),
            new CollisionTerrain(0x4D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x4D")),
            new CollisionTerrain(0x4E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x4E")),
            new CollisionTerrain(0x4F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x4F")),
            new CollisionTerrain(0x50, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x50")),
            new CollisionTerrain(0x51, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x51")),
            new CollisionTerrain(0x52, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x52")),
            new CollisionTerrain(0x53, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x53")),
            new CollisionTerrain(0x54, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x54")),
            new CollisionTerrain(0x55, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x55")),
            new CollisionTerrain(0x56, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x56")),
            new CollisionTerrain(0x57, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x57")),
            new CollisionTerrain(0x58, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x58")),
            new CollisionTerrain(0x59, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x59")),
            new CollisionTerrain(0x5A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x5A")),
            new CollisionTerrain(0x5B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x5B")),
            new CollisionTerrain(0x5C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x5C")),
            new CollisionTerrain(0x5D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x5D")),
            new CollisionTerrain(0x5E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x5E")),
            new CollisionTerrain(0x5F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x5F")),
            new CollisionTerrain(0x60, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x60")),
            new CollisionTerrain(0x61, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x61")),
            new CollisionTerrain(0x62, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x62")),
            new CollisionTerrain(0x63, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x63")),
            new CollisionTerrain(0x64, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x64")),
            new CollisionTerrain(0x65, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x65")),
            new CollisionTerrain(0x66, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x66")),
            new CollisionTerrain(0x67, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x67")),
            new CollisionTerrain(0x68, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x68")),
            new CollisionTerrain(0x69, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x69")),
            new CollisionTerrain(0x6A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x6A")),
            new CollisionTerrain(0x6B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x6B")),
            new CollisionTerrain(0x6C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x6C")),
            new CollisionTerrain(0x6D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x6D")),
            new CollisionTerrain(0x6E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x6E")),
            new CollisionTerrain(0x6F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x6F")),
            new CollisionTerrain(0x70, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x70")),
            new CollisionTerrain(0x71, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x71")),
            new CollisionTerrain(0x72, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x72")),
            new CollisionTerrain(0x73, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x73")),
            new CollisionTerrain(0x74, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x74")),
            new CollisionTerrain(0x75, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x75")),
            new CollisionTerrain(0x76, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x76")),
            new CollisionTerrain(0x77, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x77")),
            new CollisionTerrain(0x78, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x78")),
            new CollisionTerrain(0x79, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x79")),
            new CollisionTerrain(0x7A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x7A")),
            new CollisionTerrain(0x7B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x7B")),
            new CollisionTerrain(0x7C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x7C")),
            new CollisionTerrain(0x7D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x7D")),
            new CollisionTerrain(0x7E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x7E")),
            new CollisionTerrain(0x7F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x7F")),
            new CollisionTerrain(0x80, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x80")),
            new CollisionTerrain(0x81, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x81")),
            new CollisionTerrain(0x82, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x82")),
            new CollisionTerrain(0x83, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x83")),
            new CollisionTerrain(0x84, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x84")),
            new CollisionTerrain(0x85, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x85")),
            new CollisionTerrain(0x86, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x86")),
            new CollisionTerrain(0x87, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x87")),
            new CollisionTerrain(0x88, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x88")),
            new CollisionTerrain(0x89, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x89")),
            new CollisionTerrain(0x8A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x8A")),
            new CollisionTerrain(0x8B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x8B")),
            new CollisionTerrain(0x8C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x8C")),
            new CollisionTerrain(0x8D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x8D")),
            new CollisionTerrain(0x8E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x8E")),
            new CollisionTerrain(0x8F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x8F")),
            new CollisionTerrain(0x90, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x90")),
            new CollisionTerrain(0x91, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x91")),
            new CollisionTerrain(0x92, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x92")),
            new CollisionTerrain(0x93, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x93")),
            new CollisionTerrain(0x94, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x94")),
            new CollisionTerrain(0x95, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x95")),
            new CollisionTerrain(0x96, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x96")),
            new CollisionTerrain(0x97, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x97")),
            new CollisionTerrain(0x98, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x98")),
            new CollisionTerrain(0x99, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x99")),
            new CollisionTerrain(0x9A, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x9A")),
            new CollisionTerrain(0x9B, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x9B")),
            new CollisionTerrain(0x9C, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x9C")),
            new CollisionTerrain(0x9D, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x9D")),
            new CollisionTerrain(0x9E, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x9E")),
            new CollisionTerrain(0x9F, (BrawlLib.Properties.Resources.CollisionExpansion + " 0x9F")),
            new CollisionTerrain(0xA0, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA0")),
            new CollisionTerrain(0xA1, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA1")),
            new CollisionTerrain(0xA2, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA2")),
            new CollisionTerrain(0xA3, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA3")),
            new CollisionTerrain(0xA4, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA4")),
            new CollisionTerrain(0xA5, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA5")),
            new CollisionTerrain(0xA6, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA6")),
            new CollisionTerrain(0xA7, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA7")),
            new CollisionTerrain(0xA8, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA8")),
            new CollisionTerrain(0xA9, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xA9")),
            new CollisionTerrain(0xAA, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xAA")),
            new CollisionTerrain(0xAB, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xAB")),
            new CollisionTerrain(0xAC, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xAC")),
            new CollisionTerrain(0xAD, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xAD")),
            new CollisionTerrain(0xAE, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xAE")),
            new CollisionTerrain(0xAF, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xAF")),
            new CollisionTerrain(0xB0, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB0")),
            new CollisionTerrain(0xB1, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB1")),
            new CollisionTerrain(0xB2, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB2")),
            new CollisionTerrain(0xB3, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB3")),
            new CollisionTerrain(0xB4, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB4")),
            new CollisionTerrain(0xB5, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB5")),
            new CollisionTerrain(0xB6, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB6")),
            new CollisionTerrain(0xB7, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB7")),
            new CollisionTerrain(0xB8, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB8")),
            new CollisionTerrain(0xB9, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xB9")),
            new CollisionTerrain(0xBA, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xBA")),
            new CollisionTerrain(0xBB, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xBB")),
            new CollisionTerrain(0xBC, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xBC")),
            new CollisionTerrain(0xBD, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xBD")),
            new CollisionTerrain(0xBE, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xBE")),
            new CollisionTerrain(0xBF, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xBF")),
            new CollisionTerrain(0xC0, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC0")),
            new CollisionTerrain(0xC1, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC1")),
            new CollisionTerrain(0xC2, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC2")),
            new CollisionTerrain(0xC3, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC3")),
            new CollisionTerrain(0xC4, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC4")),
            new CollisionTerrain(0xC5, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC5")),
            new CollisionTerrain(0xC6, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC6")),
            new CollisionTerrain(0xC7, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC7")),
            new CollisionTerrain(0xC8, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC8")),
            new CollisionTerrain(0xC9, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xC9")),
            new CollisionTerrain(0xCA, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xCA")),
            new CollisionTerrain(0xCB, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xCB")),
            new CollisionTerrain(0xCC, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xCC")),
            new CollisionTerrain(0xCD, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xCD")),
            new CollisionTerrain(0xCE, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xCE")),
            new CollisionTerrain(0xCF, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xCF")),
            new CollisionTerrain(0xD0, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD0")),
            new CollisionTerrain(0xD1, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD1")),
            new CollisionTerrain(0xD2, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD2")),
            new CollisionTerrain(0xD3, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD3")),
            new CollisionTerrain(0xD4, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD4")),
            new CollisionTerrain(0xD5, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD5")),
            new CollisionTerrain(0xD6, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD6")),
            new CollisionTerrain(0xD7, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD7")),
            new CollisionTerrain(0xD8, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD8")),
            new CollisionTerrain(0xD9, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xD9")),
            new CollisionTerrain(0xDA, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xDA")),
            new CollisionTerrain(0xDB, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xDB")),
            new CollisionTerrain(0xDC, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xDC")),
            new CollisionTerrain(0xDD, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xDD")),
            new CollisionTerrain(0xDE, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xDE")),
            new CollisionTerrain(0xDF, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xDF")),
            new CollisionTerrain(0xE0, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE0")),
            new CollisionTerrain(0xE1, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE1")),
            new CollisionTerrain(0xE2, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE2")),
            new CollisionTerrain(0xE3, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE3")),
            new CollisionTerrain(0xE4, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE4")),
            new CollisionTerrain(0xE5, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE5")),
            new CollisionTerrain(0xE6, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE6")),
            new CollisionTerrain(0xE7, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE7")),
            new CollisionTerrain(0xE8, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE8")),
            new CollisionTerrain(0xE9, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xE9")),
            new CollisionTerrain(0xEA, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xEA")),
            new CollisionTerrain(0xEB, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xEB")),
            new CollisionTerrain(0xEC, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xEC")),
            new CollisionTerrain(0xED, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xED")),
            new CollisionTerrain(0xEE, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xEE")),
            new CollisionTerrain(0xEF, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xEF")),
            new CollisionTerrain(0xF0, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF0")),
            new CollisionTerrain(0xF1, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF1")),
            new CollisionTerrain(0xF2, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF2")),
            new CollisionTerrain(0xF3, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF3")),
            new CollisionTerrain(0xF4, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF4")),
            new CollisionTerrain(0xF5, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF5")),
            new CollisionTerrain(0xF6, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF6")),
            new CollisionTerrain(0xF7, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF7")),
            new CollisionTerrain(0xF8, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF8")),
            new CollisionTerrain(0xF9, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xF9")),
            new CollisionTerrain(0xFA, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xFA")),
            new CollisionTerrain(0xFB, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xFB")),
            new CollisionTerrain(0xFC, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xFC")),
            new CollisionTerrain(0xFD, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xFD")),
            new CollisionTerrain(0xFE, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xFE")),
            new CollisionTerrain(0xFF, (BrawlLib.Properties.Resources.CollisionExpansion + " 0xFF"))
        };
    }

    public class CollisionPlaneInteractionType
    {
        public int flags { get; private set; }
        public string Name { get; private set; }
        public override string ToString() { return Name; }

        public CollisionPlaneInteractionType(int id, string n)
        {
            Name = n;
            flags = id;
        }

        public static readonly CollisionPlaneInteractionType[] Terrains = new CollisionPlaneInteractionType[] {
            //                                ID      Display Name
            new CollisionPlaneInteractionType(0x0000, BrawlLib.Properties.Resources.CollisionTypeNone),
            new CollisionPlaneInteractionType(0x0001, BrawlLib.Properties.Resources.CollisionTypeFloor),
            new CollisionPlaneInteractionType(0x0002, BrawlLib.Properties.Resources.CollisionTypeCeiling),
            new CollisionPlaneInteractionType(0x0004, BrawlLib.Properties.Resources.CollisionTypeRightWall),
            new CollisionPlaneInteractionType(0x0008, BrawlLib.Properties.Resources.CollisionTypeLeftWall)
        };
    }

    //public enum CollisionPlaneMaterial : byte
    //{
    //    Basic = 0,                      // Used for many different objects
    //    Rock = 1,                       // Used for Spear Pillar lower floor, PS1 Mountain
    //    Grass = 2,                      // Used for grass or leaves
    //    Soil = 3,                       // Used for PS2 mountain
    //    Wood = 4,                       // Used for trees (PS1 Fire) and logs/planks (Jungle Japes)
    //    LightMetal = 5,                 // Used for thin metal platforms
    //    HeavyMetal = 6,                 // Used for thick metal platforms
    //    Carpet = 7,                     // Used by Rainbow Cruise
    //    Alien = 8,                      // Only used for Brinstar side platforms
    //    Bulborb = 9,                    // Used for Bulborb collision in Distant Planet
    //    Water = 0x0A,                   // Used for splash effects (Summit when sunk)
    //    Rubber = 0x0B,                  // Used for the Trowlon subspace enemy
    //    Slippery = 0x0C,                // Unknown where this is used, but has ice traction
    //    Snow = 0x0D,                    // Used for snowy surfaces that aren't slippery (SSE)
    //    SnowIce = 0x0E,                 // Used for Summit and PS2 Ice Transformation
    //    GameWatch = 0x0F,               // Used for all Flat Zone platforms
    //    SubspaceIce = 0x10,             // Used some places in Subspace (Purple floor where the door to Tabuu is)
    //    Checkered = 0x11,               // Used for Green Greens's checkerboard platforms and the present skin of rolling crates
    //    SpikesTargetTestOnly = 0x12,    // Used for Spike Hazards in Target Test levels and collision hazard #1 for SSE stages. Crashes or has no effect on stages not using a target test module
    //    Hazard2SSEOnly = 0x13,          // Used for hitboxes on certain SSE levels (180002). Crashes or has no effect on versus stages.
    //    Hazard3SSEOnly = 0x14,          // Used for hitboxes on certain SSE levels. Crashes or has no effect on versus stages.
    //    Electroplankton = 0x15,         // Used for Hanenbow leaves
    //    Cloud = 0x16,                   // Used for clouds on Summit and Skyworld
    //    Subspace = 0x17,                // Used for Subspace levels, Tabuu's Residence
    //    StoneBricks = 0x18,             // Used for Spear Pillar upper level
    //    UnknownDustless = 0x19,         // Unknown, doesn't generate dust clouds when landing
    //    MarioBros = 0x1A,               // Used for Mario Bros.
    //    Grate = 0x1B,                   // Used for Delfino Plaza's main platform
    //    Sand = 0x1C,                    // Used for sand (Unknown where used)
    //    Homerun = 0x1D,                 // Used for Home Run Contest, makes Olimar only spawn Purple Pikmin
    //    WaterNoSplash = 0x1E,           // Used for Distant Planet slope during rain
    //    Unknown0x1F = 0x1F,             // 
    //    CollEx32 = 0x20,                // Expanded collisions, require SCLA edits or they won't work properly
    //    CollEx33 = 0x21,
    //    CollEx34 = 0x22,
    //    CollEx35 = 0x23,
    //    CollEx36 = 0x24,
    //    CollEx37 = 0x25,
    //    CollEx38 = 0x26,
    //    CollEx39 = 0x27,
    //    CollEx40 = 0x28,
    //    CollEx41 = 0x29,
    //    CollEx42 = 0x2A,
    //    CollEx43 = 0x2B,
    //    CollEx44 = 0x2C,
    //    CollEx45 = 0x2D,
    //    CollEx46 = 0x2E,
    //    CollEx47 = 0x2F,
    //    CollEx48 = 0x30,
    //    CollEx49 = 0x31,
    //    CollEx50 = 0x32,
    //    CollEx51 = 0x33,
    //    CollEx52 = 0x34,
    //    CollEx53 = 0x35,
    //    CollEx54 = 0x36,
    //    CollEx55 = 0x37,
    //    CollEx56 = 0x38,
    //    CollEx57 = 0x39,
    //    CollEx58 = 0x3A,
    //    CollEx59 = 0x3B,
    //    CollEx60 = 0x3C,
    //    CollEx61 = 0x3D,
    //    CollEx62 = 0x3E,
    //    CollEx63 = 0x3F,
    //    CollEx64 = 0x40,
    //    CollEx65 = 0x41,
    //    CollEx66 = 0x42,
    //    CollEx67 = 0x43,
    //    CollEx68 = 0x44,
    //    CollEx69 = 0x45,
    //    CollEx70 = 0x46,
    //    CollEx71 = 0x47,
    //    CollEx72 = 0x48,
    //    CollEx73 = 0x49,
    //    CollEx74 = 0x4A,
    //    CollEx75 = 0x4B,
    //    CollEx76 = 0x4C,
    //    CollEx77 = 0x4D,
    //    CollEx78 = 0x4E,
    //    CollEx79 = 0x4F,
    //    CollEx80 = 0x50,
    //    CollEx81 = 0x51,
    //    CollEx82 = 0x52,
    //    CollEx83 = 0x53,
    //    CollEx84 = 0x54,
    //    CollEx85 = 0x55,
    //    CollEx86 = 0x56,
    //    CollEx87 = 0x57,
    //    CollEx88 = 0x58,
    //    CollEx89 = 0x59,
    //    CollEx90 = 0x5A,
    //    CollEx91 = 0x5B,
    //    CollEx92 = 0x5C,
    //    CollEx93 = 0x5D,
    //    CollEx94 = 0x5E,
    //    CollEx95 = 0x5F,
    //    CollEx96 = 0x60,
    //    CollEx97 = 0x61,
    //    CollEx98 = 0x62,
    //    CollEx99 = 0x63,
    //    CollEx100 = 0x64,
    //    CollEx101 = 0x65,
    //    CollEx102 = 0x66,
    //    CollEx103 = 0x67,
    //    CollEx104 = 0x68,
    //    CollEx105 = 0x69,
    //    CollEx106 = 0x6A,
    //    CollEx107 = 0x6B,
    //    CollEx108 = 0x6C,
    //    CollEx109 = 0x6D,
    //    CollEx110 = 0x6E,
    //    CollEx111 = 0x6F,
    //    CollEx112 = 0x70,
    //    CollEx113 = 0x71,
    //    CollEx114 = 0x72,
    //    CollEx115 = 0x73,
    //    CollEx116 = 0x74,
    //    CollEx117 = 0x75,
    //    CollEx118 = 0x76,
    //    CollEx119 = 0x77,
    //    CollEx120 = 0x78,
    //    CollEx121 = 0x79,
    //    CollEx122 = 0x7A,
    //    CollEx123 = 0x7B,
    //    CollEx124 = 0x7C,
    //    CollEx125 = 0x7D,
    //    CollEx126 = 0x7E,
    //    CollEx127 = 0x7F,
    //    CollEx128 = 0x80,
    //    CollEx129 = 0x81,
    //    CollEx130 = 0x82,
    //    CollEx131 = 0x83,
    //    CollEx132 = 0x84,
    //    CollEx133 = 0x85,
    //    CollEx134 = 0x86,
    //    CollEx135 = 0x87,
    //    CollEx136 = 0x88,
    //    CollEx137 = 0x89,
    //    CollEx138 = 0x8A,
    //    CollEx139 = 0x8B,
    //    CollEx140 = 0x8C,
    //    CollEx141 = 0x8D,
    //    CollEx142 = 0x8E,
    //    CollEx143 = 0x8F,
    //    CollEx144 = 0x90,
    //    CollEx145 = 0x91,
    //    CollEx146 = 0x92,
    //    CollEx147 = 0x93,
    //    CollEx148 = 0x94,
    //    CollEx149 = 0x95,
    //    CollEx150 = 0x96,
    //    CollEx151 = 0x97,
    //    CollEx152 = 0x98,
    //    CollEx153 = 0x99,
    //    CollEx154 = 0x9A,
    //    CollEx155 = 0x9B,
    //    CollEx156 = 0x9C,
    //    CollEx157 = 0x9D,
    //    CollEx158 = 0x9E,
    //    CollEx159 = 0x9F,
    //    CollEx160 = 0xA0,
    //    CollEx161 = 0xA1,
    //    CollEx162 = 0xA2,
    //    CollEx163 = 0xA3,
    //    CollEx164 = 0xA4,
    //    CollEx165 = 0xA5,
    //    CollEx166 = 0xA6,
    //    CollEx167 = 0xA7,
    //    CollEx168 = 0xA8,
    //    CollEx169 = 0xA9,
    //    CollEx170 = 0xAA,
    //    CollEx171 = 0xAB,
    //    CollEx172 = 0xAC,
    //    CollEx173 = 0xAD,
    //    CollEx174 = 0xAE,
    //    CollEx175 = 0xAF,
    //    CollEx176 = 0xB0,
    //    CollEx177 = 0xB1,
    //    CollEx178 = 0xB2,
    //    CollEx179 = 0xB3,
    //    CollEx180 = 0xB4,
    //    CollEx181 = 0xB5,
    //    CollEx182 = 0xB6,
    //    CollEx183 = 0xB7,
    //    CollEx184 = 0xB8,
    //    CollEx185 = 0xB9,
    //    CollEx186 = 0xBA,
    //    CollEx187 = 0xBB,
    //    CollEx188 = 0xBC,
    //    CollEx189 = 0xBD,
    //    CollEx190 = 0xBE,
    //    CollEx191 = 0xBF,
    //    CollEx192 = 0xC0,
    //    CollEx193 = 0xC1,
    //    CollEx194 = 0xC2,
    //    CollEx195 = 0xC3,
    //    CollEx196 = 0xC4,
    //    CollEx197 = 0xC5,
    //    CollEx198 = 0xC6,
    //    CollEx199 = 0xC7,
    //    CollEx200 = 0xC8,
    //    CollEx201 = 0xC9,
    //    CollEx202 = 0xCA,
    //    CollEx203 = 0xCB,
    //    CollEx204 = 0xCC,
    //    CollEx205 = 0xCD,
    //    CollEx206 = 0xCE,
    //    CollEx207 = 0xCF,
    //    CollEx208 = 0xD0,
    //    CollEx209 = 0xD1,
    //    CollEx210 = 0xD2,
    //    CollEx211 = 0xD3,
    //    CollEx212 = 0xD4,
    //    CollEx213 = 0xD5,
    //    CollEx214 = 0xD6,
    //    CollEx215 = 0xD7,
    //    CollEx216 = 0xD8,
    //    CollEx217 = 0xD9,
    //    CollEx218 = 0xDA,
    //    CollEx219 = 0xDB,
    //    CollEx220 = 0xDC,
    //    CollEx221 = 0xDD,
    //    CollEx222 = 0xDE,
    //    CollEx223 = 0xDF,
    //    CollEx224 = 0xE0,
    //    CollEx225 = 0xE1,
    //    CollEx226 = 0xE2,
    //    CollEx227 = 0xE3,
    //    CollEx228 = 0xE4,
    //    CollEx229 = 0xE5,
    //    CollEx230 = 0xE6,
    //    CollEx231 = 0xE7,
    //    CollEx232 = 0xE8,
    //    CollEx233 = 0xE9,
    //    CollEx234 = 0xEA,
    //    CollEx235 = 0xEB,
    //    CollEx236 = 0xEC,
    //    CollEx237 = 0xED,
    //    CollEx238 = 0xEE,
    //    CollEx239 = 0xEF,
    //    CollEx240 = 0xF0,
    //    CollEx241 = 0xF1,
    //    CollEx242 = 0xF2,
    //    CollEx243 = 0xF3,
    //    CollEx244 = 0xF4,
    //    CollEx245 = 0xF5,
    //    CollEx246 = 0xF6,
    //    CollEx247 = 0xF7,
    //    CollEx248 = 0xF8,
    //    CollEx249 = 0xF9,
    //    CollEx250 = 0xFA,
    //    CollEx251 = 0xFB,
    //    CollEx252 = 0xFC,
    //    CollEx253 = 0xFD,
    //    CollEx254 = 0xFE,
    //    CollEx255 = 0xFF
    //}

    //public enum CollisionPlaneMaterialUnexpanded : byte
    //{
    //    Basic = 0,                      // Used for many different objects
    //    Rock = 1,                       // Used for Spear Pillar lower floor, PS1 Mountain
    //    Grass = 2,                      // Used for grass or leaves
    //    Soil = 3,                       // Used for PS2 mountain
    //    Wood = 4,                       // Used for trees (PS1 Fire) and logs/planks (Jungle Japes)
    //    LightMetal = 5,                 // Used for thin metal platforms
    //    HeavyMetal = 6,                 // Used for thick metal platforms
    //    Carpet = 7,                     // Used by Rainbow Cruise
    //    Alien = 8,                      // Only used for Brinstar side platforms
    //    Bulborb = 9,                    // Used for Bulborb collision in Distant Planet
    //    Water = 0x0A,                   // Used for splash effects (Summit when sunk)
    //    Rubber = 0x0B,                  // Used for the Trowlon subspace enemy
    //    Slippery = 0x0C,                // Unknown where this is used, but has ice traction
    //    Snow = 0x0D,                    // Used for snowy surfaces that aren't slippery (SSE)
    //    SnowIce = 0x0E,                 // Used for Summit and PS2 Ice Transformation
    //    GameWatch = 0x0F,               // Used for all Flat Zone platforms
    //    SubspaceIce = 0x10,             // Used some places in Subspace (Purple floor where the door to Tabuu is)
    //    Checkered = 0x11,               // Used for Green Greens's checkerboard platforms and the present skin of rolling crates
    //    SpikesTargetTestOnly = 0x12,    // Used for Spike Hazards in Target Test levels and collision hazard #1 for SSE stages. Crashes or has no effect on stages not using a target test module
    //    Hazard2SSEOnly = 0x13,          // Used for hitboxes on certain SSE levels (180002). Crashes or has no effect on versus stages.
    //    Hazard3SSEOnly = 0x14,          // Used for hitboxes on certain SSE levels. Crashes or has no effect on versus stages.
    //    Electroplankton = 0x15,         // Used for Hanenbow leaves
    //    Cloud = 0x16,                   // Used for clouds on Summit and Skyworld
    //    Subspace = 0x17,                // Used for Subspace levels, Tabuu's Residence
    //    StoneBricks = 0x18,             // Used for Spear Pillar upper level
    //    UnknownDustless = 0x19,         // Unknown, doesn't generate dust clouds when landing
    //    MarioBros = 0x1A,               // Used for Mario Bros.
    //    Grate = 0x1B,                   // Used for Delfino Plaza's main platform
    //    Sand = 0x1C,                    // Used for sand (Unknown where used)
    //    Homerun = 0x1D,                 // Used for Home Run Contest, makes Olimar only spawn Purple Pikmin
    //    WaterNoSplash = 0x1E,           // Used for Distant Planet slope during rain
    //    Unknown0x1F = 0x1F              // 
    //}

    public enum CollisionPlaneType
    {
        None = 0x0000,          // 0000
        Floor = 0x0001,         // 0001
        Ceiling = 0x0002,       // 0010
        RightWall = 0x0004,     // 0100
        LeftWall = 0x0008       // 1000
    }

    [Flags]
    public enum CollisionPlaneFlags2
    {
        None = 0x0000,
        Characters = 0x0010,        // Characters (Also allows Items and PT to interact)
        Items = 0x0020,             // Items
        PokemonTrainer = 0x0040,    // Pokemon Trainer
        UnknownStageBox = 0x0080    // Unknown, used in the SSE
    }

    [Flags]
    public enum CollisionPlaneFlags : byte
    {
        None = 0x00,
        DropThrough = 0x01,         // Can fall through a floor by pressing down
        Unknown1 = 0x02,            // 
        Rotating = 0x04,            // Automatically changes between floor/wall/ceiling based on angle
        Unknown3 = 0x08,            // 
        Unknown4 = 0x10,            //
        LeftLedge = 0x20,           // Can grab ledge from the left
        RightLedge = 0x40,          // Can grab ledge from the right
        NoWalljump = 0x80           // Cannot walljump off when set
    }
}
