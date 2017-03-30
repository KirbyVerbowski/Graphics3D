using System;
using System.Collections.Generic;
using System.Drawing;

/*
    Primitives.cs
    Mar/2017
    Kirby Verbowski

    A few premade primitave objects which are ready to be used and rendered by a camera.
    This is done by creating an instance of a camera as well as an instance of the object(or objects)
    these objects (including the camera) can be rotated, scaled and moved by changing the respective properties
    of their transform property.
    To create an image, add the object instance to the camera's renderQueue list and call the render method on the camera.
    Render will return an image which can then be applied to a pictureBox etc.
*/

namespace Graphics3D {

    public enum RenderMode { Wireframe, Solid }

    /// <summary>
    /// A primitive Cube object
    /// </summary>
    public class Cube : Object3D {

        public Cube(Vector3 position, Vector3 scale, Quaternion rotation) : base(Mesh.Cube, position, scale, rotation) { }
        public Cube() : base(Mesh.Cube) { }

        public override bool Selected() {
            return selected;
        }
    }

    /// <summary>
    /// A primitive Tetrahedron object
    /// </summary>
    public class Tetrahedron : Object3D {

        public Tetrahedron(Vector3 position, Vector3 scale, Quaternion rotation) : base(Mesh.Tetahedron, position, scale, rotation) { }
        public Tetrahedron() : base(Mesh.Tetahedron) { }

        public override bool Selected() {
            return selected;
        }

    }

    /// <summary>
    /// A primitive Grid object
    /// </summary>
    public class GridFloor : Object3D {

        public GridFloor(Vector3 position, Vector3 scale, Quaternion rotation) : base(Mesh.GridFloor, position, scale, rotation) { }
        public GridFloor() : base(Mesh.GridFloor) { }

        public override bool Selected() {
            return selected;
        }

    }

    /// <summary>
    /// A camera object that can be used to render a scene
    /// </summary>
    public class Camera  : Object3D {

        #region Member Variables

        /// <summary>
        /// A list of all objects that will be rendered when Render() is called.
        /// </summary>
        public List<IRenderable> renderQueue = new List<IRenderable>();

        private int _width = 0;
        /// <summary>
        /// Width of the image.
        /// </summary>
        public int width {
            get { return _width; }
            set {
                _width = value;
                RenderSizeChanged();
            }
        }

        private int _height = 0;
        /// <summary>
        /// Height of the image.
        /// </summary>
        public int height {
            get { return _height; }
            set {
                _height = value;
                RenderSizeChanged();
            }
        }

        /// <summary>
        /// Length of the local axis gizmos (Red, Green and Blue lines)
        /// </summary>
        public double gizmoLength = 0;

        /// <summary>
        /// Should the camera render local axis gizmos?
        /// </summary>
        public bool renderGizmos = false;

        private bool _orthographic = false;
        /// <summary>
        /// Should the camera render in perspective or orthographic?
        /// </summary>
        public bool orthographic {
            get { return _orthographic; }
            set { _orthographic = value; RenderSizeChanged(); }
        }

        /// <summary>
        /// How should objects be rendered?
        /// </summary>
        public RenderMode renderMode = RenderMode.Solid;

        /// <summary>
        /// Background color of the image
        /// </summary>
        public Color worldColor;

        /// <summary>
        /// The color which will be used to draw wireframes.
        /// </summary>
        public Pen wireFramePen = Pens.Black;

        private Pen[] gizmoPens = new Pen[] { Pens.Blue, Pens.LawnGreen, Pens.Red };

        private double _farClip = 250, _nearClip = 0.1, _zoom = 1, _viewAngle = Math.PI / 4, _orthographicScale = 0.1;

        public double viewAngle {
            get { return _viewAngle; }
            set { _viewAngle = value;   RenderSizeChanged(); }
        }
        public double farClip {
            get { return _farClip; }
            set { _farClip = value;     RenderSizeChanged(); }
        }
        public double nearClip {
            get { return _nearClip; }
            set { _nearClip = value; RenderSizeChanged(); }
        }
        public double zoom {
            get { return _zoom; }
            set { _zoom = value; RenderSizeChanged(); }
        }
        public double orthographicScale {
            get { return _orthographicScale; }
            set { _orthographicScale = value; RenderSizeChanged(); }
        }

        private Vector2 screen1, screen2;
        private Projection projection;

        private Graphics g;
        private Bitmap b;
        #endregion

        #region Constructors
        public Camera(Vector3 position, Quaternion rotation, Color worldColor, int width, int height, double angle) : base(Mesh.CameraFrustrum, position, Vector3.one, rotation) {

            this.worldColor = worldColor;
            //So callback is only called once
            this._viewAngle = angle;
            this._width = width;
            this.height = height;
        }
        #endregion

