using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Serialization;

namespace TFYP.Resources.JSON
{
    public class NameList
    {
        /// <summary>
        /// Class for holding the lists of names from names.json
        /// </summary>
        public string[] boys { get; set; }
        public string[] girls { get; set; }
        public string[] last { get; set; }
    }
}
