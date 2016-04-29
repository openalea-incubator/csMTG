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
            
            Dictionary<int,dynamic> labelDictionary = new Dictionary<int, dynamic>();

            test.properties.Add("label", labelDictionary);
            test.properties["label"].Add(1, "hello");
            test.properties.Add("length", labelDictionary);
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




    }
}