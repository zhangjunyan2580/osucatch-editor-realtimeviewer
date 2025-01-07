// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

namespace osu.Framework.Extensions.IEnumerableExtensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Wraps this object instance into an <see cref="IEnumerable{T}"/>
        /// consisting of a single item.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="item">The instance that will be wrapped.</param>
        /// <returns> An <see cref="IEnumerable{T}"/> consisting of a single item.</returns>
        public static IEnumerable<T> Yield<T>(this T item) => new[] { item };
    }
}
