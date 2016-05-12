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

        [TestMethod()]
        public void PreOrderResultsAreTheSame()
        {
            PropertyTree tree = new PropertyTree();

            tree = a.RandomTree(tree, 2000);
            
            Assert.IsTrue(Enumerable.SequenceEqual<int>(t.IterativePreOrder(tree, 0), t.RecursivePreOrder(tree, 0)));

        }

        [TestMethod()]
        public void IterativePostOrderTest()
        {
            PropertyTree tree = new PropertyTree();

            tree = a.RandomTree(tree, 7000);
            IEnumerable<int> iterativeList = t.IterativePostOrder(tree, 0);

        }

        [TestMethod()]
        public void RecursivePostOrderTest()
        {
            PropertyTree tree = new PropertyTree();

            tree = a.RandomTree(tree, 7000);
            IEnumerable<int> iterativeList = t.RecursivePostOrder(tree, 0);
        }

        [TestMethod()]
        public void PostOrderResultsAreTheSame()
        {
            PropertyTree tree = new PropertyTree();

            tree = a.RandomTree(tree, 2000);

            Assert.IsTrue(Enumerable.SequenceEqual<int>(t.IterativePostOrder(tree, 0), t.RecursivePostOrder(tree, 0)));

        }
    }
}
