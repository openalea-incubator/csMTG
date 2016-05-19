using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System.Collections.Generic;

namespace csMTG.Tests
{
    [TestClass()]
    public class MtgTests
    {
        
        #region Test of constructor
        
        [TestMethod()]
        public void Mtg_NewMtg_AttributesInitializedPropertiesAdded()
        {
            mtg tree = new mtg();

            Assert.AreEqual(tree.scale[0], 0);

            List<string> realList = tree.PropertyNames();
            List<string> expectedList = new List<string>() { "Edge_Type", "label" };

            CollectionAssert.AreEqual(realList, expectedList);

        }

        #endregion

        #region Test of scale

        [TestMethod()]
        public void Scales_NormalMtg_CorrectList()
        {
            mtg tree = new mtg();

            // Add 4 vertices in two different scales.
            tree.scale.Add(1, 1);
            tree.scale.Add(2, 1);
            tree.scale.Add(3, 2);
            tree.scale.Add(4, 2);
            
            List<int> realList = tree.Scales();
            List<int> expectedList = new List<int>() {0, 1 ,2};

            // Expected number is 3 because the root is at scale 0.
            Assert.AreEqual(realList.Count, 3);

            CollectionAssert.AreEqual(realList, expectedList);

        }

        [TestMethod()]
        public void Scales_EmptyMtg_OneScale()
        {
            mtg tree = new mtg();

            CollectionAssert.AreEqual(tree.Scales(), new List<int> (){0});
        }

        [TestMethod()]
        public void Scale_NormalCase_ReturnsScale()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 1);
            tree.scale.Add(2, 1);
            tree.scale.Add(3, 2);
            tree.scale.Add(4, 2);

            // Verify that the mapping is correct.
            Assert.AreEqual(tree.Scale(0), 0);
            Assert.AreEqual(tree.Scale(1), 1);
            Assert.AreEqual(tree.Scale(2), 1);
            Assert.AreEqual(tree.Scale(3), 2);
            Assert.AreEqual(tree.Scale(4), 2);
        }

        [TestMethod()]
        public void Scale_VertexDoesntExist_ReturnsNull()
        {
            mtg tree = new mtg();

            Assert.IsNull(tree.Scale(1));

        }

        [TestMethod()]
        public void NbScales_EmptyMtg_ReturnsOne()
        {
            mtg tree = new mtg();

            Assert.AreEqual(tree.NbScales(), 1);

        }

        [TestMethod()]
        public void NbScales_NormalMtg_ReturnsNumberOfScales()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 1);
            tree.scale.Add(2, 1);
            tree.scale.Add(3, 2);
            tree.scale.Add(4, 2);

            Assert.AreEqual(tree.NbScales(), 3);

        }

        [TestMethod()]
        public void MaxScale_EmptyMtg_ReturnsZero()
        {
            mtg tree = new mtg();

            Assert.AreEqual(tree.MaxScale(), 0);

        }

        [TestMethod()]
        public void MaxScale_NormalMtg_ReturnsMaximum()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 1);
            tree.scale.Add(2, 1);
            tree.scale.Add(3, 3);
            tree.scale.Add(4, 5);

            Assert.AreEqual(tree.MaxScale(), 5);

        }


        #endregion

    }
}

