using System;
using System.Collections.Generic;
using System.Drawing;

/*
    Primitives.cs
    Dec/2016

    A few premade primitave objects which are ready to be used and rendered by a camera.
    This is done by creating an instance of a camera as well as an instance of the object(or objects)
    these objects (including the camera) can be rotated, scaled and moved by changing the respective properties
    of their transform property.
    To create an image, add the object instance to the camera's renderQueue and call the render method on the camera.
    Render will return an image which can then be applied to a pictureBox etc.

    Note that to reduce distortion, focalLength may have to be quite large since focalLength, width, and height specify the
    camera's dimensions in 3D space. 
    For example; a camera of width = 1300, height = 400 should have focal length around 300.
*/

namespace HomeCompyGraphics3D {

    public enum RenderMode { Wireframe, Solid }

    /// <summary>
    /// A primitive Cube object
    /// </summary>
    public class Cube : Object3D {

        #region Constructors

        public Cube(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale) {
            getEdgeDelegate = new GetEdgeDelegate(GetEdges);
            vertices = new Vector3[8];
            unitVertices = new Vector3[8] {
                            ((-Vector3.unitVectorX - Vector3.unitVectorY - Vector3.unitVectorZ)),
                                ((Vector3.unitVectorX - Vector3.unitVectorY - Vector3.unitVectorZ)),
                                    ((-Vector3.unitVectorX + Vector3.unitVectorY - Vector3.unitVectorZ)),
                                        ((-Vector3.unitVectorX - Vector3.unitVectorY + Vector3.unitVectorZ)),
                                            ((Vector3.unitVectorX - Vector3.unitVectorY + Vector3.unitVectorZ)),
                                                ((-Vector3.unitVectorX + Vector3.unitVectorY + Vector3.unitVectorZ)),
                                                    ((Vector3.unitVectorX + Vector3.unitVectorY - Vector3.unitVectorZ)),
                                                        ((Vector3.unitVectorX + Vector3.unitVectorY + Vector3.unitVectorZ))};


            TransformUpdate();
        }
        #endregion

        #region Member Methods

        public override bool Selected() {
            return selected;
        }

        private Tuple<Vector3, Vector3>[] GetEdges() {
            Tuple<Vector3, Vector3>[] tuples =
            {
                Tuple.Create(vertices[0], vertices[1]),
                Tuple.Create(vertices[0], vertices[2]),
                Tuple.Create(vertices[0], vertices[3]),
                Tuple.Create(vertices[4], vertices[3]),
                Tuple.Create(vertices[4], vertices[7]),
                Tuple.Create(vertices[4], vertices[1]),
                Tuple.Create(vertices[5], vertices[2]),
                Tuple.Create(vertices[5], vertices[7]),
                Tuple.Create(vertices[5], vertices[3]),
                Tuple.Create(vertices[6], vertices[2]),
                Tuple.Create(vertices[6], vertices[7]),
                Tuple.Create(vertices[6], vertices[1]),
            };
            return tuples;
        }
        #endregion
    }

    /// <summary>
    /// A primitive Tetrahedron object
    /// </summary>
    public class Tetrahedron : Object3D {

        #region Constructors
        public Tetrahedron(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale) {
            getEdgeDelegate = new GetEdgeDelegate(GetEdges);

            vertices = new Vector3[4];
            unitVertices = new Vector3[4] {
                            ((-Vector3.unitVectorX +    Vector3.unitVectorZ - Vector3.unitVectorY)),
                                ((Vector3.unitVectorX + Vector3.unitVectorZ - Vector3.unitVectorY)),
                                    ((-Vector3.unitVectorZ - Vector3.unitVectorY)),
                                        (2*(Vector3.unitVectorY))};

            TransformUpdate();
        }
        #endregion

        #region Member Methods

        public override bool Selected() {
            return selected;
        }

        private Tuple<Vector3, Vector3>[] GetEdges() {
            Tuple<Vector3, Vector3>[] tuples =
            {
                Tuple.Create(vertices[0], vertices[1]),
                Tuple.Create(vertices[0], vertices[2]),
                Tuple.Create(vertices[0], vertices[3]),
                Tuple.Create(vertices[2], vertices[1]),
                Tuple.Create(vertices[2], vertices[3]),
                Tuple.Create(vertices[1], vertices[3]),
            };
            return tuples;
        }
        #endregion
    }

    /// <summary>
    /// A primitive Grid object
    /// </summary>
    public class GridFloor : Object3D {