        #region Member Methods

        /// <summary>
        /// Return an Image using the camera's current transform, and render settings
        /// </summary>
        public Image Render() {

            g.Clear(worldColor);

            switch (renderMode) {
                case RenderMode.Solid:
                    return DrawAllObjectsSolid();
                case RenderMode.Wireframe:
                    return DrawAllObjectsWireframe();
                default:
                    return null;
            }

        }

        private Image DrawAllObjectsWireframe() {

            if(renderQueue.Count == 0) {
                return b;
            }

            foreach (IRenderable obj in renderQueue) {

             
                if (obj.Selected()) {
                    wireFramePen = Pens.Orange;
                }else {
                    wireFramePen = Pens.Black;
                }

                foreach (int[] edge in obj.mesh.edges) {

                    projection.Project(VertexToCameraSpace(obj.transformedMesh.vertices[edge[0]]),
                                        VertexToCameraSpace(obj.transformedMesh.vertices[edge[1]]),
                                          out screen1,
                                           out screen2);
                    g.DrawLine(wireFramePen, (float)screen1.x, (float)screen1.y, (float)screen2.x, (float)screen2.y);
                }

                if (renderGizmos) {

                    Vector3[] points = new Vector3[] { obj.transform.forward * gizmoLength + obj.transform.position, obj.transform.up * gizmoLength + obj.transform.position, obj.transform.right * gizmoLength + obj.transform.position };
                    for (int i = 0; i < 3; i++) {

                        projection.Project(VertexToCameraSpace(obj.transform.position),
                                        VertexToCameraSpace(points[i]),
                                          out screen1,
                                           out screen2);
                        g.DrawLine(gizmoPens[i], (float)screen1.x, (float)screen1.y, (float)screen2.x, (float)screen2.y);
                    }
                }
            }
            return b;
        }

        /// <summary>
        /// Draws all objects in the renderqueue as solid white/grey objects. Note this doesn't consider z-depth
        /// </summary>
        private Image DrawAllObjectsSolid() {

            foreach (IRenderable obj in renderQueue) {

                for (int i = 0; i < obj.transformedMesh.faces.Length; i++) {

                    double dot = Vector3.DotProduct((obj.transform.position - transform.position).normalized, obj.transformedMesh.faceNormals[i]);
                    if(dot >= 0) {
                        continue;   //backface culling
                    }
                    if (Math.Abs(dot) < MathConst.EPSILON || double.IsNaN(dot) || double.IsInfinity(dot)) {
                        continue;
                    }
                    
                    Brush b = new SolidBrush(Color.FromArgb((int)(-dot * 255), (int)(-dot * 255), (int)(-dot * 255)));
                    PointF[] verts = new PointF[obj.transformedMesh.faces[i].Length];


                    

                    for (int j = 0; j < verts.Length; j++) {
                        projection.Project(VertexToCameraSpace(obj.transformedMesh.vertices[obj.transformedMesh.faces[i][j]]),
                                        VertexToCameraSpace(obj.transformedMesh.vertices[obj.transformedMesh.faces[i][j]]),
                                          out screen1,
                                           out screen2);
                        verts[j] = new PointF((float)screen1.x, (float)screen1.y);
                    }
                    g.FillPolygon(b, verts);

                }

                if (renderGizmos) {

                    Vector3[] points = new Vector3[] { obj.transform.forward * gizmoLength + obj.transform.position, obj.transform.up * gizmoLength + obj.transform.position, obj.transform.right * gizmoLength + obj.transform.position };
                    for (int i = 0; i < 3; i++) {

                        projection.Project(VertexToCameraSpace(obj.transform.position),
                                        VertexToCameraSpace(points[i]),
                                          out screen1,
                                           out screen2);
                        g.DrawLine(gizmoPens[i], (float)screen1.x, (float)screen1.y, (float)screen2.x, (float)screen2.y);
                    }
                }
            }


            return b;
        }

        /// <summary>
        /// Helper method for render
        /// </summary>
        private Vector3 VertexToCameraSpace(Vector3 vertex) {
            return transform.rotation.conjugate.RotateVector3(vertex - transform.position);   //Convert to camera space
             
        }

        /// <summary>
        /// Updates private variables when the render size is changed.
        /// </summary>
        private void RenderSizeChanged() {
            if(width > MathConst.EPSILON && height > MathConst.EPSILON) {
                b = new Bitmap(width, height);
                g = Graphics.FromImage(b);
                g.ScaleTransform(1, -1);
                g.TranslateTransform(width / 2, -height / 2);

                projection = new Projection(this);

                transform.scale = new Vector3(projection.rect.x, projection.rect.y, 4 * viewAngle / Math.PI);
            }                 
        }

        public override bool Selected() {
            return base.selected;
        }
        #endregion
    }
}
