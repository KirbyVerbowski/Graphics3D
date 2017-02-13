using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

/*
    Object3D.cs
    Dec/2016

    Any object which exists in 3D space should inherit from Object3D.
    Object3D implements IRenderable so anything inheriting from Object3D should also implement the specified properties/methods.
    An Object3D in the general sense is something which has a relation to 3D space and can be rendered by a camera.
*/


namespace HomeCompyGraphics3D {

    public delegate void TransformUpdateDelegate();
    public delegate Tuple<Vector3, Vector3>[] GetEdgeDelegate();

    /// <summary>
    /// An interface which allows 3D objects to be rendered by the camera.
    /// </summary>
    public interface IRenderable {

        /// <summary>
        /// Return an array of every edge in this object.
        /// </summary>
        /// <returns>Array of Vector3 Tuples where each Vector3 represents a connected vertex</returns>
        Tuple<Vector3, Vector3>[] GetEdges();

        /// <summary>
        /// Requres any renderable object to have a transform
        /// </summary>
        Transform transform { get; set; }

        /// <summary>
        /// Is this object selected?
        /// </summary>
        bool Selected();
    }

    /// <summary>
    /// A class which holds information about an Object3D's relation to the world
    /// </summary>
    public class Transform {
        #region Member Variables

        protected Vector3 _position = Vector3.zero;
        public Vector3 position {
            get { return _position; }
            set {
                _position = value;
                TransformUpdate();
            }
        }

        protected Vector3 _rotation = Vector3.zero;
        public Vector3 rotation {
            get { return _rotation; }
            set {
                _rotation = value;
                TransformUpdate();
            }
        }

        protected Vector3 _scale = Vector3.zero;
        public Vector3 scale {
            get { return _scale; }
            set {
                _scale = value;
                TransformUpdate();
            }
        }

