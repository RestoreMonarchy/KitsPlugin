using KitsPlugin.Database;
using KitsPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KitsPlugin.Services
{
    public class CooldownsService : MonoBehaviour
    {
        public Dictionary<string, List<KitCooldown>> PlayersKitCooldowns { get; private set; }
        public Dictionary<string, DateTime> GlobalCooldowns { get; private set; }

        private KitCooldownsDatabase database => KitsPlugin.Instance.KitCooldownsDatabase;

        void Awake()
        {
            PlayersKitCooldowns = new Dictionary<string, List<KitCooldown>>();
            GlobalCooldowns = new Dictionary<string, DateTime>();
        }

        void Start()
        {
            
        }

        void OnDestroy()
        {

        }

        public bool HasCooldown(string playerId, string kitName, out TimeSpan timeLeft)
        {
            timeLeft = TimeSpan.Zero;

            var activeCooldown = database.Data.FirstOrDefault(x => x.KitName.Equals(kitName, StringComparison.OrdinalIgnoreCase)
                && x.PlayerId == playerId && x.ExpireDate > DateTime.UtcNow);

            if (activeCooldown == null)
            {
                return false;
            }

            timeLeft = activeCooldown.ExpireDate - DateTime.UtcNow;
            return true;
        }

        public void RegisterCooldown(string playerId, Kit kit)
        {
            var kitCooldown = new KitCooldown()
            {
                PlayerId = playerId,
                KitName = kit.Name,
                ExpireDate = DateTime.UtcNow.AddSeconds(kit.Cooldown)
            };

            database.AddKitCooldown(kitCooldown);
            GlobalCooldowns[playerId] = DateTime.UtcNow.AddSeconds(KitsPlugin.Instance.Configuration.Instance.GlobalCooldown);
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
    }
}
