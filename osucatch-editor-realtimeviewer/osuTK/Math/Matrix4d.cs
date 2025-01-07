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
    /// Represents a 4x4 matrix containing 3D rotation, scale, transform, and projection with double-precision components.
    /// </summary>
    /// <seealso cref="Matrix4"/>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4d
    {
        /// <summary>
        /// Top row of the matrix
        /// </summary>
        public Vector4d Row0;
        /// <summary>
        /// 2nd row of the matrix
        /// </summary>
        public Vector4d Row1;
        /// <summary>
        /// 3rd row of the matrix
        /// </summary>
        public Vector4d Row2;
        /// <summary>
        /// Bottom row of the matrix
        /// </summary>
        public Vector4d Row3;

        /// <summary>
        /// The identity matrix
        /// </summary>
        public static Matrix4d Identity = new Matrix4d(Vector4d.UnitX, Vector4d.UnitY, Vector4d.UnitZ, Vector4d.UnitW);

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="row0">Top row of the matrix</param>
        /// <param name="row1">Second row of the matrix</param>
        /// <param name="row2">Third row of the matrix</param>
        /// <param name="row3">Bottom row of the matrix</param>
        public Matrix4d(Vector4d row0, Vector4d row1, Vector4d row2, Vector4d row3)
        {
            Row0 = row0;
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }
    }
}
