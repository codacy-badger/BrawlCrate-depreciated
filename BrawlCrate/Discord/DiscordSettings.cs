using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.Discord
{
    class DiscordSettings
    {
        public enum ModNameType
        {
            Disabled,
            UserDefined,
            AutoInternal,
            AutoExternal
        }

        // Fields to be saved between runs
        public static bool enabled = true;
        public static string userPickedImageKey = "brawlcrate";
        public static ModNameType modNameType = ModNameType.Disabled;
        public static string workString = "Working on";
        public static string userNamedMod = "My Mod";
        public static bool showTimeElapsed = true;
        public static DiscordController DiscordController;

        public static void Update()
        {
            if (!enabled)
            {
                DiscordRpc.Shutdown();
                return;
            }
            
            DiscordController.presence = new DiscordRpc.RichPresence()
            {
                smallImageKey = "",
                smallImageText = "",
                largeImageKey = "brawlcrate",
                largeImageText = ""
            };
            if(MainForm.Instance.RootNode == null)
            {
                DiscordController.presence.details = "Idling";
            }
            else if(MainForm.Instance.RootNode.ResourceNode is ARCNode)
            {
                if (((ARCNode)MainForm.Instance.RootNode.ResourceNode).IsStage)
                {
                    if (MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("STGRESULT", StringComparison.OrdinalIgnoreCase))
                        DiscordController.presence.details = workString + " " + "the results screen";
                    else
                        DiscordController.presence.details = workString + " " + "a stage";
                }
                else if (((ARCNode)MainForm.Instance.RootNode.ResourceNode).IsCharacter)
                {
                    if (MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("0") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("1") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("2") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("3") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("4") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("5") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("6") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("7") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("8") ||
                       MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("9"))
                    {
                        DiscordController.presence.details = workString + " " + "a costume";
                    }
                    else if (MainForm.Instance.RootNode.ResourceNode.Name.Contains("motion", StringComparison.OrdinalIgnoreCase))
                    {
                        DiscordController.presence.details = workString + " " + "animations";
                    }
                    else
                    {
                        DiscordController.presence.details = workString + " " + "a fighter";
                    }
                }
                else if (MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("sc_", StringComparison.OrdinalIgnoreCase) ||
                        MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("common5", StringComparison.OrdinalIgnoreCase) ||
                        MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("mu_", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " " + "menus";
                }
                else if (MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("info", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " " + "UI";
                }
                else if (Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\stage\\adventure"))
                {
                    DiscordController.presence.details = workString + " " + "a subspace stage";
                }
                else if(MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("common2", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " " + "single player";
                }
                else if (MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("common3", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " " + "items";
                }
                else if (MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("common", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " " + "animations";
                }
                else if (MainForm.Instance.RootNode.ResourceNode.Name.StartsWith("home_", StringComparison.OrdinalIgnoreCase)
                    && Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\system\\homebutton"))
                {
                    DiscordController.presence.details = workString + " " + "the home menu";
                }
                else if (MainForm.Instance.RootNode.ResourceNode.Name.Equals("cs_pack", StringComparison.OrdinalIgnoreCase))
                {
                    DiscordController.presence.details = workString + " " + "coin launcher";
                }
                else if ((MainForm.Instance.RootNode.Name.StartsWith("Itm") || (Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item") ||
                    Program.RootPath.Substring(0, Program.RootPath.LastIndexOf('\\')).Substring(0, Program.RootPath.LastIndexOf('\\')).EndsWith("\\item")))
                    && (MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("Brres") || MainForm.Instance.RootNode.ResourceNode.Name.EndsWith("Param")))
                {
                    DiscordController.presence.details = workString + " " + "an item";
                }
                else
                {
                    DiscordController.presence.details = workString + " " + "a mod";
                }
            }
            else if (MainForm.Instance.RootNode.ResourceNode is RELNode)
            {
                DiscordController.presence.details = workString + " " + "a module";
            }
            else if (MainForm.Instance.RootNode.ResourceNode is RSTMNode)
            {
                DiscordController.presence.details = workString + " " + "a BRSTM";
            }
            else
            {
                DiscordController.presence.details = workString + " " + "a mod";
            }

            if (MainForm.Instance.RootNode != null)
            {
                string tabName = MainForm.Instance.RootNode.Text;
                switch (modNameType)
                {
                    case ModNameType.Disabled:
                        DiscordController.presence.state = "";
                        break;
                    case ModNameType.UserDefined:
                        DiscordController.presence.state = userNamedMod;
                        break;
                    case ModNameType.AutoInternal:
                        DiscordController.presence.state = ((MainForm.Instance.RootNode == null || MainForm.Instance.RootNode.Name == null || MainForm.Instance.RootNode.ResourceNode.Name.Equals("<null>", StringComparison.OrdinalIgnoreCase)) ? "" : MainForm.Instance.RootNode.ResourceNode.Name);
                        break;
                    case ModNameType.AutoExternal:
                        DiscordController.presence.state = ((Program.RootPath == null || Program.RootPath == "") ? "" : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1));
                        break;
                }
            }

            if (showTimeElapsed)
                DiscordController.presence.startTimestamp = startTime;
            DiscordRpc.UpdatePresence(DiscordController.presence);
        }

        public static void LoadSettings(bool update = false)
        {
            if (enabled != BrawlCrate.Properties.Settings.Default.DiscordRPCEnabled && enabled == true)
                DiscordController.Initialize();
            enabled = BrawlCrate.Properties.Settings.Default.DiscordRPCEnabled;
            modNameType = BrawlCrate.Properties.Settings.Default.DiscordRPCNameType;
            userNamedMod = BrawlCrate.Properties.Settings.Default.DiscordRPCNameCustom;
            if(update)
                Update();
        }
        
        //Temporary, don't save to config
        public static string lastFileOpened = null;
        public static long startTime;
    }
}