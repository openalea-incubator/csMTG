using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    public class mtg : PropertyTree
    {

        #region Attributes

        // Map a vertex to its scale.
        public Dictionary<int, int> scale;

        // Equivalent of parent. Assigns to each vertex a vertex on scale-1.
        public Dictionary<int, int> complex;

        // Equivalent of children. Assigns to each vertex a list of vertices on a lower scale (scale+1).
        public Dictionary<int, List<int>> components;

        #endregion

        /// <summary>
        /// Constructor of the MTG.
        /// </summary>
        public mtg()
        {
            // Initialize the attributes

            scale = new Dictionary<int, int>() { { 0, 0 } };
            complex = new Dictionary<int, int>() { };
            components = new Dictionary<int, List<int>>() { };

            // Add default properties

            AddProperty("Edge_Type");
            AddProperty("label");

        }

    }
}
