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
        public static string listName = "StageList.txt";

        public static string FromID(int id)
        {
            return "test " + id;
        }

        public static string FromPacName(string pacName)
        {
            foreach (string s in stageList)
                if (s.StartsWith(pacName.ToUpper()))
                    return s.Substring(16);
            return "No name found";
        }

        public static void GenerateDefaultStageList()
        {
            //File.Create(listName);
            string stagelistdefault = "Internal Name | Actual Name\n\nBATTLEFIELD:    Battlefield\nFINAL:          Final Destination\nDOLPIC:         Delfino Plaza\nMANSION:        Luigi's Mansion\nMARIOPAST:      Mushroomy Kingdom\nKART:           Mario Circuit\nDONKEY:         75m\nJUNGLE:         Rumble Falls\nPIRATES:        Pirate Ship\nNORFAIR:        Norfair\nORPHEON:        Frigate Orpheon\nCRAYON:         Yoshi's Island (Brawl)\nHALBERD:        Halberd\nSTARFOX:        Lylat Cruise\nSTADIUM:        Pokémon Stadium 2\nTENGAN:         Spear Pillar\nFZERO:          Port Town Aero Dive\nICE:            Summit\nGW:             Flat Zone 2\nEMBLEM:         Castle Siege\nMADEIN:         WarioWare\nEARTH:          Distant Planet\nPALUTENA:       Skyworld\nFAMICOM:        Mario Bros.\nNEWPORK:        New Pork City\nVILLAGE:        Smashville\nMETALGEAR:      Shadow Moses Island\nGREENHILL:      Green Hill Zone\nPICTCHAT:       PictoChat\nPLANKTON:       Hanenbow\nCONFIGTEST:     Control Configuration Stage\nDXSHRINE:       Temple\nDXYORSTER:      Yoshi's Island (Melee)\nDXGARDEN:       Jungle Japes\nDXONETT:        Onett\nDXGREENS:       Green Greens\nDXPSTADIUM:     Pokémon Stadium 1\nDXRCRUISE:      Rainbow Cruise\nDXCORNERIA:     Corneria\nDXBIGBLUE:      Big Blue\nDXZEBES:        Brinstar\nOLDIN:          Bridge of Eldin\nHOMERUN:        Home Run Contest\nEDIT:           Stage Builder\nHEAL:           All-Star Rest Area\nONLINETRAINING: Online Training Room\nTARGETLV:       Break the Targets\nCHARAROLL:      Classic Mode Credits\nCUSTOM01:       Custom 01\nCUSTOM02:       Custom 02\nCUSTOM03:       Custom 03\nCUSTOM04:       Custom 04\nCUSTOM05:       Custom 05\nCUSTOM06:       Custom 06\nCUSTOM07:       Custom 07\nCUSTOM08:       Custom 08\nCUSTOM09:       Custom 09\nCUSTOM0A:       Custom 0A\nCUSTOM0B:       Custom 0B\nCUSTOM0C:       Custom 0C\nCUSTOM0D:       Custom 0D\nCUSTOM0E:       Custom 0E\nCUSTOM0F:       Custom 0F\nCUSTOM10:       Custom 10\nCUSTOM11:       Custom 11\nCUSTOM12:       Custom 12\nCUSTOM13:       Custom 13\nCUSTOM14:       Custom 14\nCUSTOM15:       Custom 15\nCUSTOM16:       Custom 16\nCUSTOM17:       Custom 17\nCUSTOM18:       Custom 18\nCUSTOM19:       Custom 19\nCUSTOM1A:       Custom 1A\nCUSTOM1B:       Custom 1B\nCUSTOM1C:       Custom 1C\nCUSTOM1D:       Custom 1D\nCUSTOM1E:       Custom 1E\nCUSTOM1F:       Custom 1F\nCUSTOM20:       Custom 20\nCUSTOM21:       Custom 21\nCUSTOM22:       Custom 22\nCUSTOM23:       Custom 23\nCUSTOM24:       Custom 24\nCUSTOM25:       Custom 25\nCUSTOM26:       Custom 26\nCUSTOM27:       Custom 27\nCUSTOM28:       Custom 28\nCUSTOM29:       Custom 29\nCUSTOM2A:       Custom 2A\nCUSTOM2B:       Custom 2B\nCUSTOM2C:       Custom 2C\nCUSTOM2D:       Custom 2D\nCUSTOM2E:       Custom 2E\nCUSTOM2F:       Custom 2F\nCUSTOM30:       Custom 30\nCUSTOM31:       Custom 31\nCUSTOM32:       Custom 32\nCUSTOM33:       Custom 33\nCUSTOM34:       Custom 34\nCUSTOM35:       Custom 35\nCUSTOM36:       Custom 36\nCUSTOM37:       Custom 37\nCUSTOM38:       Custom 38\nCUSTOM39:       Custom 39\nCUSTOM3A:       Custom 3A\nCUSTOM3B:       Custom 3B\nCUSTOM3C:       Custom 3C\nCUSTOM3D:       Custom 3D\nCUSTOM3E:       Custom 3E\nCUSTOM3F:       Custom 3F\nCUSTOM40:       Custom 40\nCUSTOM41:       Custom 41\nCUSTOM42:       Custom 42\nCUSTOM43:       Custom 43\nCUSTOM44:       Custom 44\nCUSTOM45:       Custom 45\nCUSTOM46:       Custom 46\nCUSTOM47:       Custom 47\nCUSTOM48:       Custom 48\nCUSTOM49:       Custom 49\nCUSTOM4A:       Custom 4A\nCUSTOM4B:       Custom 4B\nCUSTOM4C:       Custom 4C\nCUSTOM4D:       Custom 4D\nCUSTOM4E:       Custom 4E\nCUSTOM4F:       Custom 4F\nCUSTOM50:       Custom 50\nCUSTOM51:       Custom 51\nCUSTOM52:       Custom 52\nCUSTOM53:       Custom 53\nCUSTOM54:       Custom 54\nCUSTOM55:       Custom 55\nCUSTOM56:       Custom 56\nCUSTOM57:       Custom 57\nCUSTOM58:       Custom 58\nCUSTOM59:       Custom 59\nCUSTOM5A:       Custom 5A\nCUSTOM5B:       Custom 5B\nCUSTOM5C:       Custom 5C\nCUSTOM5D:       Custom 5D\nCUSTOM5E:       Custom 5E\nCUSTOM5F:       Custom 5F\nCUSTOM60:       Custom 60\nCUSTOM61:       Custom 61\nCUSTOM62:       Custom 62\nCUSTOM63:       Custom 63\nCUSTOM64:       Custom 64\nCUSTOM65:       Custom 65\nCUSTOM66:       Custom 66\nCUSTOM67:       Custom 67\nCUSTOM68:       Custom 68\nCUSTOM69:       Custom 69\nCUSTOM6A:       Custom 6A\nCUSTOM6B:       Custom 6B\nCUSTOM6C:       Custom 6C\nCUSTOM6D:       Custom 6D\nCUSTOM6E:       Custom 6E\nCUSTOM6F:       Custom 6F\nCUSTOM70:       Custom 70\nCUSTOM71:       Custom 71\nCUSTOM72:       Custom 72\nCUSTOM73:       Custom 73\nCUSTOM74:       Custom 74\nCUSTOM75:       Custom 75\nCUSTOM76:       Custom 76\nCUSTOM77:       Custom 77\nCUSTOM78:       Custom 78\nCUSTOM79:       Custom 79\nCUSTOM7A:       Custom 7A\nCUSTOM7B:       Custom 7B\nCUSTOM7C:       Custom 7C\nCUSTOM7D:       Custom 7D\nCUSTOM7E:       Custom 7E\nCUSTOM7F:       Custom 7F\nCUSTOM80:       Custom 80\nCUSTOM81:       Custom 81\nCUSTOM82:       Custom 82\nCUSTOM83:       Custom 83\nCUSTOM84:       Custom 84\nCUSTOM85:       Custom 85\nCUSTOM86:       Custom 86\nCUSTOM87:       Custom 87\nCUSTOM88:       Custom 88\nCUSTOM89:       Custom 89\nCUSTOM8A:       Custom 8A\nCUSTOM8B:       Custom 8B\nCUSTOM8C:       Custom 8C\nCUSTOM8D:       Custom 8D\nCUSTOM8E:       Custom 8E\nCUSTOM8F:       Custom 8F\nCUSTOM90:       Custom 90\nCUSTOM91:       Custom 91\nCUSTOM92:       Custom 92\nCUSTOM93:       Custom 93\nCUSTOM94:       Custom 94\nCUSTOM95:       Custom 95\nCUSTOM96:       Custom 96\nCUSTOM97:       Custom 97\nCUSTOM98:       Custom 98\nCUSTOM99:       Custom 99\nCUSTOM9A:       Custom 9A\nCUSTOM9B:       Custom 9B\nCUSTOM9C:       Custom 9C\nCUSTOM9D:       Custom 9D\nCUSTOM9E:       Custom 9E\nCUSTOM9F:       Custom 9F\nCUSTOMA0:       Custom A0\nCUSTOMA1:       Custom A1\nCUSTOMA2:       Custom A2\nCUSTOMA3:       Custom A3\nCUSTOMA4:       Custom A4\nCUSTOMA5:       Custom A5\nCUSTOMA6:       Custom A6\nCUSTOMA7:       Custom A7\nCUSTOMA8:       Custom A8\nCUSTOMA9:       Custom A9\nCUSTOMAA:       Custom AA\nCUSTOMAB:       Custom AB\nCUSTOMAC:       Custom AC\nCUSTOMAD:       Custom AD\nCUSTOMAE:       Custom AE\nCUSTOMAF:       Custom AF\nCUSTOMB0:       Custom B0\nCUSTOMB1:       Custom B1\nCUSTOMB2:       Custom B2\nCUSTOMB3:       Custom B3\nCUSTOMB4:       Custom B4\nCUSTOMB5:       Custom B5\nCUSTOMB6:       Custom B6\nCUSTOMB7:       Custom B7\nCUSTOMB8:       Custom B8\nCUSTOMB9:       Custom B9\nCUSTOMBA:       Custom BA\nCUSTOMBB:       Custom BB\nCUSTOMBC:       Custom BC\nCUSTOMBD:       Custom BD\nCUSTOMBE:       Custom BE\nCUSTOMBF:       Custom BF\nCUSTOMC0:       Custom C0";
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
