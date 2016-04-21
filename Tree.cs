using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace csMTG
{
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
        
        //The key is the parent's id, and the values are a list of children
        protected Dictionary<int, List<int>> parent = new Dictionary<int, List<int>>();

        // Constructor
        public Tree() {
            root = id;
            parent.Add(root, -1);
            children.Add(root,null);
            id = NewId();
        }

        // The number of elements in the tree
         int Count() {
            int count = children.Count();
            return count;
        }
        
        //Attributes a unique new id
        private int NewId()
        {
            do
            {
                id++;
            }
            while (children.ContainsKey(id));
            return id;
        }

        // Add a child to the id specified in the parameter
        void AddChild(int parentId) {

            if (!(parent.ContainsKey(parentId)))
                Console.WriteLine("This parent does not exist!");
            else
            {
                //Case where the parent already has at least a child
                if(children[parentId] != null)
                {
                    children[parentId].Add(id);
                }
                  else
                {
                    List<int> newList = new List<int>();
                    newList.Add(id);
                    children[parentId] = newList;
                }
                parent[id] = parentId;
                id = NewId();
            }
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
