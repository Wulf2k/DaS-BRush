using DaS.ScriptLib.Injection.Structures;
using DaS.ScriptLib.LuaScripting;
using DaS.ScriptLib.LuaScripting.Structures;
using Managed.X86;
using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Addr = Managed.X86.X86Address;
using Reg32 = Managed.X86.X86Register32;

namespace DaS.ScriptLib.Injection
{
    public class DSAsmCaller : IDisposable
    {
        public const int FUNCTION_CALL_ASM_BUFFER_SIZE = 1024;
        public const int ReturnValueCheckInterval = 5;
        public const int FUNC_RETURN_ADDR_OFFSET = 0x200;
        public const int MAX_WAIT = 1000;
        public const int BUFFER_STACK_SIZE = 5;
        public const uint PLACEHOLDER_INT32 = 0xE110D00D;

        public const int INT32_SIZE = 4;

        public const uint FUNCCALL_ERR = 0xFFFFFFFF;

        private static readonly Type[] SquashIntoDword_NumericTypes = 
            { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(bool), typeof(float) };

        private MemoryStream AsmBuffer = new MemoryStream(1024);
        private object Buffer_Result;
        private List<SafeRemoteHandle> Buffer_ParamPointerList = new List<SafeRemoteHandle>();
        private List<Int32> Buffer_DefaultEmptyStack = new List<Int32>();
        private Int32[] Buffer_Stack = new Int32[BUFFER_STACK_SIZE];
        private byte[] Buffer_ResultBytes = new byte[INT32_SIZE];
        private MutatableDword Buffer_GetFunctionCallResult = 0;

        private int Buffer_StackCounter = 0;

        private MutatableDword Buffer_SquashIntoDwordResult = 0;
        public SafeRemoteHandle CodeHandle { get; private set; }

        private MoveableAddressOffset AsmLocBegin;
        private MoveableAddressOffset AsmLocAtECX;
        private MoveableAddressOffset[] AsmLocAfterEachStackMov = new MoveableAddressOffset[BUFFER_STACK_SIZE];
        private MoveableAddressOffset AsmLocAfterLuaFunctionCall;
        private MoveableAddressOffset AsmLocAfterSetReturnLocation;

        private MoveableAddressOffset AsmLocAfterReturn;
        private byte[] GetNewCopyOfAsmBuffer()
        {
            return AsmBuffer.ToArray();
        }

        private bool WriteAsm(uint address, byte[] bytes, int count)
        {
            if (address < Hook.DARKSOULS.SafeBaseMemoryOffset)
            {
                return false;
            }
            return Kernel.WriteProcessMemory_SAFE(Hook.DARKSOULS.GetHandle(), address, bytes, count, 0) && Kernel.FlushInstructionCache(Hook.DARKSOULS.GetHandle(), address, count);
        }

        private bool InjectEntireCodeBuffer()
        {
            return WriteAsm((uint)CodeHandle.GetHandle(), AsmBuffer.ToArray(), (int)AsmBuffer.Position);
        }

        private void CompletelyReInitializeAndInjectCodeInNewLocation()
        {
            UndoCodeInjection();
            CodeHandle?.Dispose();
            CodeHandle = new SafeRemoteHandle(FUNCTION_CALL_ASM_BUFFER_SIZE);
        }

        private void UndoCodeInjection()
        {
            if (CodeHandle != null && !CodeHandle.IsClosed)
            {
                CodeHandle.Close();
            }
        }

        public bool IsCodeInjected
        {
            get { return (!CodeHandle.IsClosed) && (!CodeHandle.IsInvalid); }
        }

        public DSAsmCaller()
        {
            HookEvents();
            CompletelyReInitializeAndInjectCodeInNewLocation();
        }

        private void HookEvents()
        {
            Hook.DARKSOULS.OnAttach += Proc_OnAttachToCurrentProcess;
            Hook.DARKSOULS.OnDetach += Proc_OnDetachFromCurrentProcess;
        }

        private void UnhookEvents()
        {
            Hook.DARKSOULS.OnAttach -= Proc_OnAttachToCurrentProcess;
            Hook.DARKSOULS.OnDetach -= Proc_OnDetachFromCurrentProcess;
        }

        private void Proc_OnDetachFromCurrentProcess()
        {
            UndoCodeInjection();
        }

        private void Proc_OnAttachToCurrentProcess()
        {
            CompletelyReInitializeAndInjectCodeInNewLocation();
        }

