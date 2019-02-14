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
            Auto
        }

        // Fields to be saved between runs
        public static bool enabled = true;
        public static string userPickedImageKey = "brawlcrate";
        public static ModNameType modNameType = ModNameType.Auto;
        public static string userNamedMod = "ModNameHere";
        public static bool showCurrentWindow;
        public static bool showTimeElapsed;
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
                    DiscordController.presence.state = "Working on a mod";
                    break;
                case ModNameType.UserDefined:
                    DiscordController.presence.state = $"Working on {userNamedMod}";
                    break;
                case ModNameType.Auto:
                    DiscordController.presence.state = ("Working on " + (MainForm.Instance.RootNode == null ? " a mod" : MainForm.Instance.RootNode.Text));
                    break;
            }

            if (showCurrentWindow)
            {
                if (MainForm.Instance.RootNode != null)
                {
                    string tabName = MainForm.Instance.RootNode.Name;
                    DiscordController.presence.details = $"{tabName}";
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