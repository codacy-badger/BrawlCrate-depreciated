using System;
using BrawlLib.Wii.Audio;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSAREntryNode : ResourceNode
    {
        public override bool AllowDuplicateNames { get { return false; } }

        [Browsable(false)]
        public RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while (((n = n.Parent) != null) && !(n is RSARNode)) ;
                return n as RSARNode;
            }
        }
#if DEBUG
        [Browsable(true), Category("DEBUG")]
#else
        [Browsable(false)]
#endif
        public virtual int StringId { get { return 0; } }
        
        public string InfoIndex
        {
            get
            {
                return "0x" + _infoIndex.ToString("X8");
            }
            set
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                int intValue = Convert.ToByte(field0, fromBase);
                int i = 0;
                Type t = GetType();
                switch (t.Name)
                {
                    case "RSARSoundNode": i = 0; break;
                    case "RSARBankNode": i = 1; break;
                    case "RSARPlayerInfoNode": i = 2; break;
                    case "RSARGroupNode": i = 4; break;
                }

                var list = RSARNode._infoCache[i];
                int prevIndex = _infoIndex;
                _infoIndex = intValue.Clamp(0, list.Count - 1);
                if (_infoIndex == prevIndex)
                    return;

                var temp = list[_infoIndex];
                temp._infoIndex = prevIndex;
                list[_infoIndex] = this;
                list[prevIndex] = temp;
                SignalPropertyChange();
            }
        }

        public static int _soundbankCalc = 331;
        [Category("JOJI Soundbank Expansion")]
        [DisplayName("SawndID (For Calculation Purposes Only)")]
        public int SoundbankCalc
        {
            get
            {
                return _soundbankCalc;
            }
            set
            {
                if (value < 331 || value > 586)
                    return;
                _soundbankCalc = value;
            }
        }

        [Category("JOJI Soundbank Expansion")]
        public string ExpandedInfoIndex
        {
            get
            {
                int a5mult = 0;
                if (_soundbankCalc > 331 && _soundbankCalc < 587)
                    a5mult = _soundbankCalc - 331;
                if (_infoIndex >= 0xA34 && _infoIndex <= 0xA62)
                    return "0x" + (_infoIndex + 0x35CC + (0xA5 * a5mult)).ToString("X8");
                else if (_infoIndex >= 0x18D8 && _infoIndex <= 0x194D)
                    return "0x" + (_infoIndex + 0x2757 + (0xA5 * a5mult)).ToString("X8");
                else
                    return "N/A";
            }
        }
        
        public int _infoIndex;
        internal VoidPtr Data { get { return (VoidPtr)WorkingUncompressed.Address; } }

        [Category("Data"), Browsable(true)]
        public string DataOffset { get { if (RSARNode != null) return ((uint)(Data - (VoidPtr)RSARNode.Header)).ToString("X"); else return null; } }
        
        public VoidPtr _rebuildBase;
        public int _rebuildIndex, _rebuildStringId;

        public override bool OnInitialize()
        {
            if (_name == null)
            {
                RSARNode p = RSARNode;
                if (p != null)
                    _name = p.Header->SYMBBlock->GetStringEntry(StringId);
                else
                    _name = String.Format("Entry{0}", StringId);
            }

            return false;
        }

        internal string _fullPath;
        internal virtual void GetStrings(sbyte* path, int pathLen, RSAREntryList list)
        {
            int len = _name.Length;
            int i = 0;
            if (len == 0)
                return;

            len += pathLen + ((pathLen != 0) ? 1 : 0);

            sbyte* chars = stackalloc sbyte[len];

            if (pathLen > 0)
            {
                while (i < pathLen)
                    chars[i++] = *path++;
                chars[i++] = (sbyte)'_';
            }

            fixed (char* s = _name)
                for (int x = 0; i < len; )
                    chars[i++] = (sbyte)s[x++];
            
            list.AddEntry(_fullPath = len != 0 ? new String(chars, 0, len) : "", this);
        }
    }
}
