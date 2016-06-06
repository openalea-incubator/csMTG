using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    public class Algorithm
    {

        /// <summary>
        ///  Generates a random tree with a specified number of vertices and children per vertex.
        ///  The generated tree is added to the root in the parameters.
        /// </summary>
        /// <param name="nbVertices"> The number of tree's vertices </param>
        /// <param name="nbChildren"> The maximum number for a vertex </param>
        /// <returns> Returns the generated tree </returns>
        public int RandomTree(mtg t, int root, int nbVertices, int nbChildren = 4)
        {
            int childrenToAdd;

            List<int> randomStack = new List<int>();
            Random r = new Random();
            Random r2 = new Random();
            randomStack.Add(root);

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

            return randomStack.Last();
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

        #region RandomMtg

        /// <summary>
        /// Compute missing edges on an incomplete MTG at a given scale.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="scale"></param>
        /// <param name="edgeTypeProperty"></param>
        /// <returns> Returns true. </returns>
        bool ComputeMissingEdges(mtg tree, int scale, Dictionary<int,dynamic> edgeTypeProperty = null)
        {
            List<int> roots = tree.Roots(scale);

            foreach (int vid in roots)
            {
                List<int> components = tree.Components(vid);
                if (components == null || components == new List<int>() { })
                {
                    Console.WriteLine("Error ! Missing component for vertex " + vid);
                    continue;
                }
                else
                {
                    int componentId = components[0];

                    if (tree.Parent(componentId) == null)
                        continue;

                    int? parentId = tree.Complex((int)tree.Parent(componentId));

                    if (parentId == null)
                    {
                        Console.WriteLine("Error ! Missing parent for vertex" + vid);
                        continue;
                    }

                    if (edgeTypeProperty != null)
                    {
                        try
                        {
                            string edgeType = edgeTypeProperty[componentId];
                            tree.AddChild((int)parentId, new Dictionary<string, dynamic>() { { "Edge_Type", edgeType } }, vid);
                        }
                        catch (KeyNotFoundException)
                        {
                            tree.AddChild((int)parentId, vid);
                        }
                    }
                    else
                        tree.AddChild((int)parentId, vid);
                }
            }
            return true;
        }

        #region Ancestors

        void LookForCommonAncestor(mtg tree, List<int> commonAncestors, int currentNode)
        {
            while (currentNode != -1)
            {
                for (int i = 0; i < commonAncestors.Count; i++)
                {
                    int node = commonAncestors[i];

                    if (node == currentNode)
                    {
                        for (int j = 0; j < i; j++)
                            commonAncestors.RemoveAt(0);
                        return;
                    }
                }
                
                currentNode = (int)tree.Parent(currentNode);
            }
        }

        /// <summary>
        /// Return the vertices from the vertex in the parameters up to the root.
        /// </summary>
        /// <param name="g"> The MTG. </param>
        /// <param name="vertexId"> The vertex identifier. </param>
        /// <returns> An iterator on the ancestors of the vertexId up to the root. </returns>
        public IEnumerable<int> FullAncestors(mtg g, int vertexId, string restrictedTo = "NoRestriction", string edgeType = "*", int containedIn = -1)
        {
            Dictionary<int, dynamic> edgeT = g.Property("Edge_Type");
            int cScale;
            int v = vertexId;

            while (v != -1)
            {
                if (edgeType != "*" && edgeT.ContainsKey(v))
                {
                    if (edgeT[v] != edgeType)
                        yield break;
                }

                if (restrictedTo == "SameComplex")
                {
                    if (g.Complex(v) != g.Complex(vertexId))
                        yield break;
                }
                else
                {
                    if (restrictedTo == "SameAxis")
                    {
                        if (edgeT.ContainsKey(v))
                        {
                            if (edgeT[v] == "+")
                                yield break;
                        }
                    }
                }

                if (containedIn != -1)
                {
                    cScale = (int)g.Scale((int)containedIn);

                    if (g.ComplexAtScale(v, cScale) != containedIn)
                        yield break;
                }

                yield return v;

                v = (int)g.Parent(v);
            }
        }

        #endregion

        /// <summary>
        /// Compute missing edges at each scale of the slimMtg. It is based
        /// on the explicit edges that are defined at finer scales and
        /// decomposition relantionships.
        /// </summary>
        /// <param name="slimMtg"></param>
        /// <param name="preserveOrder"> If true, the order of the children at the coarsest scales
        /// is deduced from the order of children at finest scale. </param>
        /// <returns> Computed tree. </returns>
        public mtg FatMtg(mtg slimMtg, bool preserveOrder = false)
        {
            int maxScale = slimMtg.MaxScale();

            Dictionary<int, dynamic> edgeTypeProperty = slimMtg.Property("Edge_Type");

            for (int scale = maxScale - 1; scale > 0; scale--)
            {
                ComputeMissingEdges(slimMtg, scale, edgeTypeProperty);

                if (preserveOrder)
                {
                    foreach (int v in slimMtg.Vertices(scale))
                    {
                        List<int> cref = slimMtg.Children(v);

                        if (cref.Count > 1)
                        {
                            List<int> cmp = new List<int>();
                            foreach (int x in cref)
                                cmp.Add(slimMtg.ComponentRoots(x)[0]);
                           
                            Dictionary<int, int> cmpDic = cmp.Zip(cref, (K,V) => new {Key = K, Value = V}).ToDictionary(x => x.Key , x => x.Value);

                            traversal t = new traversal();

                            List<int> descendants = t.IterativePostOrder(slimMtg, slimMtg.ComponentRoots(v)[0]).ToList();

                            List<int> orderedChildren = new List<int>();

                            foreach (int x in descendants)
                            {
                                if (cmp.Contains(x))
                                    orderedChildren.Add(x);
                            }

                            List<int> ch = new List<int>();

                            foreach (int c in orderedChildren)
                            {
                                if(cmpDic.ContainsKey(c))
                                    ch.Add(cmpDic[c]);
                            }

                            if (slimMtg.children.ContainsKey(v))
                                slimMtg.children[v] = ch;
                            else
                                slimMtg.children.Add(v, ch);

                        }

                    }
                }
            }

            return slimMtg;
        }

        /// <summary>
        /// Compute an mtg from a tree and the list of vertices to be quotiented.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="colors"></param>
        /// <returns></returns>
        public List<dynamic> ColoredTree(mtg tree, Dictionary<int, List<int>> colors)
        {
            int nbScales = colors.Keys.Max() + 1;

            Dictionary<int, Dictionary<int, int>> mapIndex = new Dictionary<int, Dictionary<int, int>>() { };

            mtg g = new mtg();

            // Scale 0 : 1 vertex

            int count = 1;

            for (int scale = 1; scale < nbScales; scale++)
            {
                mapIndex.Add(scale, new Dictionary<int, int>());

                foreach (int id in colors[scale])
                {
                    mapIndex[scale].Add(id, count);
                    count++;
                }
            }

            // Build the MTG
            // 1 - Add multiscale info

            Dictionary<int, int> indexScale = mapIndex[1];

            foreach (int id in colors[1])
                g.AddComponent(g.root, componentId: indexScale[id]);

            // 2 - Edit the graph with multiscale info

            for (int scale = 2; scale < nbScales; scale++)
            {
                Dictionary<int, int> previousIndexScale = indexScale;
                indexScale = mapIndex[scale];

                foreach (int id in colors[scale])
                {
                    int complexId = previousIndexScale[id];
                    int componentId = indexScale[id];

                    if (complexId != -1)
                    {
                        g.AddComponent(complexId, componentId: componentId);
                    }
                    else
                    {
                        if (componentId != -1)
                        {
                            if(g.scale.ContainsKey(componentId))
                                g.scale[componentId] = scale;
                            else
                                g.scale.Add(componentId, scale);
                        }
                            
                    }

                }
            }

            // 3 - Copy the tree information in the MTG

            if (tree is mtg)
            {
                int maxScale = tree.MaxScale();

                foreach (int vertexId in tree.parent.Keys)
                {
                    int parent = tree.parent[vertexId];

                    if (parent != -1 && tree.Scale(parent) == maxScale)
                        g.parent.Add(indexScale[vertexId], indexScale[parent]);
                }

                foreach (int parent in tree.children.Keys)
                {
                    if (tree.Scale(parent) == maxScale)
                    {
                        List<int> childrenToAdd = new List<int>();

                        foreach (int id in tree.children[parent])
                        {
                            childrenToAdd.Add(indexScale[id]);
                        }

                        if (g.children.ContainsKey(indexScale[parent]))
                            g.children[indexScale[parent]] = childrenToAdd;
                        else
                            g.children.Add(indexScale[parent],childrenToAdd);

                    }
                }

            }
            else
            {
                foreach (int vertexId in tree.parent.Keys)
                {
                    int parent = tree.parent[vertexId];

                    g.parent.Add(indexScale[vertexId], indexScale[parent]);
                }

                foreach (int parent in tree.children.Keys)
                {
                    List<int> childrenToAdd = new List<int>();

                    foreach (int id in tree.children[parent])
                    {
                        childrenToAdd.Add(indexScale[id]);
                    }

                    if (g.children.ContainsKey(indexScale[parent]))
                        g.children[indexScale[parent]] = childrenToAdd;
                    else
                        g.children.Add(indexScale[parent], childrenToAdd);
                }
            }

            // 4- Copy the properties of the tree

            foreach (string propertyName in tree.Properties().Keys)
            {

                if (!g.properties.ContainsKey(propertyName))
                {
                    g.properties.Add(propertyName, new Dictionary<int, dynamic>());
                }

                Dictionary<int, dynamic> props = tree.properties[propertyName];

                int maxScale = tree.MaxScale();

                foreach (int id in props.Keys)
                {
                    if (tree.Scale(id) == maxScale)
                        g.properties[propertyName].Add(indexScale[id], props[id]);
                }

            }

            List<dynamic> returnedValue = new List<dynamic>();

            returnedValue[0] = FatMtg(g);

            returnedValue[1] = indexScale.Values.Zip(indexScale.Keys, (K, V) => new { Key = K, Value = V }).ToDictionary(x => x.Key, x => x.Value);

            return returnedValue;
        }

        /// <summary>
        /// Convert a tree into an MTG of NbScales.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="nbScales"></param>
        /// <returns></returns>
        public mtg RandomMtg(mtg tree, int nbScales)
        {
            int n = tree.NbVertices();
            Random r = new Random();
            Dictionary<int, List<int>> colors = new Dictionary<int, List<int>>() { };

            colors.Add(nbScales - 1, tree.Vertices());

            for (int s = nbScales - 2; s > 0; s--)
            {
                n = r.Next(1, n);

                var sample = (IEnumerable<int>)colors[s + 1];

                sample = sample.OrderBy(x => r.Next()).Take(n);

                List<int> l = sample.ToList();

                l.Sort();

                if (!l.Contains(tree.root))
                    l.Insert(0, tree.root);

                colors.Add(s, l);
            }

            return ColoredTree(tree, colors)[0];

        }

        #endregion

        #region Displays (Tree & MTG)

        /// <summary>
        /// Display the tree structure.
        /// </summary>
        /// <returns> A string that represents the tree structure. </returns>
        public IEnumerable<string> DisplayTree(mtg tree, int vertexId, string tab = "", Dictionary<int, dynamic> labels = null, Dictionary<int, dynamic> edgeType = null)
        {
            if (labels == null)
                labels = tree.Property("label");
            if (edgeType == null)
                edgeType = tree.Property("Edge_Type");

            string edgeT;

            if (edgeType.ContainsKey(vertexId))
                edgeT = edgeType[vertexId];
            else
                edgeT = "/";

            string label;

            if(labels.ContainsKey(vertexId))
                label = labels[vertexId];
            else
                label = vertexId.ToString();

            yield return tab + edgeT + label;

            foreach (int child in tree.Children(vertexId))
            {
                if (edgeType.ContainsKey(child))
                {
                    if (edgeType[child] == "+")
                        tab += "\t";

                    foreach (string s in DisplayTree(tree, child, tab, labels, edgeType))
                    {
                        yield return s;
                    }

                    if (edgeType[child] == "+")
                        tab.Remove(tab.Length - 1);
                }
            }

        }

        /// <summary>
        /// Display an MTG.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="vertexId"></param>
        /// <returns></returns>
        public IEnumerable<string> DisplayMtg(mtg tree, int vertexId)
        {
            Dictionary<int, dynamic> label = tree.Property("label");
            Dictionary<int, dynamic> edgeType = tree.Property("Edge_Type");

            traversal t = new traversal();
            string edgeT;

            int currentVertex = vertexId;

            int tab = 0;

            foreach (int vertex in t.MtgIterator(tree, vertexId))
            {
                edgeT = "/";
                if (vertex != currentVertex)
                {
                    int scale1 = (int)tree.Scale(currentVertex);
                    int scale2 = (int)tree.Scale(vertex);

                    if (scale1 >= scale2)
                    {
                        try
                        {
                            edgeT = edgeType[vertex];
                        }
                        catch (KeyNotFoundException)
                        {

                        }

                        if (scale1 == scale2)
                        {
                            if (tree.Parent(vertex) != currentVertex)
                            {
                                tab = -1;
                                edgeT = "^" + edgeT;
                            }
                            else
                                edgeT = "^" + edgeT;
                        }
                        else
                        {
                            if (scale1 > scale2)
                            {
                                int v = currentVertex;

                                for (int i = 0; i < scale1 - scale2; i++)
                                {
                                    v = (int)tree.Complex(v);
                                }
                                if (tree.Parent(vertex) == v)
                                    edgeT = "^" + edgeT;
                                else
                                {
                                    tab -= 1;
                                    edgeT = "^" + edgeT;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Assert(scale2 - scale1 == 1);
                        tab += 1;
                    }

                    string tabs ="";

                    for (int i = 0; i < tab; i++)
                        tabs = tabs + "\t";

                    string labelVertex;

                    if (label.ContainsKey(vertex))
                        labelVertex = label[vertex];
                    else
                        labelVertex = vertex.ToString();

                    yield return tabs + edgeT + labelVertex;
                }

                currentVertex = vertex;
            }

        }


        #endregion

    }
}
