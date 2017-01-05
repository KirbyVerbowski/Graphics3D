using System;
using System.Collections.Generic;

/*
    Object3D.cs
    Dec/2016

    Any object which exists in 3D space should inherit from Object3D.
    Object3D implements IRenderable so anything inheriting from Object3D should also implement the specified properties/methods.
    An Object3D in the general sense is something which has a relation to 3D space and can be rendered by a camera.
*/


namespace Graphics3D {

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

        /// <summary>
        /// Returns a unit vector which points in this object's local forward direction
        /// </summary>
        public Vector3 forward {
            get { return Vector4.ToVector3(Matrix4x4.CombinedRotation(this.rotation) * new Vector4(0, 0, 1, 1)).normalized; }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local up direction
        /// </summary>
        public Vector3 up {
            get { return Vector4.ToVector3(Matrix4x4.CombinedRotation(this.rotation) * new Vector4(0, 1, 0, 1)).normalized; }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local right direction
        /// </summary>
        public Vector3 right {
            get { return Vector4.ToVector3(Matrix4x4.CombinedRotation(this.rotation) * new Vector4(1, 0, 0, 1)).normalized; }
            private set { }
        }

        /// <summary>
        /// This method will be called when this transform's location, rotation, or scale is changed
        /// </summary>
        public TransformUpdateDelegate TransformUpdate;
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


        public GetEdgeDelegate getEdgeDelegate;
        #endregion

        #region Member Methods
        protected virtual void TransformUpdate() {
            if(vertices == null) {
                return;
            }
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i] = Vector4.ToVector3(Matrix4x4.CombinedRotation(transform.rotation) * Vector3.ToVector4(new Vector3(unitVertices[i].x * transform.scale.x, unitVertices[i].y * transform.scale.y, unitVertices[i].z * transform.scale.z)));
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
        #endregion

        #region Constructors
        public Object3D(Vector3 position, Vector3 rotation, Vector3 scale) {
            getEdgeDelegate = new GetEdgeDelegate(DefaultGetEdge);
            this.transform = new Transform(position, rotation, scale, TransformUpdate);
        }
        #endregion
    }
}