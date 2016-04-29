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
            foreach(string name in namesValues.Keys)
            {

                // Case there is no property with such a name yet.
                if (!properties.ContainsKey(name))
                    AddProperty(name);
                
                // ???? In case the vertex already has a value for this propriety: Change it or throw an exception ?
                properties[name].Add(vertexId, namesValues[name]);
                
            }
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

        static void Main(String[] args)
        {
        }
    }
}