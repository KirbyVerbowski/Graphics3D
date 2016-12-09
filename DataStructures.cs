using System;
using System.Collections.Generic;
using System.Text;

/*
    DataStructures.cs
    Dec/2016
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
        public const float Deg2Rad = 0.0174533f;

        /// <summary>
        /// Convert an angle from radians to degrees
        /// </summary>
        public const float Rad2Deg = 57.2958f;
    }

    /// <summary>
    /// A general purpose struct for two dimensional vectors.
    /// </summary>
    public struct Vector2 {

        #region Member Variables
        /// <summary>
        /// The X component of this vector
        /// </summary>
        public float x;

        /// <summary>
        /// The Y component of this vector
        /// </summary>
        public float y;

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
        public float magnitude {
            get {
                return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
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
        public Vector2(float x, float y) {
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
        public void Rotate(float degrees, Vector2 origin) {
            Vector2 tmp = this - origin;
            this.x = tmp.x * (float)Math.Cos(degrees * MathConst.Deg2Rad) - tmp.y * (float)Math.Sin(degrees * MathConst.Deg2Rad);
            this.y = tmp.y * (float)Math.Cos(degrees * MathConst.Deg2Rad) + tmp.x * (float)Math.Sin(degrees * MathConst.Deg2Rad);
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
        public static float Angle(Vector2 v1, Vector2 v2) {
            return (float)(Math.Acos(Vector2.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude))) * MathConst.Rad2Deg;
        }

        /// <summary>
        /// The dot product of two vectors
        /// </summary>
        public static float DotProduct(Vector2 v1, Vector2 v2) {
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
        public static Vector2 operator *(Vector2 v1, float mag) {
            return new Vector2(v1.x * mag, v1.y * mag);
        }
        public static Vector2 operator *(float mag, Vector2 v1) {
            return new Vector2(v1.x * mag, v1.y * mag);
        }
        public static Vector2 operator -(Vector2 v1) {
            return v1 * (-1);
        }

        /// <summary>
        /// Get component by index (0 = x, 1 = y)
        /// </summary>
        public float this[int i] {
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
        public float x;

        /// <summary>
        /// The Y component of this vector
        /// </summary>
        public float y;

        /// <summary>
        /// The Z component of this vector
        /// </summary>
        public float z;

        /// <summary>
        /// The magnitude of this vector. Read only.
        /// </summary>
        public float magnitude {
            get {
                return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
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
        #endregion

        #region Constructors

        /// <summary>Creates a new Vector 3 with given components</summary>
        /// <param name="x">The X component of this vector</param>
        /// <param name="y">The Y component of this vector</param>
        /// <param name="z">The Z component of this vector</param>
        public Vector3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Creates a new Vector3 with given X and Y. Z set to 0.
        /// </summary>
        public Vector3(float x, float y) {
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

        public override string ToString() {
            return ("(" + x + ", " + y + ", " + z + ")");
        }
        public override bool Equals(object obj) {
            return (Vector3)obj == this;
        }
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Rotate this vector around the given axis. Not yet implemented.
        /// This might end up going under transform instead of Vector3.
        /// </summary>
        public void Rotate(Vector3 axis, float degrees) {
            throw new NotImplementedException("Rotate 3D");
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// The dot product of two vectors
        /// </summary>
        public static float DotProduct(Vector3 v1, Vector3 v2) {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Get the angle between two vectors in degrees
        /// </summary>
        public static float Angle(Vector3 v1, Vector3 v2) {
            return (float)(Math.Acos(Vector3.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude))) * MathConst.Rad2Deg;
        }

        /// <summary>
        /// Returns a new Vector4 with same x, y, and z as original with w set to 1
        /// </summary>
        public static Vector4 ToVector4(Vector3 original) {
            return new Vector4(original.x, original.y, original.z, 1);
        }
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
        public static Vector3 operator *(Vector3 v1, float mag) {
            return new Vector3(v1.x * mag, v1.y * mag, v1.z * mag);
        }
        public static Vector3 operator *(float mag, Vector3 v1) {
            return new Vector3(v1.x * mag, v1.y * mag, v1.z * mag);
        }
        public static Vector3 operator -(Vector3 v1) {
            return v1 * (-1);
        }

        /// <summary>
        /// Get component by index (0 = x, 1 = y, 2 = z)
        /// </summary>
        public float this[int i] {
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
        public float x;

        /// <summary>
        /// The Y component of this vector
        /// </summary>
        public float y;

        /// <summary>
        /// The Z component of this vector
        /// </summary>
        public float z;

        /// <summary>
        /// The W component of this vector
        /// </summary>
        public float w;

        /// <summary>
        /// The magnitude of this vector. Read only.
        /// </summary>
        public float magnitude {
            get {
                return (float)Math.Sqrt(Math.Pow(w, 2) + Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
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
        public Vector4(float x, float y, float z, float w) {
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
        public static float DotProduct(Vector4 v1, Vector4 v2) {
            return v1.w * v2.w + v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Get the angle between two vectors in degrees
        /// </summary>
        public static float Angle(Vector4 v1, Vector4 v2) {
            return (float)(Math.Acos(Vector4.DotProduct(v1, v2) / (v1.magnitude * v2.magnitude))) * MathConst.Rad2Deg;
        }

        /// <summary>
        /// Returns a new Vector3 by truncating the w component of original
        /// </summary>
        public static Vector3 ToVector3(Vector4 original) {
            return new Vector3(original.x, original.y, original.z);
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
        public static Vector4 operator *(Vector4 v1, float mag) {
            return new Vector4(v1.x * mag, v1.y * mag, v1.z * mag, v1.w * mag);
        }
        public static Vector4 operator *(float mag, Vector4 v1) {
            return new Vector4(v1.x * mag, v1.y * mag, v1.z * mag, v1.w * mag);
        }
        public static Vector4 operator -(Vector4 v1) {
            return v1 * (-1);
        }

        /// <summary>
        /// Get component by index (0 = x, 1 = y, 2 = z, 3 = w)
        /// </summary>
        public float this[int i] {
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
    /// A general purpose struct for a 3x3 matrix.
    /// </summary>
    public struct Matrix3x3 {
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

        public float m00 {
            get { return contents[0][0]; }
            set { contents[0][0] = value; }
        }
        public float m01 {
            get { return contents[0][1]; }
            set { contents[0][1] = value; }
        }
        public float m02 {
            get { return contents[0][2]; }
            set { contents[0][2] = value; }
        }
        public float m10 {
            get { return contents[1][0]; }
            set { contents[1][0] = value; }
        }
        public float m11 {
            get { return contents[1][1]; }
            set { contents[1][1] = value; }
        }
        public float m12 {
            get { return contents[1][2]; }
            set { contents[1][2] = value; }
        }
        public float m20 {
            get { return contents[2][0]; }
            set { contents[2][0] = value; }
        }
        public float m21 {
            get { return contents[2][1]; }
            set { contents[2][1] = value; }
        }
        public float m22 {
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

            public float m00 {
                get { return contents[0][0]; }
                set { contents[0][0] = value; }
            }
            public float m01 {
                get { return contents[0][1]; }
                set { contents[0][1] = value; }
            }
            public float m10 {
                get { return contents[1][0]; }
                set { contents[1][0] = value; }
            }
            public float m11 {
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
            public static float Det(Matrix2x2 matrix) {
                return (matrix.contents[0][0] * matrix.contents[1][1]) - (matrix.contents[1][0] * matrix.contents[0][1]);
            }
            #endregion

            #region Constructors
            public Matrix2x2(Vector2 v1, Vector2 v2) {
                this.contents = new Vector2[2];
                this.contents[0] = v1;
                this.contents[1] = v2;
            }
            public Matrix2x2(float m00, float m01, float m10, float m11) {
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
        public Matrix3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22) {
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
            }else {
                throw new IndexOutOfRangeException();
            }
        }

        public Vector3 GetCol(int col) {
            if (col >= 0 && col <= 2) {
                return new Vector3(contents[0][col], contents[1][col], contents[2][col]);
            }else {
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
        public float DetAt(int X, int Y) {
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
        public static float Det(Matrix3x3 matrix) {
            float det = matrix.contents[0][0] * Matrix2x2.Det(new Matrix2x2(new Vector2(matrix.contents[1][1], matrix.contents[1][2]), new Vector2(matrix.contents[2][1], matrix.contents[2][2])));
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
            float tmp = v[0][1];
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
            float det = Matrix3x3.Det(matrix);
            if(det != 0) {
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
            }else {
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
        public static Matrix3x3 operator *(Matrix3x3 m1, float f1) {
            Vector3[] tmp = m1.ToVector3Array();
            for(int i = 0; i < 3; i++) {
                tmp[i].x = tmp[i].x * f1;
                tmp[i].y = tmp[i].y * f1;
                tmp[i].z = tmp[i].z * f1;
            }
            return new Matrix3x3(tmp[0], tmp[1], tmp[2]);
        }
        public static Matrix3x3 operator *(float f1, Matrix3x3 m1) {
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

    /// <summary>
    /// A general purpose struct for a 4x4 matrix.
    /// </summary>
    public struct Matrix4x4 {
        /*
            Note not all operator overloads have been implemented.
        */
        #region Member Variables

        /// <summary>
        /// The rows of this matrix from top to bottom
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

        public float m00 {
            get { return contents[0][0]; }
            set { contents[0][0] = value; }
        }
        public float m01 {
            get { return contents[0][1]; }
            set { contents[0][1] = value; }
        }
        public float m02 {
            get { return contents[0][2]; }
            set { contents[0][2] = value; }
        }
        public float m03 {
            get { return contents[0][3]; }
            set { contents[0][3] = value; }
        }
        public float m10 {
            get { return contents[1][0]; }
            set { contents[1][0] = value; }
        }
        public float m11 {
            get { return contents[1][1]; }
            set { contents[1][1] = value; }
        }
        public float m12 {
            get { return contents[1][2]; }
            set { contents[1][2] = value; }
        }
        public float m13 {
            get { return contents[1][3]; }
            set { contents[1][3] = value; }
        }
        public float m20 {
            get { return contents[2][0]; }
            set { contents[2][0] = value; }
        }
        public float m21 {
            get { return contents[2][1]; }
            set { contents[2][1] = value; }
        }
        public float m22 {
            get { return contents[2][2]; }
            set { contents[2][2] = value; }
        }
        public float m23 {
            get { return contents[2][3]; }
            set { contents[2][3] = value; }
        }
        public float m30 {
            get { return contents[3][0]; }
            set { contents[3][0] = value; }
        }
        public float m31 {
            get { return contents[3][1]; }
            set { contents[3][1] = value; }
        }
        public float m32 {
            get { return contents[3][2]; }
            set { contents[3][2] = value; }
        }
        public float m33 {
            get { return contents[3][3]; }
            set { contents[3][3] = value; }
        }
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
                return new Matrix4x4(new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0), new Vector4(0, 0, 0, 0));
            }
            private set { }
        }
        #endregion

        #region Constructors

        /// <param name="v1">The uppermost row</param>
        /// <param name="v2">The second row</param>
        /// <param name="v3">The third row</param>
        /// <param name="v4">The bottom row</param>
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
                return contents[row];
            } else {
                throw new IndexOutOfRangeException();
            }
        }

        public Vector4 GetCol(int col) {
            if (col >= 0 && col <= 3) {
                return new Vector4(contents[0][col], contents[1][col], contents[2][col], contents[3][col]);
            }else {
                throw new IndexOutOfRangeException();
            }
        }

        public void SetRow(int row, Vector4 elements) {
            if (row >= 0 && row >= 3) {
                contents[row] = elements;
            } else {
                throw new IndexOutOfRangeException();
            }
        }

        public void SetCol(int col, Vector4 elements) {
            if (col >= 0 && col >= 3) {
                contents[col].w = elements.w;
                contents[col].x = elements.x;
                contents[col].y = elements.y;
                contents[col].z = elements.z;
            } else {
                throw new IndexOutOfRangeException();
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++) {
                sb.Append(contents[i].ToString());
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
            return contents;
        }

        /// <summary>
        /// Determines the determinant of a 3x3 submatrix which does not lie on the specified row and column
        /// </summary>
        /// <param name="X">Row Number</param>
        /// <param name="Y">Column Number</param>
        /// <returns></returns>
        public float DetAt(int X, int Y) {
            if (X < 4 && Y < 4 && Y > -1 && X > -1) {
                List<int> x = new List<int>(4) { 0, 1, 2, 3 };
                List<int> y = new List<int>(4) { 0, 1, 2, 3 };
                x.Remove(X);
                y.Remove(Y);
                return Matrix3x3.Det(new Matrix3x3(this.contents[y[0]][x[0]], this.contents[y[0]][x[1]], this.contents[y[0]][x[2]],
                                                    this.contents[y[1]][x[0]], this.contents[y[1]][x[1]], this.contents[y[1]][x[2]],
                                                      this.contents[y[2]][x[0]], this.contents[y[2]][x[1]], this.contents[y[2]][x[2]]));

            } else {
                throw new IndexOutOfRangeException();
            }
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Returns the determinant of matrix
        /// </summary>
        public static float Det(Matrix4x4 matrix) {
            float det = matrix.contents[0][0] * Matrix3x3.Det(new Matrix3x3(matrix.m11, matrix.m12, matrix.m13, matrix.m21, matrix.m22, matrix.m23, matrix.m31, matrix.m32, matrix.m33));
            det -= matrix.contents[0][1] * Matrix3x3.Det(new Matrix3x3(matrix.m10, matrix.m12, matrix.m13, matrix.m20, matrix.m22, matrix.m23, matrix.m30, matrix.m32, matrix.m33));
            det += matrix.contents[0][2] * Matrix3x3.Det(new Matrix3x3(matrix.m10, matrix.m11, matrix.m13, matrix.m20, matrix.m21, matrix.m23, matrix.m30, matrix.m31, matrix.m33));
            det -= matrix.contents[0][3] * Matrix3x3.Det(new Matrix3x3(matrix.m10, matrix.m11, matrix.m12, matrix.m20, matrix.m21, matrix.m22, matrix.m30, matrix.m31, matrix.m32));
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
            float tmp = v[0][1];
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
            float det = Matrix4x4.Det(matrix);
            if (det != 0) {
                for (int i = 0; i < 4; i++) {
                    for (int j = 0; j < 4; j++) {
                        vMinors[i][j] = matrix.DetAt(j, i);
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
        public static Matrix4x4 RotXTransform(float angle) {
            angle = angle * MathConst.Deg2Rad;
            return new Matrix4x4(new Vector4(1, 0, 0, 0),
                                 new Vector4(0, (float)Math.Cos(angle), -(float)Math.Sin(angle), 0),
                                 new Vector4(0, (float)Math.Sin(angle), (float)Math.Cos(angle), 0),
                                 new Vector4(0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation around the Y axis.
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        public static Matrix4x4 RotYTransform(float angle) {
            angle = angle * MathConst.Deg2Rad;
            return new Matrix4x4(new Vector4((float)Math.Cos(angle), 0, (float)Math.Sin(angle), 0),
                                 new Vector4(0, 1, 0, 0),
                                 new Vector4(-(float)Math.Sin(angle), 0, (float)Math.Cos(angle), 0),
                                 new Vector4(0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation around the Z axis.
        /// </summary>
        /// <param name="angle">angle in degrees</param>
        public static Matrix4x4 RotZTransform(float angle) {
            angle = angle * MathConst.Deg2Rad;
            return new Matrix4x4(new Vector4((float)Math.Cos(angle), -(float)Math.Sin(angle), 0, 0),
                                 new Vector4((float)Math.Sin(angle), (float)Math.Cos(angle), 0, 0),
                                 new Vector4(0, 0, 1, 0),
                                 new Vector4(0, 0, 0, 1));
        }

        /// <summary>
        /// Returns a rotation matrix which will represent a counter-clockwise rotation x degrees around the x axis,
        /// then y degrees around the y axis, then z degrees around the z axis
        /// </summary>
        /// <param name="Rotation">Components will represent the angle in degrees around each axis</param>
        public static Matrix4x4 CombinedRotation(Vector3 rotation) {
            return (Matrix4x4.RotXTransform(rotation.x) * Matrix4x4.RotYTransform(rotation.y) * Matrix4x4.RotZTransform(rotation.z));
        }
        #endregion

        #region Operator Overloads
        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2) {
            Vector4[] final = new Vector4[4];
            for (int i = 0; i < 4; i++) {
                for( int j = 0; j < 4; j++) {
                    final[i][j] = Vector4.DotProduct(m1.GetRow(i), m2.GetCol(j));
                }
            }
            return new Matrix4x4(final[0], final[1], final[2], final[3]);
        }
        public static Vector4 operator *(Matrix4x4 m1, Vector4 v1) {
            Vector4[] tmp = m1.ToVector4Array();
            Vector4 final = new Vector4(0, 0, 0, 0);
            for (int i = 0; i < 4; i++) {
                final[i] = Vector4.DotProduct(tmp[i], v1);
            }
            return final;
        }

        //Note these two multiplications will round values to 3 decimal places. This is useful when finding matrix inversions.
        public static Matrix4x4 operator *(Matrix4x4 m1, float f1) {
            Vector4[] tmp = m1.ToVector4Array();
            for (int i = 0; i < 4; i++) {
                tmp[i].w = (float)Math.Round(tmp[i].w * f1, 3);
                tmp[i].x = (float)Math.Round(tmp[i].x * f1, 3);
                tmp[i].y = (float)Math.Round(tmp[i].y * f1, 3);
                tmp[i].z = (float)Math.Round(tmp[i].z * f1, 3);
            }
            return new Matrix4x4(tmp[0], tmp[1], tmp[2], tmp[3]);
        }
        public static Matrix4x4 operator *(float f1, Matrix4x4 m1) {
            Vector4[] tmp = m1.ToVector4Array();
            for (int i = 0; i < 4; i++) {
                tmp[i].w = (float)Math.Round(tmp[i].w * f1, 3);
                tmp[i].x = (float)Math.Round(tmp[i].x * f1, 3);
                tmp[i].y = (float)Math.Round(tmp[i].y * f1, 3);
                tmp[i].z = (float)Math.Round(tmp[i].z * f1, 3);
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
                    m1.contents[i][j] += m2.contents[i][j];
                }
            }
            return m1;
        }
        public static bool operator ==(Matrix4x4 m1, Matrix4x4 m2) {
            return (m1.GetRow(0) == m2.GetRow(0) && m1.GetRow(1) == m2.GetRow(1) && m1.GetRow(2) == m2.GetRow(2) && m1.GetRow(3) == m2.GetRow(3));
        }
        public static bool operator !=(Matrix4x4 m1, Matrix4x4 m2) {
            return (m1.GetRow(0) != m2.GetRow(0) || m1.GetRow(1) != m2.GetRow(1) || m1.GetRow(2) != m2.GetRow(2) || m1.GetRow(3) != m2.GetRow(3));
        }
        #endregion 
    }

}
