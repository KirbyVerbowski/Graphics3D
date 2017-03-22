using System;
using System.Collections.Generic;

/*
    Projection.cs
    Mar/2017
    Kirby Verbowski

    A helper class for the camera which does most of the heavy
    lifting for perspective rendering
*/

namespace Graphics3D {

    class Projection {

        #region Member Variables
        private double angleU;
        private double angleV;
        private double farClip;
        private double nearClip;
        private double projection;
        private double width, height;
        private double zoom;
        private Vector3 origin;
        private double tanThetaU, tanThetaV;
        private Vector2 proj1, proj2;
        private double viewAngle;

        public Vector2 rect;
        #endregion

        #region Constructors
        public Projection(Camera c) {
            viewAngle = c.viewAngle;
            farClip = c.farClip;
            nearClip = c.nearClip;
            origin = c.transform.position;
            zoom = c.zoom;
            projection = 0;
            width = c.width;
            height = c.height;

            //Little bit of ratio math to make sure it doesn't distort the image
            if(Math.Abs(width - height) < MathConst.EPSILON) {
                rect = new Vector2(1, 1);
            }else if(width > height) {
                rect = new Vector2(1, height / width);
            }else if(height > width) {
                rect = new Vector2(width / height, 1);
            }

            angleU = rect.x * viewAngle;
            angleV = rect.y * viewAngle;


            if (!init()) {
                throw new Exception("Error initializing camera");
            }
        }
        #endregion

        #region Private Methods
        private bool init() {

            //Check that the clipping distances are not equal
            if(Math.Abs(farClip - nearClip) <= MathConst.EPSILON) {
                return false;
            }

            //Check that the viewing angle is valid
            if(angleU < MathConst.EPSILON || angleV < MathConst.EPSILON) {
                return false;
            }

            tanThetaU = Math.Tan(angleU);
            tanThetaV = Math.Tan(angleV);

            //Check that zoom is valid
            if(zoom < MathConst.EPSILON) {
                return false;
            }

            //Make sure theres nothing funky going on with the clipping distances
            if(nearClip < 0 || farClip < 0 || farClip <= nearClip) {
                return false;
            }

            return true;
        }

        //Move e1 and e2 so that they obey clipping planes
        private bool ClipEyeCoords(ref Vector3 e1, ref Vector3 e2) {

            double mu; //dont worry about it

            //If both points are in front the near clip
            if(e1.z <= nearClip && e2.z <= nearClip) {
                return false;
            }

            //If both points are behind the far clip
            if(e1.z >= farClip && e2.z >= farClip) {
                return false;
            }

            //one point is in front of the near clip
            if((e1.z < nearClip && e2.z > nearClip) || (e1.z > nearClip && e2.z < nearClip)) {

                mu = (nearClip - e1.z) / (e2.z - e1.z);

                if(e1.z < nearClip) {   //Need to clip e1

                    e1.x = e1.x + mu * (e2.x - e1.x);
                    e1.y = e1.y + mu * (e2.y - e1.y);
                    e1.z = nearClip;

                } else { //Need to clip e2

                    e2.x = e1.x + mu * (e2.x - e1.x);
                    e2.y = e1.y + mu * (e2.y - e1.y);
                    e2.z = nearClip;

                }
            }

            //one point is behind the far clip
            if((e1.z < farClip && e2.z > farClip) || (e1.z > farClip && e2.z < farClip)) {

                mu = (farClip - e1.z) / (e2.z - e1.z);

                if (e1.z < farClip) {   //Need to clip e1

                    e1.x = e1.x + mu * (e2.x - e1.x);
                    e1.y = e1.y + mu * (e2.y - e1.y);
                    e1.z = farClip;

                } else { //Need to clip e2

                    e2.x = e1.x + mu * (e2.x - e1.x);
                    e2.y = e1.y + mu * (e2.y - e1.y);
                    e2.z = farClip;

                }

            }

            return true;
        }

