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
    public class PropertyTreeTests
    {

        #region Tests of PropertyNames

        [TestMethod()]
        public void PropertyNames_NormalCase_ListOfNames()
        {
            PropertyTree test = new PropertyTree();
            
            test.properties.Add("label", new Dictionary<int, dynamic>());
            test.properties.Add("length", new Dictionary<int, dynamic>());
            test.properties["label"].Add(1, "hello");
            test.properties["length"].Add(12, 12.5);

            List<string> listOfProperties = new List<string>() { "label", "length" };

            CollectionAssert.AreEqual(test.PropertyNames(), listOfProperties);

        }

        [TestMethod()]
        public void PropertyNames_NoProperties_EmptyList()
        {
            PropertyTree tree = new PropertyTree();

            List<string> expectedList = new List<string>() { };

            CollectionAssert.AreEqual(tree.PropertyNames(), expectedList);
            
        }

        #endregion

        #region Tests of Property(string)

        [TestMethod()]
        public void Property_NameExists_CorrectDictionary()
        {
            PropertyTree tree = new PropertyTree();

            // Add properties to the tree
             
            tree.properties.Add("label", new Dictionary<int, dynamic>());
            tree.properties.Add("length", new Dictionary<int, dynamic>());
            tree.properties["label"].Add(1, "hello");
            tree.properties["length"].Add(12, 12.5);
            tree.properties["length"].Add(10, 7.5);
            tree.properties["length"].Add(15, 12);

            // Compare expected data and the result of the function for field "length"

            Dictionary<int, dynamic> expectedResult = new Dictionary<int, dynamic>();
            expectedResult.Add(12, 12.5);
            expectedResult.Add(10, 7.5);
            expectedResult.Add(15, 12);

            Dictionary<int, dynamic> listOfProperties = tree.Property("length");
            
            CollectionAssert.AreEqual(listOfProperties, expectedResult);
            
            // Compare expected data and the result of the function for field "label"

            Dictionary<int, dynamic> listOfProperties2 = tree.Property("label");
            Dictionary<int, dynamic> expectedResult2 = new Dictionary<int, dynamic>();

            expectedResult2.Add(1, "hello");

            CollectionAssert.AreEqual(listOfProperties2, expectedResult2);
        }

        [TestMethod()]
        public void Property_InvalidName_EmptyDictionary()
        {
            PropertyTree tree = new PropertyTree();

            Dictionary<int,dynamic> expectedList = new Dictionary<int, dynamic>() { };

            CollectionAssert.AreEqual(tree.Property("width"), expectedList);

        }
        #endregion

        #region Tests of AddProperty

        [TestMethod()]
        public void AddProperty_NewProperty_NewKeyAdded()
        {
            PropertyTree tree = new PropertyTree();

            CollectionAssert.DoesNotContain(tree.properties.Keys, "label");

            tree.AddProperty("label");
            CollectionAssert.Contains(tree.properties.Keys, "label");

            Dictionary<int, dynamic> emptyDictionary = new Dictionary<int, dynamic>() { };
            CollectionAssert.AreEqual(tree.properties["label"], emptyDictionary);

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "This key already exists.")]
        public void AddProperty_PropertyAlreadyExists_ExceptionThrown()
        {
            PropertyTree tree = new PropertyTree();
            tree.properties.Add("label", new Dictionary<int, dynamic>());

            CollectionAssert.Contains(tree.properties.Keys, "label");
            tree.AddProperty("label");
            
        }

        #endregion

        #region Tests of RemoveProperty

        [TestMethod()]
        public void RemoveProperty_PropertyExists_PropertyRemoved()
        {
            PropertyTree tree = new PropertyTree();

            tree.AddProperty("label");
            Assert.IsTrue(tree.properties.ContainsKey("label"));

            tree.properties["label"].Add(1, "element");

            tree.RemoveProperty("label");
            Assert.IsFalse(tree.properties.ContainsKey("label"));
            
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "Property doesn't exist.")]
        public void RemoveProperty_PropertyDoesntExist_ExceptionThrown()
        {
            PropertyTree tree = new PropertyTree();
            tree.RemoveProperty("label");

        }

        #endregion

    }
}