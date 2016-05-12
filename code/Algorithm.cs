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
        public PropertyTree RandomTree(PropertyTree t, int nbVertices, int nbChildren = 4)
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

    }
}
