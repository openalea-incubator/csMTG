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
        public int GetCursor()
        {
            return cursor;
        }

        /// <summary>
        /// Updates the value of the cursor.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier on which will be placed the cursor. </param>
        void SetCursor(int vertexId)
        {
            cursor = vertexId;
        }

        /// <summary>
        /// Retrieve the identifier of the canopy based on the position of the cursor.
        /// The cursor is set to the last item visited. If it's in the right scale (1), it is equal to the canopy's id.
        /// Otherwise, we will iteratively look for the complex of the cursor until scale 1 is reached.
        /// </summary>
        /// <returns> The identifier of the canopy. </returns>
        int GetCanopyId()
        {
            int canopyCursor = cursor;

            while (Scale(canopyCursor) != 1 && canopyCursor != 0)
            {
                canopyCursor = (int)Complex(canopyCursor);
            }

            if (canopyCursor == 0)
                canopyCursor = AddCanopy();

            return canopyCursor;
        }

        /// <summary>
        /// Retrieve the actual plant's identifier.
        /// The cursor's scale should be equal to 2.
        /// In case the cursor's scale is lower than 2, the plant needs to be created first.
        /// In case the cursor's scale is greater than 2, we will iteratively look for the complex until scale 2 is reached.
        /// </summary>
        /// <returns> The identifier of the plant. </returns>
        int GetPlantId()
        {
            int plantId;

            if (Scale(cursor) == 2)
                plantId = cursor;
            else
            {
                if (Scale(cursor) < 2)
                    plantId = AddPlant();
                else
                {
                    plantId = cursor;

                    while (Scale(plantId) != 2)
                        plantId = (int)Complex(plantId);

                }
            }

            return plantId;
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

        #region Internal functions

        /// <summary>
        /// Checks if the plants already has a shoot.
        /// </summary>
        /// <param name="plantId"> The plant to verify. </param>
        /// <returns> Whether it's true or not. </returns>
        bool PlantHasShoot(int plantId)
        {
            bool shootExists = false;

            if (Components(plantId).Count > 0)
            {
                foreach (int component in Components(plantId))
                {
                    if (GetVertexProperties(component)["label"].Substring(0, 5) == "shoot")
                        shootExists = true;
                }
            }
            
            return shootExists;
        }

        #endregion

        #region Editing functions (AddCanopy, AddPlant, AddShoot, AddRoot, AddAxis)

        /// <summary>
        /// Adds a canopy which will contain all plants.
        /// </summary>
        /// <param name="label"> Optional parameter. 
        /// Specified in case all plants are of the same botanical variety. </param>
        /// <returns> Identifier of the canopy added. </returns>
        public int AddCanopy(string label = "canopy")
        {
            int canopy;
            Dictionary<string, dynamic> canopyLabel = new Dictionary<string, dynamic>();

            canopyLabel.Add("label", label);

            canopy = AddComponent(0, canopyLabel);

            SetCursor(canopy);

            return canopy;
        }

        /// <summary>
        /// Add a plant to the canopy.
        /// It is to note that the plant is labelled plant+number of the plant (e.g: plant0, plant1, ..).
        /// </summary>
        /// <returns> Identifier of the plant. </returns>
        public int AddPlant()
        {

            int canopy = GetCanopyId();

            Dictionary<string,dynamic> plantLabel = new Dictionary<string,dynamic>();
            plantLabel.Add("label","plant"+nbPlants);
            plantLabel.Add("Edge_Type", "/");

            int plantId = AddComponent(canopy, namesValues: plantLabel);

            nbPlants++;

            SetCursor(plantId);

            return plantId;
        }

        /// <summary>
        /// Add a shoot to a plant.
        /// A naming convention would be that the shoot and the plant will have the same label number.
        /// (e.g: plant0 is decomposed into shoot0, plant1 into shoot1 and so on).
        /// </summary>
        /// <param name="plantId"> The plant to which the shoot will be added. </param>
        /// <returns> The identifier of the shoot created. </returns>
        public int AddShoot()
        {
            int plantId = GetPlantId();

            if (PlantHasShoot(plantId) == true)
                plantId = AddPlant();

            string plantNb = GetVertexProperties(plantId)["label"].Substring(5);

            Dictionary<string, dynamic> shootLabel = new Dictionary<string, dynamic>();
            shootLabel.Add("label", "shoot" + plantNb);
            shootLabel.Add("Edge_Type", "/");

            int shootId = AddComponent(plantId, shootLabel);

            SetCursor(shootId);

            return shootId;

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
