using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class STPMNode : ARCEntryNode
    {
        internal STPM* Header { get { return (STPM*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.STPM; } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            //if (_name == null)
                //_name = "Stage Parameters";

            return Header->_count > 0;
        }

        const int _entrySize = 260;

        public override void OnPopulate()
        {
            for (int i = 0; i < Header->_count; i++)
                new STPMEntryNode().Initialize(this, new DataSource((*Header)[i], _entrySize));
        }
        
        protected override string GetName() {
            return base.GetName("Stage Parameters");
        }

        public override int OnCalculateSize(bool force, bool rebuilding = true)
        {
            return 0x10 + Children.Count * 4 + Children.Count * _entrySize;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            STPM* header = (STPM*)address;
            *header = new STPM(Children.Count);
            uint offset = (uint)(0x10 + (Children.Count * 4));
            for (int i = 0; i < Children.Count; i++)
            {
                ResourceNode r = Children[i];
                *(buint*)((VoidPtr)address + 0x10 + i * 4) = offset;
                r.Rebuild((VoidPtr)address + offset, _entrySize, true);
                offset += _entrySize;
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((STPM*)source.Address)->_tag == STPM.Tag ? new STPMNode() : null; }
    }

    public unsafe class STPMEntryNode : ResourceNode
    {
        internal STPMEntry* Header { get { return (STPMEntry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.Unknown; } }

        public byte echo, id2;
        public ushort id;

        public STPMValueManager _values = new STPMValueManager(null);
        
        // Generate with initial values
        public STPMEntryNode() {
            echo = 0;
            id = 0;
            id2 = 0;
        }

        public class STPMValueManager
        {
            public UnsafeBuffer _values;
            public STPMValueManager(VoidPtr address)
            {
                _values = new UnsafeBuffer(256);
                if (address == null)
                {
                    byte* pOut = (byte*)_values.Address;
                    for (int i = 0; i < 256; i++)
                        *pOut++ = 0;
                }
                else
                {
                    byte* pIn = (byte*)address;
                    byte* pOut = (byte*)_values.Address;
                    for (int i = 0; i < 256; i++)
                        *pOut++ = *pIn++;
                }
            }
            ~STPMValueManager() { _values.Dispose(); }
            public float GetFloat(int index) { return ((bfloat*)_values.Address)[index]; }
            public void SetFloat(int index, float value) { ((bfloat*)_values.Address)[index] = value; }
            public int GetInt(int index) { return ((bint*)_values.Address)[index]; }
            public void SetInt(int index, int value) { ((bint*)_values.Address)[index] = value; }
            public RGBAPixel GetRGBA(int index) { return ((RGBAPixel*)_values.Address)[index]; }
            public void SetRGBA(int index, RGBAPixel value) { ((RGBAPixel*)_values.Address)[index] = value; }
            public byte GetByte(int index, int index2) { return ((byte*)_values.Address)[index * 4 + index2]; }
            public void SetByte(int index, int index2, byte value) { ((byte*)_values.Address)[index * 4 + index2] = value; }
        }

        [LocalizedCategory("STPMData")]
        [LocalizedDisplayName("Echo")]
        public byte Echo { get { return echo; } set { echo = value; SignalPropertyChange(); } }

        [LocalizedCategory("STPMData")]
        [LocalizedDisplayName("ID1")]
        public ushort Id1 { get { return id; } set { id = value; Name = BrawlLib.Properties.Resources.STPMEntry + " [" + id + "]"; SignalPropertyChange(); } }

        [LocalizedCategory("STPMData")]
        [LocalizedDisplayName("ID2")]
        public byte Id2 { get { return id2; } set { id2 = value; SignalPropertyChange(); } }
        
        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal01")]
        public float Value1 { get { return _values.GetFloat(0); } set { _values.SetFloat(0, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal02")]
        public float Value2 { get { return _values.GetFloat(1); } set { _values.SetFloat(1, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal03")]
        public float Value3 { get { return _values.GetFloat(2); } set { _values.SetFloat(2, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal04")]
        public float Value4 { get { return _values.GetFloat(3); } set { _values.SetFloat(3, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal05")]
        public RGBAPixel Value5 { get { return _values.GetRGBA(4); } set { _values.SetRGBA(4, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal06")]
        public float ShadowVerticalAngle { get { return _values.GetFloat(5); } set { _values.SetFloat(5, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal07")]
        public float ShadowHorizontalAngle { get { return _values.GetFloat(6); } set { _values.SetFloat(6, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal08")]
        public float Value8 { get { return _values.GetFloat(7); } set { _values.SetFloat(7, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal09")]
        public float Value9 { get { return _values.GetFloat(8); } set { _values.SetFloat(8, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal10")]
        public float CameraFOV { get { return _values.GetFloat(9); } set { _values.SetFloat(9, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal11")]
        public float MinimumZ { get { return _values.GetFloat(10); } set { _values.SetFloat(10, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal12")]
        public float MaximumZ { get { return _values.GetFloat(11); } set { _values.SetFloat(11, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal13")]
        public float MinimumTilt { get { return _values.GetFloat(12); } set { _values.SetFloat(12, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal14")]
        public float MaximumTilt { get { return _values.GetFloat(13); } set { _values.SetFloat(13, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal15")]
        public float HorizontalRotationFactor { get { return _values.GetFloat(14); } set { _values.SetFloat(14, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal16")]
        public float VerticalRotationFactor { get { return _values.GetFloat(15); } set { _values.SetFloat(15, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal17")]
        public float CharacterBubbleBufferMultiplier { get { return _values.GetFloat(16); } set { _values.SetFloat(16, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal18")]
        public float Value18 { get { return _values.GetFloat(17); } set { _values.SetFloat(17, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal19")]
        public float CameraSpeed { get { return _values.GetFloat(18); } set { _values.SetFloat(18, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal20")]
        public float StarKOCamTilt { get { return _values.GetFloat(19); } set { _values.SetFloat(19, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal21")]
        public float FinalSmashCamTilt { get { return _values.GetFloat(20); } set { _values.SetFloat(20, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal22")]
        public float CameraRight { get { return _values.GetFloat(21); } set { _values.SetFloat(21, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal23")]
        public float CameraLeft { get { return _values.GetFloat(22); } set { _values.SetFloat(22, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal24-25-26")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 PauseCameraPosition
        {
            get
            {
                return new Vector3(_values.GetFloat(23), _values.GetFloat(24), _values.GetFloat(25));
            }

            set
            {
                _values.SetFloat(23, value[0]);
                _values.SetFloat(24, value[1]);
                _values.SetFloat(25, value[2]);
                SignalPropertyChange();
            }
        }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal24")]
        [Browsable(false)]
        public float PauseCamX { get { return _values.GetFloat(23); } set { _values.SetFloat(23, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal25")]
        [Browsable(false)]
        public float PauseCamY { get { return _values.GetFloat(24); } set { _values.SetFloat(24, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal26")]
        [Browsable(false)]
        public float PauseCamZ { get { return _values.GetFloat(25); } set { _values.SetFloat(25, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal27")]
        public float PauseCamAngle { get { return _values.GetFloat(26); } set { _values.SetFloat(26, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal28")]
        public float PauseCamZoomIn { get { return _values.GetFloat(27); } set { _values.SetFloat(27, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal29")]
        public float PauseCamZoomDefault { get { return _values.GetFloat(28); } set { _values.SetFloat(28, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal30")]
        public float PauseCamZoomOut { get { return _values.GetFloat(29); } set { _values.SetFloat(29, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal31")]
        public float PauseCamRotYMin { get { return _values.GetFloat(30); } set { _values.SetFloat(30, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal32")]
        public float PauseCamRotYMax { get { return _values.GetFloat(31); } set { _values.SetFloat(31, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal33")]
        public float PauseCamRotXMin { get { return _values.GetFloat(32); } set { _values.SetFloat(32, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal34")]
        public float PauseCamRotXMax { get { return _values.GetFloat(33); } set { _values.SetFloat(33, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal35-36-37")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 FixedCameraPosition
        {
            get
            {
                return new Vector3(_values.GetFloat(34), _values.GetFloat(35), _values.GetFloat(36));
            }
            set
            {
                _values.SetFloat(34, value[0]);
                _values.SetFloat(35, value[1]);
                _values.SetFloat(36, value[2]);
                SignalPropertyChange();
            }
        }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal35")]
        [Browsable(false)]
        public float FixedCamX { get { return _values.GetFloat(34); } set { _values.SetFloat(34, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal36")]
        [Browsable(false)]
        public float FixedCamY { get { return _values.GetFloat(35); } set { _values.SetFloat(35, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal37")]
        [Browsable(false)]
        public float FixedCamZ { get { return _values.GetFloat(36); } set { _values.SetFloat(36, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal38")]
        public float FixedCamAngle { get { return _values.GetFloat(37); } set { _values.SetFloat(37, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal39")]
        public float FixedHorizontalAngle { get { return _values.GetFloat(38); } set { _values.SetFloat(38, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal40")]
        public float FixedVerticalAngle { get { return _values.GetFloat(39); } set { _values.SetFloat(39, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal41")]
        public float Value41 { get { return _values.GetFloat(40); } set { _values.SetFloat(40, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal42")]
        public float OlimarFinalCamAngle { get { return _values.GetFloat(41); } set { _values.SetFloat(41, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal43-44-45")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 IceClimbersFinalPosition
        {
            get
            {
                return new Vector3(_values.GetFloat(42), _values.GetFloat(43), _values.GetFloat(44));
            }
            set
            {
                _values.SetFloat(42, value[0]);
                _values.SetFloat(43, value[1]);
                _values.SetFloat(44, value[2]);
                SignalPropertyChange();
            }
        }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal43")]
        [Browsable(false)]
        public float IceClimbersFinalPosX { get { return _values.GetFloat(42); } set { _values.SetFloat(42, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal44")]
        [Browsable(false)]
        public float IceClimbersFinalPosY { get { return _values.GetFloat(43); } set { _values.SetFloat(43, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal45")]
        [Browsable(false)]
        public float IceClimbersFinalPosZ { get { return _values.GetFloat(44); } set { _values.SetFloat(44, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal46-47")]
        [TypeConverter(typeof(Vector2StringConverter))]
        public Vector2 IceClimbersFinalScale
        {
            get
            {
                return new Vector2(_values.GetFloat(45), _values.GetFloat(46));
            }
            set
            {
                _values.SetFloat(45, value._x);
                _values.SetFloat(46, value._y);
                SignalPropertyChange();
            }
        }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal46")]
        [Browsable(false)]
        public float IceClimbersFinalScaleX { get { return _values.GetFloat(45); } set { _values.SetFloat(45, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal47")]
        [Browsable(false)]
        public float IceClimbersFinalScaleY { get { return _values.GetFloat(46); } set { _values.SetFloat(46, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal48")]
        public float PitFinalPalutenaScale { get { return _values.GetFloat(47); } set { _values.SetFloat(47, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal49")]
        public float Value49 { get { return _values.GetFloat(48); } set { _values.SetFloat(48, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal50a")]
        public byte StageWindEnabled { get { return _values.GetByte(49, 0); } set { _values.SetByte(49, 0, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal50b")]
        public bool CharacterWindEnabled { get { return _values.GetByte(49, 1) != 0; } set { _values.SetByte(49, 1, (byte)(value ? 1 : 0)); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal50c")]
        public byte Value50c { get { return _values.GetByte(49, 2); } set { _values.SetByte(49, 2, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal50d")]
        public byte Value50d { get { return _values.GetByte(49, 3); } set { _values.SetByte(49, 3, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal51")]
        public float WindStrength { get { return _values.GetFloat(50); } set { _values.SetFloat(50, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal52")]
        public float HorizontalWindRotation { get { return _values.GetFloat(51); } set { _values.SetFloat(51, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal53")]
        public float VerticalWindRotation { get { return _values.GetFloat(52); } set { _values.SetFloat(52, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal54")]
        public float Value54 { get { return _values.GetFloat(53); } set { _values.SetFloat(53, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal55")]
        public RGBAPixel Value55 { get { return _values.GetRGBA(54); } set { _values.SetRGBA(54, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal56")]
        public int Value56 { get { return _values.GetInt(55); } set { _values.SetInt(55, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal57")]
        public int Value57 { get { return _values.GetInt(56); } set { _values.SetInt(56, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal58")]
        public int Value58 { get { return _values.GetInt(57); } set { _values.SetInt(57, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal59")]
        public int EchoMultiplier { get { return _values.GetInt(58); } set { _values.SetInt(58, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal60")]
        public int Value60 { get { return _values.GetInt(59); } set { _values.SetInt(59, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal61")]
        public int Value61 { get { return _values.GetInt(60); } set { _values.SetInt(60, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal62")]
        public int Value62 { get { return _values.GetInt(61); } set { _values.SetInt(61, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal63")]
        public int Value63 { get { return _values.GetInt(62); } set { _values.SetInt(62, value); SignalPropertyChange(); } }

        [LocalizedCategory("STPMValues")]
        [LocalizedDisplayName("STPMVal64")]
        public int Value64 { get { return _values.GetInt(63); } set { _values.SetInt(63, value); SignalPropertyChange(); } }
        
        public override bool OnInitialize()
        {
            id = Header->_id;
            echo = Header->_echo;
            id2 = Header->_id2;

            if (_name == null)
                _name = BrawlLib.Properties.Resources.STPMEntry + " [" + id + "]";

            _values = new STPMValueManager((VoidPtr)Header + 4);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            STPMEntry* header = (STPMEntry*)address;
            *header = new STPMEntry(id, echo, id2);
            byte* pOut = (byte*)header + 4;
            byte* pIn = (byte*)_values._values.Address;
            for (int i = 0; i < 64 * 4; i++)
                *pOut++ = *pIn++;
        }
    }
}
