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

    public class BrawlExColorID
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public BrawlExColorID(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static BrawlExColorID[] Colors = new BrawlExColorID[] {
            //                 ID     Display Name
            new BrawlExColorID(0x00, "Red (Team Color)"),
            new BrawlExColorID(0x01, "Blue (Team Color)"),
            new BrawlExColorID(0x02, "Yellow"),
            new BrawlExColorID(0x03, "Green (Team Color)"),
            new BrawlExColorID(0x04, "Purple"),
            new BrawlExColorID(0x05, "Light Blue"),
            new BrawlExColorID(0x06, "Pink"),
            new BrawlExColorID(0x07, "Brown"),
            new BrawlExColorID(0x08, "Black"),
            new BrawlExColorID(0x09, "White"),
            new BrawlExColorID(0x0A, "Orange"),
            new BrawlExColorID(0x0B, "Grey")
        };
    }

    public class RecordBank
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public RecordBank(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static RecordBank[] Records = new RecordBank[] {
            //             ID     Display Name
            new RecordBank(0x00, "Mario"),
            new RecordBank(0x01, "Donkey Kong"),
            new RecordBank(0x02, "Link"),
            new RecordBank(0x03, "Samus/Zero-Suit Samus"),
            new RecordBank(0x04, "Yoshi"),
            new RecordBank(0x05, "Kirby"),
            new RecordBank(0x06, "Fox"),
            new RecordBank(0x07, "Pikachu"),
            new RecordBank(0x08, "Luigi"),
            new RecordBank(0x09, "Captain Falcon"),
            new RecordBank(0x0a, "Ness"),
            new RecordBank(0x0b, "Bowser"),
            new RecordBank(0x0c, "Peach"),
            new RecordBank(0x0d, "Zelda/Sheik"),
            new RecordBank(0x0e, "Ice Climbers"),
            new RecordBank(0x0f, "Falco"),
            new RecordBank(0x10, "Ganondorf"),
            new RecordBank(0x11, "Wario"),
            new RecordBank(0x12, "Metaknight"),
            new RecordBank(0x13, "Pit"),
            new RecordBank(0x14, "Olimar"),
            new RecordBank(0x15, "Lucas"),
            new RecordBank(0x16, "Diddy Kong"),
            new RecordBank(0x17, "Dedede"),
            new RecordBank(0x18, "Ike"),
            new RecordBank(0x19, "R.O.B."),
            new RecordBank(0x1a, "Snake"),
            new RecordBank(0x1b, "Pokémon Trainer"),
            new RecordBank(0x1c, "Lucario"),
            new RecordBank(0x1d, "Marth"),
            new RecordBank(0x1e, "Mr. Game & Watch"),
            new RecordBank(0x1f, "Jiggly Puff"),
            new RecordBank(0x20, "Toon Link"),
            new RecordBank(0x21, "Wolf"),
            new RecordBank(0x22, "Sonic")
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

    public class DropDownListBrawlExColorIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlExColorID.Colors.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var color = BrawlExColorID.Colors.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (color == null ? "" : (" - " + color.Name));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExRecordIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(RecordBank.Records.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var record = RecordBank.Records.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (record == null ? "" : (" - " + record.Name));
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
