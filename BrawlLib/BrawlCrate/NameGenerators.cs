using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using BrawlLib.SSBB;

namespace BrawlLib.BrawlCrate
{
    public class FighterNameGenerators
    {
        public static List<string> fileList = new List<string>();

        public static List<Fighter> singlePlayerSlotIDList = new List<Fighter>();
        public static List<Fighter> slotIDList = new List<Fighter>();
        public static List<Fighter> fighterIDList = new List<Fighter>();
        public static List<Fighter> fighterIDLongList = new List<Fighter>();
        public static List<Fighter> cssSlotIDList = new List<Fighter>();
        public static List<Fighter> cosmeticIDList = new List<Fighter>();
        public static string directory = AppDomain.CurrentDomain.BaseDirectory + '\\' + "CustomLists";
        public static string listName = directory + '\\' + "FighterList.txt";

        // Used to determine offsets
        public static readonly int flagIndex = 0;
        public static readonly int slotIDIndex = 8;
        public static readonly int fighterIDIndex = 16;
        public static readonly int cssSlotIDIndex = 28;
        public static readonly int cosmeticIDIndex = 40;
        public static readonly int internalNameIndex = 52;
        public static readonly int nameIndex = 69;
        public static readonly int minimumLength = nameIndex + 1;
        static readonly char[] trimChars =  { ' ', '\t' };
        
