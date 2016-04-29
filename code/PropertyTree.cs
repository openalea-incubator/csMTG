using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{

    public class PropertyTree : Tree
    {
        // Attribute which carries all properties of the tree
        public Dictionary<string, Dictionary<int, dynamic>> properties;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyTree()
        {
            properties = new Dictionary<string, Dictionary<int, dynamic>>();
        }

        /// <summary>
        /// Lists all the names of the properties in the dictionary.
        /// </summary>
        /// <returns> A list of the properties' names. </returns>
        public List<string> PropertyNames()
        {
            List<string> names = new List<string>(properties.Keys);

            return names;
        }
        

        static void Main(String[] args)
        {
            

        }
    }
}