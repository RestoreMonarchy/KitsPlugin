using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitsPlugin.Commands
{
    public class KitCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kit";

        public string Help => "";

        public string Syntax => "<name>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, KitsPlugin.Instance.Translate("KitInvalid"), KitsPlugin.Instance.MessageColor);
                return;
            }

            var kit = KitsPlugin.Instance.Configuration.Instance.Kits.FirstOrDefault(x => x.Name == command[0]);

            if (kit == null)
            {
                UnturnedChat.Say(caller, KitsPlugin.Instance.Translate("KitNotFound"), KitsPlugin.Instance.MessageColor);
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (!player.HasPermission($"kit.{kit.Name}"))
            {
                UnturnedChat.Say(caller, KitsPlugin.Instance.Translate("KitNoPermission"), KitsPlugin.Instance.MessageColor);
                return;
            }

            TimeSpan timeLeft;
            if (KitsPlugin.Instance.CooldownsService.HasGlobalCooldown(player.Id, out timeLeft))
            {
                UnturnedChat.Say(caller, KitsPlugin.Instance.Translate("KitGlobalCooldown", (int)timeLeft.TotalSeconds), KitsPlugin.Instance.MessageColor);
                return;
            }

            if (KitsPlugin.Instance.CooldownsService.HasCooldown(player.Id, kit.Name, out timeLeft))
            {
                UnturnedChat.Say(caller, KitsPlugin.Instance.Translate("KitCooldown", (int)timeLeft.TotalSeconds), KitsPlugin.Instance.MessageColor);
                return;
            }

            foreach (var item in kit.Items)
            {
                player.GiveItem(item, 1);
            }

            KitsPlugin.Instance.CooldownsService.RegisterCooldown(player.Id, kit);
            UnturnedChat.Say(caller, KitsPlugin.Instance.Translate("KitSuccess", kit.Name), KitsPlugin.Instance.MessageColor);
        }
    }
}
