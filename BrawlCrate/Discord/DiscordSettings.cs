using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static ModNameType modNameType = ModNameType.AutoInternal;
        public static string workString = "Working on";
        public static string userNamedMod = "ModNameHere";
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
            
            DiscordController.presence.details = workString + " " + "a mod";

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
                        DiscordController.presence.state = (MainForm.Instance.RootNode == null ? "" : MainForm.Instance.RootNode.Text);
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

        //Temporary, don't save to config
        public static string lastFileOpened = null;
        public static long startTime;
    }
}