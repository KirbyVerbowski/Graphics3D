using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

/*
    DataStructures.cs
    Mar/2017
    Kirby Verbowski

    A few structs which allow transformations in 3D space
*/

namespace Graphics3D {

    /// <summary>
    /// Holds useful math constants
    /// </summary>
    public static class MathConst {
        /// <summary>
        /// Convert an angle from degrees to radians
        /// </summary>
        public const double DEG2RAD = 0.0174533f;

        /// <summary>
        /// Convert an angle from radians to degrees
        /// </summary>
        public const double RAD2DEG = 57.2958f;

        /// <summary>
        /// A very small number
        /// </summary>
        public const double EPSILON = 1E-3f;

        /// <summary>
        /// Returns a vector which lies on the unit sphere
        /// </summary>
        /// <returns></returns>
        public static Vector3 RandomOnUnitSphere() {
            Random rand = new Random();
            return (new Vector3(rand.Next(-360, 361), rand.Next(-360, 361), rand.Next(-360, 361))).normalized;
        }
    }

    public static class GlobalConstants {
        public const string PRIMITIVE_FILE_DIR = @"";
        public const string INITIAL_FILE_DIR = @"";
    }

    /// <summary>
    /// A general purpose struct for two dimensional vectors.
    /// </summary>
    public struct Vector2 {

        #region Member Variables
        /// <summary>
        /// The X component of this vector
        /// </summary>
        public double x;

        /// <summary>
        /// The Y component of this vector
        /// </summary>
        public double y;

        /// <summary>
        /// This vector with magnitude 1. Read only.
        /// </summary>
        public Vector2 normalized {
            get {
                return this * (1 / this.magnitude);
            }
            private set { }
        }

        /// <summary>
        /// The magnitude of this vector. Read only.
        /// </summary>
        public double magnitude {
            get {
                return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            }
            private set { }
        }
        #endregion

