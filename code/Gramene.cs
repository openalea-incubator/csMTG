using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{
    public class Gramene : mtg
    {
        #region Attributes

        public Dictionary<int, string> labelsOfScales;

        int nbPlants = 0;
        int canopyId;

        // We may also have attributes that would stand for memory attributes (Plant, stem, last internode)

        #endregion

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

            canopyId = AddComponent(0, componentId: 1);

        }

        #endregion

        #region Accessors

        /// <summary>
        /// Returns a list containing the identifiers of all plants.
        /// It is to note that plants are in scale number 2.
        /// </summary>
        /// <returns></returns>
        public List<int> Plants()
        {
            return Vertices(2);
        }



        #endregion


        #region Editing functions (AddPlant, AddShoot, AddRoot, AddAxis)

        /// <summary>
        /// Add a plant to the canopy.
        /// It is to note that the plant is labelled plant+number of the plant (e.g: plant0, plant1, ..).
        /// </summary>
        /// <returns> Identifier of the plant. </returns>
        public int AddPlant()
        {
            Dictionary<string,dynamic> plantLabel = new Dictionary<string,dynamic>();
            plantLabel.Add("label","plant"+nbPlants);

            nbPlants++;

            int plantId = AddComponent(canopyId, namesValues: plantLabel);

            return plantId;
        }

        /// <summary>
        /// Add a shoot to a plant.
        /// </summary>
        /// <param name="plantId"> The plant to which the shoot will be added. </param>
        /// <returns> The identifier of the shoot created. </returns>
        public int AddShoot(int plantId)
        {
            if (HasVertex(plantId))
            {
                Dictionary<string, dynamic> shootLabel = new Dictionary<string, dynamic>();
                shootLabel.Add("label", "shoot" + plantId);

                int shootId = AddComponent(plantId, shootLabel);

                return shootId;

            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Add a root to a plant.
        /// </summary>
        /// <param name="plantId"> The plant to which the root will be added. </param>
        /// <returns> The identifier of the root created. </returns>
        public int AddRoot(int plantId)
        {
            if (HasVertex(plantId))
            {
                Dictionary<string, dynamic> rootLabel = new Dictionary<string, dynamic>();
                rootLabel.Add("label", "root" + plantId);

                int rootId = AddComponent(plantId, rootLabel);

                return rootId;

            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Add an axis to a shoot. Its label is: "axis"+plantId.
        /// </summary>
        /// <param name="shootId"> The shoot to which the axis will be added. </param>
        /// <returns> The identifier of the axis created. </returns>
        public int AddAxis(int shootId)
        {
            if (HasVertex(shootId))
            {

                string plantId = GetVertexProperties(shootId)["label"].Substring(5);

                Dictionary<string, dynamic> axisLabel = new Dictionary<string, dynamic>();
                axisLabel.Add("label", "axis" + plantId);

                int axisId = AddComponent(shootId, axisLabel);

                return axisId;

            }
            else
            {
                return 0;
            }

        }



        #endregion

        // Properties (We'll have in the parameters "vid to which the properties will be added + the properties to add")

        // Query functions

        // Main

        static void Main(String[] args)
        {

        }

    }
}
