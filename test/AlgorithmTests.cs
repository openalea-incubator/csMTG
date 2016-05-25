using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;

namespace csMTGTests
{
    [TestClass]
    public class AlgorithmTests
    {
        Algorithm algorithm = new Algorithm();

        #region Tests of the random tree generator

        [TestMethod()]
        public void RandomTree_NumberOfVerticesCreated_SameAsParameter()
        {
            mtg t = new mtg();
            
            int numberOfExpectedVertices = 5000;

            t = algorithm.RandomTree(t, numberOfExpectedVertices);

            Assert.AreEqual(numberOfExpectedVertices, ((Tree)t).NbVertices());

        }

        [TestMethod()]
        public void RandomTree_NumberOfChildrenRespected_LessOrEqualToTheParameter()
        {
            mtg t = new mtg();

            int maximumNbChildren = 9;

            t = algorithm.RandomTree(t, 5000, maximumNbChildren);

            foreach (int keyId in t.children.Keys)
            {
                Assert.IsTrue(t.NbChildren(keyId) <= maximumNbChildren);
            }

        }

        [TestMethod()]
        public void RandomTree_DoChildrenAndParentCorrespond_CoherenceBetweenParentAndChildren()
        {
            mtg t = new mtg();

            t = algorithm.RandomTree(t, 5000, 100);

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

    }
}
