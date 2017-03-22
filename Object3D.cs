using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

/*
    Object3D.cs
    Mar/2017
    Kirby Verbowski

    Any object which exists in 3D space should be an Object3D.
    An Object3D in the general sense is something which has a relation to 3D space and can be rendered by a camera.
*/


namespace Graphics3D {

    /// <summary>
    /// An interface which allows 3D objects to be rendered by the camera.
    /// </summary>
    public interface IRenderable {

        /// <summary>
        /// Requres any renderable object to have a transform
        /// </summary>
        Transform transform { get; set; }

        /// <summary>
        /// Is this object selected?
        /// </summary>
        bool Selected();

        Mesh mesh { get; set; }
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
                _transformMatrix.SetCol(3, value.ToVector4());
            }
        }

        protected Quaternion _rotation = Quaternion.identity;
        public Quaternion rotation {
            get { return _rotation; }
            set {
                _rotation = value;
                _rotationMatrix = value.getMatrix4x4();
                _transformMatrix = new Matrix4x4(_rotationMatrix.GetCol(0), _rotationMatrix.GetCol(1), _rotationMatrix.GetCol(2), position.ToVector4());
            }
        }

        protected Vector3 _scale = Vector3.zero;
        public Vector3 scale {
            get { return _scale; }
            set {
                _scale = value;
                _scaleMatrix = new Matrix4x4(new Vector4(value.x, 0, 0, 0), new Vector4(0, value.y, 0, 0), 
                                            new Vector4(0, 0, value.z, 0), new Vector4(0, 0, 0, 1));
            }
        }

        protected Matrix4x4 _scaleMatrix = Matrix4x4.identity;
        public Matrix4x4 scaleMatrix {
            get {
                return _scaleMatrix;
            }
            set {
                _scaleMatrix = value;
                _scale = new Vector3(value.GetCol(0).x, value.GetCol(1).y, value.GetCol(2).z);
            }
        }

        protected Matrix4x4 _transformMatrix = Matrix4x4.identity;
        public Matrix4x4 transformMatrix {
            get { return _transformMatrix; }
            set {
                _transformMatrix = value;
                _position = value.GetCol(3).ToVector3();
            }
        }

        protected Matrix4x4 _rotationMatrix = Matrix4x4.identity;
        public Matrix4x4 rotationMatrix {
            get { return _rotationMatrix; }
            set {
                _rotationMatrix = value;
                _rotation = new Quaternion(value);
                _transformMatrix = new Matrix4x4(value.GetCol(0), value.GetCol(1), value.GetCol(2), position.ToVector4());
            }
        }


        /// <summary>
        /// Returns a unit vector which points in this object's local forward direction
        /// </summary>
        public Vector3 forward {
            get {
                return transformMatrix.GetCol(2).ToVector3();
            }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local up direction
        /// </summary>
        public Vector3 up {
            get {
                return transformMatrix.GetCol(1).ToVector3();
            }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local right direction
        /// </summary>
        public Vector3 right {
            get {
                return transformMatrix.GetCol(0).ToVector3();
            }
            private set { }
        }

        #endregion

        #region Member Methods

        /// <summary>
        /// Rotate this object by the quaternion specified
        /// </summary>
        public void Rotate(Quaternion rotBy) {
            if (this.rotation.sqrMagnitude < MathConst.EPSILON) {
                this.rotation = rotBy * Quaternion.identity;
            } else {
                this.rotation = rotBy * this.rotation;
            }
        }
        /// <summary>
        /// Rotate this object by the axis and angle specified
        /// </summary>
        public void Rotate(Vector3 axis, double angle) {
            if (this.rotation.sqrMagnitude < MathConst.EPSILON) {
                this.rotation = new Quaternion(axis, angle) * Quaternion.identity;
            } else {
                this.rotation = new Quaternion(axis, angle) * this.rotation;
            }
        }

        #endregion

        #region Constructors

        public Transform(Vector3 position, Vector3 scale, Quaternion rotation) {
            this._position = position;
            this._rotation = rotation;
            this._scale = scale;
            this._rotationMatrix = rotation.getMatrix4x4();
            this._transformMatrix.SetCol(3, position.ToVector4());
            this._scaleMatrix = new Matrix4x4(new Vector4(scale.x, 0, 0, 0), new Vector4(0, scale.y, 0, 0),
                                            new Vector4(0, 0, scale.z, 0), new Vector4(0, 0, 0, 1));
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

        public Mesh mesh { get; set; }

        public bool selected = false;

        #endregion

        #region Member Methods

        public virtual bool Selected() {
            return false;
        }

        #endregion

        #region Constructors
        public Object3D(Vector3 position, Vector3 scale, Quaternion rotation) {
            this.mesh = new Mesh();
            this.transform = new Transform(position, scale, rotation);
        }
        /// <summary>
        /// Create a new Object3D from an .obj file at the given path
        /// </summary>
        public Object3D(string path) {
            mesh = Mesh.ReadObjFile(path);
            transform = new Transform(Vector3.zero, Vector3.one, Quaternion.identity);
        }
        /// <summary>
        /// Create a new Object3D from an .obj file at the given path
        /// </summary>
        public Object3D(string path, Vector3 position, Vector3 scale, Quaternion rotation) {
            mesh = Mesh.ReadObjFile(path);
            transform = new Transform(position, scale, rotation);
        }
        public Object3D(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation) {
            this.mesh = mesh;
            this.transform = new Transform(position, scale, rotation);
        }
        public Object3D(Mesh mesh) {
            this.mesh = mesh;
            transform = new Transform(Vector3.zero, Vector3.one, Quaternion.identity);
        }
        #endregion
    }
}