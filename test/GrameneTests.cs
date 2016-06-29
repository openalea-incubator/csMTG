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

            CollectionAssert.AreEqual(new List<int>() { idOfCanopy }, g.Components(0));

        }

        #endregion
    }
}
