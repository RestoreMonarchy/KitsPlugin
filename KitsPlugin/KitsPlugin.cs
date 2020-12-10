using KitsPlugin.Database;
using KitsPlugin.Models;
using KitsPlugin.Services;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitsPlugin
{
    public class KitsPlugin : RocketPlugin<KitsPluginConfiguration>
    {
        public static KitsPlugin Instance { get; private set; }

        public UnityEngine.Color MessageColor { get; private set; }
        

        public KitCooldownsDatabase KitCooldownsDatabase { get; private set; }
        public CooldownsService CooldownsService { get; private set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.green);

            KitCooldownsDatabase = new KitCooldownsDatabase();
            KitCooldownsDatabase.Reload();

            CooldownsService = gameObject.AddComponent<CooldownsService>();

            Logger.Log(Configuration.Instance.LoadMessage);
            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!");
        }

        protected override void Unload()
        {
            Destroy(CooldownsService);
            Logger.Log($"{Name} has been unloaded!");
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "KitInvalid", "You must specify kit name!" },
            { "KitNotFound", "Kit not found" },
            { "KitSuccess", "You received kit {0}!" },
            { "KitNoPermission", "You don't have permission to use this kit" },
            { "KitCooldown", "You have to wait {0} seconds to use this kit again!" },
            { "KitGlobalCooldown", "You have {0} seconds global cooldown yet" }
        };
    }
}
