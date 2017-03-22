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
    To create an image, add the object instance to the camera's renderQueue and call the render method on the camera.
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
    public class Camera : Object3D {

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
        public double gizmoLength = 5;

        /// <summary>
        /// Should the camera render local axis gizmos?
        /// </summary>
        public bool renderGizmos = true;

        /// <summary>
        /// Should the camera render in perspective or orthographic?
        /// </summary>
        public bool orthographic = false;

        /// <summary>
        /// How should objects be rendered? Only wireframe is supported currently.
        /// </summary>
        public RenderMode renderMode = RenderMode.Wireframe;

        /// <summary>
        /// Background color of the image
        /// </summary>
        public Color worldColor;

        /// <summary>
        /// The color which will be used to draw wireframes.
        /// </summary>
        public Pen wireFramePen = Pens.Black;

        /// <summary>
        /// A Matrix4x4 which represents the inverse of this camera's rotation. It is used mainly by render calls.
        /// </summary>
        private Matrix4x4 cameraMatrix = Matrix4x4.identity;

        /// <summary>
        /// Points that are used by the render calls.
        /// </summary>
        private Vector3 point1 = Vector3.zero, point2 = Vector3.zero, up = Vector3.zero, right = Vector3.zero, forward = Vector3.zero, origin = Vector3.zero;

        private double _farClip = 250, _nearClip = 0.1, _zoom = 1, _viewAngle = Math.PI / 4;

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

        private Vector2 screen1, screen2;
        private Projection projection;

        private Graphics g;
        private Bitmap b;
        #endregion

        #region Constructors
        public Camera(Vector3 position, Quaternion rotation, Color worldColor, int width, int height, double angle) : base(Mesh.CameraFrustrum, position, Vector3.one, rotation) {

            this.worldColor = worldColor;
            TransformUpdate();
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

            TransformUpdate();
            g.Clear(worldColor);

            if (orthographic) {
                switch (renderMode) {
                    case RenderMode.Wireframe:
                        return DrawAllObjectsWireframeOrtho();
                    case RenderMode.Solid:
                        throw new NotImplementedException();
                    default:
                        return null;
                }
            } else {
                switch (renderMode) {
                    case RenderMode.Wireframe:
                        return DrawAllObjectsWireframePerspective();
                    case RenderMode.Solid:
                        throw new NotImplementedException();
                    default:
                        return null;
                }
            }

        }

        /// <summary>
        /// This should only be called by Render(). It truncates the z axis of all points in a scene to create an image to return to Render();
        /// </summary>
        private Image DrawAllObjectsWireframeOrtho() {
            if (renderQueue.Count > 0) {
                foreach (IRenderable obj in renderQueue) {
                    Matrix4x4 objMatrix = obj.transform.transformMatrix * obj.transform.scaleMatrix;
                    foreach (int[] edge in obj.mesh.edges) {

                        point1 = (objMatrix * obj.mesh.vertices[edge[0]].ToVector4()).ToVector3();
                        point2 = (objMatrix * obj.mesh.vertices[edge[1]].ToVector4()).ToVector3();

                        point1 = new Vector3(Vector3.DotProduct(point1, transform.right), Vector3.DotProduct(obj.mesh.vertices[edge[0]], transform.up));
                        point2 = new Vector3(Vector3.DotProduct(point1, transform.right), Vector3.DotProduct(obj.mesh.vertices[edge[1]], transform.up));

                        if (Math.Abs(point1.x) > width || Math.Abs(point1.y) > height || Math.Abs(point2.x) > width || Math.Abs(point2.y) > height) {
                            continue;
                        }
                        g.DrawLine(wireFramePen, (float)point1.x, (float)point1.y, (float)point2.x, (float)point2.y);
                    }

                    if (renderGizmos) {
                        origin = (cameraMatrix * (obj.transform.position - transform.position).ToVector4()).ToVector3();
                        up = (cameraMatrix * ((obj.transform.up * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();
                        right = (cameraMatrix * ((obj.transform.right * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();
                        forward = (cameraMatrix * ((obj.transform.forward * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();

                        if (Math.Abs(origin.x) > width || Math.Abs(origin.y) > height || Math.Abs(up.x) > width || Math.Abs(up.y) > height || Math.Abs(right.x) > width || Math.Abs(right.y) > height || Math.Abs(forward.x) > width || Math.Abs(forward.y) > height) {
                            continue;
                        }

                        g.DrawLine(Pens.LawnGreen, (float)(origin.x - transform.position.x), (float)(origin.y - transform.position.y), (float)(up.x - transform.position.x), (float)(up.y - transform.position.y));
                        g.DrawLine(Pens.Blue, (float)(origin.x - transform.position.x), (float)(origin.y - transform.position.y), (float)(forward.x - transform.position.x), (float)(forward.y - transform.position.y));
                        g.DrawLine(Pens.Red, (float)(origin.x - transform.position.x), (float)(origin.y - transform.position.y), (float)(right.x - transform.position.x), (float)(right.y - transform.position.y));
                    }
                }
            }
            return b;
        }

        /// <summary>
        /// This should only be called by Render(). It uses a projection algorithm to create an image to return to Render();
        /// </summary>
        /// <remarks>If one vertex of an edge is inside or behind the camera, the whole edge will not be rendered. I'm working on this.</remarks>
        private Image DrawAllObjectsWireframePerspective() {
            if(renderQueue.Count == 0) {
                return b;
            }

            foreach (IRenderable obj in renderQueue) {

                if (obj.Selected()) {
                    wireFramePen = Pens.Orange;
                }else {
                    wireFramePen = Pens.Black;
                }

                Matrix4x4 objMatrix = obj.transform.transformMatrix * obj.transform.scaleMatrix;
                    foreach (int[] edge in obj.mesh.edges) {

                    point1 = (objMatrix * (obj.mesh.vertices[edge[0]]).ToVector4()).ToVector3();
                    point2 = (objMatrix * (obj.mesh.vertices[edge[1]]).ToVector4()).ToVector3();

                    point1 = (cameraMatrix * (point1 - transform.position).ToVector4()).ToVector3();
                    point2 = (cameraMatrix * (point2 - transform.position).ToVector4()).ToVector3();

                    projection.Project(point1, point2, out screen1, out screen2);

                    g.DrawLine(wireFramePen, (float)screen1.x, (float)screen1.y, (float)screen2.x, (float)screen2.y);

                    //g.DrawLine(wireFramePen, (float)point1.x * width / (float)dispRect.x, (float)point1.y * height / (float)dispRect.y, (float)point2.x * width / (float)dispRect.x, (float)point2.y * height / (float)dispRect.y);

                }

          }
            return b;
        }

        /// <summary>
        /// Updates private variables when the render size is changed.
        /// </summary>
        private void RenderSizeChanged() {
            b = new Bitmap(width, height);
            g = Graphics.FromImage(b);
            g.ScaleTransform(1, -1);
            g.TranslateTransform(width / 2, -height / 2);

            projection = new Projection(this);

            transform.scale = new Vector3(projection.rect.x, projection.rect.y, 4*viewAngle/Math.PI);
        }

        public override bool Selected() {
            return base.selected;
        }

        protected virtual void TransformUpdate() {
            cameraMatrix = Matrix4x4.Transpose(transform.rotationMatrix);
        }
        #endregion
    }
}
