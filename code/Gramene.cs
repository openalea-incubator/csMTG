using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    public class Gramene : mtg
    {
        // Attributes

        public Dictionary<int, string> labelsOfScales;

        // We may also have attributes that would stand for memory attributes (Plant, stem, last internode)

        #region Constructor

        /// <summary>
        /// Constructor of Gramene:
        /// * Sets the labels of the 6 scales of an mtg.
        /// * Creates an mtg composed of one element (canopy).
        /// </summary>
        public Gramene()
        {
            // define the 6 scales

            labelsOfScales = new Dictionary<int, string>();
            labelsOfScales.Add(1, "canopy");
            labelsOfScales.Add(2, "plant");
            labelsOfScales.Add(3, "system");
            labelsOfScales.Add(4, "axis");
            labelsOfScales.Add(5, "botanical_unit");
            labelsOfScales.Add(6, "organ");

            // add a canopy

            AddComponent(0, componentId: 1);

        }

        #endregion

        // Accessors (g.Plants , g.Stems, ... They will all return a vid or a list of vids)

        // Edition (AddPlant, AddStem,...)

        // Properties (We'll have in the parameters "vid to which the properties will be added + the properties to add")

        // Query functions


    }
}
