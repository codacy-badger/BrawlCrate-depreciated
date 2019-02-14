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
        public static ModNameType modNameType = ModNameType.AutoExternal;
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

            switch (modNameType)
            {
                case ModNameType.Disabled:
                    DiscordController.presence.state = workString + " " + "a mod";
                    break;
                case ModNameType.UserDefined:
                    DiscordController.presence.state = workString + " " + userNamedMod;
                    break;
                case ModNameType.AutoInternal:
                    DiscordController.presence.state = workString + " " + (MainForm.Instance.RootNode == null ? "a mod" : MainForm.Instance.RootNode.Text);
                    break;
                case ModNameType.AutoExternal:
                    DiscordController.presence.state = workString + " " + ((Program.RootPath == null || Program.RootPath == "") ? "a mod" : Program.RootPath.Substring(Program.RootPath.LastIndexOf('\\') + 1));
                    break;
            }

            /*if (showCurrentWindow)
            {
                if (MainForm.Instance.RootNode != null)
                {
                    string tabName = MainForm.Instance.RootNode.Name;
                    DiscordController.presence.details = $"{tabName}";
                }
            }*/

            if (showTimeElapsed)
                DiscordController.presence.startTimestamp = startTime;
            DiscordRpc.UpdatePresence(DiscordController.presence);
        }

        //Temporary, don't save to config
        public static string lastFileOpened = null;
        public static long startTime;
    }
}