        public static string FromID(int id, int idOffset, string flagToIgnore)
        {
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idOffset).StartsWith("0X" + id.ToString("X2").ToUpper()))
                    if (!s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith(flagToIgnore))
                        return s.Substring(nameIndex).Trim(trimChars);
            return "Fighter 0x" + id.ToString("X2");
        }

        public static string InternalNameFromID(int id, int idOffset, string flagToIgnore)
        {
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idOffset).StartsWith("0X" + id.ToString("X2").ToUpper()))
                    if (!s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith(flagToIgnore))
                    {
                        string intName = s.Substring(internalNameIndex, (nameIndex- internalNameIndex) - 1).Trim(trimChars);
                        if(intName.Length > 1)
                            return char.ToUpper(intName.First()) + intName.Substring(1);
                        if (intName.Length > 0)
                            return intName.ToUpper();
                    }
            return "Fighter 0x" + id.ToString("X2");
        }

        public static string FromPacName(string pacName)
        {
            foreach (string s in fileList)
                if (s.ToUpper().Substring(internalNameIndex).StartsWith(pacName.ToUpper()))
                    return s.Substring(nameIndex).Trim(trimChars);
            return pacName.ToUpper();
        }

        public static void GenerateDefaultList()
        {
            string listDefault = "Flag   |SlotID |FighterID  |CSSSlotID  |CosmeticID |Internal Name   |Fighter Name\n-------+-------+-----------+-----------+-----------+----------------+------------------------------------------\n       |0x00   |0x00       |0x00       |0x00       |mario           |Mario\n       |0x01   |0x01       |0x01       |0x01       |donkey          |Donkey Kong\n       |0x02   |0x02       |0x02       |0x02       |link            |Link\n+s     |0x49   |           |           |           |samus/szerosuit |Samus/Zero Suit Samus // Single player only\n       |0x03   |0x03       |0x03       |0x03       |samus           |Samus\n       |0x04   |0x18       |0x04       |0x17       |szerosuit       |Zero Suit Samus\n       |0x05   |0x04       |0x05       |0x04       |yoshi           |Yoshi\n       |0x06   |0x05       |0x06       |0x05       |kirby           |Kirby\n       |0x07   |0x06       |0x07       |0x06       |fox             |Fox\n       |0x08   |0x07       |0x08       |0x07       |pikachu         |Pikachu\n       |0x09   |0x08       |0x09       |0x08       |luigi           |Luigi\n       |0x0A   |0x09       |0x0A       |0x09       |captain         |Captain Falcon\n       |0x0B   |0x0A       |0x0B       |0x0A       |ness            |Ness\n       |0x0C   |0x0B       |0x0C       |0x0B       |bowser          |Bowser\n       |0x0D   |0x0C       |0x0D       |0x0C       |peach           |Peach\n       |       |           |           |0x26       |zelda/sheik     |Zelda/Sheik\n+s     |0x4A   |           |           |           |zelda/sheik     |Zelda/Sheik // Single player only\n       |0x0E   |0x0D       |0x0E       |0x0D       |zelda           |Zelda\n       |0x0F   |0x0E       |0x0F       |0x0E       |sheik           |Sheik\n       |0x10   |           |0x10       |0x0F       |iceclimber      |Ice Climbers\n       |0x11   |0x0F       |           |           |popo            |Popo\n       |0x12   |0x10       |           |           |nana            |Nana\n       |0x13   |0x11       |0x11       |0x10       |marth           |Marth\n       |0x14   |0x12       |0x12       |0x11       |gamewatch       |Mr. Game & Watch\n       |0x15   |0x13       |0x13       |0x12       |falco           |Falco\n       |0x16   |0x14       |0x14       |0x13       |ganon           |Ganondorf\n       |0x17   |0x15       |0x15       |0x14       |wario           |Wario\n       |0x18   |0x16       |0x16       |0x15       |metaknight      |Meta Knight\n       |0x19   |0x17       |0x17       |0x16       |pit             |Pit\n       |0x1A   |0x19       |0x18       |0x18       |pikmin          |Olimar & Pikmin\n       |0x1B   |0x1A       |0x19       |0x19       |lucas           |Lucas\n       |0x1C   |0x1B       |0x1A       |0x1A       |diddy           |Diddy Kong\n       |       |0x1C       |0x1B       |0x1B       |poketrainer     |Pokémon Trainer\n+s     |0x48   |           |           |           |poketrainer     |Pokémon Trainer // Single player only\n       |0x1D   |0x1D       |0x1C       |0x1C       |pokelizardon    |Charizard // Pokémon Trainer\n       |0x1E   |           |           |           |lizardon        |Charizard (Independent)\n       |0x1F   |0x1E       |0x1D       |0x1D       |pokezenigame    |Squirtle // Pokémon Trainer\n       |0x20   |           |           |           |zenigame        |Squirtle (Independent)\n       |0x21   |0x1F       |0x1E       |0x1E       |pokefushigisou  |Ivysaur // Pokémon Trainer\n       |0x22   |           |           |           |fushigisou      |Ivysaur (Independent)\n       |0x23   |0x20       |0x1F       |0x1F       |dedede          |King Dedede\n       |0x24   |0x21       |0x20       |0x20       |lucario         |Lucario\n       |0x25   |0x22       |0x21       |0x21       |ike             |Ike\n       |0x26   |0x23       |0x22       |0x22       |robot           |R.O.B.\n       |0x27   |0x25       |0x23       |0x23       |purin           |Jigglypuff\n       |0x28   |0x29       |0x24       |0x25       |toonlink        |Toon Link\n       |0x29   |0x2C       |0x25       |0x27       |wolf            |Wolf\n       |0x2A   |0x2E       |0x26       |0x28       |snake           |Snake\n       |0x2B   |0x2F       |0x27       |0x29       |sonic           |Sonic\n       |0x2C   |0x30       |           |           |gkoopa          |Giga Bowser\n       |0x2D   |0x31       |           |           |warioman        |Wario-Man\n       |0x2E   |0x32       |0x38       |0x38       |zakoboy         |Red Alloy (Don't use in event matches)\n       |0x2F   |0x33       |0x39       |0x39       |zakogirl        |Blue Alloy (Don't use in event matches)\n       |0x30   |0x34       |0x3A       |0x3A       |zakochild       |Yellow Alloy (Don't use in event matches)\n       |0x31   |0x35       |0x3B       |0x3B       |zakoball        |Green Alloy (Don't use in event matches)\n       |0x32   |0x27       |0x2D       |0x2D       |roy             |Roy (Project M Only) // IDs are based on PMEX\n       |0x33   |0x26       |0x2E       |0x2E       |mewtwo          |Mewtwo (Project M Only) // IDs are based on PMEX\nX      |       |0x24       |           |           |pramai          |Plusle/Minun (Unused)\nX      |       |0x26       |           |           |mewtwo          |Mewtwo (Unused)\nX      |       |0x27       |           |           |roy             |Roy (Unused)\nX      |       |0x28       |           |           |dr_mario        |Dr. Mario (Unused)\nX      |       |0x2A       |           |           |toonzelda       |Toon Zelda (Unused)\nX      |       |0x2B       |           |           |toonsheik       |Toon Sheik (Unused)\nX      |       |0x2D       |           |           |dixie           |Dixie Kong (Unused)\nX      |0x32   |0x36       |           |           |mariod          |Mario Debug (unused)\nX      |0x33   |           |0x2D       |0x2D       |bosspackun      |Petey Piranha (unused)\nX      |0x34   |           |0x2E       |0x2E       |rayquaza        |Rayquaza (unused)\nX      |0x35   |           |0x2F       |0x2F       |porkystatue     |Porky Statue (unused)\nX      |0x36   |           |0x30       |0x30       |porky           |Porky Robot (unused)\nX      |0x37   |           |0x31       |0x31       |headrobo        |Galleom (unused)\nX      |0x38   |           |0x32       |0x32       |ridley          |Ridley (unused)\nX      |0x39   |           |0x33       |0x33       |duon            |Duon (unused)\nX      |0x3A   |           |0x34       |0x34       |metaridley      |Meta-Ridley (unused)\nX      |0x3B   |           |0x35       |0x35       |taboo           |Tabuu (unused)\nX      |0x3C   |           |0x36       |0x36       |masterhand      |Master Hand (unused)\nX      |0x3D   |           |0x37       |0x37       |crazyhand       |Crazy Hand (unused)\nX      |       |           |           |0x2B       |sandbag         |Sandbag (unused)\nX      |       |           |           |0x2C       |targets         |Targets (unused)\nX      |       |           |           |0x3C       |smash           |Smash Logo (unused)\nX      |       |           |           |0x24       |[-]             |[-] (unused)\n-s     |0x3E   |0xFF       |0x28       |0x3D       |none            |None // 0xFFFFFFFF is used for None if a long word is allocated for the fighter ID instead of a byte\n+s     |0x3E   |0xFF       |0x28       |0x3D       |none            |None / Select Character // \"Select Character\" is used for event matches\n       |       |           |0x29       |0x2A       |random          |Random\n       |       |0x37       |           |           |exfighter37     |ExFighter0x37 // BrawlEx Fighters. If you don't want to see these in dropdowns, delete the lines or flag them with \"X\"\n       |       |0x38       |           |           |exfighter38     |ExFighter0x38\n       |       |0x39       |           |           |exfighter39     |ExFighter0x39\n       |       |0x3A       |           |           |exfighter3A     |ExFighter0x3A\n       |       |0x3B       |           |           |exfighter3B     |ExFighter0x3B\n       |       |0x3C       |           |           |exfighter3C     |ExFighter0x3C\n       |       |0x3D       |           |           |exfighter3D     |ExFighter0x3D\n       |       |0x3E       |           |           |exfighter3E     |ExFighter0x3E\n       |0x3F   |0x3F       |0x3F       |0x3F       |exfighter3F     |ExFighter0x3F\n       |0x40   |0x40       |0x40       |0x40       |exfighter40     |ExFighter0x40\n       |0x41   |0x41       |0x41       |0x41       |exfighter41     |ExFighter0x41\n       |0x42   |0x42       |0x42       |0x42       |exfighter42     |ExFighter0x42\n       |0x43   |0x43       |0x43       |0x43       |exfighter43     |ExFighter0x43\n       |0x44   |0x44       |0x44       |0x44       |exfighter44     |ExFighter0x44\n       |0x45   |0x45       |0x45       |0x45       |exfighter45     |ExFighter0x45\n       |0x46   |0x46       |0x46       |0x46       |exfighter46     |ExFighter0x46\n       |0x47   |0x47       |0x47       |0x47       |exfighter47     |ExFighter0x47\n-s     |0x48   |0x48       |0x48       |0x48       |exfighter48     |ExFighter0x48 // Slot ID is reserved for Pokémon Trainer in single player modes\n-s     |0x49   |0x49       |0x49       |0x49       |exfighter49     |ExFighter0x49 // Slot ID is reserved for Samus/Zero Suit Samus in single player modes\n-s     |0x4A   |0x4A       |0x4A       |0x4A       |exfighter4A     |ExFighter0x4A // Slot ID is reserved for Zelda/Sheik in single player modes\n       |0x4B   |0x4B       |0x4B       |0x4B       |exfighter4B     |ExFighter0x4B\n       |0x4C   |0x4C       |0x4C       |0x4C       |exfighter4C     |ExFighter0x4C\n       |0x4D   |0x4D       |0x4D       |0x4D       |exfighter4D     |ExFighter0x4D\n       |0x4E   |0x4E       |0x4E       |0x4E       |exfighter4E     |ExFighter0x4E\n       |0x4F   |0x4F       |0x4F       |0x4F       |exfighter4F     |ExFighter0x4F\n       |0x50   |0x50       |0x50       |0x50       |exfighter50     |ExFighter0x50\n       |0x51   |0x51       |0x51       |0x51       |exfighter51     |ExFighter0x51\n       |0x52   |0x52       |0x52       |0x52       |exfighter52     |ExFighter0x52\n       |0x53   |0x53       |0x53       |0x53       |exfighter53     |ExFighter0x53\n       |0x54   |0x54       |0x54       |0x54       |exfighter54     |ExFighter0x54\n       |0x55   |0x55       |0x55       |0x55       |exfighter55     |ExFighter0x55\n       |0x56   |0x56       |0x56       |0x56       |exfighter56     |ExFighter0x56\n       |0x57   |0x57       |0x57       |0x57       |exfighter57     |ExFighter0x57\n       |0x58   |0x58       |0x58       |0x58       |exfighter58     |ExFighter0x58\n       |0x59   |0x59       |0x59       |0x59       |exfighter59     |ExFighter0x59\n       |0x5A   |0x5A       |0x5A       |0x5A       |exfighter5A     |ExFighter0x5A\n       |0x5B   |0x5B       |0x5B       |0x5B       |exfighter5B     |ExFighter0x5B\n       |0x5C   |0x5C       |0x5C       |0x5C       |exfighter5C     |ExFighter0x5C\n       |0x5D   |0x5D       |0x5D       |0x5D       |exfighter5D     |ExFighter0x5D\n       |0x5E   |0x5E       |0x5E       |0x5E       |exfighter5E     |ExFighter0x5E\n       |0x5F   |0x5F       |0x5F       |0x5F       |exfighter5F     |ExFighter0x5F\n       |0x60   |0x60       |0x60       |0x60       |exfighter60     |ExFighter0x60\n       |0x61   |0x61       |0x61       |0x61       |exfighter61     |ExFighter0x61\n       |0x62   |0x62       |0x62       |0x62       |exfighter62     |ExFighter0x62\n       |0x63   |0x63       |0x63       |0x63       |exfighter63     |ExFighter0x63\n       |0x64   |0x64       |0x64       |0x64       |exfighter64     |ExFighter0x64\n       |0x65   |0x65       |0x65       |0x65       |exfighter65     |ExFighter0x65\n       |0x66   |0x66       |0x66       |0x66       |exfighter66     |ExFighter0x66\n       |0x67   |0x67       |0x67       |0x67       |exfighter67     |ExFighter0x67\n       |0x68   |0x68       |0x68       |0x68       |exfighter68     |ExFighter0x68\n       |0x69   |0x69       |0x69       |0x69       |exfighter69     |ExFighter0x69\n       |0x6A   |0x6A       |0x6A       |0x6A       |exfighter6A     |ExFighter0x6A\n       |0x6B   |0x6B       |0x6B       |0x6B       |exfighter6B     |ExFighter0x6B\n       |0x6C   |0x6C       |0x6C       |0x6C       |exfighter6C     |ExFighter0x6C\n       |0x6D   |0x6D       |0x6D       |0x6D       |exfighter6D     |ExFighter0x6D\n       |0x6E   |0x6E       |0x6E       |0x6E       |exfighter6E     |ExFighter0x6E\n       |0x6F   |0x6F       |0x6F       |0x6F       |exfighter6F     |ExFighter0x6F\n       |0x70   |0x70       |0x70       |0x70       |exfighter70     |ExFighter0x70\n       |0x71   |0x71       |0x71       |0x71       |exfighter71     |ExFighter0x71\n       |0x72   |0x72       |0x72       |0x72       |exfighter72     |ExFighter0x72\n       |0x73   |0x73       |0x73       |0x73       |exfighter73     |ExFighter0x73\n       |0x74   |0x74       |0x74       |0x74       |exfighter74     |ExFighter0x74\n       |0x75   |0x75       |0x75       |0x75       |exfighter75     |ExFighter0x75\n       |0x76   |0x76       |0x76       |0x76       |exfighter76     |ExFighter0x76\n       |0x77   |0x77       |0x77       |0x77       |exfighter77     |ExFighter0x77\n       |0x78   |0x78       |0x78       |0x78       |exfighter78     |ExFighter0x78\n       |0x79   |0x79       |0x79       |0x79       |exfighter79     |ExFighter0x79\n       |0x7A   |0x7A       |0x7A       |0x7A       |exfighter7A     |ExFighter0x7A\n       |0x7B   |0x7B       |0x7B       |0x7B       |exfighter7B     |ExFighter0x7B\n       |0x7C   |0x7C       |0x7C       |0x7C       |exfighter7C     |ExFighter0x7C\n       |0x7D   |0x7D       |0x7D       |0x7D       |exfighter7D     |ExFighter0x7D\n       |0x7E   |0x7E       |0x7E       |0x7E       |exfighter7E     |ExFighter0x7E\n       |0x7F   |0x7F       |0x7F       |0x7F       |exfighter7F     |ExFighter0x7F";
            File.WriteAllText(listName, listDefault);
        }

        public static void GenerateLists()
        {
            Directory.CreateDirectory(directory);
            if (!File.Exists(listName))
                GenerateDefaultList();
            if (!File.Exists(listName))
                return;
            fileList = new List<string>(File.ReadAllLines(listName));
            // Remove descriptor strings and unecessary spaces
            for(int i = 0; i < fileList.Count; i++)
            {
                int commentIndex = fileList[i].IndexOf("//");
                if (commentIndex > 0)
                    fileList[i] = fileList[i].Substring(0, commentIndex);
                fileList[i] = fileList[i].TrimEnd(trimChars);
            }
            GenerateSlotLists();
            GenerateFighterLists();
            GenerateCSSSlotList();
            GenerateCosmeticSlotList();
            // Order the list by IDs
            singlePlayerSlotIDList = singlePlayerSlotIDList.OrderBy(o => o.ID).ToList();
            slotIDList = slotIDList.OrderBy(o => o.ID).ToList();
            fighterIDList = fighterIDList.OrderBy(o => o.ID).ToList();
            fighterIDLongList = fighterIDLongList.OrderBy(o => o.ID).ToList();
            cssSlotIDList = cssSlotIDList.OrderBy(o => o.ID).ToList();
            cosmeticIDList = cosmeticIDList.OrderBy(o => o.ID).ToList();
        }

        public static void GenerateSlotLists()
        {
            // Generate the single player and multiplayer slot values
            slotIDList = new List<Fighter>();
            foreach (string s in fileList)
            {
                if (s.Length >= minimumLength && s.ToUpper().Substring(slotIDIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X"))
                {
                    if (!s.ToUpper().Substring(flagIndex).StartsWith("-S"))
                        singlePlayerSlotIDList.Add(new Fighter(Convert.ToByte(s.Substring(slotIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(slotIDIndex + 2, 2), 16), slotIDIndex, "-S")));
                    if (!s.ToUpper().Substring(flagIndex).StartsWith("+S"))
                        slotIDList.Add(new Fighter(Convert.ToByte(s.Substring(slotIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(slotIDIndex + 2, 2), 16), slotIDIndex, "+S")));
                }
            }
        }
        
        public static void GenerateFighterLists()
        {
            // Generate the single player and multiplayer slot values
            fighterIDList = new List<Fighter>();
            fighterIDLongList = new List<Fighter>();
            foreach (string s in fileList)
            {
                if (s.Length >= minimumLength && s.ToUpper().Substring(fighterIDIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("+S"))
                {
                    // Don't add "0xFF - None" to the long list (uses 0xFFFFFFFF instead)
                    if(!(Convert.ToByte(s.Substring(fighterIDIndex + 2, 2), 16) == 0xFF))
                        fighterIDLongList.Add(new Fighter(Convert.ToByte(s.Substring(fighterIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(fighterIDIndex + 2, 2), 16), fighterIDIndex, "+S")));
                    fighterIDList.Add(new Fighter(Convert.ToByte(s.Substring(fighterIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(fighterIDIndex + 2, 2), 16), fighterIDIndex, "+S")));
                }
            }
            fighterIDLongList.Add(new Fighter(0xFFFFFFFF, "None"));
        }

        public static void GenerateCSSSlotList()
        {
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(cssSlotIDIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("+S"))
                    cssSlotIDList.Add(new Fighter(Convert.ToByte(s.Substring(cssSlotIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(cssSlotIDIndex + 2, 2), 16), cssSlotIDIndex, "+S")));
        }

        public static void GenerateCosmeticSlotList()
        {
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(cosmeticIDIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("+S"))
                    cosmeticIDList.Add(new Fighter(Convert.ToByte(s.Substring(cosmeticIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(cosmeticIDIndex + 2, 2), 16), cosmeticIDIndex, "+S")));
        }
    }

    public static class StageNameGenerators
    {
        public static List<string> fileList = new List<string>();
        public static List<Stage> stageList = new List<Stage>();
        public static string directory = AppDomain.CurrentDomain.BaseDirectory + '\\' + "CustomLists";
        public static string listName = directory + '\\' + "StageList.txt";

        // Used to determine offsets
        static readonly int flagIndex = 0;
        static readonly int idIndex = 8;
        static readonly int internalNameIndex = 16;
        static readonly int nameIndex = 32;
        static readonly int minimumLength = nameIndex + 1;
        static readonly char[] trimChars = { ' ', '\t' };

        public static string FromID(int id)
        {
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idIndex).StartsWith("0X" + id.ToString("X2").ToUpper()) && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("-S"))
                    return s.Substring(nameIndex).Trim(trimChars);
            return "Stage 0x" + id.ToString("X2");
        }

        public static string FromPacName(string pacName)
        {
            string exID = "NULL";
            bool isExStage = false;
            if(pacName.ToUpper().StartsWith("CUSTOM"))
            {
                exID = pacName.ToUpper().Substring(6);
                isExStage = true;
            }
            foreach (string s in fileList)
                if (s.ToUpper().Substring(internalNameIndex).StartsWith(pacName.ToUpper()) || (isExStage && s.ToUpper().Substring(internalNameIndex).StartsWith("EX" + exID)))
                    return s.Substring(nameIndex).Trim(trimChars);
            return pacName.ToUpper();
        }

        public static void GenerateDefaultList()
        {
            string listDefault = "Flag   |ID     |Internal Name  |Stage Name\n-------+-------+---------------+---------------------------\n       |0x01   |BATTLEFIELD    |Battlefield\n       |0x02   |FINAL          |Final Destination\n       |0x03   |DOLPIC         |Delfino Plaza\n       |0x04   |MANSION        |Luigi's Mansion\n       |0x05   |MARIOPAST      |Mushroomy Kingdom\n       |0x06   |KART           |Mario Circuit\n       |0x07   |DONKEY         |75m\n       |0x08   |JUNGLE         |Rumble Falls\n       |0x09   |PIRATES        |Pirate Ship\n       |0x0B   |NORFAIR        |Norfair\n       |0x0C   |ORPHEON        |Frigate Orpheon\n       |0x0D   |CRAYON         |Yoshi's Island (Brawl)\n       |0x0E   |HALBERD        |Halberd\n       |0x13   |STARFOX        |Lylat Cruise\n       |0x14   |STADIUM        |Pokémon Stadium 2\n       |0x15   |TENGAN         |Spear Pillar\n       |0x16   |FZERO          |Port Town Aero Dive\n       |0x17   |ICE            |Summit\n       |0x18   |GW             |Flat Zone 2\n       |0x19   |EMBLEM         |Castle Siege\n       |0x1C   |MADEIN         |WarioWare\n       |0x1D   |EARTH          |Distant Planet\n       |0x1E   |PALUTENA       |Skyworld\n       |0x1F   |FAMICOM        |Mario Bros.\n       |0x20   |NEWPORK        |New Pork City\n       |0x21   |VILLAGE        |Smashville\n       |0x22   |METALGEAR      |Shadow Moses Island\n       |0x23   |GREENHILL      |Green Hill Zone\n       |0x24   |PICTCHAT       |PictoChat\n       |0x25   |PLANKTON       |Hanenbow\n       |0x26   |CONFIGTEST     |Control Configuration Stage\n       |0x29   |DXSHRINE       |Temple\n       |0x2A   |DXYORSTER      |Yoshi's Island (Melee)\n       |0x2B   |DXGARDEN       |Jungle Japes\n       |0x2C   |DXONETT        |Onett\n       |0x2D   |DXGREENS       |Green Greens\n       |0x2E   |DXPSTADIUM     |Pokémon Stadium 1\n       |0x2F   |DXRCRUISE      |Rainbow Cruise\n       |0x30   |DXCORNERIA     |Corneria\n       |0x31   |DXBIGBLUE      |Big Blue\n       |0x32   |DXZEBES        |Brinstar\n       |0x33   |OLDIN          |Bridge of Eldin\n       |0x34   |HOMERUN        |Home-Run Contest\nX      |0x35   |EDIT           |Stage Builder\n       |0x36   |HEAL           |All-Star Rest Area\n       |0x37   |ONLINETRAINING |Online Waiting Room\n       |0x38   |TARGETLV       |Break the Targets\nX      |0x39   |CHARAROLL      |Classic Mode Credits\n       |0x40   |CUSTOM01       |Custom 01 // Custom Stages. If you don't want to see these in dropdowns, delete the lines or flag them with \"X\"\n       |0x41   |CUSTOM02       |Custom 02\n       |0x42   |CUSTOM03       |Custom 03\n       |0x43   |CUSTOM04       |Custom 04\n       |0x44   |CUSTOM05       |Custom 05\n       |0x45   |CUSTOM06       |Custom 06\n       |0x46   |CUSTOM07       |Custom 07\n       |0x47   |CUSTOM08       |Custom 08\n       |0x48   |CUSTOM09       |Custom 09\n       |0x49   |CUSTOM0A       |Custom 0A\n       |0x4A   |CUSTOM0B       |Custom 0B\n       |0x4B   |CUSTOM0C       |Custom 0C\n       |0x4C   |CUSTOM0D       |Custom 0D\n       |0x4D   |CUSTOM0E       |Custom 0E\n       |0x4E   |CUSTOM0F       |Custom 0F\n       |0x4F   |CUSTOM10       |Custom 10\n       |0x50   |CUSTOM11       |Custom 11\n       |0x51   |CUSTOM12       |Custom 12\n       |0x52   |CUSTOM13       |Custom 13\n       |0x53   |CUSTOM14       |Custom 14\n       |0x54   |CUSTOM15       |Custom 15\n       |0x55   |CUSTOM16       |Custom 16\n       |0x56   |CUSTOM17       |Custom 17\n       |0x57   |CUSTOM18       |Custom 18\n       |0x58   |CUSTOM19       |Custom 19\n       |0x59   |CUSTOM1A       |Custom 1A\n       |0x5A   |CUSTOM1B       |Custom 1B\n       |0x5B   |CUSTOM1C       |Custom 1C\n       |0x5C   |CUSTOM1D       |Custom 1D\n       |0x5D   |CUSTOM1E       |Custom 1E\n       |0x5E   |CUSTOM1F       |Custom 1F\n       |0x5F   |CUSTOM20       |Custom 20\n       |0x60   |CUSTOM21       |Custom 21\n       |0x61   |CUSTOM22       |Custom 22\n       |0x62   |CUSTOM23       |Custom 23\n       |0x63   |CUSTOM24       |Custom 24\n       |0x64   |CUSTOM25       |Custom 25\n       |0x65   |CUSTOM26       |Custom 26\n       |0x66   |CUSTOM27       |Custom 27\n       |0x67   |CUSTOM28       |Custom 28\n       |0x68   |CUSTOM29       |Custom 29\n       |0x69   |CUSTOM2A       |Custom 2A\n       |0x6A   |CUSTOM2B       |Custom 2B\n       |0x6B   |CUSTOM2C       |Custom 2C\n       |0x6C   |CUSTOM2D       |Custom 2D\n       |0x6D   |CUSTOM2E       |Custom 2E\n       |0x6E   |CUSTOM2F       |Custom 2F\n       |0x6F   |CUSTOM30       |Custom 30\n       |0x70   |CUSTOM31       |Custom 31\n       |0x71   |CUSTOM32       |Custom 32\n       |0x72   |CUSTOM33       |Custom 33\n       |0x73   |CUSTOM34       |Custom 34\n       |0x74   |CUSTOM35       |Custom 35\n       |0x75   |CUSTOM36       |Custom 36\n       |0x76   |CUSTOM37       |Custom 37\n       |0x77   |CUSTOM38       |Custom 38\n       |0x78   |CUSTOM39       |Custom 39\n       |0x79   |CUSTOM3A       |Custom 3A\n       |0x7A   |CUSTOM3B       |Custom 3B\n       |0x7B   |CUSTOM3C       |Custom 3C\n       |0x7C   |CUSTOM3D       |Custom 3D\n       |0x7D   |CUSTOM3E       |Custom 3E\n       |0x7E   |CUSTOM3F       |Custom 3F\n       |0x7F   |CUSTOM40       |Custom 40\n       |0x80   |CUSTOM41       |Custom 41\n       |0x81   |CUSTOM42       |Custom 42\n       |0x82   |CUSTOM43       |Custom 43\n       |0x83   |CUSTOM44       |Custom 44\n       |0x84   |CUSTOM45       |Custom 45\n       |0x85   |CUSTOM46       |Custom 46\n       |0x86   |CUSTOM47       |Custom 47\n       |0x87   |CUSTOM48       |Custom 48\n       |0x88   |CUSTOM49       |Custom 49\n       |0x89   |CUSTOM4A       |Custom 4A\n       |0x8A   |CUSTOM4B       |Custom 4B\n       |0x8B   |CUSTOM4C       |Custom 4C\n       |0x8C   |CUSTOM4D       |Custom 4D\n       |0x8D   |CUSTOM4E       |Custom 4E\n       |0x8E   |CUSTOM4F       |Custom 4F\n       |0x8F   |CUSTOM50       |Custom 50\n       |0x90   |CUSTOM51       |Custom 51\n       |0x91   |CUSTOM52       |Custom 52\n       |0x92   |CUSTOM53       |Custom 53\n       |0x93   |CUSTOM54       |Custom 54\n       |0x94   |CUSTOM55       |Custom 55\n       |0x95   |CUSTOM56       |Custom 56\n       |0x96   |CUSTOM57       |Custom 57\n       |0x97   |CUSTOM58       |Custom 58\n       |0x98   |CUSTOM59       |Custom 59\n       |0x99   |CUSTOM5A       |Custom 5A\n       |0x9A   |CUSTOM5B       |Custom 5B\n       |0x9B   |CUSTOM5C       |Custom 5C\n       |0x9C   |CUSTOM5D       |Custom 5D\n       |0x9D   |CUSTOM5E       |Custom 5E\n       |0x9E   |CUSTOM5F       |Custom 5F\n       |0x9F   |CUSTOM60       |Custom 60\n       |0xA0   |CUSTOM61       |Custom 61\n       |0xA1   |CUSTOM62       |Custom 62\n       |0xA2   |CUSTOM63       |Custom 63\n       |0xA3   |CUSTOM64       |Custom 64\n       |0xA4   |CUSTOM65       |Custom 65\n       |0xA5   |CUSTOM66       |Custom 66\n       |0xA6   |CUSTOM67       |Custom 67\n       |0xA7   |CUSTOM68       |Custom 68\n       |0xA8   |CUSTOM69       |Custom 69\n       |0xA9   |CUSTOM6A       |Custom 6A\n       |0xAA   |CUSTOM6B       |Custom 6B\n       |0xAB   |CUSTOM6C       |Custom 6C\n       |0xAC   |CUSTOM6D       |Custom 6D\n       |0xAD   |CUSTOM6E       |Custom 6E\n       |0xAE   |CUSTOM6F       |Custom 6F\n       |0xAF   |CUSTOM70       |Custom 70\n       |0xB0   |CUSTOM71       |Custom 71\n       |0xB1   |CUSTOM72       |Custom 72\n       |0xB2   |CUSTOM73       |Custom 73\n       |0xB3   |CUSTOM74       |Custom 74\n       |0xB4   |CUSTOM75       |Custom 75\n       |0xB5   |CUSTOM76       |Custom 76\n       |0xB6   |CUSTOM77       |Custom 77\n       |0xB7   |CUSTOM78       |Custom 78\n       |0xB8   |CUSTOM79       |Custom 79\n       |0xB9   |CUSTOM7A       |Custom 7A\n       |0xBA   |CUSTOM7B       |Custom 7B\n       |0xBB   |CUSTOM7C       |Custom 7C\n       |0xBC   |CUSTOM7D       |Custom 7D\n       |0xBD   |CUSTOM7E       |Custom 7E\n       |0xBE   |CUSTOM7F       |Custom 7F\n       |0xBF   |CUSTOM80       |Custom 80\n       |0xC0   |CUSTOM81       |Custom 81\n       |0xC1   |CUSTOM82       |Custom 82\n       |0xC2   |CUSTOM83       |Custom 83\n       |0xC3   |CUSTOM84       |Custom 84\n       |0xC4   |CUSTOM85       |Custom 85\n       |0xC5   |CUSTOM86       |Custom 86\n       |0xC6   |CUSTOM87       |Custom 87\n       |0xC7   |CUSTOM88       |Custom 88\n       |0xC8   |CUSTOM89       |Custom 89\n       |0xC9   |CUSTOM8A       |Custom 8A\n       |0xCA   |CUSTOM8B       |Custom 8B\n       |0xCB   |CUSTOM8C       |Custom 8C\n       |0xCC   |CUSTOM8D       |Custom 8D\n       |0xCD   |CUSTOM8E       |Custom 8E\n       |0xCE   |CUSTOM8F       |Custom 8F\n       |0xCF   |CUSTOM90       |Custom 90\n       |0xD0   |CUSTOM91       |Custom 91\n       |0xD1   |CUSTOM92       |Custom 92\n       |0xD2   |CUSTOM93       |Custom 93\n       |0xD3   |CUSTOM94       |Custom 94\n       |0xD4   |CUSTOM95       |Custom 95\n       |0xD5   |CUSTOM96       |Custom 96\n       |0xD6   |CUSTOM97       |Custom 97\n       |0xD7   |CUSTOM98       |Custom 98\n       |0xD8   |CUSTOM99       |Custom 99\n       |0xD9   |CUSTOM9A       |Custom 9A\n       |0xDA   |CUSTOM9B       |Custom 9B\n       |0xDB   |CUSTOM9C       |Custom 9C\n       |0xDC   |CUSTOM9D       |Custom 9D\n       |0xDD   |CUSTOM9E       |Custom 9E\n       |0xDE   |CUSTOM9F       |Custom 9F\n       |0xDF   |CUSTOMA0       |Custom A0\n       |0xE0   |CUSTOMA1       |Custom A1\n       |0xE1   |CUSTOMA2       |Custom A2\n       |0xE2   |CUSTOMA3       |Custom A3\n       |0xE3   |CUSTOMA4       |Custom A4\n       |0xE4   |CUSTOMA5       |Custom A5\n       |0xE5   |CUSTOMA6       |Custom A6\n       |0xE6   |CUSTOMA7       |Custom A7\n       |0xE7   |CUSTOMA8       |Custom A8\n       |0xE8   |CUSTOMA9       |Custom A9\n       |0xE9   |CUSTOMAA       |Custom AA\n       |0xEA   |CUSTOMAB       |Custom AB\n       |0xEB   |CUSTOMAC       |Custom AC\n       |0xEC   |CUSTOMAD       |Custom AD\n       |0xED   |CUSTOMAE       |Custom AE\n       |0xEE   |CUSTOMAF       |Custom AF\n       |0xEF   |CUSTOMB0       |Custom B0\n       |0xF0   |CUSTOMB1       |Custom B1\n       |0xF1   |CUSTOMB2       |Custom B2\n       |0xF2   |CUSTOMB3       |Custom B3\n       |0xF3   |CUSTOMB4       |Custom B4\n       |0xF4   |CUSTOMB5       |Custom B5\n       |0xF5   |CUSTOMB6       |Custom B6\n       |0xF6   |CUSTOMB7       |Custom B7\n       |0xF7   |CUSTOMB8       |Custom B8\n       |0xF8   |CUSTOMB9       |Custom B9\n       |0xF9   |CUSTOMBA       |Custom BA\n       |0xFA   |CUSTOMBB       |Custom BB\n       |0xFB   |CUSTOMBC       |Custom BC\n       |0xFC   |CUSTOMBD       |Custom BD\n       |0xFD   |CUSTOMBE       |Custom BE\n       |0xFE   |CUSTOMBF       |Custom BF\n-s     |0xFF   |CUSTOMC0       |Custom C0 // In single player modes, 0xFF or 0xFFFFFFFF is used for \"None\"";
            File.WriteAllText(listName, listDefault);
        }

        public static void GenerateList()
        {
            Directory.CreateDirectory(directory);
            if (!File.Exists(listName))
                GenerateDefaultList();
            if (!File.Exists(listName))
                return;
            fileList = new List<string>(File.ReadAllLines(listName));
            stageList = new List<Stage>();
            // Remove descriptor strings and unecessary spaces
            for (int i = 0; i < fileList.Count; i++)
            {
                int commentIndex = fileList[i].IndexOf("//");
                if (commentIndex > 0)
                    fileList[i] = fileList[i].Substring(0, commentIndex);
                fileList[i] = fileList[i].TrimEnd(trimChars);
            }
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("-S"))
                    stageList.Add(new Stage(Convert.ToByte(s.Substring(idIndex + 2, 2), 16), true));
            // Order the list by IDs
            stageList = stageList.OrderBy(o => o.ID).ToList();
        }
    }
}
