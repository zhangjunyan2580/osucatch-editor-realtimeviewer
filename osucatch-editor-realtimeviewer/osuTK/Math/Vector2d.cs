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
    /// <summary>Represents a 2D vector using two double-precision floating-point numbers.</summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2d
    {
        /// <summary>The X coordinate of this instance.</summary>
        public double X;

        /// <summary>The Y coordinate of this instance.</summary>
        public double Y;

        /// <summary>
        /// Defines a unit-length Vector2d that points towards the X-axis.
        /// </summary>
        public static readonly Vector2d UnitX = new Vector2d(1, 0);

        /// <summary>
        /// Defines a unit-length Vector2d that points towards the Y-axis.
        /// </summary>
        public static readonly Vector2d UnitY = new Vector2d(0, 1);

        /// <summary>
        /// Defines a zero-length Vector2d.
        /// </summary>
        public static readonly Vector2d Zero = new Vector2d(0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector2d One = new Vector2d(1, 1);

        /// <summary>
        /// Defines the size of the Vector2d struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector2d());

        /// <summary>Constructs left vector with the given coordinates.</summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        public Vector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets the value at the index of the Vector.
        /// </summary>
        public double this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return X;
                }
                else if (index == 1)
                {
                    return Y;
                }
                throw new IndexOutOfRangeException("You tried to access this vector at index: " + index);
            }
            set
            {
                if (index == 0)
                {
                    X = value;
                }
                else if (index == 1)
                {
                    Y = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <seealso cref="LengthSquared"/>
        public double Length
        {
            get
            {
                return System.Math.Sqrt(X * X + Y * Y);
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
        public double LengthSquared
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        /// <summary>
        /// Adds two instances.
        /// </summary>
        /// <param name="left">The left instance.</param>
        /// <param name="right">The right instance.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2d operator +(Vector2d left, Vector2d right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        /// <summary>
        /// Multiplies an instance by a scalar.
        /// </summary>
        /// <param name="vec">The instance.</param>
        /// <param name="f">The scalar.</param>
        /// <returns>The result of the operation.</returns>
        public static Vector2d operator *(Vector2d vec, double f)
        {
            vec.X *= f;
            vec.Y *= f;
            return vec;
        }

        /// <summary>Converts osuTK.Vector2 to osuTK.Vector2d.</summary>
        /// <param name="v2">The Vector2 to convert.</param>
        /// <returns>The resulting Vector2d.</returns>
        public static explicit operator Vector2d(Vector2 v2)
        {
            return new Vector2d(v2.X, v2.Y);
        }

        /// <summary>Converts osuTK.Vector2d to osuTK.Vector2.</summary>
        /// <param name="v2d">The Vector2d to convert.</param>
        /// <returns>The resulting Vector2.</returns>
        public static explicit operator Vector2(Vector2d v2d)
        {
            return new Vector2((float)v2d.X, (float)v2d.Y);
        }

        private static string listSeparator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
    }
}