        public GridFloor(Vector3 position, Vector3 rotation, Vector3 scale) : base(position, rotation, scale) {
            getEdgeDelegate = new GetEdgeDelegate(GetEdges);

            vertices = new Vector3[16];
            unitVertices = new Vector3[16] {
                            (-2 * Vector3.unitVectorX + 2*Vector3.unitVectorZ),
                                (-Vector3.unitVectorX + 2*Vector3.unitVectorZ),
                                    (2*Vector3.unitVectorZ),
                                        (Vector3.unitVectorX + 2*Vector3.unitVectorZ),
                                            (2 * Vector3.unitVectorX + 2*Vector3.unitVectorZ),
                                               (2 * Vector3.unitVectorX + Vector3.unitVectorZ),
                                                   (2 * Vector3.unitVectorX),
                                                       (2 * Vector3.unitVectorX - Vector3.unitVectorZ),
                                                            (2 * Vector3.unitVectorX - 2 * Vector3.unitVectorZ),
                                                              (Vector3.unitVectorX - 2*Vector3.unitVectorZ),
                                                                (- 2*Vector3.unitVectorZ),
                                                                     (-Vector3.unitVectorX - 2*Vector3.unitVectorZ),
                                                                        (-2*Vector3.unitVectorX - 2*Vector3.unitVectorZ),
                                                                             (-2*Vector3.unitVectorX - Vector3.unitVectorZ),
                                                                                 (-2*Vector3.unitVectorX),
                                                                                        (-2*Vector3.unitVectorX + Vector3.unitVectorZ) };


            TransformUpdate();
        }

        public override bool Selected() {
            return selected;
        }

