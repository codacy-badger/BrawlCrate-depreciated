using System.ComponentModel;
using System.Globalization;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Linq;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Compression;

namespace System.BrawlEx
{
    public class FranchiseIcon
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public FranchiseIcon(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static FranchiseIcon[] Icons = new FranchiseIcon[] {
            //                ID     Display Name
            new FranchiseIcon(0x00, "Super Mario"),
            new FranchiseIcon(0x01, "Donkey Kong"),
            new FranchiseIcon(0x02, "Zelda"),
            new FranchiseIcon(0x03, "Metroid"),
            new FranchiseIcon(0x04, "Yoshi"),
            new FranchiseIcon(0x05, "Kirby"),
            new FranchiseIcon(0x06, "Star Fox"),
            new FranchiseIcon(0x07, "Pokémon"),
            new FranchiseIcon(0x08, "F-Zero"),
            new FranchiseIcon(0x09, "Earthbound"),
            new FranchiseIcon(0x0A, "Ice Climbers"),
            new FranchiseIcon(0x0B, "Wario"),
            new FranchiseIcon(0x0C, "Kid Icarus"),
            new FranchiseIcon(0x0D, "Pikmin"),
            new FranchiseIcon(0x0E, "Fire Emblem"),
            new FranchiseIcon(0x0F, "Gyromite"),
            new FranchiseIcon(0x10, "Metal Gear Solid"),
            new FranchiseIcon(0x11, "Game & Watch"),
            new FranchiseIcon(0x12, "Sonic the Hedgehog"),
            new FranchiseIcon(0x13, "Super Smash Bros."),
            new FranchiseIcon(0x14, "Bowser (PM)"),
            new FranchiseIcon(0xFF, "None")
        };
    }

    public class DropDownListBrawlExIconIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(FranchiseIcon.Icons.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var icon = FranchiseIcon.Icons.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (icon == null ? "" : (" - " + icon.Name));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

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
