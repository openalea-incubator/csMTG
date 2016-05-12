using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    public class traversal : PropertyTree
    {
        #region Preorder

        /// <summary>
        /// Iteratively traverse the tree in preorder starting from a vertex.
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> IterativePreOrder(PropertyTree tree, int vertexId)
        {
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");

            Stack<int> stack = new Stack<int>();
            stack.Push(vertexId);

            while (stack.Count != 0)
            {
                List<int> plus = new List<int>();
                List<int> successor = new List<int>();

                vertexId = stack.Pop();
                yield return vertexId;

                foreach (int vid in tree.Children(vertexId))
                {
                    if (edgeType[vid].Equals("<"))
                        successor.Add(vid);
                    else
                        plus.Add(vid);
                }

                plus.AddRange(successor);

                List<int> child = plus;

                child.Reverse();

                child.ForEach(o => stack.Push(o));
            }
        }

        /// <summary>
        /// Recursively traverse the tree in preorder starting from a vertex.
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
                if (edgeType[vid].Equals("<"))
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

        #endregion

        #region PostOrder

        /// <summary>
        /// Iteratively traverse the tree in postorder starting from a vertex.
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> IterativePostOrder(PropertyTree tree, int vertexId)
        {
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");
            
            // Internal function
            Func<int, List<int>> OrderChildren = new Func<int, List<int>>(vid =>
            {
                List<int> plus = new List<int>();
                List<int> successor = new List<int>();

                foreach(int v in tree.Children(vid))
                {
                    if (edgeType[v].Equals("<"))
                        successor.Add(v);
                    else
                        plus.Add(v);
                }

                plus.AddRange(successor);
                List<int> child = plus;

                return child;
            }
            );

            List<int> visited = new List<int>();

            Stack<int> stack = new Stack<int>();
            stack.Push(vertexId);

            while(stack.Count != 0)
            {
                vertexId = stack.Peek();

                List<int> listOfChildren = OrderChildren(vertexId);

                if (listOfChildren.Count != 0 && (listOfChildren.Intersect(visited).Count() != listOfChildren.Count()) )
                {
                    foreach (int vid in listOfChildren)
                    {
                        if (!visited.Contains(vid))
                        {
                            stack.Push(vid);
                            break;
                        }
                    }
                }
                else
                    {
                        visited.Add(vertexId);
                        stack.Pop();
                        yield return vertexId;
                    }
            }

        }

        /// <summary>
        /// Recursively traverse the tree in postorder starting from a vertex.
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> RecursivePostOrder(PropertyTree tree, int vertexId)
        {
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");

            List<int> successor = new List<int>();

            foreach (int v in tree.Children(vertexId))
            {
                if (edgeType[v].Equals("<"))
                {
                    successor.Add(v);
                    continue;
                }

                foreach (int node in RecursivePostOrder(tree, v))
                {
                    yield return node;
                }
            }

            foreach (int vid in successor)
            {
                foreach (int node in RecursivePostOrder(tree, vid))
                    yield return node;
            }

            yield return vertexId;
            
        }
        
        #endregion

        static void Main(String[] args)
        {
            PropertyTree tree = new PropertyTree();

            Dictionary<string, dynamic> edges1 = new Dictionary<string, dynamic>() { { "Edge_Type", "<" } };
            Dictionary<string, dynamic> edges2 = new Dictionary<string, dynamic>() { { "Edge_Type", "+" } };

            tree.AddChild(0, edges1);
            //tree.AddChild(0, edges2);
            tree.AddChild(0, edges1);

            //tree.AddChild(1, edges2);
            tree.AddChild(1, edges1);
            tree.AddChild(1, edges1);

            tree.AddChild(4, edges1);
            //tree.AddChild(4, edges2);
            tree.AddChild(4, edges1);

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

            System.Diagnostics.Debug.Assert(Enumerable.SequenceEqual<int>(t.IterativePreOrder(tree, 0), t.RecursivePreOrder(tree, 0)));

            Console.WriteLine("PostOrder Recursive results are : ");

            foreach (int recursive in t.RecursivePostOrder(tree, 0))
                Console.WriteLine(recursive);

            Console.WriteLine("PostOrder Iterative results are : ");

            foreach (int iterative in t.IterativePostOrder(tree, 0))
                Console.WriteLine(iterative);

            System.Diagnostics.Debug.Assert(Enumerable.SequenceEqual<int>(t.IterativePostOrder(tree, 0), t.RecursivePostOrder(tree, 0)));

        }

    }
}
