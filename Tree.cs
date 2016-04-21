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
        int root;
        
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
        /// Counts the number of elements in the tree.
        /// </summary>
        /// <returns>Number of vertices</returns>
        int Count() {
            int count = children.Count();
            return count;
        }

        /// <summary>
        /// Function which generates a unique Id.
        /// </summary>
        private int NewId()
        {
            do
            {
                id++;
            }
            while (children.ContainsKey(id));
            return id;
        }

        /// <summary>
        /// Add a child to a specific parent.
        /// </summary>
        /// <param name="parentId"> The parent to which the child will be added. </param>
        /// <param name="childId"> The child to add. This parameter is optional. </param>
        /// <returns> Returns the id of the child added. </returns>
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

        static void Main(String[] args)   
        {
            Tree t = new Tree();

            //Verify that the tree contains only one node after creation
            Debug.Assert(t.Count() == 1);

            //Verify that the id of the root is 0
            int root = t.parent.Keys.First();
            Console.WriteLine(root);

            //Verify that a child is correctly added
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(1);
            t.AddChild(50);
        
            foreach(int counter in t.parent.Keys)
            {
                if (t.parent[counter] != null)
                {
                    Console.Write(counter + " => ");
                    t.parent[counter].ForEach(Console.Write);
                    Console.WriteLine();
                }
                else
                    Console.WriteLine(counter + " => " + "null");
                
            }

            Debug.Assert(t.Count() == 5);

        }
    }
}
