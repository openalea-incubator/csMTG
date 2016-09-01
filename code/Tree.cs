using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace csMTG
{
    /// <summary>
    /// Implementation of a class representing a rooted tree which only stores ids of its elements.
    /// </summary>
    public class Tree
    {
        #region Attributes

        // Every vertex has a unique id
        public int id { get; private set; }// = 0;
        
        // Root attribute
        public int root { get; private set; }
        
        // Children corresponds to : parent_id => { children's ids } 
        public Dictionary<int, List<int>> children = new Dictionary<int, List<int>>();

        // Parent corresponds to : child_id => parent_id
        public Dictionary<int, int> parent = new Dictionary<int, int>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the class.
        /// <para>When created, a root of the tree is defined. Its id is zero and it has no children.</para>
        /// </summary>
        public Tree(int root = 0) 
        {
            this.root = root;
            id = root;
            
            parent.Add(root, -1);
        }

        #endregion

        #region Children (Functions: NbChildren, Children(vertex) )

        /// <summary>
        /// Gives the number of children of a vertex.
        /// </summary>
        /// <param name="vertexId"> The id of the vertex parent. </param>
        /// <returns> Returns the number of children vertices (0 if there are none).
        /// If the vertex parent does not exist, it returns 0 .</returns>
        public int NbChildren(int vertexId)
        {
            if (Children(vertexId) != null)
                return Children(vertexId).Count;
            else
                return 0;
        }

        /// <summary>
        /// Gives a list of the specified id's Children.
        /// </summary>
        /// <param name="vertexId"> The identifier of the parent. </param>
        /// <returns> Returns a list of the parent's children.
        /// If the identifier doesn't have children, it returns an empty list.
        /// If the identifier doesn't exist, it returns null. </returns>
        public List<int> Children(int vertexId)
        {
            List<int> listOfChildren;

            if (!parent.ContainsKey(vertexId))
                listOfChildren = null;
            else
            {
                if (!children.ContainsKey(vertexId))
                {
                    listOfChildren = new List<int>() { };
                }
                else
                {
                    if (children[vertexId] != null)
                        listOfChildren = children[vertexId];
                    else
                        listOfChildren = new List<int>() { };
                }
            }
            return listOfChildren;
        }

        #endregion

        #region Parent (Functions : Parent(vertex), ReplaceParent, InsertParent)

        /// <summary>
        /// Gives the parent of the child in the parameter.
        /// </summary>
        /// <param name="vertexId"> The identifier of the child. </param>
        /// <returns> Returns the identifier of the parent.
        /// In case the parameter doesn't exist, it raises an exception and returns null. </returns>
        public int? Parent(int vertexId)
        {
            int? parentId;

            try
            {
                parentId = parent[vertexId];
            }
            catch (KeyNotFoundException)
            {
                parentId = null;
            }

            return parentId;
        }

        /// <summary>
        /// Replace an existing parent with the one specified in the parameters.
        /// </summary>
        /// <param name="parentId">The new parent.</param>
        /// <param name="childId">The child for which the parent will be changed.</param>
        public void ReplaceParent(int childId, int parentId)
        {
            int oldParent = (int)Parent(childId);

            AddChild(parentId, childId);

            if (oldParent != -1)
            {
                List<int> children = Children(oldParent);
                int index = children.IndexOf(childId);
                children.RemoveAt(index);
            }

        }

        /// <summary>
        /// Insert a parent between the vertex and its original parent.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="parentId"> Parent identifier. </param>
        /// <returns> The id of the new parent. </returns>
        public int InsertParent(int vertexId, int parentId = -1)
        {
            List<int> children = new List<int>();

            if (parentId == -1)
                parentId = NewId();

            int oldParent = (int)Parent(vertexId);

            if (oldParent != -1)
                children = Children(oldParent);

            AddChild(parentId, vertexId);

            if (oldParent != -1)
            {
                int index = children.IndexOf(vertexId);

                children[index] = parentId;
                parent[parentId] = oldParent;
            }

            return parentId;
        }

        #endregion

        #region Root (SetRoot)

        /// <summary>
        /// Sets a vertex as a root, meaning it gets -1 as a parent.
        /// Keep in mind that we can have many roots in the mtg.
        /// </summary>
        /// <param name="vertexId"> The root identifier. </param>
        public void SetARoot(int vertexId)
        {
            if (parent.ContainsKey(vertexId))
                ReplaceParent(vertexId, -1);
            else
                parent.Add(vertexId, -1);
        }

        protected void SetRoot(int vertexId)
        {
            root = vertexId;
            if (!parent.ContainsKey(root))
                parent.Add(root, -1);
        }

        #endregion

        #region Vertices (Functions: Count, HasVertex, NbVertices, Vertices)

        /// <summary>
        /// Counts the number of elements in the tree.
        /// </summary>
        /// <returns>Number of vertices</returns>
        int Count()
        {
            return parent.Count();
        }

        /// <summary>
        /// Tests whether a vertex belongs to the graph or not.
        /// </summary>
        /// <param name="vertexId"> The vertex identifier to test. </param>
        /// <returns> A boolean to assert the belonging. </returns>
        public bool HasVertex(int vertexId)
        {
            return parent.ContainsKey(vertexId);
        }

        /// <summary>
        /// Counts the number of vertices in the tree.
        /// </summary>
        /// <returns> Returns the total number of vertices. </returns>
        public int NbVertices()
        {
            return Count();
        }

        /// <summary>
        /// An iterator on the vertices.
        /// </summary>
        /// <returns> An iterator on the vertices of the tree. </returns>
        IEnumerable<int> VerticesIterator()
        {
            foreach (int vertexId in parent.Keys)
                yield return vertexId;
        }

        /// <summary>
        /// A list of the tree's vertices.
        /// </summary>
        /// <returns> A list of the vertices composing the tree. </returns>
        public List<int> Vertices()
        {
            return VerticesIterator().ToList();
        }

        #endregion

        #region Add a vertex

        /// <summary>
        /// Function which generates a unique Id.
        /// </summary>
        protected int NewId()
        {
            id = parent.Keys.Max() + 1;
            return id;
        }

        /// <summary>
        /// Add a child to a specific parent.
        /// </summary>
        /// <param name="parentId"> The parent to which the child will be added. </param>
        /// <param name="childId"> The child to add. This parameter is optional. </param>
        /// <returns> Returns the id of the child added.
        /// If the parent doesn't exist, it returns -1.</returns>
        public int AddChild(int parentId, int childId = -1)
        {
            if (childId == -1)
                childId = NewId();
            
            NewChild(parentId, childId);
            
            return childId;
        }

        /// <summary>
        /// Assign a new child to a parent.
        /// </summary>
        void NewChild(int parentId, int childId)
        {
            if (parentId != -1)
            {
                if (children.ContainsKey(parentId))
                {
                    if (children[parentId] != null)
                    {
                        children[parentId].Add(childId);
                    }
                    else
                    {
                        List<int> tmpList = new List<int>();
                        tmpList.Add(childId);
                        children[parentId] = tmpList;
                    }
                }
                else
                    children.Add(parentId, new List<int>() { childId });
            }

            parent[childId] = parentId;
        }

        #endregion

        #region Remove (RemoveVertex, Clear)

        /// <summary>
        /// Remove a vertex.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex to be removed. </param>
        /// <param name="reparentChild"> 
        /// If it is set to true, all the children of the vertex will get his parent as a parent.
        /// If it is set to false, the vertex can not be suppressed if it has children. </param>
        public void RemoveVertex(int vertexId, bool reparentChild = false)
        {
            if(vertexId == root)
                throw new ArgumentOutOfRangeException("vertexId", "The root can't be removed.");
            else
            {
                if (!parent.ContainsKey(vertexId))
                    throw new ArgumentOutOfRangeException("vertexId", "This vertex doesn't exist.");
                else
                {
                    if (reparentChild && NbChildren(vertexId) != 0)
                    {
                        int newParentId = (int)Parent(vertexId);
                        children[newParentId].Remove(vertexId);

                        int numberOfChildren = NbChildren(vertexId);

                        while (numberOfChildren > 0)
                        {
                            int child = children[vertexId][0];
                            ReplaceParent(child, newParentId);
                            numberOfChildren--;
                        }

                        children.Remove(vertexId);
                        parent.Remove(vertexId);
                    }
                    else
                    {
                        if (NbChildren(vertexId) == 0)
                        {
                            int p = (int)Parent(vertexId);

                            if (p != -1)
                            {
                                children[p].Remove(vertexId);
                                parent.Remove(vertexId);
                            }

                            if (children.ContainsKey(vertexId))
                                children.Remove(vertexId);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("vertexId", "This vertex has children and so it can't be removed.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove all vertices and edges. 
        /// </summary>
        public void Clear()
        {
            root = 0;
            id = 0;

            parent.Clear();
            children.Clear();
            parent.Add(root, -1);
        }

        #endregion

        #region Siblings (Functions: Siblings, NbSiblings, InsertSibling)

        /// <summary>
        /// An iterator of the vertex's siblings. The vertex in question is not included in the siblings.
        /// </summary>
        /// <param name="vertexId"> The vertex identifier. </param>
        /// <returns> An iterator of the siblings. </returns>
        IEnumerable<int> SiblingsIterator(int vertexId)
        {
            int? parent = Parent(vertexId);

            if (parent == -1 || parent == null)
                yield break;
            else
            {
                foreach(int child in Children((int)parent))
                {
                    if (child != vertexId)
                        yield return child;
                }
            }
        }

        /// <summary>
        /// A list of the siblings of the vertex in the parameter.
        /// </summary>
        /// <param name="vertexId"> The vertex identifier. </param>
        /// <returns> A list of the previous function. </returns>
        public List<int> Siblings(int vertexId)
        {
            return SiblingsIterator(vertexId).ToList();
        }

        /// <summary>
        /// Returns the number of siblings of the specified vertex. If the latter doesn't exist, it returns zero.
        /// </summary>
        /// <param name="vertexId"> Vertex Identifier. </param>
        /// <returns> Number of siblings. </returns>
        public int NbSiblings(int vertexId)
        {
            int? parent = Parent(vertexId);

            if (parent == null)
                return 0;
            else
            {
                int n = NbChildren((int)parent);

                if (n > 0)
                    return n - 1;
                else
                    return 0;
            }

        }

        /// <summary>
        /// Insert a sibling before the vertex specified in the parameters.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="vertexToInsert"> The vertex to insert. (Optional, if id isn't specified, a new one is attributed). </param>
        /// <returns> The id of the new sibling. </returns>
        public int InsertSibling(int vertexId, int vertexToInsert = -1)
        {
            if (vertexToInsert == -1)
                vertexToInsert = NewId();

            int parent = (int)Parent(vertexId);
            List<int> siblings = Children(parent);
            int index = siblings.IndexOf(vertexId);

            siblings.Insert(index, vertexToInsert);
            this.parent.Add(vertexToInsert, parent);

            return vertexToInsert;
        }

        /// <summary>
        /// Determines if a vertex is a leaf or not.
        /// It is to note that a leaf is a vertex which doesn't have any children.
        /// </summary>
        /// <param name="vertexId"> The vertex identifier. </param>
        /// <returns> Boolean (whether it's a leaf or not). </returns>
        public bool IsLeaf(int vertexId)
        {
            return (NbChildren(vertexId) == 0);
        }

        #endregion

        #region SubTree, InsertSiblingTree, AddChildTree, RemoveTree

        /// <summary>
        /// Returns the subtree which is rooted in the vertex in the parameters.
        /// </summary>
        /// <param name="vertexId"> The root of the subtree. </param>
        /// <param name="copy"> If true: A new tree is returned.
        /// If false: The subtree is created using the original tree. </param>
        /// <returns> The subtree. </returns>
        public Tree SubTree(int vertexId, bool copy = true)
        {
            traversal t = new traversal();

            if (!copy)
            {
                // Remove all vertices not in the Sub-tree

                IEnumerable<int> bunch = t.RecursivePreOrder((mtg)this, vertexId);

                foreach (int vid in parent.Keys)
                {
                    if (!bunch.Contains(vid))
                        RemoveVertex(vid);
                }

                root = vertexId;

                if (parent.ContainsKey(root))
                    parent[root] = -1;
                else
                    parent.Add(root, -1);

                return this;
            }
            else
            {
                Dictionary<int, int> renumberedTree = new Dictionary<int, int>();
                Tree tree = new Tree();

                tree.root = 0;

                renumberedTree.Add(vertexId, tree.root);

                IEnumerable<int> subTree = t.RecursivePreOrder((mtg)this, vertexId);

                foreach (int vid in subTree)
                {
                    if (vid != vertexId)
                    {
                        int parent = renumberedTree[(int)Parent(vid)];
                        int v = tree.AddChild(parent);
                        renumberedTree.Add(vid, v);
                    }
                }

                return tree;

            }
        }

        /// <summary>
        /// Insert a tree before the specified vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="tree"> The tree to be inserted. </param>
        /// <returns> The new identifiers after the sibling has been added. </returns>
        public Dictionary<int, int> InsertSiblingTree(int vertexId, Tree tree)
        {
            Dictionary<int, int> renumberedTree = new Dictionary<int, int>();

            int root = tree.root;
            int rootId = InsertSibling(vertexId);

            renumberedTree.Add(root, rootId);

            // PreOrder traversal from root and renumbering the sibling's vertices.

            traversal t = new traversal();

            foreach (int vertex in t.RecursivePreOrder((mtg)tree, vertexId))
            {
                int parent = renumberedTree[(int)tree.Parent(vertex)];
                int v = AddChild(parent);

                renumberedTree.Add(vertex, v);
            }

            return renumberedTree;

        }

        /// <summary>
        /// Add a tree after the children of the parent specified in the parameters.
        /// </summary>
        /// <param name="parentId"> Vertex identifier. </param>
        /// <param name="tree"> A rooted tree. </param>
        /// <returns> The dictionary which makes a correspondance between old identifier and new identifier. </returns>
        public Dictionary<int, int> AddChildTree(int parentId, Tree tree)
        {
            Dictionary<int, int> renumberedTree = new Dictionary<int, int>();
            int root = tree.root;
            int rootId = AddChild(parentId);

            renumberedTree.Add(root, rootId);

            // PreOrder traversal from root and renumbering new children.

            traversal t = new traversal();

            foreach (int vertexId in t.RecursivePreOrder((mtg)tree, root))
            {
                if (vertexId == root)
                    continue;

                parentId = renumberedTree[(int)tree.Parent(vertexId)];
                int vid = AddChild(parentId);
                renumberedTree.Add(vertexId, vid);
            }

            return renumberedTree;

        }

        /// <summary>
        /// Remove the subtree rooted on the vertex in the parameters.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> A list of the deleted vertices. </returns>
        public List<int> RemoveTree(int vertexId)
        {
            int vid = vertexId;

            List<int> vertices = new List<int>();

            traversal t = new traversal();

            foreach (int vertex in t.RecursivePostOrder((mtg)this, vid))
            {
                RemoveVertex(vertex);
                vertices.Add(vertex);
            }

            return vertices;

        }

        #endregion
    }
}