        private void InitAsmBuffer(int funcAddr, IEnumerable<object> parameters, object specialRegisters, List<SafeRemoteHandle> allocPtrList)
        {
            var args = parameters.ToArray();

            AsmBuffer.Position = 0;
            X86Writer asm = new X86Writer(AsmBuffer, CodeHandle.GetHandle());
            //ASM START:
            asm.Push32(Reg32.EBP);
            asm.Mov32(Reg32.EBP, Reg32.ESP);
            asm.Push32(Reg32.EAX);

            for (int i = args.Length - 1; i >= 0; i += -1)
            {
                asm.Mov32(Reg32.EAX, SquashIntoDword(ref allocPtrList, args[i]));
                asm.Push32(Reg32.EAX);
            }

            if (specialRegisters != null)
            {
                if (specialRegisters is LuaTable)
                {
                    foreach (var kvp in (LuaTable)specialRegisters)
                    {
                        asm.Mov32((Reg32)Enum.Parse(typeof(Reg32), (string)kvp.Key), SquashIntoDword(ref allocPtrList, kvp.Value));
                    }
                }
                else if (specialRegisters is Dictionary<string, object>)
                {
                    foreach (string r in ((Dictionary <string, object>)specialRegisters).Keys)
                    {
                        asm.Mov32((Reg32)Enum.Parse(typeof(Reg32), r), SquashIntoDword(ref allocPtrList, ((Dictionary<string, object>)specialRegisters)[r]));
                    }
                }
            }

            //CALL LUA FUNCTION:
            asm.Call(new IntPtr(funcAddr));
            AsmLocAfterLuaFunctionCall = new MoveableAddressOffset(this, asm.Position);
            //SET RETURN POS:
            asm.Mov32(Reg32.EBX, CodeHandle.GetHandle().ToInt32() + FUNC_RETURN_ADDR_OFFSET);
            asm.Mov32(new Addr(Reg32.EBX, 0), Reg32.EAX);
            //mov [ebx], eax
            asm.Pop32(Reg32.EAX);

            for (int i = args.Length - 1; i >= 0; i += -1)
            {
                asm.Pop32(Reg32.EAX);
            }

            asm.Mov32(Reg32.ESP, Reg32.EBP);
            asm.Pop32(Reg32.EBP);
            asm.Retn();
        }

        private void ____freeClrManagedResources()
        {
            UnhookEvents();

            AsmBuffer.Dispose();
            AsmBuffer = null;

            Buffer_Result = null;
            Buffer_ParamPointerList.Clear();
            Buffer_ParamPointerList = null;

            Buffer_DefaultEmptyStack.Clear();
            Buffer_DefaultEmptyStack = null;
            Buffer_Stack = null;
            Buffer_ResultBytes = null;
            //Buffer_StackCounter = null;

            AsmLocBegin = null;
            AsmLocAfterEachStackMov = null;
            AsmLocAfterLuaFunctionCall = null;
            AsmLocAfterSetReturnLocation = null;
            AsmLocAfterReturn = null;
        }

        private void ____freeNativeUnmanagedResources()
        {
            UndoCodeInjection();
            CodeHandle.Dispose();
            CodeHandle = null;
        }

        private byte[] ExecuteAsm()
        {
            var threadHandle = new SafeRemoteThreadHandle(CodeHandle);
            if (!threadHandle.IsClosed & !threadHandle.IsInvalid)
            {
                Kernel.WaitForSingleObject(threadHandle.GetHandle(), MAX_WAIT);
            }
            threadHandle.Close();
            threadHandle.Dispose();
            threadHandle = null;

            return CodeHandle.GetFuncReturnValue();
        }

