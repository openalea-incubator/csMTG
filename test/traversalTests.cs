using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG.Tests
{
    [TestClass()]
    public class traversalTests
    {
        traversal t = new traversal();
        Algorithm a = new Algorithm();
        
    [TestMethod()]
        public void IterativePreOrderTest()
        {
            PropertyTree tree = new PropertyTree();

            tree = a.RandomTree(tree, 2000);
            IEnumerable<int> iterativeList = t.IterativePreOrder(tree, 0);
            
        }

        [TestMethod()]
        public void RecursivePreOrderTest()
        {
            PropertyTree tree = new PropertyTree();

            tree = a.RandomTree(tree, 2000);
            IEnumerable<int> iterativeList = t.RecursivePreOrder(tree, 0);
        }
    }
}