using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitsPlugin
{
    public class KitsPluginConfiguration : IRocketPluginConfiguration
    {
        public string LoadMessage { get; set; }

        public void LoadDefaults()
        {
            LoadMessage = "This is kits plugin!";
        }
    }
}
