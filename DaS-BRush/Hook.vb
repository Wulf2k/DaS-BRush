Imports System.Threading

Public Class Hook

    Public Shared funcPtr As Integer
    Public Shared nodeDumpPtr As Integer = 0
    Public Shared charptr1 As Integer
    Public Shared charmapdataptr As Integer
    Public Shared charposdataptr As Integer
    Public Shared charptr2 As Integer
    Public Shared charptr3 As Integer
    Public Shared enemyptr As Integer
    Public Shared enemyptr2 As Integer
    Public Shared enemyptr3 As Integer
    Public Shared enemyptr4 As Integer
    Public Shared tendptr As Integer
    Public Shared gamestatsptr As Integer
    Public Shared bonfireptr As Integer
    Public Shared dropPtr As Integer
    Public Shared playerstableposptr As Integer
    Public Shared menuptr As Integer
    Public Shared lineptr As Integer
    Public Shared keyptr As Integer

    Public Shared rushMode As Boolean = False
    Public Shared rushName As String = ""

    Public Shared GenDiagResponse As Integer
    Public Shared GenDiagVal As Integer

    Public Shared rushTimer As Thread

    Private Shared _isHooked As Boolean = False
    Public Shared Property IsHooked As Boolean
        Get
            Return _isHooked
        End Get
        Private Set(value As Boolean)
            _isHooked = value
        End Set
    End Property

    Private Shared _detectedDarkSoulsVersion As String = "Nothing"
    Public Shared Property DetectedDarkSoulsVersion As String
        Get
            Return _detectedDarkSoulsVersion
        End Get
        Private Set(value As String)
            _detectedDarkSoulsVersion = value
        End Set
    End Property

    Private Shared Function AllocNewPtr() As Integer
        Dim TargetBufferSize = 1024
        Return VirtualAllocEx(_targetProcessHandle, 0, TargetBufferSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    End Function

    Private Shared Sub AllocPointers()
        dropPtr = AllocNewPtr()
        funcPtr = AllocNewPtr()
    End Sub

    Public Shared Function CheckHook() As Boolean
        If (RUInt32(&H400080) = &HFC293654&) Then
            DetectedDarkSoulsVersion = "Dark Souls (Latest Release Ver.)"

            Dim tmpProtect As Integer
            VirtualProtectEx(_targetProcessHandle, &H10CC000, &H1DE000, 4, tmpProtect)

            WBytes(&HBE73FE, {&H20})
            WBytes(&HBE719F, {&H20})
            WBytes(&HBE722B, {&H20})

            Funcs.setsaveenable(0) 'TODO: is this right lol

            Return True
        Else
            If (RUInt32(&H400080) = &HE91B11E2&) Then
                DetectedDarkSoulsVersion = "Dark Souls (Invalid Beta)"
            Else
                DetectedDarkSoulsVersion = "None"
            End If

            Return False
        End If
    End Function

    Public Shared Function InitHook() As Boolean
        If ScanForProcess("DARK SOULS", True) Then
            'Check if this process is even Dark Souls
            IsHooked = CheckHook()

            If IsHooked Then
                AllocPointers()
                Return True
            End If
        End If

        Return False
    End Function

    Public Shared Sub UpdateHook()
        If Not Hook.CheckHook() Then
            Return
        End If

        bonfireptr = RUInt32(&H13784A0)
        charptr1 = RInt32(&H137DC70)
        charptr1 = RInt32(charptr1 + &H4)
        charptr1 = RInt32(charptr1)
        gamestatsptr = RUInt32(&H1378700)
        charptr2 = RUInt32(gamestatsptr + &H8)
        charmapdataptr = RInt32(charptr1 + &H28)
        charposdataptr = RInt32(charmapdataptr + &H1C)
        playerstableposptr = RInt32(&H13784A0)
        menuptr = RInt32(&H13786D0)
        lineptr = RInt32(&H1378388)
        keyptr = RInt32(&H1378640)

    End Sub

    Public Class Player

        Public Shared HP As New LivePtrVarInt(Function() charptr1 + &H2D4)
        Public Shared MaxHP As New LivePtrVarInt(Function() charptr1 + &H2D8)
        Public Shared Stamina As New LivePtrVarInt(Function() charptr1 + &H2E4)
        Public Shared MaxStamina As New LivePtrVarInt(Function() charptr1 + &H2E8)

        Public Shared Heading As New LivePtrVarFloat(Function() charposdataptr + &H4)
        Public Shared PosX As New LivePtrVarFloat(Function() charposdataptr + &H10)
        Public Shared PosY As New LivePtrVarFloat(Function() charposdataptr + &H14)
        Public Shared PosZ As New LivePtrVarFloat(Function() charposdataptr + &H18)
        Public Shared StablePosX As New LivePtrVarFloat(Function() playerstableposptr + &HB70)
        Public Shared StablePosY As New LivePtrVarFloat(Function() playerstableposptr + &HB74)
        Public Shared StablePosZ As New LivePtrVarFloat(Function() playerstableposptr + &HB78)

        Public Shared BonfireID As New LivePtrVarInt(Function() bonfireptr + &HB04)

    End Class

    Public Class Stats

        Public Shared MaxHP As New LivePtrVarInt(Function() charptr2 + &H14)
        Public Shared MaxStamina As New LivePtrVarInt(Function() charptr2 + &H30)
        Public Shared VIT As New LivePtrVarInt(Function() charptr2 + &H38)
        Public Shared ATN As New LivePtrVarInt(Function() charptr2 + &H40)
        Public Shared ENDurance As New LivePtrVarInt(Function() charptr2 + &H48) ' fuck vb
        Public Shared STR As New LivePtrVarInt(Function() charptr2 + &H50)
        Public Shared DEX As New LivePtrVarInt(Function() charptr2 + &H58)
        Public Shared INT As New LivePtrVarInt(Function() charptr2 + &H60)
        Public Shared FTH As New LivePtrVarInt(Function() charptr2 + &H68)
        Public Shared RES As New LivePtrVarInt(Function() charptr2 + &H80)
        Public Shared Humanity As New LivePtrVarInt(Function() charptr2 + &H7C)
        Public Shared ExternalGenitals As New LivePtrVarInt(Function() charptr2 + &HC2)

    End Class

    Public Class GameStats

        Public Shared ClearCount As New LivePtrVarInt(Function() gamestatsptr + &H3C)
        Public Shared TrueDeathCount As New LivePtrVarInt(Function() gamestatsptr + &H58)
        'TODO: rename Hook.GameStats.TimerThing to the actual name...?
        Public Shared TotalPlayTime As New LivePtrVarInt(Function() gamestatsptr + &H68)

    End Class

End Class
