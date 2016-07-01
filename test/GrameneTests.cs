using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System.Collections.Generic;
using System.Linq;

namespace csMTG.Tests
{
    [TestClass]
    public class GrameneTests
    {
        #region Test of constructor

        [TestMethod]
        public void Gramene_NewGramene_CanopyAdded()
        {
            Gramene g = new Gramene();

            int idOfCanopy = g.labelsOfScales.FirstOrDefault(x => x.Value == "canopy").Key;

            Assert.AreEqual(1, idOfCanopy);
            Assert.AreEqual("canopy", g.labelsOfScales[idOfCanopy]);
            Assert.AreEqual(1, g.Scale(idOfCanopy));

            CollectionAssert.AreEqual(new List<int>() { idOfCanopy }, g.Components(0));

        }

        #endregion

        #region AddPlant

        [TestMethod]
        public void AddPlant_NewPlant_PlantAdded()
        {
            Gramene g = new Gramene();

            // Get the identifiers of the new plants.

            int firstPlant = g.AddPlant();
            int secondPlant = g.AddPlant();
            int thirdPlant = g.AddPlant();

            // Get the label of the plants.

            string firstLabel = g.GetVertexProperties(firstPlant)["label"];
            string secondLabel = g.GetVertexProperties(secondPlant)["label"];
            string thirdLabel = g.GetVertexProperties(thirdPlant)["label"];

            // Make sure the labels correspond to the ones expected.

            Assert.AreEqual("plant0", firstLabel);
            Assert.AreEqual("plant1", secondLabel);
            Assert.AreEqual("plant2", thirdLabel);

            // Verify that all plants belong to scale number 2.

            Assert.AreEqual(2, g.Scale(firstPlant));
            Assert.AreEqual(2, g.Scale(secondPlant));
            Assert.AreEqual(2, g.Scale(thirdPlant));

            // Verify that the complex of the plant is the canopy.

            Assert.AreEqual(1, g.Complex(firstPlant));
            Assert.AreEqual(1, g.Complex(secondPlant));
            Assert.AreEqual(1, g.Complex(thirdPlant));



        }


        #endregion

        #region Accessors (Plants)

        [TestMethod()]
        public void Plants_PlantsExist_ReturnsTheirList()
        {
            Gramene g = new Gramene();

            int firstPlant = g.AddPlant();
            int secondPlant = g.AddPlant();

            CollectionAssert.AreEqual(new List<int>(){firstPlant,secondPlant}, g.Plants());

        }

        [TestMethod()]
        public void Plants_NoPlants_ReturnsEmptyList()
        {
            Gramene g = new Gramene();

            CollectionAssert.AreEqual(new List<int>() { }, g.Plants());
        }

        #endregion

    }
}
