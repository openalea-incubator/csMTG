using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For each testing method, the name will be in this format:
//   NameOfTheTestedMethod_CaseWeAreTesting_ExpectedBehavior
namespace csMTG.Tests
{
    [TestClass()]
    public class TreeTests
    {
        #region Test of constructor
        [TestMethod()]
        public void Tree_NewTree_RootCreated()
        {
            Tree t = new Tree();
            Assert.AreEqual(t.root, 0);
        }
        #endregion

        #region Test of function NbVertices
        [TestMethod()]
        public void NbVertices_NewTreeCreated_Returns1()
        {
            Tree t = new Tree();
            Assert.AreEqual(t.NbVertices(), 1);
        }

        [TestMethod()]
        public void NbVertices_ChildrenAdded_ReturnsCorrectNumberOfVertices()
        {
            Tree t = new Tree();

            // Add 9 new vertices

            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);
            
            Assert.AreEqual(t.NbVertices(), 10);
        }
        #endregion

        #region Tests of function NbChildren
        [TestMethod()]
        public void NbChildren_NormalCase_ReturnsNumberOfChildren()
        {
            Tree t = new Tree();

            t.AddChild(t.root);
            t.AddChild(t.root);
            t.AddChild(t.root);
            t.AddChild(1);

            Assert.AreEqual(t.NbChildren(t.root), 3);
        }

        [TestMethod()]
        public void NbChildren_NoChildren_ReturnsZero()
        {
            Tree t = new Tree();

            Assert.AreEqual(t.NbChildren(t.root), 0);
        }

        [TestMethod()]
        public void NbChildren_ParameterDoesntExist_ReturnsZero()
        {
            Tree t = new Tree();

            t.AddChild(t.root);
            t.AddChild(t.root);
            t.AddChild(t.root);

            Assert.AreEqual(t.NbChildren(100), 0);
        }
        #endregion

        #region Tests of getter: Parent
        [TestMethod()]
        public void Parent_NormalCase_ReturnsParentId()
        {
            Tree t = new Tree();

            t.AddChild(0, 1);
            t.AddChild(1, 50);
            int childId = t.AddChild(0);

            Assert.AreEqual(t.Parent(1), 0);
            Assert.AreEqual(t.Parent(50), 1);
            Assert.AreEqual(t.Parent(childId), 0);
        }

        [TestMethod()]
        public void Parent_ChildIdDoesntExist_ReturnsNull()
        {
            Tree t = new Tree();

            t.AddChild(0, 1);

            Assert.IsNull(t.Parent(100));

        }
        #endregion

