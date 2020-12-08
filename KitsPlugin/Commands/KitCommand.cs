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
                UnturnedChat.Say(caller, "You must specify kit name!");
                return;
            }

            var kit = KitsPlugin.Instance.Configuration.Instance.Kits.FirstOrDefault(x => x.Name == command[0]);

            if (kit == null)
            {
                UnturnedChat.Say(caller, "Kit not found");
                return;
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;

            foreach (var item in kit.Items)
            {
                player.GiveItem(item, 1);
            }

            UnturnedChat.Say(caller, $"You received kit {kit.Name}!");
        }
    }
}
