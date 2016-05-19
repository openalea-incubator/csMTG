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

        
    }
}
