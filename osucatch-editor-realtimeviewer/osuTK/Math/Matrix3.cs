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
    /// Represents a 3x3 matrix containing 3D rotation and scale.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix3
    {
        /// <summary>
        /// First row of the matrix.
        /// </summary>
        public Vector3 Row0;

        /// <summary>
        /// Second row of the matrix.
        /// </summary>
        public Vector3 Row1;

        /// <summary>
        /// Third row of the matrix.
        /// </summary>
        public Vector3 Row2;

        /// <summary>
        /// The identity matrix.
        /// </summary>
        public static readonly Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        /// <summary>
        /// The zero matrix.
        /// </summary>
        public static readonly Matrix3 Zero = new Matrix3(Vector3.Zero, Vector3.Zero, Vector3.Zero);



        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="row0">Top row of the matrix</param>
        /// <param name="row1">Second row of the matrix</param>
        /// <param name="row2">Bottom row of the matrix</param>
        public Matrix3(Vector3 row0, Vector3 row1, Vector3 row2)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
        }
    }
}
