using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{

    public class PropertyTree : Tree
    {
        // Attribute which carries all properties of the tree
        public Dictionary<string, Dictionary<int, dynamic>> properties;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyTree()
        {
            properties = new Dictionary<string, Dictionary<int, dynamic>>();
        }

        /// <summary>
        /// Accessor to the attribute properties.
        /// </summary>
        /// <returns> Returns all the properties, the vertices and their properties' values. </returns>
        public Dictionary<string,Dictionary<int,dynamic>> Properties()
        {
            return properties;
        }

        /// <summary>
        /// Lists all the names of the properties in the dictionary.
        /// </summary>
        /// <returns> A list of the properties' names. </returns>
        public List<string> PropertyNames()
        {
            List<string> names = new List<string>(properties.Keys);

            return names;
        }
        
        /// <summary>
        /// Returns a property map between the vid and the data for the property in the parameters
        /// </summary>
        /// <param name="name"> Name of the property. </param>
        /// <returns> A dictionary of { vertexId : value of the property } </returns>
        public Dictionary<int,dynamic> Property(string name)
        {
            Dictionary<int, dynamic> propertyMap;

            if (!properties.TryGetValue(name,out propertyMap))
            {
                propertyMap = new Dictionary<int, dynamic>();
            }
            
            return propertyMap;

        }

        /// <summary>
        /// Adds a new key to the properties.
        /// <para> If the name already exists, it throws an exception.</para>
        /// </summary>
        /// <param name="name"> The name of the property to add. </param>
        public void AddProperty(string name)
        {
            Dictionary<int, dynamic> value;
            if (!properties.TryGetValue(name, out value))
            {
                properties.Add(name, new Dictionary<int, dynamic>());
            }
            else
                throw new ArgumentException("This key already exists.");
        }

        /// <summary>
        ///  Removes the property from the tree.
        /// </summary>
        /// <para> If the name of the parameter doesn't exist, it throws an exception. </para>
        /// <param name="name"> The name of the parameter to be removed. </param>
        public void RemoveProperty(string name)
        {
            Dictionary<int, dynamic> value;
            if (properties.TryGetValue(name, out value))
            {
                properties.Remove(name);
            }
            else
                throw new ArgumentException("Property doesn't exist. ");
        }

        /// <summary>
        ///  Adds a set of properties to the specified vertex.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex to which properties will be added. </param>
        /// <param name="namesValues"> A dictionary of names of the properties and their values for the vertex. </param>
        public void AddVertexProperties(int vertexId, Dictionary<string,dynamic> namesValues)
        {
            if (base.parent.ContainsKey(vertexId))
            {
                foreach (string name in namesValues.Keys)
                {

                    // Case there is no property with such a name yet.
                    if (!properties.ContainsKey(name))
                        AddProperty(name);

                    // Case the vertex already has a property with the same name.
                    if (properties[name].ContainsKey(vertexId))
                        properties[name][vertexId] = namesValues[name];
                    else
                        properties[name].Add(vertexId, namesValues[name]);
                }
            }
            else
                throw new ArgumentOutOfRangeException("vertexId", "This vertex does not exist");
        }

        /// <summary>
        /// Removes all properties for a specific vertex.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex whose properties will be removed.</param>
        public void RemoveVertexProperties(int vertexId)
        {
            foreach(string name in properties.Keys)
            {
                Dictionary<int, dynamic> props = properties[name];

                if (props.ContainsKey(vertexId))
                    properties[name].Remove(vertexId);
            }
        }

        /// <summary>
        /// Returns all properties for a vertex.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex in question. </param>
        /// <returns> A dictionary of { name of property : value for the vertex } </returns>
        public Dictionary<string,dynamic> GetVertexProperties(int vertexId)
        {
            Dictionary<string, dynamic> vertexProperties = new Dictionary<string, dynamic>();
            dynamic value;

            foreach(string name in properties.Keys)
            {
                if(properties[name].TryGetValue(vertexId, out value))
                    vertexProperties.Add(name, value);
            }

            return vertexProperties;
        }

        /// <summary>
        /// Adds a child along with its properties
        /// </summary>
        /// <param name="parentId"> The parent's identifier. </param>
        /// <param name="namesValues"> The dictionary containing the names and values of the properties for the child vertex. </param>
        /// <param name="childId"> The identifier of the child. (Optional) </param>
        /// <returns> Returns the identifier of the child. </returns>
        public int AddChild(int parentId, Dictionary<string, dynamic> namesValues, int childId = -1)
        {

            int child = base.AddChild(parentId,childId);
            if(child != -1)
                AddVertexProperties(child, namesValues);

            return child;
        }

        static void Main(String[] args)
        {

        }
    }
}
