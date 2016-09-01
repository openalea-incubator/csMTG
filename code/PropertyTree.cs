using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csMTG
{

    public class PropertyTree : Tree
    {

        #region Attributes

        // Attribute which carries all properties of the tree
        public Dictionary<string, Dictionary<int, dynamic>> properties;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PropertyTree()
        {
            properties = new Dictionary<string, Dictionary<int, dynamic>>();
        }

        #endregion

        #region Properties' accessors (Functions: Properties, PropertyNames, Property(name))

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

        #endregion

        #region Add and remove a property

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

        #endregion

        #region Add, remove and get the properties of a vertex

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

        #endregion

        #region Functions related to vertices (AddChild, RemoveVertex, InsertParent, Clear)

        /// <summary>
        /// Adds a child along with its properties
        /// </summary>
        /// <param name="parentId"> The parent's identifier. </param>
        /// <param name="namesValues"> The dictionary containing the names and values of the properties for the child vertex. </param>
        /// <param name="childId"> The identifier of the child. (Optional) </param>
        /// <returns> Returns the identifier of the child. </returns>
        public int AddChild(int parentId, Dictionary<string, dynamic> namesValues = null, int childId = -1)
        {

            int child = base.AddChild(parentId,childId);
            if(child != -1 && namesValues != null)
                AddVertexProperties(child, namesValues);

            return child;
        }

        /// <summary>
        /// Overrides the function: RemoveVertex in Tree.
        /// </summary>
        /// <param name="vertexId"> The identifier of the vertex. </param>
        /// <param name="reparentChild"> A boolean to indicate whether children should be reparented or not. </param>
        public new void RemoveVertex(int vertexId, bool reparentChild)
        {
            base.RemoveVertex(vertexId, reparentChild);
            RemoveVertexProperties(vertexId);
        }

        /// <summary>
        /// Insert a parent with properties between a vertex and its actual parent.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="namesValues"> The new parent's properties. </param>
        /// <param name="parentId"> Parent identifier. </param>
        /// <returns> The new parent's identifier. </returns>
        public int InsertParent(int vertexId, int parentId = -1, Dictionary<string, dynamic> namesValues = null)
        {
            parentId = base.InsertParent(vertexId, parentId);

            if(namesValues != null)
                AddVertexProperties(parentId, namesValues);

            return parentId;
        }

        /// <summary>
        /// Remove all properties.
        /// </summary>
        public new void Clear()
        {
            base.Clear();

            properties.Clear();
        }

        #endregion

        #region Siblings (InsertSibling)

        /// <summary>
        /// Add a sibling with its properties before the vertex in the parameters.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="namesValues"> The properties of the new vertex. </param>
        /// <param name="vertexToInsert"> The new vertex to add. </param>
        /// <returns> The identifier of the new sibling. </returns>
        public int InsertSibling(int vertexId, int vertexToInsert = -1, Dictionary<string, dynamic> namesValues = null)
        {
            vertexToInsert = base.InsertSibling(vertexId, vertexToInsert);

            if(namesValues != null)
                AddVertexProperties(vertexToInsert, namesValues);

            return vertexToInsert;
        }
       

        #endregion

        #region SubTree, InsertSiblingTree, AddChildTree, RemoveTree

        /// <summary>
        /// Return the subtree rooted on the vertex in the parameters.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="copy"> If true: return a new tree holding the subtree.
        /// If false: The subtree is created using the original tree. </param>
        public new PropertyTree SubTree(int vertexId, bool copy = true)
        {
            traversal t = new traversal();

            if (!copy)
            {
                IEnumerable<int> bunch = t.RecursivePreOrder((mtg)this, vertexId);
                IEnumerable<int> removeBunch = this.Vertices().Except(bunch);

                foreach (int vid in removeBunch)
                {
                    RemoveVertexProperties(vid);

                    // Remove parent edge

                    int parentId = (int)Parent(vid);

                    if (parentId != -1)
                    {
                        children[parentId].Remove(vid);
                        parent.Remove(vid);
                    }

                    // Remove children edges

                    foreach (int child in Children(vid))
                        parent[child] = -1;

                    if (children.ContainsKey(vid))
                        children.Remove(vid);
                }

                SetRoot(vertexId);

                return this;
            }
            else
            {
                Dictionary<int, int> renumberedTree = new Dictionary<int, int>();

                PropertyTree tree = new PropertyTree();
                SetRoot(0);

                foreach (string name in Properties().Keys)
                    tree.AddProperty(name);

                renumberedTree.Add(vertexId, tree.root);
                tree.AddVertexProperties(tree.root, GetVertexProperties(vertexId));

                IEnumerable<int> subTree = t.RecursivePreOrder((mtg)this, vertexId);

                foreach (int vid in subTree)
                {
                    if (vid != vertexId)
                    {
                        int parentId = (int)Parent(vid);

                        if (parentId != -1)
                        {
                            int parent = renumberedTree[parentId];
                            int v = tree.AddChild(parent);
                            renumberedTree.Add(vid, v);
                        }

                        tree.AddVertexProperties(vid, GetVertexProperties(vid));

                    }
                }

                return tree;

            }
        }

        /// <summary>
        /// Insert a tree before the vertex.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <param name="tree"> The tree to add. </param>
        /// <returns> A dictionary of the new identifiers. </returns>
        public Dictionary<int, int> InsertSiblingTree(int vertexId, PropertyTree tree)
        {
            Dictionary<int, int> renumberedTree = base.InsertSiblingTree(vertexId, tree);

            foreach (int key in renumberedTree.Keys)
            {
                foreach (string name in tree.Properties().Keys)
                {
                    if (tree.Property(name).ContainsKey(key))
                    {
                        int v = tree.Property(name)[key];

                        if (v != -1)
                            properties[name].Add(renumberedTree[key], v);

                    }
                }
            }

            return renumberedTree;

        }

        /// <summary>
        /// Add a tree after the children of the parent vertex.
        /// </summary>
        /// <param name="parent"> Parent identifier. </param>
        /// <param name="tree"> The tree to add. </param>
        /// <returns> A dictionary of the equivalence between identifiers of the tree in the parameters and their new value once added to the tree. </returns>
        public Dictionary<int, int> AddChildTree(int parent, PropertyTree tree)
        {
            Dictionary<int, int> renumberedTree = base.AddChildTree(parent, tree);

            foreach (int key in renumberedTree.Keys)
            {
                foreach (string name in tree.Properties().Keys)
                {
                    if (tree.Property(name).ContainsKey(key))
                    {
                        int v = tree.Property(name)[key];

                        if (v != -1)
                            properties[name].Add(renumberedTree[key], v);
                    }
                }
            }

            return renumberedTree;
        }

        /// <summary>
        /// Remove the subtree rooted on the vertex in the parameters.
        /// </summary>
        /// <param name="vertexId"> Vertex identifier. </param>
        /// <returns> A list of the deleted vertices. </returns>
        new public List<int> RemoveTree(int vertexId)
        {
            List<int> deletedVertices = base.RemoveTree(vertexId);

            foreach (int vertex in deletedVertices)
                RemoveVertexProperties(vertex);

            return deletedVertices;
        }

        #endregion

    }
}
