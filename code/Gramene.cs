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

        /// <summary>
        /// Retrieve the actual shoot's identifier.
        /// The cursor's scale should be equal to 3.
        /// In case the cursor's scale is lower than 3, the shoot needs to be created first.
        /// In case the cursor's scale is greater than 2, we will iteratively look for the complex until scale 3 is reached.
        /// </summary>
        /// <returns> The identifier of the shoot. </returns>
        int GetShootId()
        {
            int shootId = cursor;

            if (Scale(shootId) == 3)
            {
                int plantId = (int)Complex(shootId);
                
                shootId = PlantHasShoot(plantId);

                if (shootId == 0)
                    AddShoot();
            }
            else
            {
                if (Scale(shootId) < 3)
                    AddShoot();
                else
                {
                    while (Scale(shootId) != 3)
                        shootId = (int)Complex(shootId);
                }
            }

            return shootId;

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
        /// <returns> The identifier of the shoot if found. If not, it returns zero. </returns>
        int PlantHasShoot(int plantId)
        {
            int shootExists = 0;

            if (Components(plantId).Count > 0)
            {
                foreach (int component in Components(plantId))
                {
                    if (GetVertexProperties(component)["label"].Substring(0, 5) == "shoot")
                        shootExists = component;
                }
            }
            
            return shootExists;
        }

        /// <summary>
        /// Checks if the plants already has a root.
        /// </summary>
        /// <param name="plantId"> The plant to verify. </param>
        /// <returns> The identifier of the root if found. If not, it returns zero. </returns>
        int PlantHasRoot(int plantId)
        {
            int rootExists = 0;

            if (Components(plantId).Count > 0)
            {
                foreach (int component in Components(plantId))
                {
                    if (GetVertexProperties(component)["label"].Substring(0, 4) == "root")
                        rootExists = component;
                }
            }

            return rootExists;
        }

        /// <summary>
        /// Checks if the shoot already has a mainstem or not.
        /// </summary>
        /// <param name="shootId"> Identifier of the shoot. </param>
        /// <returns> Identifier of the mainstem if found. If not, it returns zero. </returns>
        int ShootHasMainstem(int shootId)
        {
            int mainstemId = 0;

            if (Components(shootId).Count > 0)
            {
                foreach (int component in Components(shootId))
                {
                    if (GetVertexProperties(component)["label"].Substring(0, 8) == "mainstem")
                        mainstemId = component;
                }
            }

            return mainstemId;
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
        /// <returns> The identifier of the shoot created. </returns>
        public int AddShoot()
        {
            int plantId = GetPlantId();

            if (PlantHasShoot(plantId) != 0)
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
        /// A naming convention would be that the root and the plant will have the same label number.
        /// (e.g: plant0 is decomposed into root0, plant1 into root1 and so on).
        /// </summary>
        /// <returns> The identifier of the root created. </returns>
        public int AddRoot()
        {
            int plantId = GetPlantId();

            if (PlantHasRoot(plantId) != 0)
                plantId = AddPlant();

            string plantNb = GetVertexProperties(plantId)["label"].Substring(5);

            Dictionary<string, dynamic> rootLabel = new Dictionary<string, dynamic>();
            rootLabel.Add("label", "root" + plantNb);
            rootLabel.Add("Edge_Type", "/");

            int rootId = AddComponent(plantId, rootLabel);

            SetCursor(rootId);

            return rootId;
             
        }

        /// <summary>
        /// Adds an axis to the plant.
        /// If the plant doesn't have a mainstem, it creates one. Its label is: "mainstem".
        /// If the plant already has one, it adds an axis on the mainstem. Its label is: "axis"+number of the axis.
        /// </summary>
        /// <returns> The identifier of the new axis added. </returns>
        public int AddAxis()
        {
            int axisId;

            int shootId = GetShootId();

            int mainstemId = ShootHasMainstem(shootId);

            Dictionary<string, dynamic> axisLabel = new Dictionary<string, dynamic>();

            if (mainstemId == 0)
            {
                axisLabel.Add("label", "mainstem");
                axisLabel.Add("Edge_Type", "/");

                axisId = AddComponent(shootId, axisLabel);

            }
            else
            {
                int axisNumber = NbChildren(mainstemId) + 1;

                axisLabel.Add("label", "axis"+axisNumber);
                axisLabel.Add("Edge_Type", "+");

                axisId = AddChild(mainstemId);
            }

            return axisId;

        }

        #endregion

        static void Main(String[] args)
        {


        }

    }
}
