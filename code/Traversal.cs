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
        /// (Equivalent of pre_order2 in Python file)
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> IterativePreOrder(mtg tree, int vertexId, int complex = -1)
        {

            if (complex != -1 && tree.Complex(vertexId) != complex)
                yield break;

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
                    if (complex != -1 && tree.Complex(vid) != complex)
                        continue;

                    if (!edgeType.ContainsKey(vid))
                        successor.Add(vid);
                    else
                    {
                        if (edgeType[vid].Equals("<"))
                            successor.Add(vid);
                        else
                            plus.Add(vid);

                    }
                }

                plus.AddRange(successor);

                List<int> child = plus;

                child.Reverse();

                child.ForEach(o => stack.Push(o));
            }
        }

        /// <summary>
        /// Recursively traverse the tree in preorder starting from a vertex.
        /// (Equivalent of pre_order in Python file)
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> RecursivePreOrder(mtg tree, int vertexId, int complex = -1)
        {
            if (complex != -1 && tree.Complex(vertexId) != complex)
                yield break;

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

                foreach (int node in RecursivePreOrder(tree, vid, complex))
                {
                    yield return node;
                }
            }

            foreach (int vid in successor)
            {
                foreach (int node in RecursivePreOrder(tree, vid, complex))
                    yield return node;
            }

        }

        #endregion

        #region PostOrder

        /// <summary>
        /// Iteratively traverse the tree in postorder starting from a vertex.
        /// (Equivalent of post_order2 in Python file)
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> IterativePostOrder(mtg tree, int vertexId, int complex = -1)
        {
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");
            Dictionary<int, dynamic> emptyDictionary = new Dictionary<int, dynamic>();

            // Internal function
            Func<int, List<int>> OrderChildren = new Func<int, List<int>>(vid =>
            {
                List<int> plus = new List<int>();
                List<int> successor = new List<int>();

                foreach(int v in tree.Children(vid))
                {
                    if (complex != -1 && tree.Complex(v) != complex)
                        continue;

                    if (!edgeType.ContainsKey(v))
                        successor.Add(v);
                    else
                    {
                        if (edgeType[v].Equals("<"))
                            successor.Add(v);
                        else
                            plus.Add(v);
                    }

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
        /// (Equivalent of function post_order in Python file).
        /// </summary>
        /// <param name="tree"> The tree to be traversed. </param>
        /// <param name="vertexId"> The identifier of the starting vertex. </param>
        /// <returns> Returns an iterator. </returns>
        public IEnumerable<int> RecursivePostOrder(mtg tree, int vertexId, int complex = -1)
        {
            if (complex != -1 && tree.Complex(vertexId) != complex)
                yield break;

            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");

            List<int> successor = new List<int>();

            foreach (int v in tree.Children(vertexId))
            {
                if (edgeType[v].Equals("<"))
                {
                    successor.Add(v);
                    continue;
                }

                foreach (int node in RecursivePostOrder(tree, v, complex))
                {
                    yield return node;
                }
            }

            foreach (int vid in successor)
            {
                foreach (int node in RecursivePostOrder(tree, vid, complex))
                    yield return node;
            }

            yield return vertexId;
            
        }
        
        #endregion

        #region Save to file

        /// <summary>
        /// Saves the PropertyTree into a file (.tlp)
        /// </summary>
        /// <param name="tree"> The property tree to save. </param>
        /// <param name="path"> Contains the path and the name of the file (Do not mention the extension). </param>
        public void SaveToFile(PropertyTree tree, string path)
        {
            
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@path+".tlp"))
            {
                // First line
                file.WriteLine("(tlp \"2.0\"");

                // List of all nodes
                file.Write("(nodes");
                foreach (int node in tree.parent.Keys)
                {
                    file.Write(" " + node);
                }
                file.WriteLine(")");

                // List of all edges
                int countEdges = 0;

                foreach (int edge in tree.parent.Keys)
                {
                    if (edge != 0)
                    {
                        countEdges++;

                        file.WriteLine("(edge " + countEdges + " " + (int)tree.Parent(edge) + " " + edge + ")");
                    }
                }

                // List of all clusters
                int clusterId = 1;

                foreach (string name in tree.PropertyNames())
                {
                    file.WriteLine("(cluster " + clusterId + " " + name);

                    file.Write("(nodes");
                    foreach (int node in tree.Property(name).Keys)
                    {
                        file.Write(" " + node);
                    }
                    file.WriteLine("))");

                    clusterId++;
                }

                // List of all properties
                int propertyId = 1;

                foreach (string name in tree.PropertyNames())
                {
                    file.WriteLine("(property " + propertyId + " string \"" + name + "\"");

                    Dictionary<int, dynamic> property = tree.Property(name);

                    foreach (int id in property.Keys)
                    {
                        file.WriteLine("(node " + id + " " + property[id] + ")");
                    }

                    file.WriteLine(")");
                    propertyId++;
                }
            }
        }

        #endregion

        #region MTG iterators

        /// <summary>
        /// Iterate all components of the vertexId.
        /// </summary>
        /// <param name="tree"> The MTG. </param>
        /// <param name="vertexId"> The vertex from which the iteration begins. </param>
        /// <returns> Iterator on components of the MTG starting from a vertex. </returns>
        public IEnumerable<int> MtgIterator(mtg tree, int vertexId)
        {
            Dictionary<int, bool> visited = new Dictionary<int, bool>();
            visited.Add(vertexId, true);

            int complexId = vertexId;

            int maxScale = tree.MaxScale();

            yield return vertexId;

            foreach(int vertex in tree.ComponentRootsAtScale(complexId,maxScale))
            {
                foreach(int vid in IterativePreOrder(tree,vertex))
                {
                    foreach (int node in ScaleIterator(tree, vid, complexId, visited))
                        yield return node;
                }
            }

        }

        /// <summary>
        /// Internal method used by MtgIterator.
        /// </summary>
        IEnumerable<int> ScaleIterator(mtg tree, int vertexId, int complexId, Dictionary<int, bool> visited)
        {
            if(vertexId != -1 && !visited.ContainsKey(vertexId) && ( tree.ComplexAtScale(vertexId,(int)tree.Scale(complexId)) == complexId))
            {
                foreach (int v in ScaleIterator(tree, (int)tree.Complex(vertexId), complexId, visited))
                {
                    yield return v;
                }

                visited.Add(vertexId, true);
                yield return vertexId;
            }
        }

        #endregion

    }

    public class Visitor
    {
        public void PreOrder(int vertexId)
        {

        }

        public void PostOrder(int vertexId)
        {

        }
    }
}