        //Change the coordinate from camera space to normalized camera space. This is where zoom is taken into account
        private void NormalizeEyeCoords(Vector3 e, out Vector3 normalized) {

            double d;

            if(projection == 0) {

                d = zoom / e.z;
                normalized.x = d * e.x / tanThetaU;
                normalized.y = d * e.y / tanThetaV;
                normalized.z = e.z;

            }else {

                normalized.x = zoom * e.x / tanThetaU;
                normalized.y = zoom * e.y / tanThetaV;
                normalized.z = e.z;

            }
        }

        //Move n1 and n2 so that they obey clipping planes
        private bool NormalizeClipCoords(ref Vector3 n1, ref Vector3 n2) {

            double mu;

            if (n1.x >= 1 && n2.x >= 1) {    //Is the line totally off the screen to the right?
                return false;
            }

            if (n1.x <= -1 && n2.x <= -1) {  //Is the line totally off the screen to the right?
                return false;
            }

            if ((n1.x > 1 && n2.x < 1) || (n1.x < 1 && n2.x > 1)) {  //Does the line pass through x = 1?

                mu = (1 - n1.x) / (n2.x - n1.x);

                if (n1.x < 1) {  //Move n2

                    n2.y = n1.y + mu * (n2.y - n1.y);
                    n2.x = 1;

                } else { //Move n1

                    n1.y = n1.y + mu * (n2.y - n1.y);
                    n1.x = 1;

                }

            }

            if ((n1.x > -1 && n2.x < -1) || (n1.x < -1 && n2.x > -1)) { //Does the line pass through x = -1?

                mu = (-1 - n1.x) / (n2.x - n1.x);

                if(n1.x > -1) { //Move n2

                    n2.y = n1.y + mu * (n2.y - n1.y);
                    n2.x = -1;

                }else { //Move n1

                    n1.y = n1.y + mu * (n2.y - n1.y);
                    n1.x = -1;

                }

            }

            if(n1.y >= 1 && n2.y >= 1) {    //Are both points above y = 1?
                return false;
            }

            if(n1.y <= -1 && n2.y <= -1) {  //Are both points below y = -1?
                return false;
            }

            if ((n1.y > 1 && n2.y < 1) || (n1.y < 1 && n2.y > 1)) { //Does the line pass through y = 1?

                mu = (1 - n1.y) / (n2.y - n1.y);

                if(n1.y < 1) {

                    n2.x = n1.x + mu * (n2.x - n1.x);
                    n2.y = 1;

                }else {

                    n1.x = n1.x + mu * (n2.x - n1.x);
                    n1.y = 1;

                }

            }


            if ((n1.y > -1 && n2.y < -1) || (n1.y < -1 && n2.y > -1)) { //Does the line pass through y = -1?

                mu = (-1 - n1.y) / (n2.y - n1.y);

                if (n1.y > -1) { //Move n2

                    n2.x = n1.x + mu * (n2.x - n1.x);
                    n2.y = -1;

                } else { //Move n1

                    n1.x = n1.x + mu * (n2.x - n1.x);
                    n1.y = -1;

                }

            }

            return true;

        }

        //Set projected to their proper screen coordinates
        private void NormalizedToScreenCoords(Vector3 norm, out Vector2 projected) {

            projected.x = (int)(width * norm.x / 2);
            projected.y = (int)(height * norm.y / 2);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Project the given points using the parameters specified. The point (0,0) corresponds to the middle of the screen.
        /// </summary>
        /// <param name="point1">First point to be projected</param>
        /// <param name="point2">Second point to be projected</param>
        /// <param name="screen1">The first point in screen coordinates</param>
        /// <param name="screen2">The second point in screen coordinates</param>
        public void Project(Vector3 point1, Vector3 point2, out Vector2 screen1, out Vector2 screen2) {
            Vector3 p1 = point1, p2 = point2;

            if(ClipEyeCoords(ref p1, ref p2)) {
                NormalizeEyeCoords(p1, out p1);
                NormalizeEyeCoords(p2, out p2);
                //Console.WriteLine(p2);
                if (NormalizeClipCoords(ref p1, ref p2)) {
                    
                    NormalizedToScreenCoords(p1, out proj1);
                    NormalizedToScreenCoords(p2, out proj2);
                }
            }
            
            screen1 = proj1;
            screen2 = proj2;
        }
        #endregion
    }
}
