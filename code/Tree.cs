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
         // Every vertex has a unique id
        public int id { get; private set; } = 0;
        
        // Root attribute
        public int root { get; private set; }
        
        // Children corresponds to : parent_id => { children's ids } 
        public Dictionary<int, List<int>> children = new Dictionary<int, List<int>>();

        // Parent corresponds to : child_id => parent_id
        public Dictionary<int, int> parent = new Dictionary<int, int>();


        /// <summary>
        /// Constructor of the class.
        /// <para>When created, a root of the tree is defined. Its id is zero and it has no children.</para>
        /// </summary>
        public Tree() {
            root = id;
            parent.Add(root, -1);
            children.Add(root,null);
        }

        /// <summary>
        /// Gives the number of children of a vertex.
        /// </summary>
        /// <param name="vertexId"> The id of the vertex parent. </param>
        /// <returns> Returns the number of children vertices (0 if there are none).
        /// If the vertex parent does not exist, it returns 0 .</returns>
        public int NbChildren(int vertexId)
        {
            int nbChildren = 0;

            if (!children.ContainsKey(vertexId))
                nbChildren = 0;
            else
            {
                if (children[vertexId] != null)
                    nbChildren = children[vertexId].Count();
            }

            return nbChildren;
        }

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
            }catch(KeyNotFoundException)
            {
                parentId = null;
            }
            
            return parentId;
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
                if (children[vertexId] != null)
                    listOfChildren = children[vertexId];
                else
                    listOfChildren = new List<int>() { };
            }
            return listOfChildren;
        }

        /// <summary>
        /// Counts the number of elements in the tree.
        /// </summary>
        /// <returns>Number of vertices</returns>
        int Count() {
            int count = parent.Count();
            return count;
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
        /// Function which generates a unique Id.
        /// </summary>
        private int NewId()
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
        public int AddChild(int parentId, int childId = -1) {

            if (!(children.ContainsKey(parentId)))
                Console.WriteLine("Parent number "+parentId+" does not exist!");
            else
            {
                // Case where the child wasn't specified
                if (childId == -1)
                {
                    childId = NewId();
                    NewChild(parentId, childId);
                    children.Add(childId, null);
                }
                // Case where the child is specified
                else
                {
                    //If the child exists already, change his parent
                    if (parent.ContainsKey(childId))
                    {
                        ReplaceParent(parentId, childId);
                    }
                    else
                    {
                        NewChild(parentId, childId);
                        children.Add(childId, null);
                    }
                }
            }
            return childId;
        }

        /// <summary>
        /// Assign a new child to a parent.
        /// </summary>
        void NewChild(int parentId, int childId)
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
            parent[childId] = parentId;
        }

        /// <summary>
        /// Replace an existing parent with the one specified in the parameters.
        /// </summary>
        /// <param name="parentId">The new parent.</param>
        /// <param name="childId">The child for which the parent will be changed.</param>
        void ReplaceParent(int parentId, int childId)
        {
            int oldParent = parent[childId];
            NewChild(parentId, childId);
            children[oldParent].Remove(childId);
        }

        /// <summary>
        /// Remove a vertex.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex to be removed. </param>
        /// <param name="reparentChild"> 
        /// If it is set to true, all the children of the vertex will get his parent as a parent.
        /// If it is set to false, the vertex can not be suppressed if it has children. </param>
        public void RemoveVertex(int vertexId, bool reparentChild)
        {
            // Root can't be removed.
            if (vertexId == root)
                throw new ArgumentOutOfRangeException("vertexId", "The root can't be removed.");
            else
            {
                // Vertex doesn't exist
                if (!parent.ContainsKey(vertexId))
                    throw new ArgumentOutOfRangeException("vertexId", "This vertex doesn't exist.");
                else
                {
                    // Delete the vertex from the list of his parent's children.
                    int newParent = (int)Parent(vertexId);
                    children[newParent].Remove(vertexId);

                    // In case the deleted vertex has children, their parent is replaced.
                    if (NbChildren(vertexId) > 0)
                    {
                        if (reparentChild)
                        {
                            int numberOfChildren = children[vertexId].Count;

                            while (numberOfChildren > 0)
                            {
                                int childId = children[vertexId][0];
                                ReplaceParent(newParent, childId);
                                numberOfChildren--;
                            }
                        }
                        else
                            throw new ArgumentOutOfRangeException("vertexId", "This vertex has children and so it can't be removed.");
                    }

                    // The vertex no longer has children or a parent.
                    children.Remove(vertexId);
                    parent.Remove(vertexId);
                }
              }
            }


        static void Main(String[] args)   
        {
            
        }
    }
}