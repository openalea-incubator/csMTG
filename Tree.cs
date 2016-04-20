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
        // Define the attributes
        int id { get; set; } = 0;
        //The key is the parent's id, and the values are a list of children
        protected Dictionary<int, List<int>> parent = new Dictionary<int, List<int>>();

        // Constructor
        public Tree() {
            parent.Add(id, null);
            id++;
        }

        // The number of elements in the tree
         int Count() {
            int count = parent.Count();
            return count;
        }

        // Add a child to the id specified in the parameter
        void AddChild(int parentId) {

            if (!(parent.ContainsKey(parentId)))
                Console.WriteLine("This parent does not exist!");
            else
            {
                //Case where the parent already has at least a child
                if(parent[parentId] != null)
                {
                    parent[parentId].Add(id);
                    parent.Add(id, null);
                    id++;
                }
                  else
                {
                    List<int> newList = new List<int>();
                    newList.Add(id);
                    parent[parentId] = newList;
                    parent.Add(id, null);
                    id++;
                }
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
