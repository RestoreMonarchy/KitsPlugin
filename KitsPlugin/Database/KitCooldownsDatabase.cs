using KitsPlugin.Models;
using KitsPlugin.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitsPlugin.Database
{
    public class KitCooldownsDatabase
    {
        private DataStorage<List<KitCooldown>> DataStorage { get; set; }

        public List<KitCooldown> Data { get; private set; }

        public KitCooldownsDatabase()
        {
            DataStorage = new DataStorage<List<KitCooldown>>(KitsPlugin.Instance.Directory, "KitCooldowns.json");
        }

        public void Reload()
        {
            Data = DataStorage.Read();
            if (Data == null)
            {
                Data = new List<KitCooldown>();
                DataStorage.Save(Data);
            }
        }

        public void AddKitCooldown(KitCooldown cooldown)
        {
            Data.Add(cooldown);
            DataStorage.Save(Data);
        }
    }
}
