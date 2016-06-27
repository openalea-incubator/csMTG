using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System.Collections.Generic;

namespace csMTGTests
{
    [TestClass]
    public class AlgorithmTests
    {
        Algorithm algorithm = new Algorithm();
        traversal t = new traversal();

        #region Tests of the random tree generator

        [TestMethod()]
        public void RandomTree_NumberOfVerticesCreated_SameAsParameter()
        {
            mtg t = new mtg();
            
            int numberOfExpectedVertices = 5000;

            int lastVertex = algorithm.RandomTree(t, t.root, numberOfExpectedVertices);

            Assert.AreEqual(numberOfExpectedVertices+1, ((Tree)t).NbVertices());

        }

        [TestMethod()]
        public void RandomTree_NumberOfChildrenRespected_LessOrEqualToTheParameter()
        {
            mtg t = new mtg();

            int maximumNbChildren = 9;

            int lastVertex = algorithm.RandomTree(t, t.root, 5000, maximumNbChildren);

            foreach (int keyId in t.children.Keys)
            {
                Assert.IsTrue(t.NbChildren(keyId) <= maximumNbChildren);
            }

        }

        [TestMethod()]
        public void RandomTree_DoChildrenAndParentCorrespond_CoherenceBetweenParentAndChildren()
        {
            mtg t = new mtg();

            int lastVertex = algorithm.RandomTree(t, t.root, 5000, 100);

            foreach (int childId in t.parent.Keys)
            {
                if (childId != 0)
                {
                    int? parentId = t.Parent(childId);
                    Assert.IsNotNull(parentId);

                    CollectionAssert.Contains(t.Children((int)parentId), childId);
                }
            }

            foreach (int parentId in t.children.Keys)
            {
                foreach (int childId in t.Children(parentId))
                {
                    Assert.AreEqual(t.Parent(childId), parentId);
                }
            }

        }

        #endregion

        #region Test of SimpleTree

        [TestMethod()]
        public void SimpleTree()
        {
            mtg tree = new mtg();
            tree = algorithm.SimpleTree(tree, tree.root);

            IEnumerable<int> s1 = t.IterativePreOrder(tree, tree.root);
            IEnumerable<int> s2 = t.IterativePostOrder(tree, tree.root);

            Func<IEnumerable<int>, int> Counter = new Func<IEnumerable<int>, int>(source =>
            {
                int res = 0;

                foreach (var item in source)
                    res++;

                return res;
            });

            Assert.AreEqual(21, tree.NbVertices());
            Assert.AreEqual(Counter(s1), Counter(s2));
            Assert.AreEqual(21, Counter(s1));
        }

        #endregion

    }
}
