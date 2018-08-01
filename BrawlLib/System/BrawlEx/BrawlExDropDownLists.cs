using System.ComponentModel;
using System.Globalization;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Linq;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Compression;

namespace System.BrawlEx
{
    // Should be same as Fighter IDs, but kept as a precaution
    public class DropDownListBrawlExSlotIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlLib.SSBB.Fighter.Fighters.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase)
                    ? 16
                    : 10;
                return Convert.ToByte(field0, fromBase);
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(byte))
            {
                var fighter = BrawlLib.SSBB.Fighter.Fighters.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
