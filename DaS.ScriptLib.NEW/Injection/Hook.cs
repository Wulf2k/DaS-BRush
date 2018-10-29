﻿using System;
using System.Linq;
using System.Text;
using DaS.ScriptLib.Injection.Structures;
using System.Runtime.InteropServices;

namespace DaS.ScriptLib.Injection
{
    public static class Hook
    {

        public static SafeDarkSoulsHandle DARKSOULS { get; private set; } = new SafeDarkSoulsHandle();

        private static byte[] ByteBuffer = new byte[8];

        private static object LOCK_OBJ = new object();
        public static void Init()
        {
            DARKSOULS.TryAttachToDarkSouls(true);

            Array.Clear(ByteBuffer, 0, 8);
        }

        public class Injected
        {
            public static SafeRemoteHandle ItemDropPtr = new SafeRemoteHandle(1024);
        }

        private static bool CheckAddress(long addr)
        {
            //var result = false;
            //lock (LOCK_OBJ)
            //{
            //    result = (addr >= DARKSOULS.SafeBaseMemoryOffset);
            //    // AndAlso addr.ToInt32() < &H10000000 'may need adjusting
            //}
            return addr >= DARKSOULS.SafeBaseMemoryOffset;
        }

        public static bool InjectDll(string dllName)
        {
            //TODO:
            //TODO:
            //TODO:
            //SECURE THIS FUCKING THIng BEFORE IT SEES THE LIGHT OF DAY
            //TODO:
            //TODO:
            //TODO:

            int MEM_COMMIT = 0x00001000;
            int MEM_RESERVE = 0x00002000;
            int PAGE_READWRITE = 4;

            uint loadLibraryAddr = (uint)Kernel.GetProcAddress(Kernel.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            uint allocMemAddress = Kernel.VirtualAllocEx(DARKSOULS.GetHandle(), 0, (int)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

            int bytesWritten = 0;

            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), allocMemAddress, Encoding.Default.GetBytes(dllName), (int)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), bytesWritten);


            Kernel.CreateRemoteThread(DARKSOULS.GetHandle(), 0, 0, loadLibraryAddr, allocMemAddress, 0, 0);

            return true;
        }



        public static bool SetDllLinePos(int lineNum, int x, int y)
        {
            //DLLEXPORT void TestFunc(int lineNum, int x, int y)
            //[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            //static internal extern IntPtr GetProcAddress(IntPtr hModule, string procName);
            //linePos(lineNum, x, y);
            


            return true;
        }


        public static sbyte RInt8(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 1, 0);
            return (sbyte)ByteBuffer[0];
        }


        public static short RInt16(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 2, 0);
            return BitConverter.ToInt16(ByteBuffer, 0);
        }


        public static int RInt32(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 4, 0);
            return BitConverter.ToInt32(ByteBuffer, 0);
        }


        public static long RInt64(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 8, 0);
            return BitConverter.ToInt64(ByteBuffer, 0);
        }


        public static ushort RUInt16(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 2, 0);
            return BitConverter.ToUInt16(ByteBuffer, 0);
        }


        public static uint RUInt32(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 4, 0);
            return BitConverter.ToUInt32(ByteBuffer, 0);
        }


        public static ulong RUInt64(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 8, 0);
            return BitConverter.ToUInt64(ByteBuffer, 0);
        }


        public static float RFloat(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 4, 0);
            return BitConverter.ToSingle(ByteBuffer, 0);
        }


        public static double RDouble(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 8, 0);
            return BitConverter.ToDouble(ByteBuffer, 0);
        }


        public static IntPtr RIntPtr(long addr)
        {
            if (!CheckAddress(addr))
                return new IntPtr(0);
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, IntPtr.Size, 0);
            if (IntPtr.Size == 4)
            {
                return new IntPtr(BitConverter.ToUInt32(ByteBuffer, 0));
            }
            else
            {
                return new IntPtr(BitConverter.ToInt64(ByteBuffer, 0));
            }
        }


        public static byte[] RBytes(long addr, int size)
        {
            if (!CheckAddress(addr))
            {
                byte[] dummyArr = new byte[size];
                return dummyArr;
            }
            byte[] _rtnBytes = new byte[size];
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, _rtnBytes, size, 0);
            return _rtnBytes;
        }


        public static byte RByte(long addr)
        {
            if (!CheckAddress(addr))
                return 0;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 1, 0);
            return ByteBuffer[0];
        }


        public static string RAsciiStr(long addr, int maxLength)
        {
            if (!CheckAddress(addr))
                return null;

            System.Text.StringBuilder Str = new System.Text.StringBuilder(maxLength);
            int loc = 0;

            var nextChr = '?';


            if (maxLength != 0)
            {
                byte[] bytes = new byte[2];

                while ((maxLength < 0 || loc < maxLength))
                {
                    Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)(addr + loc), bytes, 1, 0);
                    nextChr = System.Text.Encoding.ASCII.GetChars(bytes)[0];

                    if (nextChr == (char)0)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    else
                    {
                        Str.Append(nextChr);
                    }

                    loc += 1;
                }

            }

            return Str.ToString();
        }


        public static string RUnicodeStr(long addr, int maxLength)
        {
            if (!CheckAddress(addr))
                return null;

            System.Text.StringBuilder Str = new System.Text.StringBuilder(maxLength);
            int loc = 0;

            var nextChr = '?';


            if (maxLength != 0)
            {
                byte[] bytes = new byte[3];

                while ((maxLength < 0 || loc < maxLength))
                {
                    Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), ((uint)addr) + (uint)loc * 2, bytes, 2, 0);
                    nextChr = System.Text.Encoding.Unicode.GetChars(bytes)[0];

                    if (nextChr == (char)0)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                    else
                    {
                        Str.Append(nextChr);
                    }

                    loc += 1;
                }

            }

            return Str.ToString();
        }


        public static bool RBool(long addr)
        {
            if (!CheckAddress(addr))
                return false;
            Kernel.ReadProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, ByteBuffer, 1, 0);
            return (ByteBuffer[0] != 0);
        }


        public static void WBool(long addr, bool val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 1, 0);
        }


        public static void WInt16(long addr, Int16 val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 2, 0);
        }


        public static void WUInt16(long addr, UInt16 val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 2, 0);
        }


        public static void WInt32(long addr, int val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 4, 0);
        }


        public static void WUInt32(long addr, uint val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 4, 0);
        }


        public static void WInt64(long addr, Int64 val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 8, 0);
        }


        public static void WUInt64(long addr, UInt64 val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 8, 0);
        }


        public static void WFloat(long addr, float val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, BitConverter.GetBytes(val), 4, 0);
        }


        public static void WBytes(long addr, byte[] val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, val, val.Length, 0);
        }


        public static void WByte(long addr, byte val)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, new byte[] { val }, 1, 0);
        }


        public static void WAsciiStr(long addr, string str)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, System.Text.Encoding.ASCII.GetBytes(str).Concat(new byte[] { 0 }).ToArray(), str.Length + 1, 0);
        }


        public static void WUnicodeStr(long addr, string str)
        {
            if (!CheckAddress(addr))
                return;
            Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), (uint)addr, System.Text.Encoding.Unicode.GetBytes(str).Concat(new byte[] {
                0,
                0
            }).ToArray(), str.Length * 2 + 2, 0);
        }
    }
}