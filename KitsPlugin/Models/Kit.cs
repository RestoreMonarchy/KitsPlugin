using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KitsPlugin.Models
{
    public class Kit
    {
        public string Name { get; set; }

        [XmlArrayItem("itemId")]
        public ushort[] Items { get; set; }
    }
}
