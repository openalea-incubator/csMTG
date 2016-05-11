using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    class traversal : PropertyTree
    {
        /// <summary>
        /// Iteratively traverse the tree in preorder starting from a vertex.
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> IterativePreOrder(PropertyTree tree, int vertexId)
        {
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(vertexId);

            while (queue.Count != 0)
            {
                List<int> plus = new List<int>();
                List<int> successor = new List<int>();

                vertexId = queue.Dequeue();
                yield return vertexId;

                foreach (int vid in tree.Children(vertexId))
                {
                    if (edgeType[vid].Equals('<'))
                        successor.Add(vid);
                    else
                        plus.Add(vid);
                }

                plus.AddRange(successor);

                List<int> child = plus;

                child.Reverse();

                child.ForEach(o => queue.Enqueue(o));
            }
        }

        /// <summary>
        /// Traverse recursively the tree in preorder starting from a vertex.
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> RecursivePreOrder(PropertyTree tree, int vertexId)
        {
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");

            List<int> successor = new List<int>();
            yield return vertexId;

            foreach (int vid in tree.Children(vertexId))
            {
                if (edgeType[vid].Equals('<'))
                {
                    successor.Add(vid);
                    continue;
                }

                foreach (int node in RecursivePreOrder(tree, vid))
                {
                    yield return node;
                }
            }

            foreach (int vid in successor)
            {
                foreach (int node in RecursivePreOrder(tree, vid))
                    yield return node;
            }

        }


        static void Main(String[] args)
        {
            PropertyTree tree = new PropertyTree();

            Dictionary<string, dynamic> edges1 = new Dictionary<string, dynamic>() { { "Edge_Type", "<" } };
            Dictionary<string, dynamic> edges2 = new Dictionary<string, dynamic>() { { "Edge_Type", "+" } };

            tree.AddChild(0, edges1);
            tree.AddChild(0, edges2);

            tree.AddChild(1, edges2);
            tree.AddChild(1, edges1);

            tree.AddChild(4, edges1);
            tree.AddChild(4, edges2);

            tree.AddChild(2, edges1);
            tree.AddChild(2, edges1);

            tree.AddChild(7, edges1);

            tree.AddChild(8, edges1);

            traversal t = new traversal();

            Console.WriteLine("Iterative results are : ");

            foreach (int iter in t.IterativePreOrder(tree, 0))
                Console.WriteLine(iter);

            Console.WriteLine("Recursive results are : ");

            foreach (int recursive in t.RecursivePreOrder(tree, 0))
                Console.WriteLine(recursive);


        }

    }
}