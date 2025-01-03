// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace osu.Framework.Utils
{
    public static class Validation
    {
        /// <summary>
        /// Returns whether the two coordinates of a <see cref="Vector2"/> are not infinite or NaN.
        /// <para>For further information, see <seealso cref="float.IsFinite(float)"/>.</para>
        /// </summary>
        /// <param name="toCheck">The <see cref="Vector2"/> to check.</param>
        /// <returns>False if X or Y are Infinity or NaN, true otherwise. </returns>
        public static bool IsFinite(Vector2 toCheck) => float.IsFinite(toCheck.X) && float.IsFinite(toCheck.Y);


        /// <summary>
        /// Whether the specified type <typeparamref name="T"/> is a number type supported by <see cref="BindableNumber{T}"/>.
        /// </summary>
        /// <remarks>
        /// Directly comparing typeof(T) to type literal is recognized pattern of JIT and very fast.
        /// Just a pointer comparison for reference types, or constant for value types.
        /// The check will become NOP in usages after optimization.
        /// </remarks>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <returns><see langword="true"/> if the type is supported; <see langword="false"/> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSupportedBindableNumberType<T>() =>
            typeof(T) == typeof(sbyte)
            || typeof(T) == typeof(byte)
            || typeof(T) == typeof(short)
            || typeof(T) == typeof(ushort)
            || typeof(T) == typeof(int)
            || typeof(T) == typeof(uint)
            || typeof(T) == typeof(long)
            || typeof(T) == typeof(ulong)
            || typeof(T) == typeof(float)
            || typeof(T) == typeof(double);
    }
}
