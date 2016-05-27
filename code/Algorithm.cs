using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    public class Algorithm
    {

        /// <summary>
        ///  Generates a random tree with a specified number of vertices and children per vertex
        /// </summary>
        /// <param name="nbVertices"> The number of tree's vertices </param>
        /// <param name="nbChildren"> The maximum number for a vertex </param>
        /// <returns> Returns the generated tree </returns>
        public mtg RandomTree(mtg t, int nbVertices, int nbChildren = 4)
        {
            int childrenToAdd;

            List<int> randomStack = new List<int>();
            Random r = new Random();
            Random r2 = new Random();
            randomStack.Add(t.root);
            nbVertices--;

            while (nbVertices > 0)
            {
                // Set a random number of children to add within the specified range.

                int randomInt = r.Next(1, nbChildren);
                childrenToAdd = Math.Min(randomInt, nbVertices);

                // Choose a random vertex among those in the stack of potential parents

                int randomNumber = r2.Next(0, randomStack.Count() - 1);
                int randomVertex = randomStack[randomNumber];

                // Add the specified number of children to the random parent
                for (int i = 0; i < childrenToAdd; ++i)
                {
                    int newChild;

                    if (i == childrenToAdd % 2)
                        newChild = t.AddChild(randomVertex, new Dictionary<string, dynamic>() { { "Edge_Type", "<" } });
                    else
                        newChild = t.AddChild(randomVertex, new Dictionary<string, dynamic>() { { "Edge_Type", "+" } });

                    randomStack.Add(newChild);
                    nbVertices--;
                    

                }

                randomStack.Remove(randomVertex);

            }

            return t;
        }

        /// <summary>
        /// Generate and add a regular tree to an existing one at a given vertex.
        /// </summary>
        /// <param name="tree"> The tree that will be modified. </param>
        /// <param name="vertexId"> Vertex on which the subtree will be added. </param>
        /// <param name="NbChildren"> Number of children per parent. </param>
        /// <param name="NbVertices"> Number of vertices of the new tree. </param>
        /// <returns> The tree after being modified. </returns>
        public mtg SimpleTree(mtg tree, int vertexId, int NbChildren = 3, int NbVertices = 20)
        {
            int vid = vertexId;
            List<int> l = new List<int>() { vid };

            while (NbVertices > 0)
            {
                int n = Math.Min(NbChildren, NbVertices);
                vid = l[0];
                l.RemoveAt(0);

                for (int i = 0; i < n; i++)
                {
                    int v = tree.AddChild(vid);
                    NbVertices--;
                    l.Add(v);
                }

            }

            return tree;
        }

    }
}