        protected Matrix4x4 _transformMatrix = Matrix4x4.identity;
        public Matrix4x4 transformMatrix {
            get { return _transformMatrix; }
            set { _transformMatrix = value; TransformUpdate(); }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local forward direction
        /// </summary>
        public Vector3 forward {
            get { return (Matrix4x4.CombinedRotation(-this.rotation).GetRow(2).ToVector3()); }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local up direction
        /// </summary>
        public Vector3 up {
            get { return (Matrix4x4.CombinedRotation(-this.rotation).GetRow(1)).ToVector3(); }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local right direction
        /// </summary>
        public Vector3 right {
            get { return (Matrix4x4.CombinedRotation(-this.rotation).GetRow(0).ToVector3()); }
            private set { }
        }

        /// <summary>
        /// This method will be called when this transform's location, rotation, or scale is changed
        /// </summary>
        public TransformUpdateDelegate TransformUpdate;
        #endregion

        #region Member Methods

        /// <summary>
        /// Rotates this transform so that forward points at target
        /// </summary>
        /// <param name="target">A direction vector NOT a position vector</param>
        public void LookAt(Vector3 target) {
            rotation = Matrix4x4.FromToRotation(Vector3.unitVectorZ, target).GetEulerAngles();
        }

        #endregion

        #region Constructors

        //Disallow the default constructor so the dlegate callback is always assigned when this object is created
        protected Transform() { }

        public Transform(Vector3 position, Vector3 rotation, Vector3 scale, TransformUpdateDelegate callback) {
            this.TransformUpdate = callback;
            this._position = position;
            this._rotation = rotation;
            this._scale = scale;
            
        }
        #endregion
    }

    /// <summary>
    /// The base class for every object that exists in 3D space.
    /// </summary>
    public class Object3D : IRenderable {

        #region Member Variables

        public Transform transform {
            get;
            set;
        }

        /// <summary>
        /// The vertices of this object as they relate to the object's origin as specified by the transform
        /// </summary>
        public Vector3[] vertices;

        /// <summary>
        /// The vertices of this object with respect to the object's origin as it should appear with no scale or rotation
        /// </summary>
        public Vector3[] unitVertices;

        /// <summary>
        /// Is this object selected?
        /// </summary>
        public bool selected = false;

        private Mesh mesh;


        public GetEdgeDelegate getEdgeDelegate;
        #endregion

        #region Member Methods
        protected virtual void TransformUpdate() {
            if(vertices == null) {
                return;
            }
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i] = (Matrix4x4.CombinedRotation(transform.rotation) * (new Vector4(unitVertices[i].x * transform.scale.x, unitVertices[i].y * transform.scale.y, unitVertices[i].z * transform.scale.z, 1))).ToVector3();
                vertices[i] += this.transform.position;
            }
        }
        public Tuple<Vector3, Vector3>[] GetEdges() {
            return getEdgeDelegate();
        }
        public virtual bool Selected() {
            return false;
        }

        private Tuple<Vector3, Vector3>[] DefaultGetEdge() {

            List<Tuple<Vector3, Vector3>> temp = new List<Tuple<Vector3, Vector3>>();

            if (vertices == null) {
                temp.Add(Tuple.Create<Vector3,Vector3>(Vector3.zero,Vector3.zero));
                return temp.ToArray();
            }
            else {
                for (int i = 0; i < vertices.Length; i++) {
                    for (int j = 0; j < vertices.Length; j++) {
                        if (i == j) {
                            continue;
                        }
                        else {
                            temp.Add(new Tuple<Vector3, Vector3>(vertices[i], vertices[j]));
                        }
                    }
                }
                return temp.ToArray();
                
            }
        }

        private Mesh ReadObjFile(StreamReader stream) {
            string line;
            string[] words;
            char[] lineCh;
            List<Vector3> vertices = new List<Vector3>();
            Vector3 temp = Vector3.zero;
            List<List<int>> faces = new List<List<int>>();
            List<int[]> edges = new List<int[]>();
            int face = 0;
            Mesh result = new Mesh();


            while ((line = stream.ReadLine()) != null) {
                lineCh = line.ToCharArray();
                if (lineCh[0] == '#') {
                    continue;
                }

                words = line.Split(' ');

                switch (words[0]) {
                    case "v":
                        for (int i = 1; i < 4; i++) {
                            temp[i - 1] = float.Parse(words[i]);
                        }
                        vertices.Add(temp);
                        break;

                    case "f":
                        StringBuilder sb = new StringBuilder();
                        int j = 0;

                        for (int i = 1; i < words.Length; i++) {
                            while (words[i].ElementAt(j) != '/') {
                                sb.Append(words[i].ElementAt<char>(j));
                                j++;
                            }
                            j = 0;
                            faces.Add(new List<int>());
                            faces[face].Add(int.Parse(sb.ToString()) - 1);
                            sb.Clear();
                        }
                        face++;
                        break;

                    case "o":
                        result.name = words[1];
                        break;
                }

            }
            stream.Close();

            result.vertices = vertices.ToArray();
            result.faces = new int[faces.Count][];
            for (int i = 0; i < faces.Count; i++) {
                result.faces[i] = faces[i].ToArray();
            }

            int current = 0;
            for (int i = 0; i < result.faces.Length; i++) {
                for (int j = 0; j < result.faces[i].Length - 1; j++) {

                    edges.Add(new int[2]);
                    edges[current][0] = result.faces[i][j];
                    edges[current][1] = result.faces[i][j + 1];
                    current++;
                }
                if (result.faces[i].Length != 0) {

                    edges.Add(new int[2]);
                    edges[current][0] = result.faces[i][0];
                    edges[current][1] = result.faces[i][faces[i].Count - 1];
                    current++;
                }
            }

            result.edges = edges.ToArray();

            return result;
        }

        private Tuple<Vector3, Vector3>[] EdgesFromMesh(Mesh mesh) {
            List<Tuple<Vector3, Vector3>> result = new List<Tuple<Vector3, Vector3>>();
            for (int i = 0; i < mesh.edges.Length; i++) {
                result.Add(Tuple.Create<Vector3, Vector3>(this.vertices[mesh.edges[i][0]], this.vertices[mesh.edges[i][1]]));
            }
            return result.ToArray();
        }
        #endregion


        #region Constructors
        public Object3D(Vector3 position, Vector3 rotation, Vector3 scale) {
            getEdgeDelegate = new GetEdgeDelegate(DefaultGetEdge);
            this.transform = new Transform(position, rotation, scale, TransformUpdate);
        }

        /// <summary>
        /// Create a new Object3D from an .obj file at the given path
        /// </summary>
        public Object3D(StreamReader sr) {
            if(sr != null) {
                this.mesh = ReadObjFile(sr);
            }
            this.vertices = mesh.vertices;
            this.unitVertices = mesh.vertices;
            transform = new Transform(Vector3.zero, Vector3.zero, Vector3.one, TransformUpdate);
            getEdgeDelegate = new GetEdgeDelegate(() => EdgesFromMesh(this.mesh));
        }
        #endregion
    }
}