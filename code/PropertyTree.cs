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

    }
}