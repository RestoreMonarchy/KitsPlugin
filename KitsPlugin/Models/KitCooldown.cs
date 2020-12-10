using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitsPlugin.Models
{
    public class KitCooldown
    {
        public string PlayerId { get; set; }
        public string KitName { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