        #region Static Variables
        /// <summary>
        /// A read only vector of magnitude 1 in the positive X direction
        /// </summary>
        public static Vector2 unitVectorX {
            get {
                return new Vector2(1, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1 in the positive Y direction
        /// </summary>
        public static Vector2 unitVectorY {
            get {
                return new Vector2(0, 1);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 0
        /// </summary>
        public static Vector2 zero {
            get { return new Vector2(0, 0); }
            private set { }
        }
        #endregion

        #region Constructors

        /// <summary>Creates a new Vector 3 with given components</summary>
        /// <param name="x">The X component of this vector</param>
        /// <param name="y">The Y component of this vector</param>
        public Vector2(double x, double y) {
            this.x = x;
            this.y = y;
        }
        public Vector2(Vector3 v) {
            this.x = v.x;
            this.y = v.y;
        }
        #endregion

        #region Member Methods

        /// <summary>
        /// Make this vector have a magnitude of 1
        /// </summary>
        public void Normalize() {
            this = this * (1 / this.magnitude);
        }

        /// <summary>
        /// Rotate this vector counter-clockwise around a given origin
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <param name="origin">Point to rotate around</param>
        public void Rotate(double angle, Vector2 origin) {
            Vector2 tmp = this - origin;
            this.x = tmp.x * Math.Cos(angle) - tmp.y * Math.Sin(angle);
            this.y = tmp.y * Math.Cos(angle) + tmp.x * Math.Sin(angle);
            this += origin;
        }

        public override bool Equals(object obj) {
            return (Vector2)obj == this;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
        public override string ToString() {
            return ("(" + x + ", " + y + ")");
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Get the angle between two vectors in degrees
        /// </summary>
        public static double Angle(Vector2 v1, Vector2 v2) {
            return (Math.Acos(Vector2.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude)));
        }

        /// <summary>
        /// The dot product of two vectors
        /// </summary>
        public static double DotProduct(Vector2 v1, Vector2 v2) {
            return v1.x * v2.x + v1.y * v2.y;
        }
        #endregion

        #region Operator Overloads
        public static bool operator ==(Vector2 v1, Vector2 v2) {
            return (v1.x == v2.x && v1.y == v2.y);
        }
        public static bool operator !=(Vector2 v1, Vector2 v2) {
            return (v1.x != v2.x || v1.y != v2.y);
        }
        public static Vector2 operator +(Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2) {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vector2 operator *(Vector2 v1, double mag) {
            return new Vector2(v1.x * mag, v1.y * mag);
        }
        public static Vector2 operator *(double mag, Vector2 v1) {
            return new Vector2(v1.x * mag, v1.y * mag);
        }
        public static Vector2 operator -(Vector2 v1) {
            return v1 * (-1);
        }

        /// <summary>
        /// Get component by index (0 = x, 1 = y)
        /// </summary>
        public double this[int i] {
            get {
                switch (i) {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i) {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

        }
        #endregion

    }

    /// <summary>
    /// A general purpose struct for three dimensional vectors.
    /// </summary>
    public struct Vector3 {

        #region Member Variables

        /// <summary>
        /// The X component of this vector
        /// </summary>
        public double x;

        /// <summary>
        /// The Y component of this vector
        /// </summary>
        public double y;

        /// <summary>
        /// The Z component of this vector
        /// </summary>
        public double z;

        /// <summary>
        /// The magnitude of this vector. Read only.
        /// </summary>
        public double magnitude {
            get {
                return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            }
            private set { }
        }

        /// <summary>
        /// This vector with magnitude 1. Read only.
        /// </summary>
        public Vector3 normalized {
            get {
                return this * (1 / this.magnitude);
            }
            private set { }
        }
        #endregion

        #region Static Variables

        /// <summary>
        /// A read only vector of magnitude 1 in the positive X direction
        /// </summary>
        public static Vector3 unitVectorX {
            get {
                return new Vector3(1, 0, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1 in the positive Y direction
        /// </summary>
        public static Vector3 unitVectorY {
            get {
                return new Vector3(0, 1, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1 in the positive Z direction
        /// </summary>
        public static Vector3 unitVectorZ {
            get {
                return new Vector3(0, 0, 1);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 0
        /// </summary>
        public static Vector3 zero {
            get {
                return new Vector3(0, 0, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1
        /// </summary>
        public static Vector3 one {
            get { return new Vector3(1, 1, 1); }
            private set { }
        }
        #endregion

        #region Constructors

        /// <summary>Creates a new Vector 3 with given components</summary>
        /// <param name="x">The X component of this vector</param>
        /// <param name="y">The Y component of this vector</param>
        /// <param name="z">The Z component of this vector</param>
        public Vector3(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Creates a new Vector3 with given X and Y. Z set to 0.
        /// </summary>
        public Vector3(double x, double y) {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        /// <summary>
        /// Creates a new Vector3 with same components as original. Z set to 0.
        /// </summary>
        public Vector3(Vector2 original) {
            this.x = original.x;
            this.y = original.y;
            this.z = 0;
        }
        #endregion

        #region Member Methods

        /// <summary>
        /// Make this vector have a magnitude of 1
        /// </summary>
        public void Normalize() {
            this = this * (1 / this.magnitude);
        }
        /// <summary>
        /// Returns this vector as a vector4 with w set to 1
        /// </summary>
        public Vector4 ToVector4() {
            return new Vector4(x, y, z, 1);
        }
        public override string ToString() {
            return ("(" + x + ", " + y + ", " + z + ")");
        }
        public override bool Equals(object obj) {
            return (Vector3)obj == this;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }
      
        #endregion

        #region Static Methods

        /// <summary>
        /// The dot product of two vectors
        /// </summary>
        public static double DotProduct(Vector3 v1, Vector3 v2) {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// The vector cross product
        /// </summary>
        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2) {
            double x, y, z;
            x = v1.y * v2.z - v1.z * v2.y;
            y = v1.z * v2.x - v1.x * v2.z;
            z = v1.x * v2.y - v1.y * v2.x;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Get the unsigned angle between two vectors in degrees
        /// </summary>
        public static double Angle(Vector3 v1, Vector3 v2) {
            if(!double.IsNaN((Math.Acos(Vector3.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude))))) {
                return (Math.Acos(Vector3.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude)));
            }else {
                return 0;
            }
           
        }

        /*
      three unit axes L M N

          |
          |L
          |______
         /    M
        /N
        such that unit axis L lies paralell to the rotation axis
*/

        /// <summary>
        /// Returns the given vector after it has been rotated clockwise around the given axis.
        /// </summary>
        public static Vector3 Rotate(Vector3 vec, Vector3 axis, double angle) {
            Vector3 ax = axis.normalized;
            return Math.Cos(angle) * vec + Math.Sin(angle) * Vector3.CrossProduct(axis, vec) + (1 - Math.Cos(angle)) * Vector3.DotProduct(axis, vec) * axis;
        }

        //Not using euler angles
        //public static Vector3 AxisAngleToEulerAngles(Vector3 axis, double angle) {
        //    axis.Normalize();
        //    angle *= MathConst.Deg2Rad;
        //    double s = Math.Sin(angle);
        //    double c = Math.Cos(angle);
        //    double t = 1 - c;
        //    double heading, attitude, bank;
        //    if ((axis.x * axis.y * t + axis.z * s) > 0.998) { // north pole singularity detected
        //        heading = 2 * Math.Atan2(axis.x * Math.Sin(angle / 2), Math.Cos(angle / 2));
        //        attitude = Math.PI / 2;
        //        bank = 0;
        //        return new Vector3(heading * MathConst.Rad2Deg, attitude * MathConst.Rad2Deg, bank * MathConst.Rad2Deg);
        //    }
        //    if ((axis.x * axis.y * t + axis.z * s) < -0.998) { // south pole singularity detected
        //        heading = -2 * Math.Atan2(axis.x * Math.Sin(angle / 2), Math.Cos(angle / 2));
        //        attitude = -Math.PI / 2;
        //        bank = 0;
        //        return new Vector3(heading * MathConst.Rad2Deg, attitude * MathConst.Rad2Deg, bank * MathConst.Rad2Deg);
        //    }
        //    heading = Math.Atan2(axis.y * s - axis.x * axis.z * t, 1 - (axis.y * axis.y + axis.z * axis.z) * t);
        //    attitude = Math.Asin(axis.x * axis.y * t + axis.z * s);
        //    bank = Math.Atan2(axis.x * s - axis.y * axis.z * t, 1 - (axis.x * axis.x + axis.z * axis.z) * t);
        //    return new Vector3(bank * MathConst.Rad2Deg, attitude * MathConst.Rad2Deg, heading * MathConst.Rad2Deg);
        //}

        #endregion

        #region Operator Overloads
        public static bool operator ==(Vector3 v1, Vector3 v2) {
            return (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
        }
        public static bool operator !=(Vector3 v1, Vector3 v2) {
            return (v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }
        public static Vector3 operator +(Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2) {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static Vector3 operator +(Vector3 v1, Vector2 v2) {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z);
        }
        public static Vector3 operator -(Vector2 v1, Vector3 v2) {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v2.z);
        }
        public static Vector3 operator *(Vector3 v1, double mag) {
            return new Vector3(v1.x * mag, v1.y * mag, v1.z * mag);
        }
        public static Vector3 operator *(double mag, Vector3 v1) {
            return new Vector3(v1.x * mag, v1.y * mag, v1.z * mag);
        }
        public static Vector3 operator -(Vector3 v1) {
            return v1 * (-1);
        }

        /// <summary>
        /// Get component by index (0 = x, 1 = y, 2 = z)
        /// </summary>
        public double this[int i] {
            get {
                switch (i) {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i) {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

        }
        #endregion
    }

    /// <summary>
    /// A general purpose struct for four dimensional vectors.
    /// </summary>
    public struct Vector4 {

        #region Member Variables

        /// <summary>
        /// The X component of this vector
        /// </summary>
        public double x;

        /// <summary>
        /// The Y component of this vector
        /// </summary>
        public double y;

        /// <summary>
        /// The Z component of this vector
        /// </summary>
        public double z;

        /// <summary>
        /// The W component of this vector
        /// </summary>
        public double w;

        /// <summary>
        /// The magnitude of this vector. Read only.
        /// </summary>
        public double magnitude {
            get {
                return Math.Sqrt(Math.Pow(w, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            }
            private set { }
        }

        /// <summary>
        /// This vector with magnitude 1. Read only.
        /// </summary>
        public Vector4 normalized {
            get {
                return this * (1 / this.magnitude);
            }
            private set { }
        }
        #endregion

        #region Static Variables

        /// <summary>
        /// A read only vector of magnitude 1 in the positive W direction
        /// </summary>
        public static Vector4 unitVectorW {
            get {
                return new Vector4(0, 0, 0, 1);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1 in the positive X direction
        /// </summary>
        public static Vector4 unitVectorX {
            get {
                return new Vector4(1, 0, 0, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1 in the positive Y direction
        /// </summary>
        public static Vector4 unitVectorY {
            get {
                return new Vector4(0, 1, 0, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 1 in the positive Z direction
        /// </summary>
        public static Vector4 unitVectorZ {
            get {
                return new Vector4(0, 0, 1, 0);
            }
            private set { }
        }

        /// <summary>
        /// A read only vector of magnitude 0
        /// </summary>
        public static Vector4 zero {
            get {
                return new Vector4(0, 0, 0, 0);
            }
            private set { }
        }
        #endregion

        #region Constructors

        /// <summary>Creates a new Vector 3 with given components</summary>
        /// <param name="x">The X component of this vector</param>
        /// <param name="y">The Y component of this vector</param>
        /// <param name="z">The Z component of this vector</param>
        /// <param name="w">The W component of this vector</param>
        public Vector4(double x, double y, double z, double w) {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        #endregion

        #region Member Methods

        /// <summary>
        /// Make this vector have a magnitude of 1
        /// </summary>
        public void Normalize() {
            this = this * (1 / this.magnitude);
        }

        /// <summary>
        /// Returns this vector as a vector3 by truncating the w value
        /// </summary>
        public Vector3 ToVector3() {
            return new Vector3(x, y, z);
        }

        public override string ToString() {
            return ("(" + x + ", " + y + ", " + z + ", " + w + ")");
        }
        public override bool Equals(object obj) {
            return (Vector4)obj == this;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// The dot product of two vectors
        /// </summary>
        public static double DotProduct(Vector4 v1, Vector4 v2) {
            return v1.w * v2.w + v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Get the angle between two vectors in degrees
        /// </summary>
        public static double Angle(Vector4 v1, Vector4 v2) {
            return (Math.Acos(Vector4.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude)));
        }
        #endregion

        #region Operator Overloads
        public static bool operator ==(Vector4 v1, Vector4 v2) {
            return (v1.w == v2.w && v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);
        }
        public static bool operator !=(Vector4 v1, Vector4 v2) {
            return (v1.w != v2.w || v1.x != v2.x || v1.y != v2.y || v1.z != v2.z);
        }
        public static Vector4 operator +(Vector4 v1, Vector4 v2) {
            return new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }
        public static Vector4 operator -(Vector4 v1, Vector4 v2) {
            return new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }
        public static Vector4 operator *(Vector4 v1, double mag) {
            return new Vector4(v1.x * mag, v1.y * mag, v1.z * mag, v1.w * mag);
        }
        public static Vector4 operator *(double mag, Vector4 v1) {
            return new Vector4(v1.x * mag, v1.y * mag, v1.z * mag, v1.w * mag);
        }
        public static Vector4 operator -(Vector4 v1) {
            return v1 * (-1);
        }

        /// <summary>
        /// Get component by index (0 = x, 1 = y, 2 = z, 3 = w)
        /// </summary>
        public double this[int i] {
            get {
                switch (i) {
                    case 0:
                        return this.x;
                    case 1:
                        return this.y;
                    case 2:
                        return this.z;
                    case 3:
                        return this.w;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (i) {
                    case 0:
                        this.x = value;
                        break;
                    case 1:
                        this.y = value;
                        break;
                    case 2:
                        this.z = value;
                        break;
                    case 3:
                        this.w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }
        #endregion
    }


    /// <summary>
    /// A general purpose struct for a 4x4 matrix 
    /// </summary>
    public struct Matrix4x4 {
        /*
            Note not all operator overloads have been implemented.
        */

        /// <summary>
        /// A general purpose struct for a 3x3 matrix.
        /// </summary>
        private struct Matrix3x3 {
            /*
                Can be used for matrix calculations, however it is only used currently by Matrix4x4
                Note not all operator overloads have been implemented.
            */
            #region Member Variables

            /// <summary>
            /// Is this an identity matrix?
            /// </summary>
            public bool isIdentity {
                get {
                    return (this == Matrix3x3.identity);
                }
                private set { }
            }

            public double m00 {
                get { return contents[0][0]; }
                set { contents[0][0] = value; }
            }
            public double m01 {
                get { return contents[0][1]; }
                set { contents[0][1] = value; }
            }
            public double m02 {
                get { return contents[0][2]; }
                set { contents[0][2] = value; }
            }
            public double m10 {
                get { return contents[1][0]; }
                set { contents[1][0] = value; }
            }
            public double m11 {
                get { return contents[1][1]; }
                set { contents[1][1] = value; }
            }
            public double m12 {
                get { return contents[1][2]; }
                set { contents[1][2] = value; }
            }
            public double m20 {
                get { return contents[2][0]; }
                set { contents[2][0] = value; }
            }
            public double m21 {
                get { return contents[2][1]; }
                set { contents[2][1] = value; }
            }
            public double m22 {
                get { return contents[2][2]; }
                set { contents[2][2] = value; }
            }

            /// <summary>
            /// The rows of this matrix from top to bottom
            /// </summary>
            private Vector3[] contents;

            private struct Matrix2x2 {

                /*
                  A general purpose 2x2 matrix.
                  Can be used for any matrix calculations, however it is only used currently by Matrix4x4
                  This struct is much less robust than the 3x3 and 4x4 matrix structs since it is only used by Matrix3x3
                */
                #region Member Variables

                /// <summary>
                /// The rows of this matrix from top to bottom.
                /// </summary>
                private Vector2[] contents;

                public bool isIdentity {
                    get {
                        return (this == Matrix2x2.identity);
                    }
                    private set { }
                }

                public double m00 {
                    get { return contents[0][0]; }
                    set { contents[0][0] = value; }
                }
                public double m01 {
                    get { return contents[0][1]; }
                    set { contents[0][1] = value; }
                }
                public double m10 {
                    get { return contents[1][0]; }
                    set { contents[1][0] = value; }
                }
                public double m11 {
                    get { return contents[1][1]; }
                    set { contents[1][1] = value; }
                }
                #endregion

                #region Static Variables
                public static Matrix2x2 identity {
                    get {
                        return new Matrix2x2(new Vector2(1, 0), new Vector2(0, 1));
                    }
                    private set { }
                }
                #endregion

                #region Member Methods
                public Vector2 GetRow(int row) {
                    return contents[row];
                }
                public Vector2 GetCol(int col) {
                    return new Vector2(contents[0][col], contents[1][col]);
                }
                private Vector2[] ToVector2Array() {
                    return contents;
                }

                public override bool Equals(object obj) {
                    return (Matrix2x2)obj == this;
                }
                public override int GetHashCode() {
                    return base.GetHashCode();
                }
                #endregion

                #region Static Methods
                public static double Det(Matrix2x2 matrix) {
                    return (matrix.contents[0][0] * matrix.contents[1][1]) - (matrix.contents[1][0] * matrix.contents[0][1]);
                }
                #endregion

                #region Constructors
                public Matrix2x2(Vector2 v1, Vector2 v2) {
                    this.contents = new Vector2[2];
                    this.contents[0] = v1;
                    this.contents[1] = v2;
                }
                public Matrix2x2(double m00, double m01, double m10, double m11) {
                    this.contents = new Vector2[2];
                    this.m00 = m00;
                    this.m01 = m01;
                    this.m10 = m10;
                    this.m11 = m11;
                }
                #endregion

                #region Operator Overloads
                public static Matrix2x2 operator *(Matrix2x2 m1, Matrix2x2 m2) {
                    Vector2[] final = new Vector2[2];
                    for (int i = 0; i < 2; i++) {
                        for (int j = 0; j < 2; j++) {
                            final[i][j] = Vector2.DotProduct(m1.GetRow(i), m2.GetCol(j));
                        }
                    }
                    return new Matrix2x2(final[0], final[1]);
                }
                public static Vector2 operator *(Matrix2x2 m1, Vector2 v1) {
                    Vector2[] tmp = m1.ToVector2Array();
                    Vector2 final = new Vector2(0, 0);
                    for (int i = 0; i < 2; i++) {
                        final[i] = Vector2.DotProduct(tmp[i], v1);
                    }
                    return final;
                }
                public static Matrix2x2 operator +(Matrix2x2 m1, Matrix2x2 m2) {
                    for (int i = 0; i < 2; i++) {
                        for (int j = 0; j < 2; j++) {
                            m1.contents[i][j] += m2.contents[i][j];
                        }
                    }
                    return m1;
                }
                public static Matrix2x2 operator -(Matrix2x2 m1, Matrix2x2 m2) {
                    for (int i = 0; i < 2; i++) {
                        for (int j = 0; j < 2; j++) {
                            m1.contents[i][j] += m2.contents[i][j];
                        }
                    }
                    return m1;
                }
                public static bool operator ==(Matrix2x2 m1, Matrix2x2 m2) {
                    return (m1.GetRow(0) == m2.GetRow(0) && m1.GetRow(1) == m2.GetRow(1) && m1.GetRow(2) == m2.GetRow(2));
                }
                public static bool operator !=(Matrix2x2 m1, Matrix2x2 m2) {
                    return (m1.GetRow(0) != m2.GetRow(0) || m1.GetRow(1) != m2.GetRow(1) || m1.GetRow(2) != m2.GetRow(2));
                }
                #endregion

            }

            #endregion

            #region Static Variables
            /// <summary>
            /// The read only 3x3 identity matrix
            /// </summary>
            public static Matrix3x3 identity {
                get {
                    return new Matrix3x3(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));
                }
                private set { }
            }

            /// <summary>
            /// The read only matrix representing a null matrix
            /// </summary>
            public static Matrix3x3 doesNotExist {
                get {
                    return new Matrix3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
                private set { }
            }
            #endregion

            #region Constructors

            /// <param name="v1">The uppermost row</param>
            /// <param name="v2">The middle row</param>
            /// <param name="v3">The bottom row</param>
            public Matrix3x3(Vector3 v1, Vector3 v2, Vector3 v3) {
                this.contents = new Vector3[3];
                this.contents[0] = v1;
                this.contents[1] = v2;
                this.contents[2] = v3;
            }

            /// <summary>
            /// Create Matrix with specified values at m(X, Y) where m00 is the top-left
            /// </summary>
            public Matrix3x3(double m00, double m01, double m02, double m10, double m11, double m12, double m20, double m21, double m22) {
                this.contents = new Vector3[3];
                this.m00 = m00;
                this.m01 = m01;
                this.m02 = m02;
                this.m10 = m10;
                this.m11 = m11;
                this.m12 = m12;
                this.m20 = m20;
                this.m21 = m21;
                this.m22 = m22;
            }
            #endregion

            #region Member Methods
            public Vector3 GetRow(int row) {
                if (row >= 0 && row <= 2) {
                    return contents[row];
                } else {
                    throw new IndexOutOfRangeException();
                }
            }

            public Vector3 GetCol(int col) {
                if (col >= 0 && col <= 2) {
                    return new Vector3(contents[0][col], contents[1][col], contents[2][col]);
                } else {
                    throw new IndexOutOfRangeException();
                }
            }

            public void SetRow(int row, Vector3 elements) {
                if (row >= 0 && row <= 2) {
                    contents[row] = elements;
                } else {
                    throw new IndexOutOfRangeException();
                }
            }

            public void SetCol(int col, Vector3 elements) {
                if (col >= 0 && col <= 2) {
                    contents[col].x = elements.x;
                    contents[col].y = elements.y;
                    contents[col].z = elements.z;
                } else {
                    throw new IndexOutOfRangeException();
                }
            }

            public override string ToString() {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 3; i++) {
                    sb.Append(contents[i].ToString());
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }

            public override bool Equals(object obj) {
                return (Matrix3x3)obj == this;
            }

            public override int GetHashCode() {
                return base.GetHashCode();
            }

            private Vector3[] ToVector3Array() {
                return contents;
            }

            /// <summary>
            /// Determines the determinant of a 2x2 submatrix which does not lie on the specified row and column
            /// </summary>
            /// <param name="X">Row Number</param>
            /// <param name="Y">Column Number</param>
            /// <returns></returns>
            public double DetAt(int X, int Y) {
                if (X < 3 && Y < 3 && Y > -1 && X > -1) {
                    List<int> x = new List<int>(3) { 0, 1, 2 };
                    List<int> y = new List<int>(3) { 0, 1, 2 };
                    x.Remove(X);
                    y.Remove(Y);
                    return Matrix2x2.Det(new Matrix2x2(this.contents[x[0]][y[0]], this.contents[x[1]][y[0]], this.contents[x[0]][y[1]], this.contents[x[1]][y[1]]));

                } else {
                    throw new IndexOutOfRangeException();
                }
            }


            #endregion

            #region Static Methods
            /// <summary>
            /// Returns the determinant of matrix
            /// </summary>
            public static double Det(Matrix3x3 matrix) {
                double det = matrix.contents[0][0] * Matrix2x2.Det(new Matrix2x2(new Vector2(matrix.contents[1][1], matrix.contents[1][2]), new Vector2(matrix.contents[2][1], matrix.contents[2][2])));
                det -= matrix.contents[0][1] * Matrix2x2.Det(new Matrix2x2(new Vector2(matrix.contents[1][0], matrix.contents[1][2]), new Vector2(matrix.contents[2][0], matrix.contents[2][2])));
                det += matrix.contents[0][2] * Matrix2x2.Det(new Matrix2x2(new Vector2(matrix.contents[1][0], matrix.contents[1][1]), new Vector2(matrix.contents[2][0], matrix.contents[2][1])));
                return det;
            }

            /// <summary>
            /// Returns the cofactor of matrix
            /// </summary>
            public static Matrix3x3 Cofactor(Matrix3x3 matrix) {
                Vector3[] v = matrix.ToVector3Array();
                v[0][1] = -v[0][1];
                v[1][0] = -v[1][0];
                v[1][2] = -v[1][2];
                v[2][1] = -v[2][1];
                return new Matrix3x3(v[0], v[1], v[2]);
            }

            /// <summary>
            /// Returns the transpose of matrix
            /// </summary>
            public static Matrix3x3 Transpose(Matrix3x3 matrix) {
                Vector3[] v = matrix.ToVector3Array();
                double tmp = v[0][1];
                v[0][1] = v[1][0];
                v[1][0] = tmp;
                tmp = v[0][2];
                v[0][2] = v[2][0];
                v[2][0] = tmp;
                tmp = v[1][2];
                v[1][2] = v[2][1];
                v[2][1] = tmp;
                return new Matrix3x3(v[0], v[1], v[2]);
            }

            /// <summary>
            /// Returns the inverse of matrix
            /// </summary>
            public static Matrix3x3 Inverse(Matrix3x3 matrix) {
                Vector3[] vMinors = new Vector3[3];
                double det = Matrix3x3.Det(matrix);
                if (det != 0) {
                    for (int i = 0; i < 3; i++) {
                        for (int j = 0; j < 3; j++) {
                            vMinors[i][j] = matrix.DetAt(j, i);
                        }
                    }
                    Matrix3x3 final = new Matrix3x3(vMinors[0], vMinors[1], vMinors[2]);
                    final = Matrix3x3.Cofactor(final);
                    final = Matrix3x3.Transpose(final);
                    final = (1 / det) * final;
                    return final;
                } else {
                    return Matrix3x3.doesNotExist;
                }

            }
            #endregion

            #region Operator Overloads
            public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2) {
                Vector3[] final = new Vector3[3];
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        final[i][j] = Vector3.DotProduct(m1.GetRow(i), m2.GetCol(j));
                    }
                }
                return new Matrix3x3(final[0], final[1], final[2]);
            }
            public static Vector3 operator *(Matrix3x3 m1, Vector3 v1) {
                Vector3[] tmp = m1.ToVector3Array();
                Vector3 final = new Vector3(0, 0, 0);
                for (int i = 0; i < 3; i++) {
                    final[i] = Vector3.DotProduct(tmp[i], v1);
                }
                return final;
            }
            public static Matrix3x3 operator *(Matrix3x3 m1, double f1) {
                Vector3[] tmp = m1.ToVector3Array();
                for (int i = 0; i < 3; i++) {
                    tmp[i].x = tmp[i].x * f1;
                    tmp[i].y = tmp[i].y * f1;
                    tmp[i].z = tmp[i].z * f1;
                }
                return new Matrix3x3(tmp[0], tmp[1], tmp[2]);
            }
            public static Matrix3x3 operator *(double f1, Matrix3x3 m1) {
                Vector3[] tmp = m1.ToVector3Array();
                for (int i = 0; i < 3; i++) {
                    tmp[i].x = tmp[i].x * f1;
                    tmp[i].y = tmp[i].y * f1;
                    tmp[i].z = tmp[i].z * f1;
                }
                return new Matrix3x3(tmp[0], tmp[1], tmp[2]);
            }
            public static Matrix3x3 operator +(Matrix3x3 m1, Matrix3x3 m2) {
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        m1.contents[i][j] += m2.contents[i][j];
                    }
                }
                return m1;
            }
            public static Matrix3x3 operator -(Matrix3x3 m1, Matrix3x3 m2) {
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        m1.contents[i][j] += m2.contents[i][j];
                    }
                }
                return m1;
            }
            public static bool operator ==(Matrix3x3 m1, Matrix3x3 m2) {
                return (m1.GetRow(0) == m2.GetRow(0) && m1.GetRow(1) == m2.GetRow(1) && m1.GetRow(2) == m2.GetRow(2));
            }
            public static bool operator !=(Matrix3x3 m1, Matrix3x3 m2) {
                return (m1.GetRow(0) != m2.GetRow(0) || m1.GetRow(1) != m2.GetRow(1) || m1.GetRow(2) != m2.GetRow(2));
            }
            #endregion
        }


        #region Member Variables

        /// <summary>
        /// The columns of this matrix from left to right
        /// </summary>
        private Vector4[] contents;

        /// <summary>
        /// Is this an identity matrix?
        /// </summary>
        public bool isIdentity {
            get {
                return (this == Matrix4x4.identity);
            }
            private set { }
        }

        #region Matrix Elements
        //columns, rows
        public double m11 {
            get { return contents[0][0]; }
            set { contents[0][0] = value; }
        }
        public double m12 {
            get { return contents[1][0]; }
            set { contents[1][0] = value; }
        }
        public double m13 {
            get { return contents[2][0]; }
            set { contents[2][0] = value; }
        }
        public double m14 {
            get { return contents[3][0]; }
            set { contents[3][0] = value; }
        }
        public double m21 {
            get { return contents[0][1]; }
            set { contents[0][1] = value; }
        }
        public double m22 {
            get { return contents[1][1]; }
            set { contents[1][1] = value; }
        }
        public double m23 {
            get { return contents[2][1]; }
            set { contents[2][1] = value; }
        }
        public double m24 {
            get { return contents[3][1]; }
            set { contents[3][1] = value; }
        }
        public double m31 {
            get { return contents[0][2]; }
            set { contents[0][2] = value; }
        }
        public double m32 {
            get { return contents[1][2]; }
            set { contents[1][2] = value; }
        }
        public double m33 {
            get { return contents[2][2]; }
            set { contents[2][2] = value; }
        }
        public double m34 {
            get { return contents[3][2]; }
            set { contents[3][2] = value; }
        }
        public double m41 {
            get { return contents[0][3]; }
            set { contents[0][3] = value; }
        }
        public double m42 {
            get { return contents[1][3]; }
            set { contents[1][3] = value; }
        }
        public double m43 {
            get { return contents[3][1]; }
            set { contents[3][1] = value; }
        }
        public double m44 {
            get { return contents[3][3]; }
            set { contents[3][3] = value; }
        }
        #endregion
        #endregion

        #region Static Variables

        /// <summary>
        /// Returns the 4x4 identity matrix
        /// </summary>
        public static Matrix4x4 identity {
            get {
                return new Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));
            }
            private set { }
        }

        /// <summary>
        /// The read only matrix representing a null matrix
        /// </summary>
        public static Matrix4x4 doesNotExist {
            get {
                return new Matrix4x4(new Vector4(double.NaN, double.NaN, double.NaN, double.NaN), new Vector4(double.NaN, double.NaN, double.NaN, double.NaN), new Vector4(double.NaN, double.NaN, double.NaN, double.NaN), new Vector4(double.NaN, double.NaN, double.NaN, double.NaN));
            }
            private set { }
        }
        #endregion

        #region Constructors

        /// <param name="v1">The leftmost column</param>
        /// <param name="v2">The second column</param>
        /// <param name="v3">The third column</param>
        /// <param name="v4">The rightmost column</param>
        public Matrix4x4(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 v4) {
            this.contents = new Vector4[4];
            this.contents[0] = v1;
            this.contents[1] = v2;
            this.contents[2] = v3;
            this.contents[3] = v4;
        }
        #endregion

        #region Member Methods

        public Vector4 GetRow(int row) {
            if (row >= 0 && row <= 3) {
                return new Vector4(contents[0][row], contents[1][row], contents[2][row], contents[3][row]);
            } else {
                throw new IndexOutOfRangeException();
            }
        }

        public Vector4 GetCol(int col) {
            if (col >= 0 && col <= 3) {
                return contents[col];
            }else {
                throw new IndexOutOfRangeException();
            }
        }

        public void SetRow(int row, Vector4 elements) {
            if (row >= 0 && row >= 3) {                
                contents[0][row] = elements[0];
                contents[1][row] = elements[1];
                contents[2][row] = elements[2];
                contents[3][row] = elements[3];
            } else {
                throw new IndexOutOfRangeException();
            }
        }

        public void SetCol(int col, Vector4 elements) {
            if (col >= 0 && col >= 3) {
                contents[col] = elements;
            } else {
                throw new IndexOutOfRangeException();
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++) {
                sb.Append(new Vector4(contents[0][i], contents[1][i], contents[2][i], contents[3][i]));
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
        public override bool Equals(object obj) {
            return (Matrix4x4)obj == this;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        private Vector4[] ToVector4Array() {
            return new Vector4[4] { contents[0], contents[1], contents[2], contents[3]};
        }

        /// <summary>
        /// Determines the determinant of a 3x3 submatrix which does not lie on the specified row and column
        /// </summary>
        /// <param name="X">Row Number</param>
        /// <param name="Y">Column Number</param>
        /// <returns></returns>
        public double DetAt(int X, int Y) {
            if (X < 4 && Y < 4 && Y > -1 && X > -1) {
                List<int> x = new List<int>(4) { 0, 1, 2, 3 };
                List<int> y = new List<int>(4) { 0, 1, 2, 3 };
                x.Remove(X);
                y.Remove(Y);
                return Matrix3x3.Det(new Matrix3x3(this.contents[x[0]][y[0]], this.contents[x[0]][y[1]], this.contents[x[0]][y[2]],
                                                    this.contents[x[1]][y[0]], this.contents[x[1]][y[1]], this.contents[x[1]][y[2]],
                                                      this.contents[x[2]][y[0]], this.contents[x[2]][y[1]], this.contents[x[2]][y[2]]));

            } else {
                throw new IndexOutOfRangeException();
            }
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the determinant of matrix
        /// </summary>
        public static double Det(Matrix4x4 matrix) {
            Vector4 col = matrix.GetCol(0);
            double det = col[0] * matrix.DetAt(0, 0);
            det -= col[1] * matrix.DetAt(0, 1);
            det += col[2] * matrix.DetAt(0, 2);
            det -= col[3] * matrix.DetAt(0, 3);
            return det;
        }

        /// <summary>
        /// Returns the cofactor of matrix
        /// </summary>
        public static Matrix4x4 Cofactor(Matrix4x4 matrix) {
            Vector4[] v = matrix.ToVector4Array();
            v[0][1] = -v[0][1];
            v[0][3] = -v[0][3];
            v[1][0] = -v[1][0];
            v[1][2] = -v[1][2];
            v[2][1] = -v[2][1];
            v[2][3] = -v[2][3];
            v[3][0] = -v[3][0];
            v[3][2] = -v[3][2];
            return new Matrix4x4(v[0], v[1], v[2], v[3]);
        }

        /// <summary>
        /// Returns the transpose of matrix
        /// </summary>
        public static Matrix4x4 Transpose(Matrix4x4 matrix) {
            Vector4[] v = matrix.ToVector4Array();
            double tmp = v[0][1];
            v[0][1] = v[1][0];
            v[1][0] = tmp;
            tmp = v[0][2];
            v[0][2] = v[2][0];
            v[2][0] = tmp;
            tmp = v[1][2];
            v[1][2] = v[2][1];
            v[2][1] = tmp;
            tmp = v[3][0];
            v[3][0] = v[0][3];
            v[0][3] = tmp;
            tmp = v[1][3];
            v[1][3] = v[3][1];
            v[3][1] = tmp;
            tmp = v[3][2];
            v[3][2] = v[2][3];
            v[2][3] = tmp;
            return new Matrix4x4(v[0], v[1], v[2], v[3]);
        }

        /// <summary>
        /// Returns the inverse of matrix
        /// </summary>
        public static Matrix4x4 Inverse(Matrix4x4 matrix) {
            Vector4[] vMinors = new Vector4[4];
            double det = Matrix4x4.Det(matrix);
            if (det != 0) {
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        vMinors[i][j] = matrix.DetAt(i, j);
                    }
                }
                Matrix4x4 final = new Matrix4x4(vMinors[0], vMinors[1], vMinors[2], vMinors[3]);

                final = Matrix4x4.Cofactor(final);
                final = Matrix4x4.Transpose(final);

                final = (1 / det) * final;
                return final;
            } else {
                return Matrix4x4.doesNotExist;
            }
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation around the X axis.
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        public static Matrix4x4 RotXTransform(double angle) {
            return new Matrix4x4(new Vector4(1, 0, 0, 0),
                                 new Vector4(0, Math.Cos(angle), -Math.Sin(angle), 0),
                                 new Vector4(0, Math.Sin(angle), Math.Cos(angle), 0),
                                 new Vector4(0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation around the Y axis.
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        public static Matrix4x4 RotYTransform(double angle) {
            return new Matrix4x4(new Vector4(Math.Cos(angle), 0, Math.Sin(angle), 0),
                                 new Vector4(0, 1, 0, 0),
                                 new Vector4(-Math.Sin(angle), 0, Math.Cos(angle), 0),
                                 new Vector4(0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation around the Z axis.
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        public static Matrix4x4 RotZTransform(double angle) {
            return new Matrix4x4(new Vector4(Math.Cos(angle), -Math.Sin(angle), 0, 0),
                                 new Vector4(Math.Sin(angle), Math.Cos(angle), 0, 0),
                                 new Vector4(0, 0, 1, 0),
                                 new Vector4(0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation for all axes
        /// </summary>
        /// <param name="Rotation">Components will represent the angle in radians around each axis</param>
        public static Matrix4x4 CombinedRotation(Vector3 angles) {
            return ((RotYTransform(angles.y) * RotZTransform(angles.z)) * RotXTransform(angles.x));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation for all axes in the global reference frame
        /// </summary>
        /// <param name="angles">Components will represent the angle in radians around each axis</param>
        public static Matrix4x4 GlobalReferenceRotation(Vector3 angles) {
            return (RotZTransform(angles.z) * (RotYTransform(angles.y) * RotXTransform(angles.x)));
        }

        /// <summary>
        /// Returns a rotation matrix which will perform a rotation as specified
        /// </summary>
        /// <param name="angle">The arbitrary axis about which the rotation will be performed</param>
        /// <param name="degrees">The counter-clockwise angle in radians</param>
        public static Matrix4x4 RotationFromAxisAngle(Vector3 axis, double angle) {
            Vector3 ax = axis.normalized;
            Matrix4x4 ret = new Matrix4x4(Vector4.zero, Vector4.zero, Vector4.zero, new Vector4(0, 0, 0, 1));
            double first = 1 - Math.Cos(angle);

            ret.m11 = (first * Math.Pow(ax.x, 2) + Math.Cos(angle));
            ret.m12 = (first * ax.x * ax.y - ax.z * Math.Sin(angle));
            ret.m13 = (first * ax.x * ax.z + ax.y * Math.Sin(angle));
            ret.m21 = (first * ax.x * ax.y + ax.z * Math.Sin(angle));
            ret.m22 = (first * Math.Pow(ax.y, 2) + Math.Cos(angle));
            ret.m23 = (first * ax.y * ax.z - ax.x * Math.Sin(angle));
            ret.m31 = (first * ax.x * ax.z - ax.y * Math.Sin(angle));
            ret.m32 = (first * ax.z * ax.y + ax.x * Math.Sin(angle));
            ret.m33 = (first * Math.Pow(ax.z, 2) + Math.Cos(angle));
            return ret;
        }
        
        /// <summary>
        /// Returns a rotation matrix which will perform a rotation that will move "from" to "to"
        /// </summary>
        /// <param name="from">Starting location</param>
        /// <param name="to">Final location</param>
        public static Matrix4x4 FromToRotation(Vector3 from, Vector3 to) {
            if(Vector3.Angle(from, to) < MathConst.EPSILON) {
                return identity;
            }
            return RotationFromAxisAngle(Vector3.CrossProduct(from, to), Vector3.Angle(from, to));
        }
        #endregion

        #region Operator Overloads
        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2) {
            Vector4[] col = new Vector4[4];
            for (int i = 0; i < 4; i++) {
                col[i] = new Vector4();
                for(int j = 0; j < 4; j++) {
                    col[i][j] = Vector4.DotProduct(m1.GetRow(j), m2.GetCol(i));
                }
            }
            return new Matrix4x4(col[0], col[1], col[2], col[3]);
        }
        public static Vector4 operator *(Matrix4x4 m1, Vector4 v1) {
            Vector4 final = new Vector4(0, 0, 0, 0);
            for (int i = 0; i < 4; i++) {
                final[i] = Vector4.DotProduct(m1.GetRow(i), v1);
            }
            return final;
        }

        //Note these two multiplications will round values to 3 decimal places. This is useful when finding matrix inversions.
        public static Matrix4x4 operator *(Matrix4x4 m1, double f1) {
            Vector4[] tmp = m1.ToVector4Array();
            for (int i = 0; i < 4; i++) {
                tmp[i].w = Math.Round(tmp[i].w * f1, 3);
                tmp[i].x = Math.Round(tmp[i].x * f1, 3);
                tmp[i].y = Math.Round(tmp[i].y * f1, 3);
                tmp[i].z = Math.Round(tmp[i].z * f1, 3);
            }
            return new Matrix4x4(tmp[0], tmp[1], tmp[2], tmp[3]);
        }
        public static Matrix4x4 operator *(double f1, Matrix4x4 m1) {
            Vector4[] tmp = m1.ToVector4Array();
            for (int i = 0; i < 4; i++) {
                tmp[i].w = Math.Round(tmp[i].w * f1, 3);
                tmp[i].x = Math.Round(tmp[i].x * f1, 3);
                tmp[i].y = Math.Round(tmp[i].y * f1, 3);
                tmp[i].z = Math.Round(tmp[i].z * f1, 3);
            }
            return new Matrix4x4(tmp[0], tmp[1], tmp[2], tmp[3]);
        }

        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2) {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    m1.contents[i][j] += m2.contents[i][j];
                }
            }
            return m1;
        }
        public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2) {
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    m1.contents[i][j] -= m2.contents[i][j];
                }
            }
            return m1;
        }
        public static bool operator ==(Matrix4x4 m1, Matrix4x4 m2) {
            return (m1.GetCol(0) == m2.GetCol(0) && m1.GetCol(1) == m2.GetCol(1) && m1.GetCol(2) == m2.GetCol(2) && m1.GetCol(3) == m2.GetCol(3));
        }
        public static bool operator !=(Matrix4x4 m1, Matrix4x4 m2) {
            return (m1.GetRow(0) != m2.GetCol(0) || m1.GetCol(1) != m2.GetCol(1) || m1.GetCol(2) != m2.GetCol(2) || m1.GetCol(3) != m2.GetCol(3));
        }
        #endregion 
    }


    public struct Quaternion {

        #region Member Variables
        private double w, x, y, z;

        /// <summary>
        /// The "length" of this quaternion
        /// </summary>
        public double magnitude {
            get {
                return Math.Sqrt(Math.Pow(w, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            }
            private set { }
        }
        /// <summary>
        /// The square of this quaternion's magnitude
        /// </summary>
        public double sqrMagnitude {
            get {
                return (Math.Pow(w, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            }
            private set { }
        }

        public Quaternion conjugate {
            get { return new Quaternion(w, -x, -y, -z); }
            private set { }
        }

        /// <summary>
        /// Does this quaternion have a magnitude of 1?
        /// </summary>
        public bool isUnitQuaternion {
            get { return (Math.Pow(w, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)) - 1 < MathConst.EPSILON; }
            private set { }
        }

        /// <summary>
        /// The Rotation Matrix which corresponds to this quaternion
        /// </summary>
        public Matrix4x4 rotationMatrix {
            get {
                return new Matrix4x4(new Vector4(1 - 2 * Math.Pow(y, 2) - 2 * Math.Pow(z, 2),
                                                    2 * x * y + 2 * w * z,
                                                        2 * x * z - 2 * w * y, 0),
                                         new Vector4(2 * x * y + 2 * w * z,
                                                        1 - 2 * Math.Pow(x, 2) - 2 * Math.Pow(z, 2),
                                                            2 * y * z - 2 * w * x, 0),
                                            new Vector4(2 * x * z - 2 * w * y,
                                                            2 * y * z + 2 * w * x,
                                                                1 - 2 * Math.Pow(x, 2) - 2 * Math.Pow(y, 2), 0),
                                                new Vector4(0, 0, 0, 1));
            }
            private set { }
        }
        #endregion

        #region Static Variables
        /// <summary>
        /// The identity quaternion (1,0,0,0)
        /// </summary>
        public static Quaternion identity {
            get { return new Quaternion(1, 0, 0, 0); }
            private set { }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a quaternion with w,x,y,z. This is not a unit quaternion so don't use it for rotations
        /// </summary>
        public Quaternion(double w, double x, double y, double z) {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// Creates a unit quaternion based on the rotation specified
        /// </summary>
        public Quaternion(Vector3 axis, double angle) {
            axis.Normalize();
            this.w = Math.Cos(-angle / 2);
            this.x = axis.x * Math.Sin(-angle / 2);
            this.y = axis.y * Math.Sin(-angle / 2);
            this.z = axis.z * Math.Sin(-angle / 2);
        }

        public Quaternion(Matrix4x4 m1) {
            double tr = m1.m11 + m1.m22 + m1.m33;

            if (tr > 0) {
                double S = Math.Sqrt(tr + 1.0) * 2; // S=4*qw 
                w = 0.25 * S;
                x = (m1.m32 - m1.m32) / S;
                y = (m1.m31 - m1.m13) / S;
                z = (m1.m12 - m1.m21) / S;
            } else if ((m1.m11 > m1.m22) && (m1.m11 > m1.m33)) {
                double S = Math.Sqrt(1.0 + m1.m11 - m1.m22 - m1.m33) * 2; // S=4*qx 
                w = (m1.m23 - m1.m32) / S;
                x = 0.25 * S;
                y = (m1.m21 + m1.m12) / S;
                z = (m1.m31 + m1.m13) / S;
            } else if (m1.m22 > m1.m33) {
                double S = Math.Sqrt(1.0 + m1.m22 - m1.m11 - m1.m33) * 2; // S=4*qy
                w = (m1.m31 - m1.m13) / S;
                x = (m1.m21 + m1.m12) / S;
                y = 0.25 * S;
                z = (m1.m32 + m1.m23) / S;
            } else {
                double S = Math.Sqrt(1.0 + m1.m33 - m1.m11 - m1.m22) * 2; // S=4*qz
                w = (m1.m12 - m1.m21) / S;
                x = (m1.m31 + m1.m13) / S;
                y = (m1.m32 + m1.m23) / S;
                z = 0.25 * S;
            }
        }
        #endregion

        #region Member Methods

        public Vector3 RotateVector3(Vector3 original) {
            Quaternion temp = (this * new Quaternion(0, original.x, original.y, original.z) * this.conjugate);
            return new Vector3(temp.x, temp.y, temp.z);
        }

        /// <summary>
        /// Get the 4x4 matrix corresponding to this
        /// </summary>
        public Matrix4x4 getMatrix4x4() {
            Normalize();
            double sw = w * w, sx = x * x, sy = y * y, sz = z * z;
            double temp1 = x * y, temp2 = x * w, temp3 = x*z, temp4 = y*w, temp5 = y*z, temp6 = x*w;
            Matrix4x4 m1 = new Matrix4x4(new Vector4(sx - sy - sz + sw, 2*(temp1 + temp2), 2*(temp3-temp4), 0),
                                            new Vector4(2*(temp1 - temp2), -sx + sy - sz + sw, 2*(temp5+temp6), 0),
                                                new Vector4(2*(temp4+temp3), 2*(temp5 - temp6), -sx - sy + sz + sw, 0),
                                                    new Vector4(0, 0, 0, 1));

            return m1;
        }

        /// <summary>
        /// Make this quaternion have a magnitude of 1
        /// </summary>
        public void Normalize() {
            if (isUnitQuaternion) {
                return;
            }
            double mag = Math.Sqrt(Math.Pow(w, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            w /= mag;
            x /= mag;
            y /= mag;
            z /= mag;
        }

        public override string ToString() {
            return "(" + w + ", " + x + "i, " + y + "j, " + z + "k)";
        }
        #endregion

        #region Operator Overloads
        public static Quaternion operator *(Quaternion q1, Quaternion q2) {
            double w, x, y, z;
            w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
            x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y;
            y = q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x;
            z = q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w;
            return new Quaternion(w, x, y, z);
        }
        #endregion
    }

    public class Mesh {
        #region Member Variables
        public Vector3[] vertices { get; set; }
        public int[][] faces { get; set; }
        public int[][] edges { get; set; }
        public string name { get; set; }
        #endregion

        #region Static Variables
        public static Mesh Cube {
            get { return ReadObjFile(GlobalConstants.PRIMITIVE_FILE_DIR + "Cube.obj"); }
            private set { }
        }
        public static Mesh Tetahedron {
            get { return ReadObjFile(GlobalConstants.PRIMITIVE_FILE_DIR + "Tetrahedron.obj"); }
            private set { }
        }
        public static Mesh GridFloor {
            get { return ReadObjFile(GlobalConstants.PRIMITIVE_FILE_DIR + "Grid.obj"); }
            private set { }
        }
        public static Mesh CameraFrustrum {
            get { return ReadObjFile(GlobalConstants.PRIMITIVE_FILE_DIR + "Camera.obj"); }
            private set { }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Return a mesh that corresponds with the .obj file at the specified path
        /// </summary>
        public static Mesh ReadObjFile(string path) {
            string line;
            string[] words;
            char[] lineCh;
            List<Vector3> vertices = new List<Vector3>();
            Vector3 temp = Vector3.zero;
            List<List<int>> faces = new List<List<int>>();
            List<int[]> edges = new List<int[]>();
            int face = 0;
            Mesh result = new Mesh();
            System.IO.StreamReader stream = new System.IO.StreamReader(path);

            while ((line = stream.ReadLine()) != null) {
                lineCh = line.ToCharArray();
                if (lineCh[0] == '#') {
                    continue;
                }

                words = line.Split(' ');

                switch (words[0]) {
                    case "v":
                        for (int i = 1; i < 4; i++) {
                            temp[i - 1] = double.Parse(words[i]);
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
        #endregion

        #region Constructors
        public Mesh(Vector3[] vertices, int[][] faces, int[][] edges) {
            this.vertices = vertices;
            this.faces = faces;
            this.edges = edges;
            this.name = "";
        }
        public Mesh(Vector3[] vertices) {
            this.vertices = vertices;
            this.faces = new int[1][];
            this.edges = new int[1][];
            this.name = "";
        }
        public Mesh() {
            faces = new int[0][];
            vertices = new Vector3[0];
            edges = new int[0][];
        }
        public Mesh(System.IO.StreamReader sr) {
            
        }
        #endregion
    }

}