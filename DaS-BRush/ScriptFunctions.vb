Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class ScriptFunction
    Public Shared Sub warp_coords(ByVal x As Single, y As Single, z As Single, rotx As Integer)
        Dim charptr1 = RInt32(&H137DC70)
        charptr1 = RInt32(charptr1 + &H4)
        charptr1 = RInt32(charptr1)
        Dim charmapdataptr = RInt32(charptr1 + &H28)

        WFloat(charmapdataptr + &HD0, x)
        WFloat(charmapdataptr + &HD4, y)
        WFloat(charmapdataptr + &HD8, z)

        Dim facing As Single
        facing = ((rotx / 360) * 2 * Math.PI) - Math.PI


        WFloat(charmapdataptr + &HE4, facing)
        WBytes(charmapdataptr + &HC8, {1})
    End Sub
    Public Shared Sub warpentity_coords(entityPtr As Integer, x As Single, y As Single, z As Single, rotx As Integer)
        entityPtr = RInt32(entityPtr + &H28)
        WFloat(entityPtr + &HD0, x)
        WFloat(entityPtr + &HD4, y)
        WFloat(entityPtr + &HD8, z)

        Dim facing As Single
        facing = ((rotx / 360) * 2 * Math.PI) - Math.PI


        WFloat(entityPtr + &HE4, facing)
        WBytes(entityPtr + &HC8, {1})
    End Sub


    Public Shared Sub blackscreen()
        Dim tmpptr As Integer
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        WFloat(tmpptr + &H270, 0)
        WFloat(tmpptr + &H274, 0)
        WFloat(tmpptr + &H278, 0)
    End Sub
    Public Shared Sub camfocusentity(entityptr As Integer)
        Dim camPtr As Integer = RInt32(&H137D648) + &HEC

        WInt32(camPtr, entityptr)
    End Sub
    Public Shared Sub clearplaytime()
        Dim tmpPtr As Integer = RIntPtr(&H1378700)
        WInt32(tmpPtr + &H68, 0)
    End Sub
    Public Shared Sub controlentity(entityPtr As Integer, state As Byte)
        entityPtr = RInt32(entityPtr + &H28)

        Dim ctrlptr As Integer = RInt32(&H137DC70)
        ctrlptr = RInt32(ctrlptr + 4)
        ctrlptr = RInt32(ctrlptr)
        ctrlptr = RInt32(ctrlptr + &H28)
        ctrlptr = RInt32(ctrlptr + &H54)

        WInt32(entityPtr + &H244, ctrlptr * (state And 1))

    End Sub
    Public Shared Sub test_disableai(ByVal state As Byte)
        WBytes(&H13784EE, {state})
    End Sub
    Public Shared Sub test_playerexterminate(ByVal state As Byte)
        WBytes(&H13784D3, {state})
    End Sub
    Public Shared Sub fadein()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378520)
        tmpptr = RInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        Dim val As Single = 0.0


        For i = 0 To 33
            val = val + 0.03
            WFloat(tmpptr + &H270, val)
            WFloat(tmpptr + &H274, val)
            WFloat(tmpptr + &H278, val)
            Thread.Sleep(33)
        Next

        WBytes(tmpptr + &H26D, {0})
    End Sub
    Public Shared Sub fadeout()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378520)
        tmpptr = RInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        Dim val As Single = 1.0





        For i = 0 To 33
            val = val - 0.03
            val = val - 0.03
            val = val - 0.03
            WFloat(tmpptr + &H270, val)
            WFloat(tmpptr + &H274, val)
            WFloat(tmpptr + &H278, val)
            Thread.Sleep(33)
        Next
    End Sub
    Public Shared Sub forceentitydrawgroup(entityptr As Integer)
        WInt32(entityptr + &H264, -1)
        WInt32(entityptr + &H268, -1)
        WInt32(entityptr + &H26C, -1)
        WInt32(entityptr + &H270, -1)
    End Sub

    Public Shared Sub setcampos(ByVal xpos As Single, ypos As Single, zpos As Single, xrot As Single, yrot As Single)
        Dim tmpPtr As Integer

        tmpPtr = RInt32(&H1378714)

        WFloat(tmpPtr + &HB0, xpos)
        WFloat(tmpPtr + &HB4, ypos)
        WFloat(tmpPtr + &HB8, zpos)


        tmpPtr = RInt32(&H137D6DC)
        tmpPtr = RInt32(tmpPtr + &H3C)
        tmpPtr = RInt32(tmpPtr + &H60)


        WFloat(tmpPtr + &H144, xrot)
        WFloat(tmpPtr + &H150, yrot)



    End Sub
    Public Shared Sub setfreecam(ByVal state As Byte)
        If state Then
            'WBytes(&HEFDBAF, {&H90, &H90, &H90, &H90, &H90})
            WBytes(&H404E59, {&H90, &H90, &H90, &H90, &H90})
            WBytes(&H404E63, {&H90, &H90, &H90, &H90, &H90})
            WBytes(&HF06C46, {&H90, &H90, &H90, &H90, &H90, &H90, &H90, &H90})




        Else
            'WBytes(&HEFDBAF, {&HE8, &H7c, &H72, &H50, &HFF})
            WBytes(&H404E59, {&H66, &HF, &HD6, &H46, &H20})
            WBytes(&H404E63, {&H66, &HF, &HD6, &H46, &H28})
            WBytes(&HF06C46, {&HF3, &HF, &H11, &H83, &H44, &H1, &H0, &H0})




        End If
    End Sub
    Public Shared Sub setclearcount(ByVal clearCount As Integer)
        Dim tmpPtr As Integer
        tmpPtr = RInt32(&H1378700)

        WInt32(tmpPtr + &H3C, clearCount)

    End Sub
    Private Shared Sub setcaption(ByVal str As String)
        Dim tmpptr As Integer
        Dim alpha As Byte

        Dim state As Boolean
        state = (str.Length > 0)

        If state Then
            alpha = 254
        Else
            alpha = 0
        End If

        tmpptr = RInt32(&H13786D0)

        WInt32(tmpptr + &H40, state And 4)
        WInt32(tmpptr + &HB18, alpha)
        WInt32(tmpptr + &HB14, 100)

        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H10)

        WUniStr(tmpptr + &H12C, str & ChrW(0))

    End Sub
    Public Shared Sub setsaveenable(ByVal state As Byte)
        Dim tmpPtr As Integer
        tmpPtr = RInt32(&H13784A0)

        WBytes(tmpPtr + &HB40, {state})
    End Sub
    Public Shared Sub setsaveslot(ByVal slot As Integer)
        WInt32(RInt32(&H13784A0) + &HA70, slot)
    End Sub
    Public Shared Sub setunknownnpcname(ByVal name As String)
        If name.Length > 21 Then name = Strings.Left(name, 21) 'Prevent runover into code
        WUniStr(&H11A784C, name + ChrW(0))
    End Sub

    Public Shared Sub playerhide(ByVal state As Byte)
        WBytes(&H13784E7, {state})
    End Sub
    Public Shared Sub showhud(ByVal state As Byte)
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378700)
        tmpptr = RUInt32(tmpptr + &H2C)

        WBytes(tmpptr + &HD, {state})
    End Sub
    Public Shared Sub waitforload()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378700)

        Dim msPlayed As Integer
        Dim loading As Boolean = True

        msPlayed = RInt32(tmpptr + &H68)

        Do While loading
            loading = (msPlayed = RInt32(tmpptr + &H68))
            Thread.Sleep(33)
        Loop
    End Sub
    Public Shared Sub waittillload()
        Dim tmpptr As Integer
        tmpptr = RInt32(&H1378700)

        Dim msPlayed As Integer
        Dim loading As Boolean = False

        msPlayed = RInt32(tmpptr + &H68)

        Do While Not loading
            loading = (msPlayed = RInt32(tmpptr + &H68))
            Thread.Sleep(33)
        Loop
    End Sub
    Public Shared Sub warpentity_player(entityptr As Integer)
        Dim playerptr As Integer = ScriptEnvironment.Run("GetEntityPtr 10000")
        ScriptEnvironment.Run("warpentity_entity " & entityptr & ", " & playerptr)
    End Sub
    Public Shared Sub warpplayer_entity(entityptr As Integer)
        Dim playerptr As Integer = ScriptEnvironment.Run("GetEntityPtr 10000")
        ScriptEnvironment.Run("warpentity_entity " & playerptr & ", " & entityptr)
    End Sub
    Public Shared Sub warpentity_entity(entityptrSrc As Integer, entityptrDest As Integer)
        'TODO: Check validity of entity pointers
        Dim destEntityPosPtr = RInt32(entityptrDest + &H28)
        destEntityPosPtr = RInt32(destEntityPosPtr + &H1C)
        Dim facing = RInt32(destEntityPosPtr + &H4)
        Dim posX = RFloat(destEntityPosPtr + &H10)
        Dim posY = RFloat(destEntityPosPtr + &H14)
        Dim posZ = RFloat(destEntityPosPtr + &H18)

        warpentity_coords(entityptrSrc, posX, posY, posZ, facing)
    End Sub
    ''' <summary>
    ''' TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO 
    ''' </summary>
    ''' <param name="entityId">TODO</param>
    Public Shared Function getentityptr(entityId As Integer) As Integer 'TODO
        'TODO 
        Return ScriptEnvironment.Run("ChrFadeIn " & entityId & ", 0, 0") 'TODO 
        'TODO 
    End Function 'TODO


    Public Shared Sub setbriefingmsg(ByVal str As String)
        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H7C)

        WUniStr(tmpptr + &H3B7A, str + ChrW(0))
        ScriptEnvironment.Run("RequestOpenBriefingMsg 10010721, 1")

    End Sub
    'TODO: Make less bad
    Public Shared Sub setgendialog(ByRef genDiagResponse As Integer, ByRef genDiagVal As Integer, ByVal str As String, type As Integer, Optional btn0 As String = "", Optional btn1 As String = "")
        '50002 = Overridden Maintext
        '65000 = Overridden Button 0
        '70000 = Overridden Button 1

        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H174)

        str = str.Replace("\n", ChrW(&HA))

        'Weird issues if exactly 6 characters
        If str.Length = 6 Then str = str & "  "
        WUniStr(tmpptr + &H1A5C, str + ChrW(0))

        'Set Default Ok/Cancel if not overridden
        WInt32(&H12E33E4, 1)
        WInt32(&H12E33E8, 2)

        'Clear previous values
        WInt32(&H12E33F8, -1)
        WInt32(&H12E33FC, -1)

        WInt32(&H12E33E0, 50002)
        If btn0.Length > 0 Then
            WInt32(&H12E33E4, 65000)
            WUniStr(tmpptr + &H2226, btn0 + ChrW(0))
        End If
        If btn1.Length > 0 Then
            WInt32(&H12E33E8, 70000)
            WUniStr(tmpptr + &H350C, btn1 + ChrW(0))
        End If

        tmpptr = RInt32(&H13786D0)
        WInt32(tmpptr + &H60, type)


        'Wait for response
        genDiagResponse = -1
        genDiagVal = -1

        tmpptr = &H12E33F8

        While genDiagResponse = -1
            genDiagResponse = RInt32(tmpptr)
            genDiagVal = RInt32(tmpptr + &H4)
            Thread.Sleep(33)
        End While
        ScriptEnvironment.Run("Wait 500")
    End Sub
    Public Shared Sub wait(val As Integer)
        Thread.Sleep(val)
    End Sub

    Public Shared Function waitforbossdeath(ByVal boost As Integer, match As Integer) As Integer
        Dim eventPtr As Integer
        eventPtr = RInt32(&H137D7D4)
        eventPtr = RInt32(eventPtr)

        Dim hpPtr As Integer
        hpPtr = RInt32(&H137DC70)
        hpPtr = RInt32(hpPtr + 4)
        hpPtr = RInt32(hpPtr)
        hpPtr = hpPtr + &H2D4

        Dim bossdead As Boolean = False
        Dim selfdead As Boolean = False

        While Not (bossdead Or selfdead)
            bossdead = (RInt32(eventPtr + boost) And match)
            selfdead = (RInt32(hpPtr) = 0)
            Console.WriteLine(Hex(eventPtr) & " - " & Hex(RInt32(eventPtr)))
            Thread.Sleep(33)
        End While

        If bossdead Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Public Shared Function funccall(func As String, Optional param1 As String = "", Optional param2 As String = "", Optional param3 As String = "", Optional param4 As String = "", Optional param5 As String = "") As Integer

        Dim Params() As String = {param1, param2, param3, param4, param5}
        Dim param As IntPtr = Marshal.AllocHGlobal(4)
        Dim intParam As Integer
        Dim floatParam As Single
        Dim a As New asm

        func = func.ToLower

        Dim funcPtr = ScriptEnvironment.Current.funcPtr

        a.pos = funcPtr
        a.AddVar("funcloc", ScriptEnvironment.Current.clsFuncLocs(func.ToLower))
        a.AddVar("returnedloc", funcPtr + &H200)

        a.Asm("push ebp")
        a.Asm("mov ebp,esp")
        a.Asm("push eax")

        'Parse params, add as variables to the ASM
        For i As Integer = 4 To 0 Step -1
            If Params(i).ToLower = "false" Then Params(i) = "0"
            If Params(i).ToLower = "true" Then Params(i) = "1"
            If Params(i).Length < 1 Then Params(i) = "0"

            If Params(i).Contains(".") Then
                floatParam = Convert.ToSingle(Params(i), New CultureInfo("en-us"))
                Marshal.StructureToPtr(floatParam, param, False)
                a.AddVar("param" & i, Marshal.ReadInt32(param))
            Else
                intParam = Convert.ToInt32(Params(i), New CultureInfo("en-us"))
                a.AddVar("param" & i, intParam)
            End If

            a.Asm("mov eax,param" & i)
            a.Asm("push eax")

        Next
        a.Asm("call funcloc")
        a.Asm("mov ebx,returnedloc")
        a.Asm("mov [ebx],eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("pop eax")
        a.Asm("mov esp,ebp")
        a.Asm("pop ebp")
        a.Asm("ret")

        Marshal.FreeHGlobal(param)


        WriteProcessMemory(_targetProcessHandle, funcPtr, a.bytes, 1024, 0)
        CreateRemoteThread(_targetProcessHandle, 0, 0, funcPtr, 0, 0, 0)
        Thread.Sleep(5)



        Return RInt32(funcPtr + &H200)


    End Function
End Class
