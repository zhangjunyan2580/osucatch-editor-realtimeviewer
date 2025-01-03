// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using osu.Framework.Extensions.ObjectExtensions;
using osu.Framework.Utils;

namespace osu.Framework.Extensions.EnumExtensions
{
    public static class EnumExtensions
    {

#pragma warning disable RS0030 // (banned API)
        /// <summary>
        /// A fast alternative functionally equivalent to <see cref="Enum.HasFlag"/>, eliminating boxing in all scenarios.
        /// </summary>
        /// <param name="enumValue">The enum to check.</param>
        /// <param name="flag">The flag to check for.</param>
#pragma warning restore RS0030
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool HasFlagFast<T>(this T enumValue, T flag) where T : unmanaged, Enum
        {
            // Note: Using a switch statement would eliminate inlining.

            if (sizeof(T) == 1)
            {
                byte value1 = Unsafe.As<T, byte>(ref enumValue);
                byte value2 = Unsafe.As<T, byte>(ref flag);
                return (value1 & value2) == value2;
            }

            if (sizeof(T) == 2)
            {
                short value1 = Unsafe.As<T, short>(ref enumValue);
                short value2 = Unsafe.As<T, short>(ref flag);
                return (value1 & value2) == value2;
            }

            if (sizeof(T) == 4)
            {
                int value1 = Unsafe.As<T, int>(ref enumValue);
                int value2 = Unsafe.As<T, int>(ref flag);
                return (value1 & value2) == value2;
            }

            if (sizeof(T) == 8)
            {
                long value1 = Unsafe.As<T, long>(ref enumValue);
                long value2 = Unsafe.As<T, long>(ref flag);
                return (value1 & value2) == value2;
            }

            throw new ArgumentException($"Invalid enum type provided: {typeof(T)}.");
        }
    }
}
