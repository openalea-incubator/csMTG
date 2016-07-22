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

        int cursor = 0;
        int nbPlants = 0;
        int canopyId;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of Gramene:
        /// * Sets the labels of the 6 scales of an mtg.
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

        }

        #endregion

        #region Cursor

        /// <summary>
        /// Gets the value of the cursor.
        /// </summary>
        /// <returns> Value of the cursor. </returns>
        public int getCursor()
        {
            return cursor;
        }

        /// <summary>
        /// Updates the value of the cursor.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier on which will be placed the cursor. </param>
        void setCursor(int vertexId)
        {
            cursor = vertexId;
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
        /// Adds a canopy which will contain all plants.
        /// </summary>
        /// <param name="label"> Optional parameter. 
        /// Specified in case all plants are of the same botanical variety. </param>
        /// <returns> Identifier of the canopy added. </returns>
        public int AddCanopy(string label = null)
        {
            int canopy;

            if (label != null)
            {
                Dictionary<string, dynamic> canopyLabel = new Dictionary<string, dynamic>();
                canopyLabel.Add("label", label);

                canopy = AddComponent(0, canopyLabel);

            }
            else
                canopy = AddComponent(0);

            setCursor(canopy);
            canopyId = canopy;

            return canopy;
        }

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