        private int SquashIntoDword(ref List<SafeRemoteHandle> allocPtrList, object arg)
        {
            Type typ = arg.GetType();

            Buffer_SquashIntoDwordResult.Int1 = 0;

            if (arg is int)
                Buffer_SquashIntoDwordResult = (int)arg;
            else if (arg is bool)
                Buffer_SquashIntoDwordResult = (bool)arg;
            else if (arg is float)
                Buffer_SquashIntoDwordResult = (float)arg;
            else if (arg is uint)
                Buffer_SquashIntoDwordResult = (uint)arg;
            else if (arg is short)
                Buffer_SquashIntoDwordResult = (short)arg;
            else if (arg is ushort)
                Buffer_SquashIntoDwordResult = (ushort)arg;
            else if (arg is byte)
                Buffer_SquashIntoDwordResult = (byte)arg;
            else if (arg is string argStr)
            {
                var hand = new SafeRemoteHandle((argStr.Length + 1) * 2);
                var handVal = hand.GetHandle();

                Hook.WUnicodeStr((uint)handVal, argStr);

                allocPtrList.Add(hand);
                Buffer_SquashIntoDwordResult = (uint)handVal;
            }
            else if (arg is IntPtr)
            {
                Buffer_SquashIntoDwordResult.Int1 = ((IntPtr)arg).ToInt32();
            }
            else
            {
                var size = Marshal.SizeOf(arg);

                if (size <= INT32_SIZE)
                {
                    IntPtr ptrToArg = Marshal.AllocHGlobal(size);
                    //Allocate a place for our arg
                    try
                    {
                        Marshal.StructureToPtr(arg, ptrToArg, true); //Move arg to where that pointer points
                        byte[] argByt = new byte[size]; //Make a new byte array the size of the arg
                        Marshal.Copy(ptrToArg, argByt, 0, size); //Copy bytes from [ptrToArg] to argByt
                        Buffer_SquashIntoDwordResult.SetBytes(argByt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(ptrToArg);
                    }
                }
                else
                {
                    //Allocate a place for our arg
                    IntPtr ptrToArg = Marshal.AllocHGlobal(size);

                    try
                    {
                        var hand = new SafeRemoteHandle(size);
                        var unmanagedArg = new SafeMarshalledHandle(arg);
                        hand.MemPatch(unmanagedArg);
                        allocPtrList.Add(hand);

                        Buffer_SquashIntoDwordResult = unmanagedArg.GetHandle().ToInt32();

                        if (unmanagedArg != null)
                        {
                            unmanagedArg.Close();
                            unmanagedArg.Dispose();
                            unmanagedArg = null;
                        }


                        //##### OLD METHOD: #####
                        //Move arg to where that pointer points
                        //Marshal.StructureToPtr(arg, ptrToArg, True)
                        //'Make a new byte array the size of the arg
                        //Dim argByt(size - 1) As Byte
                        //'Copy bytes from where we just moved that object to, over to our byte array
                        //Marshal.Copy(ptrToArg, argByt, 0, size)
                        //' > argByt NOW CONTAINS ARG AS BYTES <
                        //Dim ingamePtrToArg As New IngameAllocatedPtr(size)
                        //WriteProcessMemory(CurrentProcessHandle, ingamePtrToArg.Address, argByt, size, New Integer())
                        //allocPtrList.Add(ingamePtrToArg)
                        //Return ingamePtrToArg.Address
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //We mainly here for dat Finally my boi
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(ptrToArg);
                    }
                }
            }

            return Buffer_SquashIntoDwordResult.Int1;
        }

        private object GetFunctionCallResult(FuncReturnType returnType, byte[] result)
        {
            switch (returnType)
            {
                case FuncReturnType.VOID:
                    return 0;
                case FuncReturnType.INT:
                    Buffer_GetFunctionCallResult.Int1 = BitConverter.ToInt32(result, 0);
                    //Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.Int1;
                case FuncReturnType.UINT:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.UInt1;
                case FuncReturnType.SHORT:
                    return Buffer_GetFunctionCallResult.Short1;
                case FuncReturnType.USHORT:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.UShort1;
                case FuncReturnType.BOOL:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.Bool1;
                case FuncReturnType.BYTE:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.Byte1;
                case FuncReturnType.SBYTE:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.SByte1;
                case FuncReturnType.FLOAT:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Buffer_GetFunctionCallResult.Float1;
                case FuncReturnType.STR_ANSI:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Hook.RAsciiStr(Buffer_GetFunctionCallResult.Int1, -1);
                case FuncReturnType.STR_UNI:
                    Buffer_GetFunctionCallResult.SetBytes(result);
                    return Hook.RUnicodeStr(Buffer_GetFunctionCallResult.Int1, -1);
                default:
                    return BitConverter.ToInt32(result, 0);
            }
        }

        public object CallIngameFunc(FuncReturnType returnType, int functionAddress, IEnumerable<object> args, object specialRegisters)
        {

            if (CodeHandle.IsClosed || CodeHandle.IsInvalid)
            {
                CompletelyReInitializeAndInjectCodeInNewLocation();
            }

            Buffer_ParamPointerList.Clear();

            InitAsmBuffer(functionAddress, args, specialRegisters, Buffer_ParamPointerList);

            if (!InjectEntireCodeBuffer())
            {
                Dbg.PrintErr("WARNING: CODE INJECT FAILURE");
            }

            //luai.DebugUpdate()

            foreach (SafeRemoteHandle ptr in Buffer_ParamPointerList)
            {
                ptr.Dispose();
            }


            Buffer_ResultBytes = ExecuteAsm();

            Buffer_Result = GetFunctionCallResult(returnType, Buffer_ResultBytes);

            foreach (SafeRemoteHandle ptr in Buffer_ParamPointerList)
            {
                ptr.Close();
                ptr.Dispose();
            }

            Buffer_ParamPointerList.Clear();

            return Buffer_Result;
        }

        public object CallIngameFunc_FromLua(FuncReturnType returnType, int functionAddress, LuaTable args, object specialRegisters)
        {
            return CallIngameFunc(returnType, functionAddress, args.ArrayList, specialRegisters);
        }

        #region "IDisposable Support"
        // To detect redundant calls
        private bool ____disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!____disposedValue)
            {
                if (disposing)
                {
                    ____freeClrManagedResources();
                }

                ____freeNativeUnmanagedResources();
            }
            ____disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        ~DSAsmCaller()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(false);
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(true);
            // TODO: uncomment the following line if Finalize() is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
