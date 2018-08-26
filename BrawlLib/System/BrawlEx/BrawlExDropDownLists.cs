using System.ComponentModel;
using System.Globalization;
using System.Linq;

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
            new RecordBank(0x14, "Olimar"),
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

    /*public class ExFighterLong
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
            new ExFighterLong(0x19, "Olimar"),
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
            new ExFighterLong(0x3F, "ExFighter3F"),
            new ExFighterLong(0x40, "ExFighter40"),
            new ExFighterLong(0x41, "ExFighter41"),
            new ExFighterLong(0x42, "ExFighter42"),
            new ExFighterLong(0x43, "ExFighter43"),
            new ExFighterLong(0x44, "ExFighter44"),
            new ExFighterLong(0x45, "ExFighter45"),
            new ExFighterLong(0x46, "ExFighter46"),
            new ExFighterLong(0x47, "ExFighter47"),
            new ExFighterLong(0x48, "ExFighter48"),
            new ExFighterLong(0x49, "ExFighter49"),
            new ExFighterLong(0x4A, "ExFighter4A"),
            new ExFighterLong(0x4B, "ExFighter4B"),
            new ExFighterLong(0x4C, "ExFighter4C"),
            new ExFighterLong(0x4D, "ExFighter4D"),
            new ExFighterLong(0x4E, "ExFighter4E"),
            new ExFighterLong(0x4F, "ExFighter4F"),
            new ExFighterLong(0x50, "ExFighter50"),
            new ExFighterLong(0x51, "ExFighter51"),
            new ExFighterLong(0x52, "ExFighter52"),
            new ExFighterLong(0x53, "ExFighter53"),
            new ExFighterLong(0x54, "ExFighter54"),
            new ExFighterLong(0x55, "ExFighter55"),
            new ExFighterLong(0x56, "ExFighter56"),
            new ExFighterLong(0x57, "ExFighter57"),
            new ExFighterLong(0x58, "ExFighter58"),
            new ExFighterLong(0x59, "ExFighter59"),
            new ExFighterLong(0x5A, "ExFighter5A"),
            new ExFighterLong(0x5B, "ExFighter5B"),
            new ExFighterLong(0x5C, "ExFighter5C"),
            new ExFighterLong(0x5D, "ExFighter5D"),
            new ExFighterLong(0x5E, "ExFighter5E"),
            new ExFighterLong(0x5F, "ExFighter5F"),
            new ExFighterLong(0x60, "ExFighter60"),
            new ExFighterLong(0x61, "ExFighter61"),
            new ExFighterLong(0x62, "ExFighter62"),
            new ExFighterLong(0x63, "ExFighter63"),
            new ExFighterLong(0x64, "ExFighter64"),
            new ExFighterLong(0x65, "ExFighter65"),
            new ExFighterLong(0x66, "ExFighter66"),
            new ExFighterLong(0x67, "ExFighter67"),
            new ExFighterLong(0x68, "ExFighter68"),
            new ExFighterLong(0x69, "ExFighter69"),
            new ExFighterLong(0x6A, "ExFighter6A"),
            new ExFighterLong(0x6B, "ExFighter6B"),
            new ExFighterLong(0x6C, "ExFighter6C"),
            new ExFighterLong(0x6D, "ExFighter6D"),
            new ExFighterLong(0x6E, "ExFighter6E"),
            new ExFighterLong(0x6F, "ExFighter6F"),
            new ExFighterLong(0x70, "ExFighter70"),
            new ExFighterLong(0x71, "ExFighter71"),
            new ExFighterLong(0x72, "ExFighter72"),
            new ExFighterLong(0x73, "ExFighter73"),
            new ExFighterLong(0x74, "ExFighter74"),
            new ExFighterLong(0x75, "ExFighter75"),
            new ExFighterLong(0x76, "ExFighter76"),
            new ExFighterLong(0x77, "ExFighter77"),
            new ExFighterLong(0x78, "ExFighter78"),
            new ExFighterLong(0x79, "ExFighter79"),
            new ExFighterLong(0x7A, "ExFighter7A"),
            new ExFighterLong(0x7B, "ExFighter7B"),
            new ExFighterLong(0x7C, "ExFighter7C"),
            new ExFighterLong(0x7D, "ExFighter7D"),
            new ExFighterLong(0x7E, "ExFighter7E"),
            new ExFighterLong(0x7F, "ExFighter7F"),

            new ExFighterLong(0xFFFFFFFF, "None")
        };
    }

    public class ExFighter
    {
        /// <summary>
        /// The character slot index, as used by common2.pac event match and all-star data.
        /// See: http://opensa.dantarion.com/wiki/Character_Slots
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// The fighter name (e.g. "Yoshi").
        /// </summary>
        public string Name { get; private set; }

        public ExFighter(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public static string getNameFromID(int searchID)
        {
            for (int i = 0; i < Fighters.Length; i++)
                if (Fighters[i].ID == searchID)
                    return Fighters[i].Name;
            return null;
        }

        public override string ToString() { return Name; }

        public readonly static ExFighter[] Fighters = new ExFighter[] {
            //            ID     Display Name
			new ExFighter(0x00, "Mario"),
            new ExFighter(0x01, "Donkey Kong"),
            new ExFighter(0x02, "Link"),
            new ExFighter(0x03, "Samus"),
            new ExFighter(0x04, "Zero Suit Samus"),
            new ExFighter(0x05, "Yoshi"),
            new ExFighter(0x06, "Kirby"),
            new ExFighter(0x07, "Fox"),
            new ExFighter(0x08, "Pikachu"),
            new ExFighter(0x09, "Luigi"),
            new ExFighter(0x0a, "Captain Falcon"),
            new ExFighter(0x0b, "Ness"),
            new ExFighter(0x0c, "Bowser"),
            new ExFighter(0x0d, "Peach"),
            new ExFighter(0x0e, "Zelda"),
            new ExFighter(0x0f, "Sheik"),
            new ExFighter(0x10, "Ice Climbers"),
            new ExFighter(0x11, "Popo"),
            new ExFighter(0x12, "Nana"),
            new ExFighter(0x13, "Marth"),
            new ExFighter(0x14, "Mr. Game & Watch"),
            new ExFighter(0x15, "Falco"),
            new ExFighter(0x16, "Ganondorf"),
            new ExFighter(0x17, "Wario"),
            new ExFighter(0x18, "Meta Knight"),
            new ExFighter(0x19, "Pit"),
            new ExFighter(0x1a, "Olimar"),
            new ExFighter(0x1b, "Lucas"),
            new ExFighter(0x1c, "Diddy Kong"),
            new ExFighter(0x1d, "Charizard"),
            new ExFighter(0x1e, "Charizard (independent)"),
            new ExFighter(0x1f, "Squirtle"),
            new ExFighter(0x20, "Squirtle (independent)"),
            new ExFighter(0x21, "Ivysaur"),
            new ExFighter(0x22, "Ivysaur (independent)"),
            new ExFighter(0x23, "King Dedede"),
            new ExFighter(0x24, "Lucario"),
            new ExFighter(0x25, "Ike"),
            new ExFighter(0x26, "R.O.B."),
            new ExFighter(0x27, "Jigglypuff"),
            new ExFighter(0x28, "Toon Link"),
            new ExFighter(0x29, "Wolf"),
            new ExFighter(0x2a, "Snake"),
            new ExFighter(0x2b, "Sonic"),
            new ExFighter(0x2c, "Giga Bowser"),
            new ExFighter(0x2d, "Warioman"),
            new ExFighter(0x2e, "Red Alloy (don't use in event matches)"),
            new ExFighter(0x2f, "Blue Alloy (don't use in event matches)"),
            new ExFighter(0x30, "Yellow Alloy (don't use in event matches)"),
            new ExFighter(0x31, "Green Alloy (don't use in event matches)"),
            new ExFighter(0x32, "Roy (PM)"),
            new ExFighter(0x33, "Mewtwo (PM)"),
            new ExFighter(0x3F, "ExFighter3F"),
            new ExFighter(0x40, "ExFighter40"),
            new ExFighter(0x41, "ExFighter41"),
            new ExFighter(0x42, "ExFighter42"),
            new ExFighter(0x43, "ExFighter43"),
            new ExFighter(0x44, "ExFighter44"),
            new ExFighter(0x45, "ExFighter45"),
            new ExFighter(0x46, "ExFighter46"),
            new ExFighter(0x47, "ExFighter47"),
            new ExFighter(0x48, "ExFighter48"),
            new ExFighter(0x49, "ExFighter49"),
            new ExFighter(0x4A, "ExFighter4A"),
            new ExFighter(0x4B, "ExFighter4B"),
            new ExFighter(0x4C, "ExFighter4C"),
            new ExFighter(0x4D, "ExFighter4D"),
            new ExFighter(0x4E, "ExFighter4E"),
            new ExFighter(0x4F, "ExFighter4F"),
            new ExFighter(0x50, "ExFighter50"),
            new ExFighter(0x51, "ExFighter51"),
            new ExFighter(0x52, "ExFighter52"),
            new ExFighter(0x53, "ExFighter53"),
            new ExFighter(0x54, "ExFighter54"),
            new ExFighter(0x55, "ExFighter55"),
            new ExFighter(0x56, "ExFighter56"),
            new ExFighter(0x57, "ExFighter57"),
            new ExFighter(0x58, "ExFighter58"),
            new ExFighter(0x59, "ExFighter59"),
            new ExFighter(0x5A, "ExFighter5A"),
            new ExFighter(0x5B, "ExFighter5B"),
            new ExFighter(0x5C, "ExFighter5C"),
            new ExFighter(0x5D, "ExFighter5D"),
            new ExFighter(0x5E, "ExFighter5E"),
            new ExFighter(0x5F, "ExFighter5F"),
            new ExFighter(0x60, "ExFighter60"),
            new ExFighter(0x61, "ExFighter61"),
            new ExFighter(0x62, "ExFighter62"),
            new ExFighter(0x63, "ExFighter63"),
            new ExFighter(0x64, "ExFighter64"),
            new ExFighter(0x65, "ExFighter65"),
            new ExFighter(0x66, "ExFighter66"),
            new ExFighter(0x67, "ExFighter67"),
            new ExFighter(0x68, "ExFighter68"),
            new ExFighter(0x69, "ExFighter69"),
            new ExFighter(0x6A, "ExFighter6A"),
            new ExFighter(0x6B, "ExFighter6B"),
            new ExFighter(0x6C, "ExFighter6C"),
            new ExFighter(0x6D, "ExFighter6D"),
            new ExFighter(0x6E, "ExFighter6E"),
            new ExFighter(0x6F, "ExFighter6F"),
            new ExFighter(0x70, "ExFighter70"),
            new ExFighter(0x71, "ExFighter71"),
            new ExFighter(0x72, "ExFighter72"),
            new ExFighter(0x73, "ExFighter73"),
            new ExFighter(0x74, "ExFighter74"),
            new ExFighter(0x75, "ExFighter75"),
            new ExFighter(0x76, "ExFighter76"),
            new ExFighter(0x77, "ExFighter77"),
            new ExFighter(0x78, "ExFighter78"),
            new ExFighter(0x79, "ExFighter79"),
            new ExFighter(0x7A, "ExFighter7A"),
            new ExFighter(0x7B, "ExFighter7B"),
            new ExFighter(0x7C, "ExFighter7C"),
            new ExFighter(0x7D, "ExFighter7D"),
            new ExFighter(0x7E, "ExFighter7E"),
            new ExFighter(0x7F, "ExFighter7F"),
            new ExFighter(0xFF, "None")
        };
    }

    public class CSSSlotIDs
    {
        /// <summary>
        /// The character slot index, as used by common2.pac event match and all-star data.
        /// See: http://opensa.dantarion.com/wiki/Character_Slots
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// The fighter name (e.g. "Yoshi").
        /// </summary>
        public string Name { get; private set; }

        public CSSSlotIDs(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public static string getNameFromID(int searchID)
        {
            for (int i = 0; i < CSSSlots.Length; i++)
                if (CSSSlots[i].ID == searchID)
                    return CSSSlots[i].Name;
            return null;
        }

        public override string ToString() { return Name; }

        public readonly static CSSSlotIDs[] CSSSlots = new CSSSlotIDs[] {
            //             ID     Display Name
            new CSSSlotIDs(0x00, "Mario"),
            new CSSSlotIDs(0x01, "Donkey Kong"),
            new CSSSlotIDs(0x02, "Link"),
            new CSSSlotIDs(0x03, "Samus"),
            new CSSSlotIDs(0x04, "Zero Suit Samus"),
            new CSSSlotIDs(0x05, "Yoshi"),
            new CSSSlotIDs(0x06, "Kirby"),
            new CSSSlotIDs(0x07, "Fox"),
            new CSSSlotIDs(0x08, "Pikachu"),
            new CSSSlotIDs(0x09, "Luigi"),
            new CSSSlotIDs(0x0A, "Captain Falcon"),
            new CSSSlotIDs(0x0B, "Ness"),
            new CSSSlotIDs(0x0C, "Bowser"),
            new CSSSlotIDs(0x0D, "Peach"),
            new CSSSlotIDs(0x0E, "Zelda"),
            new CSSSlotIDs(0x0F, "Sheik"),
            new CSSSlotIDs(0x10, "Ice Climbers"),
            new CSSSlotIDs(0x11, "Marth"),
            new CSSSlotIDs(0x12, "Mr. Game & Watch"),
            new CSSSlotIDs(0x13, "Falco"),
            new CSSSlotIDs(0x14, "Ganondorf"),
            new CSSSlotIDs(0x15, "Wario"),
            new CSSSlotIDs(0x16, "Meta Knight"),
            new CSSSlotIDs(0x17, "Pit"),
            new CSSSlotIDs(0x18, "Olimar"),
            new CSSSlotIDs(0x19, "Lucas"),
            new CSSSlotIDs(0x1A, "Diddy Kong"),
            new CSSSlotIDs(0x1B, "Pokémon Trainer"),
            new CSSSlotIDs(0x1C, "Charizard"),
            new CSSSlotIDs(0x1D, "Squirtle"),
            new CSSSlotIDs(0x1E, "Ivysaur"),
            new CSSSlotIDs(0x1F, "King Dedede"),
            new CSSSlotIDs(0x20, "Lucario"),
            new CSSSlotIDs(0x21, "Ike"),
            new CSSSlotIDs(0x22, "R.O.B."),
            new CSSSlotIDs(0x23, "Jigglypuff"),
            new CSSSlotIDs(0x24, "Toon Link"),
            new CSSSlotIDs(0x25, "Wolf"),
            new CSSSlotIDs(0x26, "Snake"),
            new CSSSlotIDs(0x27, "Sonic"),
            new CSSSlotIDs(0x28, "None"),
            new CSSSlotIDs(0x29, "Random"),
            new CSSSlotIDs(0x2D, "Roy (PM)"),
            new CSSSlotIDs(0x2E, "Mewtwo (PM)"),
            new CSSSlotIDs(0x3F, "ExFighter3F"),
            new CSSSlotIDs(0x40, "ExFighter40"),
            new CSSSlotIDs(0x41, "ExFighter41"),
            new CSSSlotIDs(0x42, "ExFighter42"),
            new CSSSlotIDs(0x43, "ExFighter43"),
            new CSSSlotIDs(0x44, "ExFighter44"),
            new CSSSlotIDs(0x45, "ExFighter45"),
            new CSSSlotIDs(0x46, "ExFighter46"),
            new CSSSlotIDs(0x47, "ExFighter47"),
            new CSSSlotIDs(0x48, "ExFighter48"),
            new CSSSlotIDs(0x49, "ExFighter49"),
            new CSSSlotIDs(0x4A, "ExFighter4A"),
            new CSSSlotIDs(0x4B, "ExFighter4B"),
            new CSSSlotIDs(0x4C, "ExFighter4C"),
            new CSSSlotIDs(0x4D, "ExFighter4D"),
            new CSSSlotIDs(0x4E, "ExFighter4E"),
            new CSSSlotIDs(0x4F, "ExFighter4F"),
            new CSSSlotIDs(0x50, "ExFighter50"),
            new CSSSlotIDs(0x51, "ExFighter51"),
            new CSSSlotIDs(0x52, "ExFighter52"),
            new CSSSlotIDs(0x53, "ExFighter53"),
            new CSSSlotIDs(0x54, "ExFighter54"),
            new CSSSlotIDs(0x55, "ExFighter55"),
            new CSSSlotIDs(0x56, "ExFighter56"),
            new CSSSlotIDs(0x57, "ExFighter57"),
            new CSSSlotIDs(0x58, "ExFighter58"),
            new CSSSlotIDs(0x59, "ExFighter59"),
            new CSSSlotIDs(0x5A, "ExFighter5A"),
            new CSSSlotIDs(0x5B, "ExFighter5B"),
            new CSSSlotIDs(0x5C, "ExFighter5C"),
            new CSSSlotIDs(0x5D, "ExFighter5D"),
            new CSSSlotIDs(0x5E, "ExFighter5E"),
            new CSSSlotIDs(0x5F, "ExFighter5F"),
            new CSSSlotIDs(0x60, "ExFighter60"),
            new CSSSlotIDs(0x61, "ExFighter61"),
            new CSSSlotIDs(0x62, "ExFighter62"),
            new CSSSlotIDs(0x63, "ExFighter63"),
            new CSSSlotIDs(0x64, "ExFighter64"),
            new CSSSlotIDs(0x65, "ExFighter65"),
            new CSSSlotIDs(0x66, "ExFighter66"),
            new CSSSlotIDs(0x67, "ExFighter67"),
            new CSSSlotIDs(0x68, "ExFighter68"),
            new CSSSlotIDs(0x69, "ExFighter69"),
            new CSSSlotIDs(0x6A, "ExFighter6A"),
            new CSSSlotIDs(0x6B, "ExFighter6B"),
            new CSSSlotIDs(0x6C, "ExFighter6C"),
            new CSSSlotIDs(0x6D, "ExFighter6D"),
            new CSSSlotIDs(0x6E, "ExFighter6E"),
            new CSSSlotIDs(0x6F, "ExFighter6F"),
            new CSSSlotIDs(0x70, "ExFighter70"),
            new CSSSlotIDs(0x71, "ExFighter71"),
            new CSSSlotIDs(0x72, "ExFighter72"),
            new CSSSlotIDs(0x73, "ExFighter73"),
            new CSSSlotIDs(0x74, "ExFighter74"),
            new CSSSlotIDs(0x75, "ExFighter75"),
            new CSSSlotIDs(0x76, "ExFighter76"),
            new CSSSlotIDs(0x77, "ExFighter77"),
            new CSSSlotIDs(0x78, "ExFighter78"),
            new CSSSlotIDs(0x79, "ExFighter79"),
            new CSSSlotIDs(0x7A, "ExFighter7A"),
            new CSSSlotIDs(0x7B, "ExFighter7B"),
            new CSSSlotIDs(0x7C, "ExFighter7C"),
            new CSSSlotIDs(0x7D, "ExFighter7D"),
            new CSSSlotIDs(0x7E, "ExFighter7E"),
            new CSSSlotIDs(0x7F, "ExFighter7F")
        };
    }*/
    
    public class AIController
    {
        public uint ID { get; private set; }
        public string Name { get; private set; }

        public AIController(uint id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public override string ToString() { return Name; }

        public readonly static AIController[] aIControllers = new AIController[] {
            //            ID     Display Name     
			new AIController(0x00, "Mario"),
            new AIController(0x01, "Donkey Kong"),
            new AIController(0x02, "Link"),
            new AIController(0x03, "Samus"),
            new AIController(0x04, "Yoshi"),
            new AIController(0x05, "Kirby"),
            new AIController(0x06, "Fox"),
            new AIController(0x07, "Pikachu"),
            new AIController(0x08, "Luigi"),
            new AIController(0x09, "Captain Falcon"),
            new AIController(0x0A, "Ness"),
            new AIController(0x0B, "Bowser"),
            new AIController(0x0C, "Peach"),
            new AIController(0x0D, "Zelda"),
            new AIController(0x0E, "Sheik"),
            new AIController(0x0F, "Popo"),
            new AIController(0x10, "Nana"),
            new AIController(0x11, "Marth"),
            new AIController(0x12, "Mr. Game & Watch"),
            new AIController(0x13, "Falco"),
            new AIController(0x14, "Ganondorf"),
            new AIController(0x15, "Wario"),
            new AIController(0x16, "Meta Knight"),
            new AIController(0x17, "Pit"),
            new AIController(0x18, "Zero Suit Samus"),
            new AIController(0x19, "Olimar"),
            new AIController(0x1A, "Lucas"),
            new AIController(0x1B, "Diddy Kong"),
            new AIController(0x1C, "Pokémon Trainer"),
            new AIController(0x1D, "Charizard"),
            new AIController(0x1E, "Squirtle"),
            new AIController(0x1F, "Ivysaur"),
            new AIController(0x20, "King Dedede"),
            new AIController(0x21, "Lucario"),
            new AIController(0x22, "Ike"),
            new AIController(0x23, "R.O.B."),
            new AIController(0x25, "Jigglypuff"),
            new AIController(0x29, "Toon Link"),
            new AIController(0x2C, "Wolf"),
            new AIController(0x2E, "Snake"),
            new AIController(0x2F, "Sonic"),
            new AIController(0x30, "Giga Bowser"),
            new AIController(0x31, "Warioman"),
            new AIController(0x32, "Alloys"),
            new AIController(0xFFFFFFFF, "None")
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
            else if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null && value.GetType() == typeof(string))
                return 0;
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
            else if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null && value.GetType() == typeof(string))
                return 0;
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
            else if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null && value.GetType() == typeof(string))
                return 0;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // Different from fighterIDs as it includes EX48-4A
    public class DropDownListBrawlExSlotIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlLib.BrawlCrate.FighterNameGenerators.slotIDList.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var fighter = BrawlLib.BrawlCrate.FighterNameGenerators.slotIDList.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            else if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null && value.GetType() == typeof(string))
                return 0;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    // Used by SLTC
    public class DropDownListBrawlExFighterIDsLong : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlLib.BrawlCrate.FighterNameGenerators.fighterIDLongList.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var fighter = BrawlLib.BrawlCrate.FighterNameGenerators.fighterIDLongList.Where(s => s.ID == (uint)value).FirstOrDefault();
                return "0x" + ((uint)value).ToString("X8") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            else if ((destinationType == typeof(int) || destinationType == typeof(uint)) && value != null && value.GetType() == typeof(string))
                return 0;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    
    public class DropDownListBrawlExAIControllers : UInt32Converter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(AIController.aIControllers.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var fighter = AIController.aIControllers.Where(s => s.ID == (uint)value).FirstOrDefault();
                return "0x" + ((uint)value).ToString("X8") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            else if ((destinationType == typeof(int) || destinationType == typeof(uint)) && value != null && value.GetType() == typeof(string))
                return 0;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExCSSIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlLib.BrawlCrate.FighterNameGenerators.cssSlotIDList.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var fighter = BrawlLib.BrawlCrate.FighterNameGenerators.cssSlotIDList.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            else if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null && value.GetType() == typeof(string))
                return 0;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class DropDownListBrawlExCosmeticIDs : ByteConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDList.Select(s => "0x" + s.ID.ToString("X2") + " - " + s.Name).ToList());
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
                var fighter = BrawlLib.BrawlCrate.FighterNameGenerators.cosmeticIDList.Where(s => s.ID == (byte)value).FirstOrDefault();
                return "0x" + ((byte)value).ToString("X2") + (fighter == null ? "" : (" - " + fighter.Name));
            }
            else if ((destinationType == typeof(int) || destinationType == typeof(byte)) && value != null && value.GetType() == typeof(string))
                return 0;
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
