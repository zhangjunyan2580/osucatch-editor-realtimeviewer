// Copyright (c) 2019 Karoo13. Licensed under https://github.com/Karoo13/EditorReader/blob/master/LICENSE
// See the LICENCE file in the EditorReader folder for full licence text.
// https://github.com/Karoo13/EditorReader
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Editor_Reader;

internal class Internals
{
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;

        public IntPtr AllocationBase;

        public uint AllocationProtect;

        public IntPtr RegionSize;

        public uint State;

        public uint Protect;

        public uint Type;
    }

    public List<MEMORY_BASIC_INFORMATION> MemReg { get; set; } = new List<MEMORY_BASIC_INFORMATION>();


    [DllImport("kernel32.dll", SetLastError = true)]
    protected static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

    public void MemInfo(IntPtr pHandle)
    {
        IntPtr lpAddress = IntPtr.Zero;
        while (true)
        {
            MEMORY_BASIC_INFORMATION lpBuffer = default(MEMORY_BASIC_INFORMATION);
            if (VirtualQueryEx(pHandle, lpAddress, out lpBuffer, Marshal.SizeOf(lpBuffer)) == 0 || lpBuffer.RegionSize.ToInt64() > int.MaxValue)
            {
                break;
            }

            if (lpBuffer.State == 4096 && lpBuffer.Protect == 4 && lpBuffer.Type == 131072)
            {
                MemReg.Add(lpBuffer);
            }

            lpAddress = IntPtr.Add(lpBuffer.BaseAddress, lpBuffer.RegionSize.ToInt32());
        }

        MemReg.Sort((MEMORY_BASIC_INFORMATION a, MEMORY_BASIC_INFORMATION b) => ((int)a.RegionSize).CompareTo((int)b.RegionSize));
    }
}