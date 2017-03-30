/*
    Object3D.cs
    Mar/2017
    Kirby Verbowski

    Any object which exists in 3D space should be an Object3D.
    An Object3D is something which has a relation to 3D space and can be rendered by a camera.
*/

namespace Graphics3D {

    public enum TransformOperation { Position, Rotation, Scale, All }

    public delegate void TransformUpdateDelegate(TransformOperation op);

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

        Mesh transformedMesh { get; set; }
    }

    /// <summary>
    /// A class which holds information about an Object3D's relation to the world
    /// </summary>
    public class Transform {

        #region Member Variables

        public TransformUpdateDelegate upd;

        private Vector3 _position = Vector3.zero;
        public Vector3 position {
            get { return _position; }
            set {
                _position = value;
                upd(TransformOperation.Position);
            }
        }

        private Quaternion _rotation = Quaternion.identity;
        public Quaternion rotation {
            get { return _rotation; }
            set {
                _rotation = value;
                upd(TransformOperation.Rotation);
            }
        }

        private Vector3 _scale = Vector3.one;
        public Vector3 scale {
            get { return _scale; }
            set {
                _scale = value;
                upd(TransformOperation.Scale);
            }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local forward direction
        /// </summary>
        public Vector3 forward {
            get {
                return rotation.RotateVector3(Vector3.unitVectorZ);
            }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local up direction
        /// </summary>
        public Vector3 up {
            get {
                return rotation.RotateVector3(Vector3.unitVectorY);
            }
            private set { }
        }

        /// <summary>
        /// Returns a unit vector which points in this object's local right direction
        /// </summary>
        public Vector3 right {
            get {
                return rotation.RotateVector3(Vector3.unitVectorX);
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
        #endregion

        #region Constructors

        public Transform(Vector3 position, Vector3 scale, Quaternion rotation) {
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

        public Mesh mesh { get; set; }

        public Mesh transformedMesh { get; set; }

        public bool selected = false;
        #endregion

        #region Member Methods

        public virtual bool Selected() {
            return false;
        }

        private void TransformUpdate(TransformOperation op) {
            if (transformedMesh == null) {
                if(mesh == null || mesh.vertices == null) {
                    return;
                }else {
                    transformedMesh = new Mesh(mesh.vertices, mesh.faces, mesh.edges);
                }
            }
            for(int i = 0; i < mesh.vertices.Length; i++) {
                transformedMesh.vertices[i] = transform.rotation.RotateVector3(mesh.vertices[i] * transform.scale) + transform.position;
            }
            if(op == TransformOperation.Rotation || op == TransformOperation.All) {
                transformedMesh.calculateFaceNormals();
            }
        }

        #endregion

        #region Constructors
        public Object3D(Vector3 position, Vector3 scale, Quaternion rotation) {
            this.mesh = new Mesh();
            this.transform = new Transform(position, scale, rotation);
            transform.upd = TransformUpdate;
            transform.upd.Invoke(TransformOperation.All);
        }
        /// <summary>
        /// Create a new Object3D from an .obj file at the given path
        /// </summary>
        public Object3D(string path) {
            mesh = Mesh.ReadObjFile(path);
            mesh.calculateFaceNormals();
            this.transformedMesh = new Mesh(mesh.vertices, mesh.faces, mesh.edges);
            transformedMesh.calculateFaceNormals();
            transform = new Transform(Vector3.zero, Vector3.one, Quaternion.identity);
            transform.upd = TransformUpdate;
            transform.upd.Invoke(TransformOperation.All);
        }
        /// <summary>
        /// Create a new Object3D from an .obj file at the given path
        /// </summary>
        public Object3D(string path, Vector3 position, Vector3 scale, Quaternion rotation) {
            mesh = Mesh.ReadObjFile(path);
            this.transformedMesh = new Mesh(mesh.vertices, mesh.faces, mesh.edges);
            transformedMesh.calculateFaceNormals();
            mesh.calculateFaceNormals();

            transform = new Transform(position, scale, rotation);
            transform.upd = TransformUpdate;
            transform.upd.Invoke(TransformOperation.All);
        }
        public Object3D(Mesh mesh, Vector3 position, Vector3 scale, Quaternion rotation) {
            this.mesh = mesh;
            this.transformedMesh = new Mesh(mesh.vertices, mesh.faces, mesh.edges);
            transformedMesh.calculateFaceNormals();
            mesh.calculateFaceNormals();

            this.transform = new Transform(position, scale, rotation);
            transform.upd = TransformUpdate;
            transform.upd.Invoke(TransformOperation.All);
        }
        public Object3D(Mesh mesh) {
            this.mesh = mesh;
            this.transformedMesh = new Mesh(mesh.vertices, mesh.faces, mesh.edges);
            transformedMesh.calculateFaceNormals();
            mesh.calculateFaceNormals();

            transform = new Transform(Vector3.zero, Vector3.one, Quaternion.identity);
            transform.upd = TransformUpdate;
            transform.upd.Invoke(TransformOperation.All);
        }
        #endregion
    }
}