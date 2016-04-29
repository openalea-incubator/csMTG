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


    }
}