﻿using System;

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
        public Tree() {
            id = 0;
            root = id;
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
        void ReplaceParent(int parentId, int childId)
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

        #region Vertices (Functions: Count, HasVertex, NbVertices)

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

        #region Remove a vertex

        /// <summary>
        /// Remove a vertex.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex to be removed. </param>
        /// <param name="reparentChild"> 
        /// If it is set to true, all the children of the vertex will get his parent as a parent.
        /// If it is set to false, the vertex can not be suppressed if it has children. </param>
        public void RemoveVertex(int vertexId, bool reparentChild)
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
                            ReplaceParent(newParentId, child);
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

        #endregion

    }
}
