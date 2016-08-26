using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace csMTG.Tests
{
    [TestClass]
    public class GrameneTests : Gramene
    {

        #region AddCanopy

        [TestMethod()]
        public void AddCanopy_NoCanopyBefore_CanopyAddedAndCursorMoved()
        {
            // Add a canopy

            Gramene g = new Gramene();
            int canopyId = g.TestAddCanopy();

            // Compare the expected scale of the canopy and the actual one.

            int scaleOfCanopy = g.labelsOfScales.FirstOrDefault(x => x.Value == "canopy").Key;

            Assert.AreEqual(1, scaleOfCanopy);

            Assert.AreEqual(1, g.Scale(canopyId));

            // Verify that the components are correct.

            CollectionAssert.AreEqual(new List<int>() { canopyId }, g.Components(0));

            // Verify that the cursor moved to the newly created canopy.

            Assert.AreEqual(canopyId, g.GetCursor());

            // Verify that the label of the canopy is "canopy".

            Assert.AreEqual("canopy", g.GetVertexProperties(canopyId)["label"]);
        }

        #endregion

        #region AddPlant

        [TestMethod]
        public void AddPlant_NewPlant_PlantAdded()
        {
            Gramene g = new Gramene();

            Assert.AreEqual(0, g.GetCursor());

            // Get the identifiers of the new plants.

            int firstPlant = g.TestAddPlant();

            Assert.AreEqual(firstPlant, g.GetCursor());

            int secondPlant = g.TestAddPlant();
            int thirdPlant = g.TestAddPlant();

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

        #region AddShoot

        [TestMethod()]
        public void AddShoot_PlantExists_ShootCreated()
        {
            Gramene g = new Gramene();

            int plantId = g.TestAddPlant();

            Assert.AreEqual(plantId, g.GetCursor());

            int shootId = g.TestAddShoot();

            Assert.AreEqual(shootId, g.GetCursor());

            // Verification of the scale.

            Assert.AreEqual(g.Scale(plantId) + 1, g.Scale(shootId));

            // Verification of the label.

            string plantNb = g.GetVertexProperties(plantId)["label"].Substring(5);

            Assert.AreEqual("shoot" + plantNb, g.GetVertexProperties(shootId)["label"]);

            // Verification of the complex.

            Assert.AreEqual(plantId, g.Complex(shootId));

        }

        [TestMethod()]
        public void AddShoot_PlantDoesntExist_PlantCreatedAndShootAdded()
        {
            Gramene g = new Gramene();

            int shootId = g.TestAddShoot();

            // Verify that the canopy is created.

            int canopy = shootId;

            while (g.Scale(canopy) != 1)
                canopy = (int)g.Complex(canopy);

            Assert.AreEqual(1, g.Scale(canopy));
            CollectionAssert.AreEqual(new List<int>() { canopy }, g.Components(0));

            // Verify that the plant is created.

            int plant = (int)g.Complex(shootId);

            Assert.AreEqual(2, g.Scale(plant));

            string plantLabel = g.GetVertexProperties(plant)["label"];
            Assert.AreEqual("plant", plantLabel.Substring(0,5));

            // Verify that the cursor is placed on the shoot.

            Assert.AreEqual(shootId, g.GetCursor());

        }

        [TestMethod()]
        public void AddShoot_PlantAlreadyHasShoot_NewPlantCreatedAndShootAdded()
        {
            Gramene g = new Gramene();

            // First shoot created.

            int shootId1 = g.TestAddShoot();
            int plantId1 = (int)g.Complex(shootId1);
            int canopyId1 = (int)g.Complex(plantId1);

            // Second shoot created.

            int shootId2 = g.TestAddShoot();
            int plantId2 = (int)g.Complex(shootId2);
            int canopyId2 = (int)g.Complex(plantId2);
            
            // Verify that the plants aren't the same but the canopy is.

            Assert.AreEqual(canopyId1, canopyId2);
            Assert.AreNotEqual(plantId1, plantId2);

            Assert.AreEqual("shoot0", g.GetVertexProperties(shootId1)["label"]);
            Assert.AreEqual("shoot1", g.GetVertexProperties(shootId2)["label"]);
        }

        #endregion

        #region AddRoot

        [TestMethod()]
        public void AddRoot_PlantExists_RootCreated()
        {
            Gramene g = new Gramene();

            int plantId = g.TestAddPlant();

            Assert.AreEqual(plantId, g.GetCursor());

            int rootId = g.TestAddRoot();

            Assert.AreEqual(rootId, g.GetCursor());

            // Verification of the scale.

            Assert.AreEqual(g.Scale(plantId) + 1, g.Scale(rootId));

            // Verification of the label.

            string plantNb = g.GetVertexProperties(plantId)["label"].Substring(5);

            Assert.AreEqual("root" + plantNb, g.GetVertexProperties(rootId)["label"]);

            // Verification of the complex.

            Assert.AreEqual(plantId, g.Complex(rootId));

        }

        [TestMethod()]
        public void AddRoot_PlantDoesntExist_PlantCreatedAndRootAdded()
        {
            Gramene g = new Gramene();

            int rootId = g.TestAddRoot();

            // Verify that the canopy is created.

            int canopy = rootId;

            while (g.Scale(canopy) != 1)
                canopy = (int)g.Complex(canopy);

            Assert.AreEqual(1, g.Scale(canopy));
            CollectionAssert.AreEqual(new List<int>() { canopy }, g.Components(0));

            // Verify that the plant is created.

            int plant = (int)g.Complex(rootId);

            Assert.AreEqual(2, g.Scale(plant));

            string plantLabel = g.GetVertexProperties(plant)["label"];
            Assert.AreEqual("plant0", plantLabel);

            // Verify that the cursor is placed on the shoot.

            Assert.AreEqual(rootId, g.GetCursor());

        }

        [TestMethod()]
        public void AddRoot_PlantAlreadyHasRoot_NewPlantCreatedAndRootAdded()
        {
            Gramene g = new Gramene();

            // First root created.

            int rootId1 = g.TestAddRoot();
            int plantId1 = (int)g.Complex(rootId1);
            int canopyId1 = (int)g.Complex(plantId1);

            // Second root created.

            int rootId2 = g.TestAddRoot();
            int plantId2 = (int)g.Complex(rootId2);
            int canopyId2 = (int)g.Complex(plantId2);

            // Verify that the plants aren't the same but the canopy is.

            Assert.AreEqual(canopyId1, canopyId2);
            Assert.AreNotEqual(plantId1, plantId2);

            Assert.AreEqual("root0", g.GetVertexProperties(rootId1)["label"]);
            Assert.AreEqual("root1", g.GetVertexProperties(rootId2)["label"]);
        }


        #endregion
        /*
        #region AddAxis

        [TestMethod()]
        public void AddAxis_PlantExists_AxisCreated()
        {
            Gramene g = new Gramene();

            int plantId = g.AddPlant();
            int shootId = g.AddShoot(plantId);

            int axisId = g.AddAxis(shootId);

            // Verification of the scale.

            Assert.AreEqual(g.Scale(shootId) + 1, g.Scale(axisId));
            Assert.AreEqual(4, g.Scale(axisId));

            // Verification of the label.

            Assert.AreEqual("axis" + plantId, g.GetVertexProperties(axisId)["label"]);

            // Verification of the complex.

            Assert.AreEqual(shootId, g.Complex(axisId));

        }

        #endregion
        */
        #region Accessors (Plants)

        [TestMethod()]
        public void Plants_PlantsExist_ReturnsTheirList()
        {
            Gramene g = new Gramene();

            int firstPlant = g.TestAddPlant();
            int secondPlant = g.TestAddPlant();

            CollectionAssert.AreEqual(new List<int>(){firstPlant,secondPlant}, g.Plants());

        }

        [TestMethod()]
        public void Plants_NoPlants_ReturnsEmptyList()
        {
            Gramene g = new Gramene();

            CollectionAssert.AreEqual(new List<int>() { }, g.Plants());
        }

        #endregion

        #region CreateBasicWheat

        [TestMethod()]
        public void CreateBasicWheat_NewPlant_WheatCreatedAndNumberOfLeavesUpdated()
        {
            Gramene g = new Gramene();
            
            g.CreateBasicWheat(10);

            Assert.AreEqual(10, g.GetLeafNumber());

        }


        #endregion

        #region LeafNumber

        [TestMethod()]
        public void SetLeafNumber_SameNumberOfLeavesOnPlant_NoChanges()
        {
            Gramene g = new Gramene();
            g.CreateBasicWheat(10);

            Assert.AreEqual(10, g.GetLeafNumber());

            g.SetLeafNumber(10);

            Assert.AreEqual(10, g.GetLeafNumber());
        }

        [TestMethod()]
        public void SetLeafNumber_GreaterThanNumberOfLeavesOnPlant_NewLeavesAdded()
        {
            Gramene g = new Gramene();
            g.CreateBasicWheat(10);

            Assert.AreEqual(10, g.GetLeafNumber());

            g.SetLeafNumber(20);

            Assert.AreEqual(20, g.GetLeafNumber());
        }

        [TestMethod()]
        public void SetLeafNumber_FractionalNumberOfLeaves_SameNumberOnOutput()
        {
            Gramene g = new Gramene();
            g.CreateBasicWheat(10);

            Assert.AreEqual(10, g.GetLeafNumber());

            g.SetLeafNumber(20.6);

            Assert.AreEqual(20.6, g.GetLeafNumber());
        }

        [TestMethod()]
        public void SetLeafNumber_UseItTwice_LeafNumberIncremented()
        {
            Gramene g = new Gramene();
            g.CreateBasicWheat(10);

            Assert.AreEqual(10, g.GetLeafNumber());

            g.SetLeafNumber(15);
            Assert.AreEqual(15, g.GetLeafNumber());

            g.SetLeafNumber(20);
            Assert.AreEqual(20, g.GetLeafNumber());

        }


        #endregion

        #region Copy Constructor

        [TestMethod()]
        public void Gramene_CopyOfGramene_CorrectlyCopied()
        {
            // Creation of a gramene

            Gramene g = new Gramene();
            g.CreateBasicWheat(10);

            // Copy of a gramene

            Gramene g1 = new Gramene(g);

            Assert.AreEqual(g1.GetCursor(), g.GetCursor());

            Assert.AreEqual(10, g1.GetLeafNumber());

        }

        #endregion

    }
}
