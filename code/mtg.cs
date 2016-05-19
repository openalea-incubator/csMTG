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

        #region Querying scale information

        /// <summary>
        /// Returns a list of the different scales of the mtg.
        /// </summary>
        /// <returns> A distinct list of the mtg's scales. </returns>
        public List<int> Scales()
        {
            return scale.Values.Distinct().ToList<int>();
        }

        /// <summary>
        ///  Maps the scale corresponding to the vertex in the parameter.
        /// </summary>
        /// <param name="vertexId"> The vertex identifier. </param>
        /// <returns> The scale to which the vertex belongs. </returns>
        public int? Scale(int vertexId)
        {
            int? returnedScale;

            try
            {
                returnedScale = scale[vertexId];
            }
            catch (KeyNotFoundException)
            {
                returnedScale = null;
            }

            return returnedScale;

        }

        /// <summary>
        /// Counts the number of scales in the mtg.
        /// </summary>
        /// <returns> The number of scales. </returns>
        public int NbScales()
        {
            return Scales().Count;
        }

        /// <summary>
        /// Calculates the highest scale identifier.
        /// </summary>
        /// <returns> The maximum scale. </returns>
        public int MaxScale()
        {
            return scale.Values.Max();
        }

        #endregion

        #region Vertices

        /// <summary>
        /// The set of all vertices is returned.
        /// </summary>
        /// <param name="scale"> Optional. If specified, only vertices
        /// belonging to the scale will be listed. Otherwise, all vertices are listed.</param>
        /// <returns> An iterator of the MTG's vertices according to their scale. </returns>
        IEnumerable<int> VerticesIterator(int scale = -1)
        {
            if (scale < 0)
            {
                foreach (int vid in this.scale.Keys)
                {
                    yield return vid;
                }
            }
            else
            {
                foreach (int vid in this.scale.Keys)
                {
                    if (this.scale[vid] == scale)
                       yield return vid;
                }
            }
        }

        /// <summary>
        /// Returns a list of all vertices in a specific scale.
        /// </summary>
        /// <param name="scale"> Optional parameter. </param>
        /// <returns> A list of the previous function. </returns>
        public List<int> Vertices(int scale = -1)
        {
            return VerticesIterator(scale).ToList();
        }

        public int NbVertices(int scale = -1)
        {
            if(scale < 0)
                return this.scale.Keys.Count;
            else
                return Vertices(scale).Count;
        }



        #endregion

        static void Main(String[] args)
        {
           
        }
    }
}

