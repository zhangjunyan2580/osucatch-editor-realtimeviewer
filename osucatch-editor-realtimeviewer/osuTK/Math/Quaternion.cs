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
    /// Represents a Quaternion.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        /// <summary>
        /// The X, Y and Z components of this instance.
        /// </summary>
        public Vector3 Xyz;

        /// <summary>
        /// The W component of this instance.
        /// </summary>
        public float W;

        /// <summary>
        /// Construct a new Quaternion from vector and w components
        /// </summary>
        /// <param name="v">The vector part</param>
        /// <param name="w">The w part</param>
        public Quaternion(Vector3 v, float w)
        {
            Xyz = v;
            W = w;
        }

        /// <summary>
        /// Construct a new Quaternion
        /// </summary>
        /// <param name="x">The x component</param>
        /// <param name="y">The y component</param>
        /// <param name="z">The z component</param>
        /// <param name="w">The w component</param>
        public Quaternion(float x, float y, float z, float w)
            : this(new Vector3(x, y, z), w)
        { }

        /// <summary>
        /// Defines the identity quaternion.
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);
    }
}
