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

        #region Constructor

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

        #endregion

        #region Querying scale information (Functions: Scales, Scale, NbScales, MaxScale)

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

        #region Vertices (Functions: VerticesIterator, Vertices, NbVertices)

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

        /// <summary>
        /// Counts the number of vertices in a scale.
        /// </summary>
        /// <param name="scale"> Optional. If the scale isn't specified, the total number of vertices is counted. </param>
        /// <returns> The number of vertices in a scale. </returns>
        public int NbVertices(int scale = -1)
        {
            if(scale < 0)
                return this.scale.Keys.Count;
            else
                return Vertices(scale).Count;
        }

        #endregion

        #region Edges (Functions: HasVertex, IterEdges, Edges)

        /// <summary>
        /// Verifies that a vertex belongs to the MTG.
        /// </summary>
        /// <param name="vertexId"> The vertex identifier to verify. </param>
        /// <returns> A boolean. </returns>
        public new bool HasVertex(int vertexId)
        {
            return scale.ContainsKey(vertexId);
        }

        /// <summary>
        /// Iterates on the edges of the MTG at a given scale.
        /// </summary>
        /// <param name="scale"> Optional parameter. In case it's not specified, the function iterates on the whole MTG.
        /// If it is specified, it returns an iterator of {parent , child } where the parent belongs to the specified scale. </param>
        /// <returns> An iterator on the MTG's edges. </returns>
        IEnumerable<KeyValuePair<int, int>> IterEdges(int scale = -1)
        {
            
            if (scale < 0)
            {
                foreach (KeyValuePair<int, int> childParent in parent)
                {
                    if (childParent.Value >= 0)
                    {
                        KeyValuePair<int, int> edges = new KeyValuePair<int, int>(childParent.Value, childParent.Key);

                        yield return edges;

                    }
                }
            }
            else
            {
                foreach (KeyValuePair<int, int> childParent in parent)
                {
                    if (childParent.Value >= 0)
                    {
                        if (this.scale[childParent.Value] == scale)
                        {
                            KeyValuePair<int, int> edges = new KeyValuePair<int, int>(childParent.Value, childParent.Key);

                            yield return edges;
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// Returns a list of edges at a scale.
        /// </summary>
        /// <param name="scale"> Optional. If it's specified, it returns all parents which belong to the scale and their children.</param>
        /// <returns> A list of edges at a scale. </returns>
        public List<KeyValuePair<int, int>> Edges(int scale = -1)
        {
            return IterEdges(scale).ToList();
        }

        #endregion

        #region Roots (Functions: Roots)

        /// <summary>
        /// Returns an iterator of the roots of the tree graphs at a given scale.
        /// </summary>
        /// <param name="scale"> The specified scale (Optional). If not specified, all roots of the MTG will be listed. </param>
        /// <returns> iterator on vertex identifiers of root vertices at a given scale. </returns>
        IEnumerable<int> RootsIterator(int scale = 0)
        {
            foreach (int vertexId in Vertices(scale))
            {
                if (Parent(vertexId) == -1)
                    yield return vertexId;
            }
        }

        /// <summary>
        /// Returns a list of the roots of the tree graphs at a given scale.
        /// </summary>
        /// <param name="scale"> The specified scale (Optional). If not specified, all MTG's roots will be listed. </param>
        /// <returns> List on vertex identifiers of root vertices at a given scale. </returns>
        public List<int> Roots(int scale = 0)
        {
            return RootsIterator(scale).ToList();
        }

        #endregion

        #region Complex

        /// <summary>
        /// Gets the complex value for a key.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier.</param>
        /// <returns> Returns the vertex's complex. If it doesn't have a complex, it returns null. </returns>
        int? GetComplex(int vertexId)
        {
            if (complex.ContainsKey(vertexId))
                return complex[vertexId];
            else
                return null;
        }

        /// <summary>
        /// Returns the complex of a vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> The complex of the vertex or null. </returns>
        public int? Complex(int vertexId)
        {
            if (HasVertex(vertexId))
            {
                int? complexId = GetComplex(vertexId);

                while (complexId == null)
                {
                    vertexId = (int)Parent(vertexId);
                    if(vertexId == -1)
                        break;
                    complexId = GetComplex(vertexId);
                }

                return complexId;
            }
            else
                return null;
        }

        /// <summary>
        /// Returns the complex of a vertex at a specific scale.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="scale"> Scale. </param>
        /// <returns> The complex of a vertex at a scale. </returns>
        public int ComplexAtScale(int vertexId, int scale)
        {
            int complexId = vertexId;
            int? currentScale = Scale(complexId);

            if (currentScale != null)
            {
                for (int i = scale; i < currentScale; i++)
                {
                    complexId = (int)Complex(complexId);
                }
                return complexId;
            }
            else
                throw new ArgumentException("This vertex does not exist.");
        }

        #endregion

        #region Components (Functions: ComponentsRoots, Components, NbComponents, AddComponent, ComponentsAtScale, ComponentRootsAtScale)

        /// <summary>
        /// For the vertex in question, go over the tree graphs that compose it
        /// and return their roots.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> Iterator over the roots of the trees that compose the vertex. </returns>
        IEnumerable<int> ComponentRootsIter(int vertexId)
        {
            List<int> components = this.components[vertexId];

            foreach (int ci in components)
            {
                int p = (int)Parent(ci);
                if (p == -1 || Complex(p) != vertexId)
                    yield return ci;
            }

        }

        /// <summary>
        /// For the vertex in question, go over the tree graphs that compose it
        /// and return their roots.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> A list of roots of the trees that compose the vertex. </returns>
        public List<int> ComponentRoots(int vertexId)
        {
            return ComponentRootsIter(vertexId).ToList();
        }

        /// <summary>
        /// Iterate the components of a vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> Iterator over the components. </returns>
        IEnumerable<int> ComponentsIterator(int vertexId)
        {
            traversal t = new traversal();

            if (components.ContainsKey(vertexId))
            {
                foreach (int v in ComponentRootsIter(vertexId))
                {
                    foreach (int vertex in t.IterativePreOrder(this, v, vertexId))
                    {
                        yield return vertex;
                    }
                }
            }
            else
                yield break;
        }

        /// <summary>
        /// Lists the components of a vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> List of components. </returns>
        public List<int> Components(int vertexId)
        {
            return ComponentsIterator(vertexId).ToList();
        }

        /// <summary>
        /// Counts the number of components for a specific vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> Number of vertex's components. </returns>
        public int NbComponents(int vertexId)
        {
            if (components.ContainsKey(vertexId))
                return Components(vertexId).Count;
            else
                return 0;
        }

        /// <summary>
        /// Add a component at the end of the components.   
        /// </summary>
        /// <param name="complexId"> The complex to which the component will be added. </param>
        /// <param name="namesValues"> Dictionary of properties. </param>
        /// <param name="componentId"> Optional. If defined, we set the component identifier to this parameter. </param>
        /// <returns> The identifier of the new component. </returns>
        public int AddComponent(int complexId, Dictionary<string, dynamic> namesValues = null, int componentId = -1)
        {
            if (componentId == -1)
                componentId = NewId();

            SetARoot(componentId);

            if(namesValues != null)
                AddVertexProperties(componentId, namesValues);

            if (components.ContainsKey(complexId))
                components[complexId].Add(componentId);
            else
                components.Add(complexId, new List<int>() { componentId });

            complex.Add(componentId, complexId);
            scale.Add(componentId, scale[complexId] + 1);

            return componentId;
        }

        /// <summary>
        /// Iterate the components up to a scale.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier.</param>
        /// <param name="scale"> Scale. </param>
        /// <returns> Iterator. </returns>
        IEnumerable<int> ComponentsAtScaleIterator(int vertexId, int scale)
        {
            int currentScale = this.scale[vertexId];

            List<int> gen = new List<int>() { vertexId };

            yield return vertexId;

            for (int i = currentScale; i < scale; i++)
            {
                foreach (int vtx in gen)
                {
                    foreach (int vid in ComponentsIterator(vtx))
                    {
                        gen.Add(vid);
                        yield return vid;
                    }
                }
            }
        }

        /// <summary>
        /// Lists the components of the vertex in the parameters up to a specific scale.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier.</param>
        /// <param name="scale"> Scale. </param>
        /// <returns> List of components. </returns>
        public List<int> ComponentsAtScale(int vertexId, int scale)
        {
            return ComponentsAtScaleIterator(vertexId, scale).ToList();
        }

        /// <summary>
        /// Returns the set of roots of the tree graph that compose the vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="scale"> Scale. </param>
        /// <returns> Roots composing the vertex at a specific scale. </returns>
        IEnumerable<int> ComponentRootsAtScaleIterator(int vertexId, int scale)
        {
            int currentScale = (int)this.Scale(vertexId);

            if (scale == -1 || scale == currentScale + 1)
            {
                foreach (int root in ComponentRootsIter(vertexId))
                    yield return root;
            }
            else
            {
                if (scale > currentScale + 1)
                {
                    List<int> gen = new List<int>() { vertexId };
                    yield return vertexId;

                    for (int i = currentScale; i < scale; i++)
                    {
                        foreach (int vtx in gen)
                        {
                            foreach (int vid in ComponentRootsIter(vtx))
                            {
                                gen.Add(vid);
                                yield return vid;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of roots of the tree graph that compose the vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="scale"> Specified scale. </param>
        /// <returns> A list of the roots that compose the vertex. </returns>
        public List<int> ComponentRootsAtScale(int vertexId, int scale)
        {
            return ComponentRootsAtScaleIterator(vertexId, scale).ToList();
        }

        #endregion

        #region Functions related to adding vertices (InsertParent, AddChild, AddChildAndComplex, AddChildTree)

        /// <summary>
        /// Insert a parent between a vertex and its old parent.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="namesValues"> Properties of the new parent. </param>
        /// <param name="parentId"> The new parent's identifier. (Optional. If not specified, it will be added automatically.) </param>
        /// <returns> The new parent's identifier. </returns>
        public new int InsertParent(int vertexId, int parentId = -1, Dictionary<string, dynamic> namesValues = null)
        {
            if (parentId == -1)
                parentId = NewId();

            if (scale.ContainsKey(parentId))
                scale[parentId] = scale[vertexId];
            else
                scale.Add(parentId, scale[vertexId]);

            parentId = base.InsertParent(vertexId, parentId, namesValues);

            return parentId;
        }

        /// <summary>
        /// Add a child (And assign a scale to the new child).
        /// </summary>
        /// <param name="parentId"> Parent identifier. </param>
        /// <param name="namesValues"> Dictionary of properties. </param>
        /// <param name="childId"> New child identifier. (Optional) </param>
        /// <returns> The new child. </returns>
        public new int AddChild(int parentId, Dictionary<string, dynamic> namesValues = null, int childId = -1)
        {
            childId = base.AddChild(parentId, namesValues, childId);

            scale.Add(childId, scale[parentId]);

            return childId;
        }

        /// <summary>
        /// Add a child and a complex. The child will become a component of this complex.
        /// </summary>
        /// <param name="parentId"> Parent identifier. </param>
        /// <param name="childId"> New child identifier. </param>
        /// <param name="complexId"> New complex identifier. </param>
        /// <param name="namesValues"> Dictionary of properties. </param>
        /// <returns> A list which contains: { childId, complexId } </returns>
        public List<int> AddChildAndComplex(int parentId, int childId = -1, int complexId = -1, Dictionary<string, dynamic> namesValues = null)
        {
            List<int> childAndComplexIds = new List<int>();

            if (Children(parentId).Contains(childId))
            {
                if(namesValues != null)
                    AddVertexProperties(childId, namesValues);
            }
            else
                childId = AddChild(parentId, namesValues, childId);

            if (scale.ContainsKey(childId))
                scale[childId] = scale[parentId];
            else
                scale.Add(childId, scale[parentId]);

            int parentComplex = (int)Complex(parentId);

            if (complexId == -1)
                complexId = NewId();

            if (!Children(parentComplex).Contains(complexId))
                AddChild(parentComplex, complexId);

            if (scale.ContainsKey(complexId))
                scale[complexId] = scale[parentComplex];
            else
                scale.Add(complexId, scale[parentComplex]);

            if (components.ContainsKey(complexId))
                components[complexId].Add(childId);
            else
                components.Add(complexId, new List<int>(){childId});

            if (complex.ContainsKey(childId))
                complex[childId] = complexId;
            else
                complex.Add(childId, complexId);

            childAndComplexIds.Add(childId);
            childAndComplexIds.Add(complexId);

            return childAndComplexIds;
        }

        /// <summary>
        /// Add a tree as a child to the specified parent.
        /// </summary>
        /// <param name="parentId"> Parent identifier. </param>
        /// <param name="tree"> The tree to add. </param>
        /// <returns> A dictionary of the correspondance between ids in the tree and the new Ids once the tree is added. </returns>
        public Dictionary<int, int> AddChildTree(int parentId, mtg tree)
        {
            traversal t = new traversal();

            Dictionary<int, int> renumberedTree = new Dictionary<int, int>();
            int vId;

            int root = tree.root;

            int rootId = AddChild(parentId);
            renumberedTree.Add(root, rootId);

            // PreOrder traversal from root and renumbering all subtree vertices

            foreach (int vertexId in t.IterativePreOrder(tree, root))
            {
                if (vertexId == root)
                    continue;

                parentId = (int)Parent(vertexId);

                if (parentId != -1)
                    parentId = renumberedTree[(int)Parent(vertexId)];

                vId = AddChild(parentId);

                renumberedTree.Add(vertexId, vId);

            }

            return renumberedTree;
        }

        #endregion

        #region Functions related to removing vertices (RemoveVertex, Clear)

        public new void RemoveVertex(int vertexId, bool reparentChild = false)
        {
            if (reparentChild && NbComponents(vertexId) != 0)
            {
                int newParentId = (int)Parent(vertexId);
                List<int> children = Children(vertexId);

                foreach (int cid in children)
                {
                    ReplaceParent(cid, newParentId);
                }

                children.Remove(vertexId);
                parent.Remove(vertexId);

                if (scale.ContainsKey(vertexId))
                    scale.Remove(vertexId);

                if (complex.ContainsKey(vertexId))
                {
                    int cid = complex[vertexId];
                    List<int> l = components[cid];
                    try
                    {
                        int i = l.IndexOf(vertexId);
                        l.RemoveAt(i);
                    }
                    catch (Exception)
                    {
                    }
                    complex.Remove(vertexId);
                }

            }
            else
            {
                if (NbComponents(vertexId) == 0)
                {
                    base.RemoveVertex(vertexId, reparentChild);

                    if (components.ContainsKey(vertexId))
                        components.Remove(vertexId);

                    if (scale.ContainsKey(vertexId))
                        scale.Remove(vertexId);

                    if (complex.ContainsKey(vertexId))
                    {
                        int cid = complex[vertexId];
                        List<int> l = components[cid];
                        try
                        {
                            int i = l.IndexOf(vertexId);
                            l.RemoveAt(i);
                        }
                        catch(Exception)
                        {
                        }
                        complex.Remove(vertexId);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("vertexId", "This vertex has components and so it can't be removed.");
                }
            }
        }

        /// <summary>
        /// Remove all vertices from the MTG.
        /// </summary>
        public new void Clear()
        {
            base.Clear();

            scale.Clear();
            scale.Add(0, 0);

            complex.Clear();
            components.Clear();
        }

        #endregion

        #region Siblings (InsertSibling)

        /// <summary>
        /// Add a sibling with its properties before the vertex in the parameters.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="namesValues"> The properties of the new vertex. </param>
        /// <param name="vertexToInsert"> The new vertex to add. </param>
        /// <returns> The identifier of the new sibling. </returns>
        public new int InsertSibling(int vertexId, int vertexToInsert = -1, Dictionary<string, dynamic> namesValues = null)
        {
            vertexToInsert = base.InsertSibling(vertexId, vertexToInsert, namesValues);

            scale.Add(vertexToInsert, scale[vertexId]);

            return vertexToInsert;

        }

        #endregion

        #region Ancestors

        public List<int> Ancestors(int vertexId, string EdgeType = "*", string RestrictedTo = "NoRestriction", int containedIn = -1)
        {
            Algorithm a = new Algorithm();

            return a.FullAncestors(this, vertexId, RestrictedTo, EdgeType, containedIn).ToList();
        }

        #endregion

        static void Main(String[] args)
        {
            Algorithm a = new Algorithm();
            traversal t = new traversal();

            mtg tree1 = new mtg();

            int whatever = a.RandomTree(tree1, tree1.root, 200);

            t.SaveToFile(tree1, "C:\\Users\\aannaque\\Desktop\\tulip1.tlp");

            mtg tree2 = new mtg();

            int whatever2 = a.RandomTree(tree2, tree2.root, 200);

            t.SaveToFile(tree2, "C:\\Users\\aannaque\\Desktop\\tulip2.tlp");


        }
    }
}
