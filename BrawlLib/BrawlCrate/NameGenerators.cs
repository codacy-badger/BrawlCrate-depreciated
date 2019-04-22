﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using BrawlLib.SSBB;
using System.Reflection;

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
        public static string directory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + "CustomLists"; }
        }
        public static string listName
        {
            get { return directory + '\\' + "FighterList.txt"; }
        }

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
        public static bool generated = false;

        public static string FromID(int id, int idOffset, string flagToIgnore)
        {
            if (!generated)
                generated = GenerateLists();
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idOffset).StartsWith("0X" + id.ToString("X2").ToUpper()))
                    if (!s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith(flagToIgnore))
                        return s.Substring(nameIndex).Trim(trimChars);
            return "Fighter 0x" + id.ToString("X2");
        }

        public static string InternalNameFromID(int id, int idOffset, string flagToIgnore)
        {
            if (!generated)
                generated = GenerateLists();
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idOffset).StartsWith("0X" + id.ToString("X2").ToUpper()))
                    if (!s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith(flagToIgnore))
                    {
                        string intName = s.Substring(internalNameIndex, (nameIndex- internalNameIndex) - 1).Trim(trimChars);
                        if(intName.Length > 0)
                            return intName;
                    }
            return "Fighter0x" + id.ToString("X2");
        }

        public static string FromPacName(string pacName)
        {
            if (!generated)
                generated = GenerateLists();
            foreach (string s in fileList)
                if (s.ToUpper().Substring(internalNameIndex).StartsWith(pacName.ToUpper()))
                    return s.Substring(nameIndex).Trim(trimChars);
            return pacName.ToUpper();
        }

        public static void GenerateDefaultList()
        {
            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                var resourceName = ("BrawlLib.CustomLists." + (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2).Equals("en", StringComparison.OrdinalIgnoreCase) ? "" : (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2).ToLower() + '.')) + "FighterList.txt");
                string listDefault = "";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                        listDefault = reader.ReadToEnd();
                File.WriteAllText(listName, listDefault);
            }
            catch
            {
                var resourceName = "BrawlLib.CustomLists.FighterList.txt";
                string listDefault = "";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                        listDefault = reader.ReadToEnd();
                File.WriteAllText(listName, listDefault);
            }
        }

        static readonly byte[] listV1Hash = { 0xD2, 0x03, 0xA9, 0x9E, 0x92, 0xD4, 0xCC, 0xCB, 0xD4, 0xBD, 0x85, 0x28, 0xD8, 0x7C, 0x72, 0xB4 };

        public static bool GenerateLists()
        {
            Directory.CreateDirectory(directory);
            bool fileOutdated = false;
            // Ensure unmodified files get updated properly
            if(File.Exists(listName))
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    using (var stream = File.OpenRead(listName))
                    {
                        byte[] hash = md5.ComputeHash(stream);
                        if(hash == listV1Hash)
                            fileOutdated = true;
                    }
                }
            }
            if (fileOutdated)
                File.Delete(listName);
            if (!File.Exists(listName))
                GenerateDefaultList();
            if (!File.Exists(listName))
                return false;
            generated = true;
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
            return true;
        }

        public static void GenerateSlotLists()
        {
            if (!generated)
                generated = GenerateLists();
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
            if (!generated)
                generated = GenerateLists();
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
            if (!generated)
                generated = GenerateLists();
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(cssSlotIDIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("+S"))
                    cssSlotIDList.Add(new Fighter(Convert.ToByte(s.Substring(cssSlotIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(cssSlotIDIndex + 2, 2), 16), cssSlotIDIndex, "+S")));
        }

        public static void GenerateCosmeticSlotList()
        {
            if (!generated)
                generated = GenerateLists();
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(cosmeticIDIndex).StartsWith("0X") && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("+S"))
                    cosmeticIDList.Add(new Fighter(Convert.ToByte(s.Substring(cosmeticIDIndex + 2, 2), 16), FromID(Convert.ToByte(s.Substring(cosmeticIDIndex + 2, 2), 16), cosmeticIDIndex, "+S")));
        }
    }

    public static class StageNameGenerators
    {
        public static List<string> fileList = new List<string>();
        public static List<Stage> stageList = new List<Stage>();
        public static string directory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory + "CustomLists"; }
        }
        public static string listName
        {
            get { return directory + '\\' + "StageList.txt"; }
        }

        // Used to determine offsets
        static readonly int flagIndex = 0;
        static readonly int idIndex = 8;
        static readonly int internalNameIndex = 16;
        static readonly int nameIndex = 32;
        static readonly int minimumLength = nameIndex + 1;
        static readonly char[] trimChars = { ' ', '\t' };
        static bool generated = false;

        public static string FromID(int id)
        {
            if (!generated)
                generated = GenerateList();
            foreach (string s in fileList)
                if (s.Length >= minimumLength && s.ToUpper().Substring(idIndex).StartsWith("0X" + id.ToString("X2").ToUpper()) && !s.ToUpper().Substring(flagIndex).StartsWith("X") && !s.ToUpper().Substring(flagIndex).StartsWith("-S"))
                    return s.Substring(nameIndex).Trim(trimChars);
            return "Stage 0x" + id.ToString("X2");
        }

        public static string FromPacName(string pacName)
        {
            if (!generated)
                generated = GenerateList();
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
            var assembly = Assembly.GetExecutingAssembly();
            try
            {
                var resourceName = ("BrawlLib.CustomLists." + (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2).Equals("en", StringComparison.OrdinalIgnoreCase) ? "" : (System.Threading.Thread.CurrentThread.CurrentUICulture.ToString().Substring(0, 2).ToLower() + '.')) + "StageList.txt");
                string listDefault = "";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                        listDefault = reader.ReadToEnd();
                File.WriteAllText(listName, listDefault);
            }
            catch
            {
                var resourceName = "BrawlLib.CustomLists.StageList.txt";
                string listDefault = "";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                        listDefault = reader.ReadToEnd();
                File.WriteAllText(listName, listDefault);
            }
        }

        public static bool GenerateList()
        {
            Directory.CreateDirectory(directory);
            if (!File.Exists(listName))
                GenerateDefaultList();
            if (!File.Exists(listName))
                return false;
            generated = true;
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
            return true;
        }
    }
}
