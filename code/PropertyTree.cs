using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{

    class PropertyTree : Tree
    {

        Dictionary<string, Dictionary<int, dynamic>> properties;

        PropertyTree()
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
            PropertyTree prop = new PropertyTree();

            prop.properties.Add("label", null);
            prop.properties.Add("height", null);

            prop.PropertyNames().ForEach(Console.WriteLine);
        }
    }
}