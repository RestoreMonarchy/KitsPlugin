using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitsPlugin.Models;

namespace KitsPlugin
{
    public class KitsPluginConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public string LoadMessage { get; set; }
        public Kit[] Kits { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            LoadMessage = "This is kits plugin!";
            Kits = new Kit[]
            {
                new Kit()
                {
                    Name = "default",
                    Items = new ushort[]
                    {
                        363,
                        6,
                        6
                    }
                }
            };
        }
    }
}
