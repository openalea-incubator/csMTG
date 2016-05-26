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

        #region Test of vertices

        [TestMethod()]
        public void Vertices_NormalMtg_ReturnsListOfVertices()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 1);
            tree.scale.Add(2, 1);
            tree.scale.Add(3, 3);
            tree.scale.Add(4, 5);

            CollectionAssert.AreEqual(tree.Vertices(1), new List<int>() { 1, 2 });
            CollectionAssert.AreEqual(tree.Vertices(3), new List<int>() { 3 });

        }

        [TestMethod()]
        public void Vertices_EmptyMtg_ReturnsEmptyList()
        {
            mtg tree = new mtg();

            CollectionAssert.AreEqual(tree.Vertices(1), new List<int>() { });
        }

        [TestMethod()]
        public void NbVertices_NormalMtg_CorrectNumberForEachScale()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 1);
            tree.scale.Add(2, 1);
            tree.scale.Add(3, 3);
            tree.scale.Add(4, 5);

            // Counts all vertices since scale isn't specified.
            Assert.AreEqual(tree.NbVertices(), 5);

            // Gives the right count for vertices by scale.
            Assert.AreEqual(tree.NbVertices(0), 1);
            Assert.AreEqual(tree.NbVertices(1), 2);
            Assert.AreEqual(tree.NbVertices(3), 1);
            Assert.AreEqual(tree.NbVertices(5), 1);

            // Gives answer zero for a scale which doesn't exist.
            Assert.AreEqual(tree.NbVertices(2), 0);

        }

        #endregion

        #region Test of edges

        [TestMethod()]
        public void HasVertex_VertexExists_ReturnsTrue()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 12);

            Assert.IsTrue(tree.HasVertex(1));
            Assert.IsTrue(tree.HasVertex(0));
        }

        [TestMethod()]
        public void HasVertex_VertexDoesntExist_ReturnsFalse()
        {
            mtg tree = new mtg();

            tree.scale.Add(1, 12);

            Assert.IsFalse(tree.HasVertex(2));
        }

        [TestMethod()]
        public void Edges_NormalTree_ListOfKeyValues()
        {
            mtg tree = new mtg();

            // Add 4 children. Parents/children are : (0 => 1), (1 => 2,3) , (3 => 4).
            int firstChild = tree.AddChild(0);
            int secondChild = tree.AddChild(firstChild);
            int thirdChild = tree.AddChild(firstChild);
            int fourthChild = tree.AddChild(thirdChild);

            // Assign a scale to each vertex.
            tree.scale[firstChild] = 1;
            tree.scale[secondChild] = 1;
            tree.scale[thirdChild] = 2;
            tree.scale[fourthChild] = 2;

            // Without specifying the scale
            List<KeyValuePair<int, int>> expectedResult = new List<KeyValuePair<int, int>>();
            expectedResult.Add(new KeyValuePair<int,int>(0,1));
            expectedResult.Add(new KeyValuePair<int, int>(1,2));
            expectedResult.Add(new KeyValuePair<int, int>(1, 3));
            expectedResult.Add(new KeyValuePair<int, int>(3, 4));

            CollectionAssert.AreEqual(expectedResult, tree.Edges());

            // Scale 0
            List<KeyValuePair<int, int>> expectedResult2 = new List<KeyValuePair<int, int>>();
            expectedResult2.Add(new KeyValuePair<int, int>(0, 1));
            CollectionAssert.AreEqual(expectedResult2, tree.Edges(0));

            // Scale 1
            List<KeyValuePair<int, int>> expectedResult3 = new List<KeyValuePair<int, int>>();
            expectedResult3.Add(new KeyValuePair<int, int>(1, 2));
            expectedResult3.Add(new KeyValuePair<int, int>(1, 3));
            CollectionAssert.AreEqual(expectedResult3, tree.Edges(1));

            // Scale 2
            List<KeyValuePair<int, int>> expectedResult4 = new List<KeyValuePair<int, int>>();
            expectedResult4.Add(new KeyValuePair<int, int>(3, 4));
            CollectionAssert.AreEqual(expectedResult4, tree.Edges(2));

        }

        #endregion

        #region Test of complex

        [TestMethod()]
        public void Complex_NormalMtg_ComplexReturned()
        {
            mtg tree = new mtg();

            // Add 4 children. Parents/children are : (0 => 1), (1 => 2,3) , (3 => 4).
            int firstChild = tree.AddChild(0);
            int secondChild = tree.AddChild(firstChild);
            int thirdChild = tree.AddChild(firstChild);
            int fourthChild = tree.AddChild(thirdChild);
            int fifthChild = tree.AddChild(0);

            // Assign a complex to the firstChild
            tree.complex.Add(firstChild, 0);

            Assert.AreEqual(tree.Complex(firstChild), 0);
            Assert.AreEqual(tree.Complex(secondChild), 0);
            Assert.AreEqual(tree.Complex(thirdChild), 0);
            Assert.AreEqual(tree.Complex(fourthChild), 0);

            Assert.IsNull(tree.Complex(fifthChild));

        }

        #endregion

        #region Global test

        [TestMethod()]
        public void MtgTest()
        {
            mtg tree = new mtg();

            int root = tree.root;

            Assert.AreEqual(0, root);

            // Scale 1

            int root1 = tree.AddComponent(root, new Dictionary<string, dynamic>() { });
            int vertex1 = tree.AddChild(root1);
            int vertex2 = tree.AddChild(root1);
            int vertex3 = tree.AddChild(root1);

            int vertex4 = tree.AddChild(vertex1);
            int vertex5 = tree.AddChild(vertex1);

            // Verify complex

            Assert.AreEqual(root, tree.Complex(root1));
            Assert.AreEqual(root, tree.Complex(vertex1));
            Assert.AreEqual(root, tree.Complex(vertex2));
            Assert.AreEqual(root, tree.Complex(vertex3));
            Assert.AreEqual(root, tree.Complex(vertex4));
            Assert.AreEqual(root, tree.Complex(vertex5));

            // Verify parents

            Assert.AreEqual(vertex1, tree.Parent(vertex5));
            Assert.AreEqual(vertex1, tree.Parent(vertex4));
            Assert.AreEqual(root1, tree.Parent(vertex3));
            Assert.AreEqual(root1, tree.Parent(vertex2));
            Assert.AreEqual(root1, tree.Parent(vertex1));

            // Verify children

            CollectionAssert.AreEqual(new List<int>(){vertex1, vertex2, vertex3}, tree.Children(root1));
            CollectionAssert.AreEqual(new List<int>() {vertex4, vertex5 }, tree.Children(vertex1));
            Assert.IsFalse(tree.children.ContainsKey(vertex2));
            Assert.IsFalse(tree.children.ContainsKey(vertex3));
            Assert.IsFalse(tree.children.ContainsKey(vertex4));
            Assert.IsFalse(tree.children.ContainsKey(vertex5));

            // Verify length of the mtg

            Assert.AreEqual(7, tree.NbVertices());

        }

        #endregion

    }
}
