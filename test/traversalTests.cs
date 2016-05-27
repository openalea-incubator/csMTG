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
            mtg tree = new mtg();

            int lastVertex = a.RandomTree(tree, tree.root, 2000);
            IEnumerable<int> iterativeList = t.IterativePreOrder(tree, 0);
            
        }

        [TestMethod()]
        public void RecursivePreOrderTest()
        {
            mtg tree = new mtg();

            int lastVertex = a.RandomTree(tree, tree.root, 2000);
            IEnumerable<int> iterativeList = t.RecursivePreOrder(tree, 0);

        }

        [TestMethod()]
        public void PreOrderResultsAreTheSame()
        {
            mtg tree = new mtg();

            int lastVertex = a.RandomTree(tree, tree.root, 2000);
            
            Assert.IsTrue(Enumerable.SequenceEqual<int>(t.IterativePreOrder(tree, 0), t.RecursivePreOrder(tree, 0)));

        }

        [TestMethod()]
        public void IterativePostOrderTest()
        {
            mtg tree = new mtg();

            int lastVertex = a.RandomTree(tree, tree.root, 7000);
            IEnumerable<int> iterativeList = t.IterativePostOrder(tree, 0);

        }

        [TestMethod()]
        public void RecursivePostOrderTest()
        {
            mtg tree = new mtg();

            int lastVertex = a.RandomTree(tree, tree.root, 7000);
            IEnumerable<int> iterativeList = t.RecursivePostOrder(tree, 0);
        }

        [TestMethod()]
        public void PostOrderResultsAreTheSame()
        {
            mtg tree = new mtg();

            int lastVertex = a.RandomTree(tree, tree.root, 2000);

            Assert.IsTrue(Enumerable.SequenceEqual<int>(t.IterativePostOrder(tree, 0), t.RecursivePostOrder(tree, 0)));

        }
    }
}
