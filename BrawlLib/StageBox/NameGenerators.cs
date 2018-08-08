using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace BrawlLib.StageBox
{
    public class FighterNameGenerators
    {

    }

    public static class StageNameGenerators
    {
        public static List<string> stageList = new List<string>();
        public static string listName = "CustomLists\\StageList.txt";

        public static string FromID(int id)
        {
            foreach (string s in stageList)
                if (s.ToUpper().StartsWith("0X" + id.ToString("X2").ToUpper()))
                    return s.Substring(24);
            return "Stage0x" + id.ToString("X2");
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
            foreach (string s in stageList)
                if (s.ToUpper().Substring(8).StartsWith(pacName.ToUpper()) || (isExStage && s.ToUpper().Substring(8).StartsWith("EX" + exID)))
                    return s.Substring(24);
            return pacName.ToUpper();
        }

        public static void GenerateDefaultStageList()
        {
            //File.Create(listName);
            string stagelistdefault = "ID     |Internal Name  |Actual Name\nDON'T EDIT THESE TWO   |EDIT THIS\n----------------------------------------------------\n0x01   |BATTLEFIELD:   |Battlefield\n0x02   |FINAL:         |Final Destination\n0x03   |DOLPIC:        |Delfino Plaza\n0x04   |MANSION:       |Luigi's Mansion\n0x05   |MARIOPAST:     |Mushroomy Kingdom\n0x06   |KART:          |Mario Circuit\n0x07   |DONKEY:        |75m\n0x08   |JUNGLE:        |Rumble Falls\n0x09   |PIRATES:       |Pirate Ship\n0x0B   |NORFAIR:       |Norfair\n0x0C   |ORPHEON:       |Frigate Orpheon\n0x0D   |CRAYON:        |Yoshi's Island (Brawl)\n0x0E   |HALBERD:       |Halberd\n0x13   |STARFOX:       |Lylat Cruise\n0x14   |STADIUM:       |Pokémon Stadium 2\n0x15   |TENGAN:        |Spear Pillar\n0x16   |FZERO:         |Port Town Aero Dive\n0x17   |ICE:           |Summit\n0x18   |GW:            |Flat Zone 2\n0x19   |EMBLEM:        |Castle Siege\n0x1C   |MADEIN:        |WarioWare\n0x1D   |EARTH:         |Distant Planet\n0x1E   |PALUTENA:      |Skyworld\n0x1F   |FAMICOM:       |Mario Bros.\n0x20   |NEWPORK:       |New Pork City\n0x21   |VILLAGE:       |Smashville\n0x22   |METALGEAR:     |Shadow Moses Island\n0x23   |GREENHILL:     |Green Hill Zone\n0x24   |PICTCHAT:      |PictoChat\n0x25   |PLANKTON:      |Hanenbow\n0x26   |CONFIGTEST:    |Control Configuration Stage\n0x29   |DXSHRINE:      |Temple\n0x2A   |DXYORSTER:     |Yoshi's Island (Melee)\n0x2B   |DXGARDEN:      |Jungle Japes\n0x2C   |DXONETT:       |Onett\n0x2D   |DXGREENS:      |Green Greens\n0x2E   |DXPSTADIUM:    |Pokémon Stadium 1\n0x2F   |DXRCRUISE:     |Rainbow Cruise\n0x30   |DXCORNERIA:    |Corneria\n0x31   |DXBIGBLUE:     |Big Blue\n0x32   |DXZEBES:       |Brinstar\n0x33   |OLDIN:         |Bridge of Eldin\n0x34   |HOMERUN:       |Home Run Contest\n0x35   |EDIT:          |Stage Builder\n0x36   |HEAL:          |All-Star Rest Area\n0x37   |ONLINETRAINING:|Online Training Room\n0x38   |TARGETLV:      |Break the Targets\n0x39   |CHARAROLL:     |Classic Mode Credits\n0x40   |CUSTOM01:      |Custom 01\n0x41   |CUSTOM02:      |Custom 02\n0x42   |CUSTOM03:      |Custom 03\n0x43   |CUSTOM04:      |Custom 04\n0x44   |CUSTOM05:      |Custom 05\n0x45   |CUSTOM06:      |Custom 06\n0x46   |CUSTOM07:      |Custom 07\n0x47   |CUSTOM08:      |Custom 08\n0x48   |CUSTOM09:      |Custom 09\n0x49   |CUSTOM0A:      |Custom 0A\n0x4A   |CUSTOM0B:      |Custom 0B\n0x4B   |CUSTOM0C:      |Custom 0C\n0x4C   |CUSTOM0D:      |Custom 0D\n0x4D   |CUSTOM0E:      |Custom 0E\n0x4E   |CUSTOM0F:      |Custom 0F\n0x4F   |CUSTOM10:      |Custom 10\n0x50   |CUSTOM11:      |Custom 11\n0x51   |CUSTOM12:      |Custom 12\n0x52   |CUSTOM13:      |Custom 13\n0x53   |CUSTOM14:      |Custom 14\n0x54   |CUSTOM15:      |Custom 15\n0x55   |CUSTOM16:      |Custom 16\n0x56   |CUSTOM17:      |Custom 17\n0x57   |CUSTOM18:      |Custom 18\n0x58   |CUSTOM19:      |Custom 19\n0x59   |CUSTOM1A:      |Custom 1A\n0x5A   |CUSTOM1B:      |Custom 1B\n0x5B   |CUSTOM1C:      |Custom 1C\n0x5C   |CUSTOM1D:      |Custom 1D\n0x5D   |CUSTOM1E:      |Custom 1E\n0x5E   |CUSTOM1F:      |Custom 1F\n0x5F   |CUSTOM20:      |Custom 20\n0x60   |CUSTOM21:      |Custom 21\n0x61   |CUSTOM22:      |Custom 22\n0x62   |CUSTOM23:      |Custom 23\n0x63   |CUSTOM24:      |Custom 24\n0x64   |CUSTOM25:      |Custom 25\n0x65   |CUSTOM26:      |Custom 26\n0x66   |CUSTOM27:      |Custom 27\n0x67   |CUSTOM28:      |Custom 28\n0x68   |CUSTOM29:      |Custom 29\n0x69   |CUSTOM2A:      |Custom 2A\n0x6A   |CUSTOM2B:      |Custom 2B\n0x6B   |CUSTOM2C:      |Custom 2C\n0x6C   |CUSTOM2D:      |Custom 2D\n0x6D   |CUSTOM2E:      |Custom 2E\n0x6E   |CUSTOM2F:      |Custom 2F\n0x6F   |CUSTOM30:      |Custom 30\n0x70   |CUSTOM31:      |Custom 31\n0x71   |CUSTOM32:      |Custom 32\n0x72   |CUSTOM33:      |Custom 33\n0x73   |CUSTOM34:      |Custom 34\n0x74   |CUSTOM35:      |Custom 35\n0x75   |CUSTOM36:      |Custom 36\n0x76   |CUSTOM37:      |Custom 37\n0x77   |CUSTOM38:      |Custom 38\n0x78   |CUSTOM39:      |Custom 39\n0x79   |CUSTOM3A:      |Custom 3A\n0x7A   |CUSTOM3B:      |Custom 3B\n0x7B   |CUSTOM3C:      |Custom 3C\n0x7C   |CUSTOM3D:      |Custom 3D\n0x7D   |CUSTOM3E:      |Custom 3E\n0x7E   |CUSTOM3F:      |Custom 3F\n0x7F   |CUSTOM40:      |Custom 40\n0x80   |CUSTOM41:      |Custom 41\n0x81   |CUSTOM42:      |Custom 42\n0x82   |CUSTOM43:      |Custom 43\n0x83   |CUSTOM44:      |Custom 44\n0x84   |CUSTOM45:      |Custom 45\n0x85   |CUSTOM46:      |Custom 46\n0x86   |CUSTOM47:      |Custom 47\n0x87   |CUSTOM48:      |Custom 48\n0x88   |CUSTOM49:      |Custom 49\n0x89   |CUSTOM4A:      |Custom 4A\n0x8A   |CUSTOM4B:      |Custom 4B\n0x8B   |CUSTOM4C:      |Custom 4C\n0x8C   |CUSTOM4D:      |Custom 4D\n0x8D   |CUSTOM4E:      |Custom 4E\n0x8E   |CUSTOM4F:      |Custom 4F\n0x8F   |CUSTOM50:      |Custom 50\n0x90   |CUSTOM51:      |Custom 51\n0x91   |CUSTOM52:      |Custom 52\n0x92   |CUSTOM53:      |Custom 53\n0x93   |CUSTOM54:      |Custom 54\n0x94   |CUSTOM55:      |Custom 55\n0x95   |CUSTOM56:      |Custom 56\n0x96   |CUSTOM57:      |Custom 57\n0x97   |CUSTOM58:      |Custom 58\n0x98   |CUSTOM59:      |Custom 59\n0x99   |CUSTOM5A:      |Custom 5A\n0x9A   |CUSTOM5B:      |Custom 5B\n0x9B   |CUSTOM5C:      |Custom 5C\n0x9C   |CUSTOM5D:      |Custom 5D\n0x9D   |CUSTOM5E:      |Custom 5E\n0x9E   |CUSTOM5F:      |Custom 5F\n0x9F   |CUSTOM60:      |Custom 60\n0xA0   |CUSTOM61:      |Custom 61\n0xA1   |CUSTOM62:      |Custom 62\n0xA2   |CUSTOM63:      |Custom 63\n0xA3   |CUSTOM64:      |Custom 64\n0xA4   |CUSTOM65:      |Custom 65\n0xA5   |CUSTOM66:      |Custom 66\n0xA6   |CUSTOM67:      |Custom 67\n0xA7   |CUSTOM68:      |Custom 68\n0xA8   |CUSTOM69:      |Custom 69\n0xA9   |CUSTOM6A:      |Custom 6A\n0xAA   |CUSTOM6B:      |Custom 6B\n0xAB   |CUSTOM6C:      |Custom 6C\n0xAC   |CUSTOM6D:      |Custom 6D\n0xAD   |CUSTOM6E:      |Custom 6E\n0xAE   |CUSTOM6F:      |Custom 6F\n0xAF   |CUSTOM70:      |Custom 70\n0xB0   |CUSTOM71:      |Custom 71\n0xB1   |CUSTOM72:      |Custom 72\n0xB2   |CUSTOM73:      |Custom 73\n0xB3   |CUSTOM74:      |Custom 74\n0xB4   |CUSTOM75:      |Custom 75\n0xB5   |CUSTOM76:      |Custom 76\n0xB6   |CUSTOM77:      |Custom 77\n0xB7   |CUSTOM78:      |Custom 78\n0xB8   |CUSTOM79:      |Custom 79\n0xB9   |CUSTOM7A:      |Custom 7A\n0xBA   |CUSTOM7B:      |Custom 7B\n0xBB   |CUSTOM7C:      |Custom 7C\n0xBC   |CUSTOM7D:      |Custom 7D\n0xBD   |CUSTOM7E:      |Custom 7E\n0xBE   |CUSTOM7F:      |Custom 7F\n0xBF   |CUSTOM80:      |Custom 80\n0xC0   |CUSTOM81:      |Custom 81\n0xC1   |CUSTOM82:      |Custom 82\n0xC2   |CUSTOM83:      |Custom 83\n0xC3   |CUSTOM84:      |Custom 84\n0xC4   |CUSTOM85:      |Custom 85\n0xC5   |CUSTOM86:      |Custom 86\n0xC6   |CUSTOM87:      |Custom 87\n0xC7   |CUSTOM88:      |Custom 88\n0xC8   |CUSTOM89:      |Custom 89\n0xC9   |CUSTOM8A:      |Custom 8A\n0xCA   |CUSTOM8B:      |Custom 8B\n0xCB   |CUSTOM8C:      |Custom 8C\n0xCC   |CUSTOM8D:      |Custom 8D\n0xCD   |CUSTOM8E:      |Custom 8E\n0xCE   |CUSTOM8F:      |Custom 8F\n0xCF   |CUSTOM90:      |Custom 90\n0xD0   |CUSTOM91:      |Custom 91\n0xD1   |CUSTOM92:      |Custom 92\n0xD2   |CUSTOM93:      |Custom 93\n0xD3   |CUSTOM94:      |Custom 94\n0xD4   |CUSTOM95:      |Custom 95\n0xD5   |CUSTOM96:      |Custom 96\n0xD6   |CUSTOM97:      |Custom 97\n0xD7   |CUSTOM98:      |Custom 98\n0xD8   |CUSTOM99:      |Custom 99\n0xD9   |CUSTOM9A:      |Custom 9A\n0xDA   |CUSTOM9B:      |Custom 9B\n0xDB   |CUSTOM9C:      |Custom 9C\n0xDC   |CUSTOM9D:      |Custom 9D\n0xDD   |CUSTOM9E:      |Custom 9E\n0xDE   |CUSTOM9F:      |Custom 9F\n0xDF   |CUSTOMA0:      |Custom A0\n0xE0   |CUSTOMA1:      |Custom A1\n0xE1   |CUSTOMA2:      |Custom A2\n0xE2   |CUSTOMA3:      |Custom A3\n0xE3   |CUSTOMA4:      |Custom A4\n0xE4   |CUSTOMA5:      |Custom A5\n0xE5   |CUSTOMA6:      |Custom A6\n0xE6   |CUSTOMA7:      |Custom A7\n0xE7   |CUSTOMA8:      |Custom A8\n0xE8   |CUSTOMA9:      |Custom A9\n0xE9   |CUSTOMAA:      |Custom AA\n0xEA   |CUSTOMAB:      |Custom AB\n0xEB   |CUSTOMAC:      |Custom AC\n0xEC   |CUSTOMAD:      |Custom AD\n0xED   |CUSTOMAE:      |Custom AE\n0xEE   |CUSTOMAF:      |Custom AF\n0xEF   |CUSTOMB0:      |Custom B0\n0xF0   |CUSTOMB1:      |Custom B1\n0xF1   |CUSTOMB2:      |Custom B2\n0xF2   |CUSTOMB3:      |Custom B3\n0xF3   |CUSTOMB4:      |Custom B4\n0xF4   |CUSTOMB5:      |Custom B5\n0xF5   |CUSTOMB6:      |Custom B6\n0xF6   |CUSTOMB7:      |Custom B7\n0xF7   |CUSTOMB8:      |Custom B8\n0xF8   |CUSTOMB9:      |Custom B9\n0xF9   |CUSTOMBA:      |Custom BA\n0xFA   |CUSTOMBB:      |Custom BB\n0xFB   |CUSTOMBC:      |Custom BC\n0xFC   |CUSTOMBD:      |Custom BD\n0xFD   |CUSTOMBE:      |Custom BE\n0xFE   |CUSTOMBF:      |Custom BF\n0xFF   |CUSTOMC0:      |Custom C0";
            File.WriteAllText(listName, stagelistdefault);
        }

        public static void GenerateStageList()
        {
            if (!File.Exists(listName))
                GenerateDefaultStageList();
            stageList = new List<string>(File.ReadAllLines(listName));
        }
    }
}
