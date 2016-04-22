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
        [TestMethod()]
        public void Tree_NewTree_RootCreated()
        {
            Tree t = new Tree();
            int root = t.children.Keys.First();
            Assert.AreEqual(root, 0);
        }

        [TestMethod()]
        public void Count_NewTreeCreated_Returns1()
        {
            Tree t = new Tree();
            Assert.AreEqual(t.Count(), 1);
        }

        [TestMethod()]
        public void AddChild_NormalScenario_OneChildAdded()
        {
            Tree t = new Tree();
            t.AddChild(0);

            int childId = t.id;

            Assert.IsTrue(t.children[0].Contains(childId));
            Assert.AreEqual(t.children[0].Count(), 1);
            Assert.AreEqual(t.parent[childId], 0);
        }

        [TestMethod()]
        public void AddChild_ChildIdDoesntExist_SpecificIdCreated()
        {
            Tree t = new Tree();

            int childId = t.AddChild(0, 5);

            Assert.IsTrue(t.children[0].Contains(childId));
            Assert.AreEqual(t.children[0].Count(), 1);
            Assert.AreEqual(t.parent[childId], 0);
        }

        [TestMethod()]
        public void AddChild_SpecifyChildId_IdIsNotRepeated()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root); //Should be equal to 1
            int secondChild = t.AddChild(t.root, 2); //Should be equal to 2
            int thirdChild = t.AddChild(t.root); //Should be equal to 3

            //All children have been added to the root
            Assert.AreEqual(t.children[t.root].Count(), 3);
            Assert.IsTrue(t.children[t.root].Contains(firstChild));
            Assert.IsTrue(t.children[t.root].Contains(secondChild));
            Assert.IsTrue(t.children[t.root].Contains(thirdChild));

            //All children have the right id (2 isn't repeated)
            Assert.AreEqual(firstChild, 1);
            Assert.AreEqual(secondChild, 2);
            Assert.AreEqual(thirdChild, 3);

            //All children have the right parent
            Assert.AreEqual(t.parent[firstChild], t.root);
            Assert.AreEqual(t.parent[secondChild], t.root);
            Assert.AreEqual(t.parent[thirdChild], t.root);
        }

        [TestMethod()]
        public void AddChild_ChildAlreadyExists_ParentIsChanged()
        {
            Tree t = new Tree();

            int firstChild = t.AddChild(t.root);
            int secondChild = t.AddChild(firstChild, 2);
            int thirdChild = t.AddChild(t.root, secondChild);

            //All children have been added to the right parent
            Assert.AreEqual(t.children[t.root].Count(), 2);
            Assert.IsTrue(t.children[t.root].Contains(firstChild));
            Assert.IsTrue(t.children[t.root].Contains(thirdChild));
            Assert.IsFalse(t.children[firstChild].Contains(secondChild));

            //All children have the right id (2 isn't repeated)
            Assert.AreEqual(firstChild, 1);
            Assert.AreEqual(secondChild, 2);
            Assert.AreEqual(thirdChild, 2);

            //All children have the right parent
            Assert.AreEqual(t.parent[firstChild], t.root);
            Assert.AreEqual(t.parent[secondChild], t.root);
            Assert.AreEqual(t.parent[thirdChild], t.root);
        }

        [TestMethod()]
        public void AddChild_ParentDoesntExist_ChildNotAdded()
        {
            Tree t = new Tree();

            int childId = t.AddChild(1);

            //Child hasn't been added
            Assert.IsFalse(t.children.ContainsKey(1));

            //An id hasn't been attributed
            Assert.AreEqual(childId, -1);
        }
    }
}