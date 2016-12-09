using System;

/*
    Object3D.cs
    Dec/2016

    Any object which exists in 3D space should inherit from Object3D.
    Object3D implements IRenderable so anything inheriting from Object3D should also implement the specified properties/methods.
    An Object3D in the general sense is something which has a relation to 3D space and can be rendered by a camera.
*/


namespace Graphics3D {

    public delegate void TransformUpdateDelegate();

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
    public abstract class Object3D : IRenderable {

        #region Member Variables

        public Transform transform {
            get;
            set;
        }

        /// <summary>
        /// Is this object selected?
        /// </summary>
        public bool selected = false;
        #endregion

        #region Member Methods
        protected abstract void TransformUpdate();
        public abstract Tuple<Vector3, Vector3>[] GetEdges();
        public abstract bool Selected();
        #endregion

        #region Constructors
        public Object3D(Vector3 position, Vector3 rotation, Vector3 scale) {
            this.transform = new Transform(position, rotation, scale, TransformUpdate);
        }
        #endregion
    }
}