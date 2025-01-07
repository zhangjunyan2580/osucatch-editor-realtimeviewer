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

namespace osuTK
{
    /// <summary>
    /// Represents a 4x4 matrix containing 3D rotation, scale, transform, and projection.
    /// </summary>
    /// <seealso cref="Matrix4d"/>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4
    {
        /// <summary>
        /// Top row of the matrix.
        /// </summary>
        public Vector4 Row0;

        /// <summary>
        /// 2nd row of the matrix.
        /// </summary>
        public Vector4 Row1;

        /// <summary>
        /// 3rd row of the matrix.
        /// </summary>
        public Vector4 Row2;

        /// <summary>
        /// Bottom row of the matrix.
        /// </summary>
        public Vector4 Row3;

        /// <summary>
        /// The identity matrix.
        /// </summary>
        public static readonly Matrix4 Identity = new Matrix4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        /// <summary>
        /// The zero matrix.
        /// </summary>
        public static readonly Matrix4 Zero = new Matrix4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="row0">Top row of the matrix.</param>
        /// <param name="row1">Second row of the matrix.</param>
        /// <param name="row2">Third row of the matrix.</param>
        /// <param name="row3">Bottom row of the matrix.</param>
        public Matrix4(Vector4 row0, Vector4 row1, Vector4 row2, Vector4 row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
    }
}
