Imports System.Collections.ObjectModel
Imports System.Runtime.ConstrainedExecution
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.Injection.Structures
Imports Microsoft.Win32.SafeHandles

Namespace Injection.Structures

    Public Class SafeDarkSoulsHandle
        Inherits SafeHandleZeroOrMinusOneIsInvalid

        Friend Event OnDetach()
        Friend Event OnAttach()

        Public ReadOnly Property ModuleOffsets As ReadOnlyDictionary(Of String, List(Of UInteger))
        Public ReadOnly SafeBaseMemoryOffset As Integer = &H400000

        Public Shared ReadOnly CompatibleVersions As String() = New String() {
            DarkSoulsVersion.LatestRelease
        }

        Public ReadOnly Property Attached As Boolean
            Get
                Return (Not IsClosed) AndAlso (Not IsInvalid)
            End Get
        End Property

        Public ReadOnly Property Version As DarkSoulsVersion = DarkSoulsVersion.None
        Public ReadOnly Property VersionDisplayName As String
            Get
                Select Case Version
                    Case DarkSoulsVersion.None : Return "None"
                    Case DarkSoulsVersion.LatestRelease : Return "Latest Steam Release"
                    Case DarkSoulsVersion.SteamWorksBeta : Return "Steamworks December 2014 Beta (Incompatible)"
                    Case DarkSoulsVersion.AncientGFWL : Return "Games for Windows Live Release (Incompatible)"
                End Select
                Return Version.ToString() 'Shut up compiler
            End Get
        End Property

        Public Sub New()
            MyBase.New(True)
        End Sub

        Public Function GetHandle() As IntPtr
            Return handle
        End Function

        Private Function GetIngameDllAddress(moduleName As String) As UInteger

            Dim modules(255 - 1) As UInteger
            Dim cbNeeded As Integer = 0
            PSAPI.EnumProcessModules(DARKSOULS.GetHandle(), modules, 4 * 1024, cbNeeded)

            Dim numModules = cbNeeded / IntPtr.Size

            For i = 0 To numModules - 1

                Dim disModule = New IntPtr(modules(i))
                Dim name As New Text.StringBuilder()
                PSAPI.GetModuleBaseName(DARKSOULS.GetHandle(), disModule, name, 255)

                If (name.ToString().ToUpper().Equals(moduleName.ToUpper())) Then
                    Return modules(i)
                End If

            Next

            Return 0
        End Function

        Public Function TryAttachToDarkSouls(Optional suppressMessageBox As Boolean = False) As Boolean
            Dim selectedProcess As Process = Nothing
            Dim _allProcesses() As Process = Process.GetProcesses
            For Each proc As Process In _allProcesses
                If selectedProcess Is Nothing AndAlso proc.MainWindowTitle.ToUpper().Equals("DARK SOULS") Then
                    selectedProcess = proc
                Else
                    proc.Dispose()
                End If
            Next

            If selectedProcess IsNot Nothing Then
                SetHandle(Kernel.OpenProcess(Kernel.PROCESS_ALL_ACCESS, False, selectedProcess.Id))
                CheckHook()
                Dim modulesInputDict As New Dictionary(Of String, List(Of UInteger))()

                If Attached Then
                    For Each dll As ProcessModule In selectedProcess.Modules
                        Dim indexName = dll.ModuleName.ToUpper()
                        If modulesInputDict.ContainsKey(indexName) Then
                            modulesInputDict(indexName).Add(dll.BaseAddress)
                        Else
                            modulesInputDict.Add(indexName, New UInteger() {dll.BaseAddress}.ToList())
                        End If
                    Next
                End If

                _ModuleOffsets = New ReadOnlyDictionary(Of String, List(Of UInteger))(modulesInputDict)
                selectedProcess.Dispose()
            End If

            Lua.LuaInterface.ReInitSharedInstance()

            If Not Attached Then
                If Not suppressMessageBox Then 'Showing 2 message boxes as soon as you start the program is too annoying.
                    System.Windows.Forms.MessageBox.
                        Show("Found Dark Souls process but failed to attach to it." & vbCrLf &
                             "Try explicitly running this program as an administrator.", "Failed to Attach",
                             System.Windows.Forms.MessageBoxButtons.OK,
                             System.Windows.Forms.MessageBoxIcon.Stop)
                End If

                Return False
            Else
                RaiseEvent OnAttach()
                Return True
            End If
        End Function

        Private Sub CheckHook()
            If (RUInt32(&H400080) = &HFC293654&) Then
                _Version = DarkSoulsVersion.LatestRelease

                Dim tmpProtect As Integer
                If Not Kernel.VirtualProtectEx(handle, &H10CC000, &H1DE000, Kernel.PAGE_READWRITE, tmpProtect) Then
                    Throw New Exception("VirtualProtectEx Returned False")
                End If

                If Not Kernel.FlushInstructionCache(handle, &H10CC000, &H1DE000) Then
                    Throw New Exception("FlushInstructionCache Returned False")
                End If

                Kernel.WriteProcessMemory_SAFE(handle, &HBE73FE, {&H20}, 1, Nothing)
                Kernel.WriteProcessMemory_SAFE(handle, &HBE719F, {&H20}, 1, Nothing)
                Kernel.WriteProcessMemory_SAFE(handle, &HBE722B, {&H20}, 1, Nothing)

                Lua.Funcs.SetSaveEnable(False)
            Else
                Dim buffer(3) As Byte
                Kernel.ReadProcessMemory_SAFE(handle, &H400080, buffer, 4, Nothing)
                Dim dwBetaChk As Integer = BitConverter.ToUInt32(buffer, 0)
                If (dwBetaChk = &HE91B11E2&) Then
                    _Version = DarkSoulsVersion.SteamWorksBeta
                Else
                    _Version = DarkSoulsVersion.None
                End If
            End If

            If Not CompatibleVersions.Contains(Version) Then
                Close()
            End If
        End Sub

        <ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>
        Protected Overrides Function ReleaseHandle() As Boolean
            RaiseEvent OnDetach()

            Return Kernel.CloseHandle(handle)
        End Function

    End Class

End Namespace