        #region Test of getter : Children
        [TestMethod()]
        public void Children_NormalCase_ReturnsListOfChildren()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root);
            int secondChild = t.AddChild(t.root);
            int thirdChild = t.AddChild(t.root);

            List<int> expectedListOfChildren = new List<int>();
            expectedListOfChildren.Add(firstChild);
            expectedListOfChildren.Add(secondChild);
            expectedListOfChildren.Add(thirdChild);

            CollectionAssert.AreEqual(t.Children(t.root), expectedListOfChildren);
        }

        [TestMethod()]
        public void Children_ParameterDoesntExist_ReturnsNull()
        {
            Tree t = new Tree();

            List<int> expectedResult = null;

            CollectionAssert.AreEqual(t.Children(10),expectedResult);
        }

        [TestMethod()]
        public void Children_NoChildren_ReturnsEmptyList()
        {
            Tree t = new Tree();

            int childId = t.AddChild(t.root);

            CollectionAssert.AreEqual(new List<int>() { }, t.Children(childId));
        }
        #endregion

        #region Tests of function AddChild
        [TestMethod()]
        public void AddChild_NormalScenario_OneChildAdded()
        {
            Tree t = new Tree();
            
            int childId = t.AddChild(0);

            Assert.IsTrue(t.Children(0).Contains(childId));
            Assert.AreEqual(t.Children(0).Count(), 1);
            Assert.AreEqual(t.Parent(childId), 0);
        }

        [TestMethod()]
        public void AddChild_ChildIdDoesntExist_SpecificIdCreated()
        {
            Tree t = new Tree();

            int childId = t.AddChild(0, 5);

            Assert.AreEqual(childId, 5);
            Assert.IsTrue(t.Children(0).Contains(childId));
            Assert.AreEqual(t.Children(0).Count(), 1);
            Assert.AreEqual(t.Parent(childId), 0);
        }

        [TestMethod()]
        public void AddChild_SpecifyChildId_IdIsNotRepeated()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root); //Should be equal to 1
            int secondChild = t.AddChild(t.root, 2); //Should be equal to 2
            int thirdChild = t.AddChild(t.root); //Should be equal to 3

            //All children have been added to the root
            Assert.AreEqual(t.Children(t.root).Count(), 3);
            Assert.IsTrue(t.Children(t.root).Contains(firstChild));
            Assert.IsTrue(t.Children(t.root).Contains(secondChild));
            Assert.IsTrue(t.Children(t.root).Contains(thirdChild));

            //All children have the right id (2 isn't repeated)
            Assert.AreEqual(firstChild, 1);
            Assert.AreEqual(secondChild, 2);
            Assert.AreEqual(thirdChild, 3);

            //All children have the right parent
            Assert.AreEqual(t.Parent(firstChild), t.root);
            Assert.AreEqual(t.Parent(secondChild), t.root);
            Assert.AreEqual(t.Parent(thirdChild), t.root);
        }

        // I think we will never have this case !

        //[TestMethod()]
        //public void AddChild_ChildAlreadyExists_ParentIsChanged()
        //{
        //    Tree t = new Tree();

        //    int firstChild = t.AddChild(t.root);
        //    int secondChild = t.AddChild(firstChild, 2);
        //    int thirdChild = t.AddChild(t.root, secondChild);

        //    //All children have been added to the right parent
        //    Assert.AreEqual(t.Children(t.root).Count(), 2);
        //    Assert.IsTrue(t.Children(t.root).Contains(firstChild));
        //    Assert.IsTrue(t.Children(t.root).Contains(thirdChild));
        //    Assert.IsFalse(t.Children(firstChild).Contains(secondChild));

        //    //All children have the right id (2 isn't repeated)
        //    Assert.AreEqual(firstChild, 1);
        //    Assert.AreEqual(secondChild, 2);
        //    Assert.AreEqual(thirdChild, 2);

        //    //All children have the right parent
        //    Assert.AreEqual(t.Parent(firstChild), t.root);
        //    Assert.AreEqual(t.Parent(secondChild), t.root);
        //    Assert.AreEqual(t.Parent(thirdChild), t.root);
        //}

        //[TestMethod()]
        //public void AddChild_ParentDoesntExist_ChildNotAdded()
        //{
        //    Tree t = new Tree();

        //    int childId = t.AddChild(1);

        //    //Child hasn't been added
        //    Assert.IsNull(t.Children(1));

        //    //An id hasn't been attributed
        //    Assert.AreEqual(childId, -1);
        //}
        #endregion

        #region Tests of function RemoveVertex

        [TestMethod()]
        public void RemoveVertex_NormalScenario_ChildRemoved()
        {
            Tree t = new Tree();
            
            int childId = t.AddChild(0);

            CollectionAssert.Contains(t.Children(0), childId);

            t.RemoveVertex(childId, true);

            CollectionAssert.DoesNotContain(t.Children(0),childId);
            Assert.IsNull(t.Parent(childId));
            Assert.IsNull(t.Children(childId));
        }
        
        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "The root can't be removed.")]
        public void RemoveVertex_RemoveRoot_ExceptionThrown()
        {
            Tree t = new Tree();
            
            t.RemoveVertex(0, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "This vertex doesn't exist.")]
        public void RemoveVertex_VertexDoesntExist_ExceptionThrown()
        {
            Tree t = new Tree();

            t.RemoveVertex(10, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "This vertex has children and so it can't be removed.")]
        public void RemoveVertex_VertexHasChildrenAndReparentIsFalse_VertexNotRemoved()
        {
            Tree t = new Tree();

            int childId = t.AddChild(0);
            t.AddChild(childId, 5);

            t.RemoveVertex(childId, false);
        }


        [TestMethod()]
        public void RemoveVertex_VertexHasChildrenAndReparentIsTrue_ChildrenHaveNewParentAndVertexRemoved()
        {
            Tree t = new Tree();

            t.AddChild(0);
            t.AddChild(0);
            t.AddChild(0);

            int reparentedChild1 = t.AddChild(1);
            int reparentedChild2 = t.AddChild(1);

            Assert.AreEqual(t.Children(0).Count,3);
            Assert.AreEqual(t.Children(1).Count, 2);
            Assert.AreEqual(t.Parent(reparentedChild1), 1);
            Assert.AreEqual(t.Parent(reparentedChild2), 1);

            t.RemoveVertex(1, true);

            Assert.AreEqual(t.Children(0).Count, 4); // Original Children (3) + New children (2) - Deleted Vertex (1)
            Assert.AreEqual(t.Parent(reparentedChild1), 0);
            Assert.AreEqual(t.Parent(reparentedChild2), 0);
            Assert.IsNull(t.Children(1));
            Assert.IsNull(t.Parent(1));
            CollectionAssert.Contains(t.Children(0), reparentedChild1);
            CollectionAssert.Contains(t.Children(0), reparentedChild2);

        }

        #endregion

        #region Tests of Siblings

        [TestMethod()]
        public void Siblings_VertexWithSiblings_ReturnsListOfSiblings()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root);
            int secondChild = t.AddChild(t.root);
            int thirdChild = t.AddChild(t.root);
            int fourthChild = t.AddChild(firstChild);

            List<int> expectedListOfChildren = new List<int>();
            expectedListOfChildren.Add(secondChild);
            expectedListOfChildren.Add(thirdChild);

            CollectionAssert.AreEqual(t.Siblings(1), expectedListOfChildren);
        }

        [TestMethod()]
        public void Siblings_VertexWithoutSiblings_ReturnsEmptyList()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root);
            List<int> expectedList = new List<int>();

            CollectionAssert.AreEqual(expectedList, t.Siblings(firstChild));

        }

        [TestMethod()]
        public void Siblings_VertexDoesntExist_ReturnsEmptyList()
        {
            Tree t = new Tree();

            List<int> expectedList = new List<int>();

            CollectionAssert.AreEqual(expectedList, t.Siblings(5));

        }

        [TestMethod()]
        public void NbSiblings_VertexWithTwoSiblings_ReturnsTwo()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root);
            int secondChild = t.AddChild(t.root);
            int thirdChild = t.AddChild(t.root);
            int fourthChild = t.AddChild(firstChild);

            Assert.AreEqual(t.NbSiblings(firstChild), 2);

        }

        [TestMethod()]
        public void NbSiblings_VertexWithNoSiblings_ReturnsZero()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root);

            Assert.AreEqual(t.NbSiblings(firstChild), 0);

        }

        [TestMethod()]
        public void NbSiblings_VertexDoesntExist_ReturnsZero()
        {
            Tree t = new Tree();

            Assert.AreEqual(t.NbSiblings(10), 0);

        }

        [TestMethod()]
        public void InsertSibling_AddSiblingToATree_SiblingInserted()
        {
            // Tree: {0 => 1 , 2} and {1 => 3 , 4}

            Tree tree = new Tree();

            int firstChild = tree.AddChild(tree.root);
            int secondChild = tree.AddChild(tree.root);

            int thirdChild = tree.AddChild(firstChild);
            int fourthChild = tree.AddChild(firstChild);

            // Assertions before inserting the sibling

            Assert.AreEqual(tree.root, tree.Parent(firstChild));
            Assert.AreEqual(tree.root, tree.Parent(secondChild));
            Assert.AreEqual(firstChild, tree.Parent(thirdChild));
            Assert.AreEqual(firstChild, tree.Parent(fourthChild));

            CollectionAssert.AreEqual(new List<int>() { firstChild, secondChild }, tree.Children(tree.root));
            CollectionAssert.AreEqual(new List<int>() { thirdChild, fourthChild }, tree.Children(firstChild));

            // Insert 2 siblings so that we have: {0 => 1,5,2} & {1 => 6,3,4}

            int firstSibling = tree.InsertSibling(secondChild);
            int secondSibling = tree.InsertSibling(thirdChild);

            // Verification of the parents

            Assert.AreEqual(tree.root, tree.Parent(firstSibling));
            Assert.AreEqual(tree.root, tree.Parent(secondChild));
            Assert.AreEqual(firstChild, tree.Parent(secondSibling));
            Assert.AreEqual(firstChild, tree.Parent(thirdChild));

            // Verification of the children (and the correct order)

            CollectionAssert.AreEqual(new List<int>() { firstChild, firstSibling, secondChild }, tree.Children(tree.root));
            CollectionAssert.AreEqual(new List<int>() { secondSibling, thirdChild, fourthChild }, tree.Children(firstChild));


        }

        #endregion

        #region Tests of InsertParent

        [TestMethod()]
        public void InsertParent_ParentWithFourChildren_ParentWithFourChildrenAndGrandChild()
        {
            Tree tree = new Tree();

            int firstChild = tree.AddChild(tree.root);
            int secondChild = tree.AddChild(tree.root);
            int thirdChild = tree.AddChild(tree.root);
            int fourthChild = tree.AddChild(tree.root);

            Assert.AreEqual(tree.root, tree.Parent(firstChild));
            Assert.AreEqual(tree.root, tree.Parent(secondChild));
            Assert.AreEqual(tree.root, tree.Parent(thirdChild));
            Assert.AreEqual(tree.root, tree.Parent(fourthChild));

            CollectionAssert.AreEqual(new List<int>() { firstChild, secondChild, thirdChild, fourthChild }, tree.Children(tree.root));

            int newParent = tree.InsertParent(thirdChild);

            Assert.AreEqual(tree.root, tree.Parent(firstChild));
            Assert.AreEqual(tree.root, tree.Parent(secondChild));
            Assert.AreEqual(newParent, tree.Parent(thirdChild));
            Assert.AreEqual(tree.root, tree.Parent(fourthChild));
            Assert.AreEqual(tree.root, tree.Parent(newParent));

            CollectionAssert.AreEqual(new List<int>() { firstChild, secondChild, newParent, fourthChild }, tree.Children(tree.root));
            CollectionAssert.AreEqual(new List<int>() { thirdChild }, tree.Children(newParent));

        }

        #endregion
    }
}
