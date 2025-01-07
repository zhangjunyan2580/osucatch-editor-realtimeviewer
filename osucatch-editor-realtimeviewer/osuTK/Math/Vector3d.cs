/*
Copyright (c) 2006 - 2008 The Open Toolkit library.

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace osuTK
{
    /// <summary>
    /// Represents a 3D vector using three double-precision floating-point numbers.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3d
    {
        /// <summary>
        /// The X component of the Vector3.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y component of the Vector3.
        /// </summary>
        public double Y;

        /// <summary>
        /// The Z component of the Vector3.
        /// </summary>
        public double Z;

        /// <summary>
        /// Constructs a new Vector3.
        /// </summary>
        /// <param name="x">The x component of the Vector3.</param>
        /// <param name="y">The y component of the Vector3.</param>
        /// <param name="z">The z component of the Vector3.</param>
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <see cref="LengthFast"/>
        /// <seealso cref="LengthSquared"/>
        public double Length
        {
            get
            {
                return System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        /// <summary>
        /// Gets an approximation of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthSquared"/>
        public double LengthFast
        {
            get
            {
                return 1.0 / MathHelper.InverseSqrtFast(X * X + Y * Y + Z * Z);
            }
        }

        /// <summary>
        /// Gets the square of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthFast"/>
        public double LengthSquared
        {
            get
            {
                return X * X + Y * Y + Z * Z;
            }
        }

        /// <summary>
        /// Scales the Vector3d to unit length.
        /// </summary>
        public void Normalize()
        {
            double scale = 1.0 / this.Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        /// <summary>
        /// Defines a unit-length Vector3d that points towards the X-axis.
        /// </summary>
        public static readonly Vector3d UnitX = new Vector3d(1, 0, 0);

        /// <summary>
        /// Defines a unit-length Vector3d that points towards the Y-axis.
        /// </summary>
        public static readonly Vector3d UnitY = new Vector3d(0, 1, 0);

        /// <summary>
        /// Defines a unit-length Vector3d that points towards the Z-axis.
        /// </summary>
        public static readonly Vector3d UnitZ = new Vector3d(0, 0, 1);

        /// <summary>
        /// Defines a zero-length Vector3.
        /// </summary>
        public static readonly Vector3d Zero = new Vector3d(0, 0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector3d One = new Vector3d(1, 1, 1);

        /// <summary>
        /// Defines the size of the Vector3d struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector3d());

        /// <summary>Converts osuTK.Vector3 to osuTK.Vector3d.</summary>
        /// <param name="v3">The Vector3 to convert.</param>
        /// <returns>The resulting Vector3d.</returns>
        public static explicit operator Vector3d(Vector3 v3)
        {
            return new Vector3d(v3.X, v3.Y, v3.Z);
        }

        /// <summary>Converts osuTK.Vector3d to osuTK.Vector3.</summary>
        /// <param name="v3d">The Vector3d to convert.</param>
        /// <returns>The resulting Vector3.</returns>
        public static explicit operator Vector3(Vector3d v3d)
        {
            return new Vector3((float)v3d.X, (float)v3d.Y, (float)v3d.Z);
        }

        private static string listSeparator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
    }
}