        private Tuple<Vector3, Vector3>[] GetEdges() {
            Tuple<Vector3, Vector3>[] tuples =
            {
                Tuple.Create(vertices[1], vertices[11]),
                Tuple.Create(vertices[2], vertices[10]),
                Tuple.Create(vertices[3], vertices[9]),
                Tuple.Create(vertices[5], vertices[15]),
                Tuple.Create(vertices[6], vertices[14]),
                Tuple.Create(vertices[7], vertices[13]),
            };
            return tuples;
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
        public float gizmoLength = 50;

        /// <summary>
        /// Focal length of this camera. This will only affect Perspective rendering
        /// </summary>
        public float focalLength = 1;

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
        private Matrix4x4 cameraMatrix;

        /// <summary>
        /// Points that are used by the render calls.
        /// </summary>
        private Vector3 point1 = Vector3.zero, point2 = Vector3.zero, up = Vector3.zero, right = Vector3.zero, forward = Vector3.zero, origin = Vector3.zero;

        private Graphics g;
        private Bitmap b;
        #endregion

        #region Constructors
        public Camera(float focalLength, Vector3 position, Vector3 rotation, Color worldColor, int width, int height) : base(position, rotation, new Vector3(1, 1, 1)) {
            this.focalLength = focalLength;
            this.transform.position = position;
            this.transform.rotation = rotation;
            this.worldColor = worldColor;

            cameraMatrix = Matrix4x4.Inverse(Matrix4x4.CombinedRotation(transform.rotation * MathConst.Deg2Rad));
            //So callback is only called once
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
                    foreach (Tuple<Vector3, Vector3> edge in obj.GetEdges()) {



                        point1 = (cameraMatrix * (edge.Item1 - transform.position).ToVector4()).ToVector3();
                        point2 = (cameraMatrix * (edge.Item2 - transform.position).ToVector4()).ToVector3();

                        point1 = new Vector3(Vector3.DotProduct(edge.Item1, transform.right), Vector3.DotProduct(edge.Item1, transform.up));
                        point2 = new Vector3(Vector3.DotProduct(edge.Item2, transform.right), Vector3.DotProduct(edge.Item2, transform.up));

                        if (Math.Abs(point1.x) > width || Math.Abs(point1.y) > height || Math.Abs(point2.x) > width || Math.Abs(point2.y) > height) {
                            continue;
                        }
                        g.DrawLine(wireFramePen, point1.x, point1.y, point2.x, point2.y);
                    }

                    if (renderGizmos) {
                        origin = (cameraMatrix * (obj.transform.position - transform.position).ToVector4()).ToVector3();
                        up = (cameraMatrix * ((obj.transform.up * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();
                        right = (cameraMatrix * ((obj.transform.right * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();
                        forward = (cameraMatrix * ((obj.transform.forward * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();

                        if (Math.Abs(origin.x) > width || Math.Abs(origin.y) > height || Math.Abs(up.x) > width || Math.Abs(up.y) > height || Math.Abs(right.x) > width || Math.Abs(right.y) > height || Math.Abs(forward.x) > width || Math.Abs(forward.y) > height) {
                            continue;
                        }

                        g.DrawLine(Pens.LawnGreen, origin.x - transform.position.x, origin.y - transform.position.y, up.x - transform.position.x, up.y - transform.position.y);
                        g.DrawLine(Pens.Blue, origin.x - transform.position.x, origin.y - transform.position.y, forward.x - transform.position.x, forward.y - transform.position.y);
                        g.DrawLine(Pens.Red, origin.x - transform.position.x, origin.y - transform.position.y, right.x - transform.position.x, right.y - transform.position.y);
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
            Vector3 point1 = Vector3.zero, point2 = Vector3.zero, origin = transform.forward * focalLength - transform.right * width - transform.up * height;
            if (renderQueue.Count > 0) {
                foreach (IRenderable obj in renderQueue) {
                    foreach (Tuple<Vector3, Vector3> edge in obj.GetEdges()) {

                        point1 = (cameraMatrix * (edge.Item1 - transform.position).ToVector4()).ToVector3();
                        point2 = (cameraMatrix * (edge.Item2 - transform.position).ToVector4()).ToVector3();

                        if (point1.z < 0.1E-6f || point2.z < 0.1E-6f) {
                            continue;
                        } else {

                            point1 = point1.normalized * (new Vector3((focalLength * point1.x) / point1.z, (focalLength * point1.y) / point1.z, focalLength)).magnitude;
                            point2 = point2.normalized * (new Vector3((focalLength * point2.x) / point2.z, (focalLength * point2.y) / point2.z, focalLength)).magnitude;

                        }

                        if (Math.Abs(point1.x) > width || Math.Abs(point1.y) > height || Math.Abs(point2.x) > width || Math.Abs(point2.y) > height) {
                            continue;
                        }
                        g.DrawLine(wireFramePen, point1.x, point1.y, point2.x, point2.y);
                    }
                    if (renderGizmos) {

                        origin = (cameraMatrix * (obj.transform.position - transform.position).ToVector4()).ToVector3();
                        up = (cameraMatrix * ((obj.transform.up * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();
                        right = (cameraMatrix * ((obj.transform.right * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();
                        forward = (cameraMatrix * ((obj.transform.forward * gizmoLength + obj.transform.position) - transform.position).ToVector4()).ToVector3();

                        if (up.z < 0.1E-6f || right.z < 0.1E-6f || forward.z < 0.1E-6f || origin.z < 0.1E-6f) {
                            continue;
                        }


                        origin = origin.normalized * (new Vector3((focalLength * origin.x) / origin.z, (focalLength * origin.y) / origin.z, focalLength)).magnitude;
                        up = up.normalized * (new Vector3((focalLength * up.x) / up.z, (focalLength * up.y) / up.z, focalLength)).magnitude;
                        right = right.normalized * (new Vector3((focalLength * right.x) / right.z, (focalLength * right.y) / right.z, focalLength)).magnitude;
                        forward = forward.normalized * (new Vector3((focalLength * forward.x) / forward.z, (focalLength * forward.y) / forward.z, focalLength)).magnitude;

                        if (Math.Abs(origin.x) > width || Math.Abs(origin.y) > height || Math.Abs(up.x) > width || Math.Abs(up.y) > height || Math.Abs(right.x) > width || Math.Abs(right.y) > height || Math.Abs(forward.x) > width || Math.Abs(forward.y) > height) {
                            continue;
                        }

                        g.DrawLine(Pens.LawnGreen, origin.x, origin.y, up.x, up.y);
                        g.DrawLine(Pens.Blue, origin.x, origin.y, forward.x, forward.y);
                        g.DrawLine(Pens.Red, origin.x, origin.y, right.x, right.y);

                    }
                }
            }
            return b;
        }

        /*Unused Fisheye protocol
        private Image DrawAllObjectsWireframeFishEye() {

            Vector3 point1, point2, d1, d2;
            if (renderQueue.Count > 0) {
                foreach (IRenderable obj in renderQueue) {
                    foreach (Tuple<Vector3, Vector3> edge in obj.GetEdges()) {

                        d1 = Vector4.ToVector3((Matrix4x4.CombinedRotation(transform.rotation * MathConst.Deg2Rad)) * Vector3.ToVector4(edge.Item1 - transform.position));
                        d2 = Vector4.ToVector3((Matrix4x4.CombinedRotation(transform.rotation * MathConst.Deg2Rad)) * Vector3.ToVector4(edge.Item2 - transform.position));

                        if (d1.z == 0 || d2.z == 0) {
                            continue;
                        }

                        point1 = d1.normalized * focalLength;
                        point2 = d2.normalized * focalLength;

                        if (Math.Abs(point1.x) > screenCenter.x * 2 || Math.Abs(point1.y) > screenCenter.y * 2 || Math.Abs(point2.x) > screenCenter.x * 2 || Math.Abs(point2.y) > screenCenter.y * 2) {
                            continue;
                        }

                        g.DrawLine(wireFramePen, point1.x + screenCenter.x, point1.y + screenCenter.y, point2.x + screenCenter.x, point2.y + screenCenter.y);
                    }
                }
            }
            return b;
        }
        */

        /// <summary>
        /// Updates private variables when the render size is changed.
        /// </summary>
        private void RenderSizeChanged() {
            b = new Bitmap(width, height);
            g = Graphics.FromImage(b);
            g.ScaleTransform(1, -1);
            g.TranslateTransform(width / 2, -height / 2);
        }

        public override bool Selected() {
            return base.selected;
        }

        protected override void TransformUpdate() {
            cameraMatrix = Matrix4x4.Inverse(Matrix4x4.CombinedRotation(transform.rotation));
        }
        #endregion
    }
}
