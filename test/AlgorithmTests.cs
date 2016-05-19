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
            PropertyTree t = new PropertyTree();
            

            int numberOfExpectedVertices = 50000;

            t = algorithm.RandomTree(t, numberOfExpectedVertices);

            Assert.AreEqual(numberOfExpectedVertices, t.NbVertices());

        }

        [TestMethod()]
        public void RandomTree_NumberOfChildrenRespected_LessOrEqualToTheParameter()
        {
            PropertyTree t = new PropertyTree();

            int maximumNbChildren = 9;

            t = algorithm.RandomTree(t, 50000, maximumNbChildren);

            foreach (int keyId in t.children.Keys)
            {
                Assert.IsTrue(t.NbChildren(keyId) <= maximumNbChildren);
            }

        }

        [TestMethod()]
        public void RandomTree_DoChildrenAndParentCorrespond_CoherenceBetweenParentAndChildren()
        {
            PropertyTree t = new PropertyTree();

            t = algorithm.RandomTree(t, 50000, 100);

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
