using Microsoft.VisualStudio.TestTools.UnitTesting;
using csMTG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
            test.properties["label"][1]= "hello";
            test.properties["length"][12] = 12.5;

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
        public void RemoveProperty_PropertyWithNoValues_PropertyRemoved()
        {
            PropertyTree tree = new PropertyTree();

            tree.AddProperty("label");
            Assert.IsTrue(tree.properties.ContainsKey("label"));

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

        #region Tests of AddVertexProperties

        [TestMethod()]
        public void AddVertexProperties_NewProperties_PropertiesAndValueAdded()
        {
            PropertyTree tree = new PropertyTree();
            tree.AddChild(tree.root, 1);

            Dictionary<string, dynamic> propertyDict = new Dictionary<string, dynamic>();
            propertyDict.Add("label", "leaf");
            propertyDict.Add("length", 12.5);
            propertyDict.Add("order", 1);

            tree.AddVertexProperties(1, propertyDict);

            Dictionary<string, Dictionary<int, dynamic>> expectedResult = new Dictionary<string, Dictionary<int, dynamic>>();
            Dictionary<int, dynamic> firstRow = new Dictionary<int, dynamic>() { { 1, "leaf" } };
            Dictionary<int, dynamic> secondRow = new Dictionary<int, dynamic>() { { 1, 12.5 } };
            Dictionary<int, dynamic> thirdRow = new Dictionary<int, dynamic>() { { 1, 1 }};

            expectedResult.Add("label", firstRow);
            expectedResult.Add("length", secondRow);
            expectedResult.Add("order", thirdRow);

            CollectionAssert.AreEqual(expectedResult.Keys, tree.properties.Keys);
            CollectionAssert.AreEqual(expectedResult["label"], tree.properties["label"]);
            CollectionAssert.AreEqual(expectedResult["length"], tree.properties["length"]);
            CollectionAssert.AreEqual(expectedResult["order"], tree.properties["order"]);

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Property doesn't exist.")]
        public void AddVertexProperties_VertexDoesntExist_ExceptionThrown()
        {
            PropertyTree tree = new PropertyTree();
            Dictionary<string, dynamic> propertyDict = new Dictionary<string, dynamic>();
            propertyDict.Add("label", "leaf");
            propertyDict.Add("length", 12.5);
            propertyDict.Add("order", 1);

            tree.AddVertexProperties(1, propertyDict);

        }


        [TestMethod()]
        public void AddVertexProperties_ExistingPropertyForId_PropertyChanged()
        {
            PropertyTree tree = new PropertyTree();
            tree.AddChild(tree.root, 1);
            tree.AddProperty("label");
            tree.properties["label"].Add(1, "wrong");

            // Check that the value of the label property is < 1 , "wrong" >
            Assert.IsTrue(tree.properties.ContainsKey("label"));
            Dictionary<int, dynamic> value = new Dictionary<int, dynamic>();
            value.Add(1, "wrong");

            CollectionAssert.AreEqual(tree.properties["label"], value);

            // Add the same property for a vertex with a new value
            Dictionary<string, dynamic> propertyDict = new Dictionary<string, dynamic>();
            propertyDict.Add("label", "leaf");
            propertyDict.Add("length", 12.5);

            tree.AddVertexProperties(1, propertyDict);

            // Check that the value has been changed
            Assert.IsTrue(tree.properties["label"].ContainsKey(1));
            Dictionary<int, dynamic> newValue = new Dictionary<int, dynamic>();
            newValue.Add(1, "leaf");

            CollectionAssert.AreEqual(tree.properties["label"], newValue);
        }

        #endregion

        #region Tests of RemoveVertexProperties

        [TestMethod()]
        public void RemoveVertexProperties_NormalCase_NoPropertiesForTheVid()
        {
            PropertyTree tree = new PropertyTree();
            tree.AddChild(tree.root,1);
            tree.AddChild(1,2);

            Dictionary<string, dynamic> props = new Dictionary<string, dynamic>();
            props.Add("label", "leaf");
            props.Add("height", 12.5);

            tree.AddVertexProperties(1,props);
            tree.AddVertexProperties(2, props);

            CollectionAssert.Contains(tree.properties["label"].Keys, 1);

            tree.RemoveVertexProperties(1);
            CollectionAssert.DoesNotContain(tree.properties["label"].Keys, 1);
            CollectionAssert.DoesNotContain(tree.properties["height"].Keys, 1);
            CollectionAssert.Contains(tree.properties["label"].Keys, 2);
            CollectionAssert.Contains(tree.properties["height"].Keys, 2);

        }
        
        [TestMethod()]
        public void RemoveVertexProperties_NoProperties_NoProblem()
        {
            PropertyTree tree = new PropertyTree();

            tree.RemoveVertexProperties(1);

            CollectionAssert.AreEqual(tree.properties, new Dictionary<string, Dictionary<int, dynamic>>());
        }

        [TestMethod()]
        public void RemoveVertexProperties_VertexDoesntExist_NoProblem()
        {
            PropertyTree tree = new PropertyTree();
            tree.AddChild(tree.root,1);
            tree.AddChild(tree.root, 2);

            Dictionary<string, dynamic> props = new Dictionary<string, dynamic>();
            props.Add("label", "leaf");
            props.Add("height", 12.5);
            
            tree.AddVertexProperties(2, props);
            
            tree.RemoveVertexProperties(1);

            CollectionAssert.DoesNotContain(tree.properties["label"].Keys, 1);
            CollectionAssert.DoesNotContain(tree.properties["height"].Keys, 1);
            CollectionAssert.Contains(tree.properties["label"].Keys, 2);
            CollectionAssert.Contains(tree.properties["height"].Keys, 2);
        }

        #endregion

        #region Tests of GetVertexProperties

        [TestMethod()]
        public void GetVertexProperties_VertexWithProperties_CorrectDict()
        {
            PropertyTree tree = new PropertyTree();
            tree.AddChild(tree.root, 1);

            Dictionary<string, dynamic> props = new Dictionary<string, dynamic>();
            props.Add("label", "leaf");
            props.Add("height", 12.5);

            tree.AddVertexProperties(1, props);
            
            CollectionAssert.AreEqual(tree.GetVertexProperties(1), props);

        }

        [TestMethod()]
        public void GetVertexProperties_VertexWithNoProperties_EmptyDict()
        {
            PropertyTree tree = new PropertyTree();

            CollectionAssert.AreEqual(tree.GetVertexProperties(1), new Dictionary<string, dynamic>() { });

        }
        #endregion

        #region Tests of AddChild

        [TestMethod()]
        public void AddChild_NormalCase_ChildAndPropertiesAdded()
        {
            PropertyTree tree = new PropertyTree();

            // The properties to add
            Dictionary<string, dynamic> propertyDict = new Dictionary<string, dynamic>();
            propertyDict.Add("label", "leaf");
            propertyDict.Add("length", 12.5);
            propertyDict.Add("order", 1);

            int childId = tree.AddChild(tree.root, propertyDict);

            // Make sure the child was added
            Assert.IsTrue(tree.Children(tree.root).Contains(childId));
            Assert.AreEqual(tree.Children(tree.root).Count(), 1);
            Assert.AreEqual(tree.Parent(childId), tree.root);

            // Make sure the properties were correctly added
            Dictionary<string, Dictionary<int, dynamic>> expectedResult = new Dictionary<string, Dictionary<int, dynamic>>();
            Dictionary<int, dynamic> firstRow = new Dictionary<int, dynamic>() { { childId, "leaf" } };
            Dictionary<int, dynamic> secondRow = new Dictionary<int, dynamic>() { { childId, 12.5 } };
            Dictionary<int, dynamic> thirdRow = new Dictionary<int, dynamic>() { { childId, 1 } };

            expectedResult.Add("label", firstRow);
            expectedResult.Add("length", secondRow);
            expectedResult.Add("order", thirdRow);

            CollectionAssert.AreEqual(expectedResult.Keys, tree.properties.Keys);
            CollectionAssert.AreEqual(expectedResult["label"], tree.properties["label"]);
            CollectionAssert.AreEqual(expectedResult["length"], tree.properties["length"]);
            CollectionAssert.AreEqual(expectedResult["order"], tree.properties["order"]);
        }

        //[TestMethod()]
        //public void AddChild_InvalidParent_ReturnsMinusOneAndNoPropertiesAdded()
        //{
        //    PropertyTree tree = new PropertyTree();

        //    // The properties to add
        //    Dictionary<string, dynamic> propertyDict = new Dictionary<string, dynamic>();
        //    propertyDict.Add("label", "leaf");
        //    propertyDict.Add("length", 12.5);
        //    propertyDict.Add("order", 1);

        //    int childId = tree.AddChild(100, propertyDict);

        //    // Make sure the child wasn't added
        //    Assert.IsNull(tree.Children(100));
        //    Assert.AreEqual(childId, -1);

        //    // Make sure no properties were added
        //    Dictionary<string, Dictionary<int, dynamic>> expectedProperties = new Dictionary<string, Dictionary<int, dynamic>>() { };
        //    CollectionAssert.AreEqual(tree.properties, expectedProperties);
        //}

        [TestMethod()]
        public void AddChild_NoProperties_ChildAddedAndNoPropertiesForTheChild()
        {
            PropertyTree tree = new PropertyTree();
            
            Dictionary<string, dynamic> propertyDict = new Dictionary<string, dynamic>() { };

            int childId = tree.AddChild(tree.root, propertyDict);

            // Make sure the child was added
            Assert.IsTrue(tree.Children(tree.root).Contains(childId));
            Assert.AreEqual(tree.Children(tree.root).Count(), 1);
            Assert.AreEqual(tree.Parent(childId), tree.root);

            // Make sure the dictionary is empty
            Dictionary<string, Dictionary<int, dynamic>> expectedProperties = new Dictionary<string, Dictionary<int, dynamic>>() { };
            CollectionAssert.AreEqual(tree.properties, expectedProperties);
        }


        #endregion

        #region Tests of InsertSibling

        [TestMethod()]
        public void InsertSibling_AddSiblingToATree_SiblingAndPropertiesAdded()
        {
            // PropertyTree: {0 => 1 , 2} and {1 => 3 , 4}

            PropertyTree tree = new PropertyTree();

            int firstChild = tree.AddChild(tree.root);
            int secondChild = tree.AddChild(tree.root);

            int thirdChild = tree.AddChild(firstChild);
            int fourthChild = tree.AddChild(firstChild);

            // Add siblings so that: {0 => 1,5,2} & {1 => 6,3,4}

            int firstSibling = tree.InsertSibling(secondChild, namesValues: new Dictionary<string, dynamic>() { { "Edge_Type", "<" } });
            int secondSibling = tree.InsertSibling(thirdChild, namesValues: new Dictionary<string, dynamic>() { { "Edge_Type", "+" } });

            // Verification of the parents

            Assert.AreEqual(tree.root, tree.Parent(firstSibling));
            Assert.AreEqual(tree.root, tree.Parent(secondChild));
            Assert.AreEqual(firstChild, tree.Parent(secondSibling));
            Assert.AreEqual(firstChild, tree.Parent(thirdChild));

            // Verification of the children (and the correct order)

            CollectionAssert.AreEqual(new List<int>() { firstChild, firstSibling, secondChild }, tree.Children(tree.root));
            CollectionAssert.AreEqual(new List<int>() { secondSibling, thirdChild, fourthChild }, tree.Children(firstChild));

            // Verification of the insertion of the properties

            Assert.IsTrue(tree.properties.ContainsKey("Edge_Type"));

            Dictionary<string, dynamic> props = new Dictionary<string, dynamic>();
            props.Add("Edge_Type", "<");

            CollectionAssert.AreEqual(tree.GetVertexProperties(firstSibling), props);

            props["Edge_Type"] = "+";
            CollectionAssert.AreEqual(tree.GetVertexProperties(secondSibling), props);

        }

        #endregion

    }
}
