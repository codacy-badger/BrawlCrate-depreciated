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
            new RecordBank(0x03, "Samus/Zero Suit Samus"),
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
            new RecordBank(0x12, "Meta Knight"),
            new RecordBank(0x13, "Pit"),
            new RecordBank(0x14, "Pikmin & Olimar"),
            new RecordBank(0x15, "Lucas"),
            new RecordBank(0x16, "Diddy Kong"),
            new RecordBank(0x17, "King Dedede"),
            new RecordBank(0x18, "Ike"),
            new RecordBank(0x19, "R.O.B."),
            new RecordBank(0x1a, "Snake"),
            new RecordBank(0x1b, "Pokémon Trainer"),
            new RecordBank(0x1c, "Lucario"),
            new RecordBank(0x1d, "Marth"),
            new RecordBank(0x1e, "Mr. Game & Watch"),
            new RecordBank(0x1f, "Jigglypuff"),
            new RecordBank(0x20, "Toon Link"),
            new RecordBank(0x21, "Wolf"),
            new RecordBank(0x22, "Sonic")
        };
    }

    public class ExFighterLong
    {
        public uint ID { get; private set; }
        public string Name { get; private set; }

        public ExFighterLong(uint id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static ExFighterLong[] ExFighters = new ExFighterLong[] {
            //            ID     Display Name     
			new ExFighterLong(0x00, "Mario"),
            new ExFighterLong(0x01, "Donkey Kong"),
            new ExFighterLong(0x02, "Link"),
            new ExFighterLong(0x03, "Samus"),
            new ExFighterLong(0x04, "Yoshi"),
            new ExFighterLong(0x05, "Kirby"),
            new ExFighterLong(0x06, "Fox"),
            new ExFighterLong(0x07, "Pikachu"),
            new ExFighterLong(0x08, "Luigi"),
            new ExFighterLong(0x09, "Captain Falcon"),
            new ExFighterLong(0x0A, "Ness"),
            new ExFighterLong(0x0B, "Bowser"),
            new ExFighterLong(0x0C, "Peach"),
            new ExFighterLong(0x0D, "Zelda"),
            new ExFighterLong(0x0E, "Sheik"),
            new ExFighterLong(0x0F, "Popo"),
            new ExFighterLong(0x10, "Nana"),
            new ExFighterLong(0x11, "Marth"),
            new ExFighterLong(0x12, "Mr. Game & Watch"),
            new ExFighterLong(0x13, "Falco"),
            new ExFighterLong(0x14, "Ganondorf"),
            new ExFighterLong(0x15, "Wario"),
            new ExFighterLong(0x16, "Meta Knight"),
            new ExFighterLong(0x17, "Pit"),
            new ExFighterLong(0x18, "Zero Suit Samus"),
            new ExFighterLong(0x19, "Pikmin & Olimar"),
            new ExFighterLong(0x1A, "Lucas"),
            new ExFighterLong(0x1B, "Diddy Kong"),
            new ExFighterLong(0x1C, "Pokémon Trainer"),
            new ExFighterLong(0x1D, "Charizard"),
            new ExFighterLong(0x1E, "Squirtle"),
            new ExFighterLong(0x1F, "Ivysaur"),
            new ExFighterLong(0x20, "King Dedede"),
            new ExFighterLong(0x21, "Lucario"),
            new ExFighterLong(0x22, "Ike"),
            new ExFighterLong(0x23, "R.O.B."),
            new ExFighterLong(0x25, "Jigglypuff"),
            new ExFighterLong(0x26, "Mewtwo (PM)"),
            new ExFighterLong(0x27, "Roy (PM)"),
            new ExFighterLong(0x29, "Toon Link"),
            new ExFighterLong(0x2C, "Wolf"),
            new ExFighterLong(0x2E, "Snake"),
            new ExFighterLong(0x2F, "Sonic"),
            new ExFighterLong(0x30, "Giga Bowser"),
            new ExFighterLong(0x31, "Warioman"),
            new ExFighterLong(0x32, "Red Alloy"),
            new ExFighterLong(0x33, "Blue Alloy"),
            new ExFighterLong(0x34, "Yellow Alloy"),
            new ExFighterLong(0x35, "Green Alloy"),
            new ExFighterLong(0x3F, "EXCharacter 3F"),
            new ExFighterLong(0x40, "EXCharacter 40"),
            new ExFighterLong(0x41, "EXCharacter 41"),
            new ExFighterLong(0x42, "EXCharacter 42"),
            new ExFighterLong(0x43, "EXCharacter 43"),
            new ExFighterLong(0x44, "EXCharacter 44"),
            new ExFighterLong(0x45, "EXCharacter 45"),
            new ExFighterLong(0x46, "EXCharacter 46"),
            new ExFighterLong(0x47, "EXCharacter 47"),
            new ExFighterLong(0x4B, "EXCharacter 4B"),
            new ExFighterLong(0x4C, "EXCharacter 4C"),
            new ExFighterLong(0x4D, "EXCharacter 4D"),
            new ExFighterLong(0x4E, "EXCharacter 4E"),
            new ExFighterLong(0x4F, "EXCharacter 4F"),
            new ExFighterLong(0x50, "EXCharacter 50"),
            new ExFighterLong(0x51, "EXCharacter 51"),
            new ExFighterLong(0x52, "EXCharacter 52"),
            new ExFighterLong(0x53, "EXCharacter 53"),
            new ExFighterLong(0x54, "EXCharacter 54"),
            new ExFighterLong(0x55, "EXCharacter 55"),
            new ExFighterLong(0x56, "EXCharacter 56"),
            new ExFighterLong(0x57, "EXCharacter 57"),
            new ExFighterLong(0x58, "EXCharacter 58"),
            new ExFighterLong(0x59, "EXCharacter 59"),
            new ExFighterLong(0x5A, "EXCharacter 5A"),
            new ExFighterLong(0x5B, "EXCharacter 5B"),
            new ExFighterLong(0x5C, "EXCharacter 5C"),
            new ExFighterLong(0x5D, "EXCharacter 5D"),
            new ExFighterLong(0x5E, "EXCharacter 5E"),
            new ExFighterLong(0x5F, "EXCharacter 5F"),
            new ExFighterLong(0x60, "EXCharacter 60"),
            new ExFighterLong(0x61, "EXCharacter 61"),
            new ExFighterLong(0x62, "EXCharacter 62"),
            new ExFighterLong(0x63, "EXCharacter 63"),
            new ExFighterLong(0x64, "EXCharacter 64"),
            new ExFighterLong(0x65, "EXCharacter 65"),
            new ExFighterLong(0x66, "EXCharacter 66"),
            new ExFighterLong(0x67, "EXCharacter 67"),
            new ExFighterLong(0x68, "EXCharacter 68"),
            new ExFighterLong(0x69, "EXCharacter 69"),
            new ExFighterLong(0x6A, "EXCharacter 6A"),
            new ExFighterLong(0x6B, "EXCharacter 6B"),
            new ExFighterLong(0x6C, "EXCharacter 6C"),
            new ExFighterLong(0x6D, "EXCharacter 6D"),
            new ExFighterLong(0x6E, "EXCharacter 6E"),
            new ExFighterLong(0x6F, "EXCharacter 6F"),
            new ExFighterLong(0x70, "EXCharacter 70"),
            new ExFighterLong(0x71, "EXCharacter 71"),
            new ExFighterLong(0x72, "EXCharacter 72"),
            new ExFighterLong(0x73, "EXCharacter 73"),
            new ExFighterLong(0x74, "EXCharacter 74"),
            new ExFighterLong(0x75, "EXCharacter 75"),
            new ExFighterLong(0x76, "EXCharacter 76"),
            new ExFighterLong(0x77, "EXCharacter 77"),
            new ExFighterLong(0x78, "EXCharacter 78"),
            new ExFighterLong(0x79, "EXCharacter 79"),
            new ExFighterLong(0x7A, "EXCharacter 7A"),
            new ExFighterLong(0x7B, "EXCharacter 7B"),
            new ExFighterLong(0x7C, "EXCharacter 7C"),
            new ExFighterLong(0x7D, "EXCharacter 7D"),
            new ExFighterLong(0x7E, "EXCharacter 7E"),
            new ExFighterLong(0x7F, "EXCharacter 7F"),

            new ExFighterLong(0xFFFFFFFF, "None")
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

    // Used by SLTC
    public class DropDownListBrawlExFighterIDsLong : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ExFighterLong.ExFighters.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                return Convert.ToUInt32(field0, fromBase);
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(uint))
            {
                var fighter = ExFighterLong.ExFighters.Where(s => s.ID == (uint)value).FirstOrDefault();
                return "0x" + ((uint)value).ToString("X8") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
