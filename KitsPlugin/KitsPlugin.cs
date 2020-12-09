using KitsPlugin.Models;
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
        public Dictionary<string, List<KitCooldown>> PlayersKitCooldowns { get; private set; }
        public Dictionary<string, DateTime> GlobalCooldowns { get; private set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.green);
            PlayersKitCooldowns = new Dictionary<string, List<KitCooldown>>();
            GlobalCooldowns = new Dictionary<string, DateTime>();

            Logger.Log(Configuration.Instance.LoadMessage);
            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!");
        }

        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!");
        }

        public bool HasCooldown(string playerId, string kitName, out TimeSpan timeLeft)
        {
            timeLeft = TimeSpan.Zero;

            if (!PlayersKitCooldowns.ContainsKey(playerId))
            {
                return false;
            }

            var kitCooldown = PlayersKitCooldowns[playerId].FirstOrDefault(x => x.KitName.Equals(kitName, StringComparison.OrdinalIgnoreCase));

            if (kitCooldown == null)
            {
                return false;
            }

            timeLeft = kitCooldown.ExpireDate - DateTime.UtcNow;
            if (timeLeft.TotalSeconds > 0)
            {
                return true;
            }
            return false;
        }

        public void RegisterCooldown(string playerId, Kit kit)
        {
            var kitCooldown = new KitCooldown()
            {
                KitName = kit.Name,
                ExpireDate = DateTime.UtcNow.AddSeconds(kit.Cooldown)
            };

            if (!PlayersKitCooldowns.ContainsKey(playerId))
                PlayersKitCooldowns[playerId] = new List<KitCooldown>();

            PlayersKitCooldowns[playerId].Add(kitCooldown);
            GlobalCooldowns[playerId] = DateTime.UtcNow.AddSeconds(Configuration.Instance.GlobalCooldown);
        }

        public bool HasGlobalCooldown(string playerId, out TimeSpan timeLeft)
        {
            timeLeft = TimeSpan.Zero;
            if (!GlobalCooldowns.ContainsKey(playerId))
            {
                return false;
            }

            timeLeft = GlobalCooldowns[playerId] - DateTime.UtcNow;
            if (timeLeft.TotalSeconds > 0)
            {
                return true;
            }

            return false;
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
