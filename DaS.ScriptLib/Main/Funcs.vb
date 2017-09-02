Imports System.Threading

Public Class Funcs
    Private Shared ReadOnly Property checkIfLoadingScreen_PrevFrameInGameTime

    'btw I never mentioned why these overloads are here when we have Optional args.
    'That's because all optional args are treated as regular args in lua.
    'Without these, doing "ChrFadeIn(10000)" makes it complain about wrong args

    '<HideFromScripting>
    'Public Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, func As String) As T
    '    Return AsmExecutor.FuncCall(Of T)(retType, func, "", "", "", "", "", "", "", "", "", "")
    'End Function

    '<HideFromScripting>
    'Public Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, func As String, Optional param1 As Object = "") As T
    '    Return FuncCall(Of T)(retType, func, param1, "", "", "", "")
    'End Function

    '<HideFromScripting>
    'Public Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, func As String, Optional param1 As Object = "", Optional param2 As Object = "") As T
    '    Return FuncCall(Of T)(retType, func, param1, param2, "", "", "")
    'End Function

    '<HideFromScripting>
    'Public Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, func As String, Optional param1 As Object = "", Optional param2 As Object = "", Optional param3 As Object = "") As T
    '    Return FuncCall(Of T)(retType, func, param1, param2, param3, "", "")
    'End Function

    '<HideFromScripting>
    'Public Shared Function FuncCall(Of T)(retType As IngameFuncReturnType, func As String, Optional param1 As Object = "", Optional param2 As Object = "", Optional param3 As Object = "", Optional param4 As Object = "") As T
    '    Return FuncCall(Of T)(retType, func, param1, param2, param3, param4, "")
    'End Function

    Public Shared Function GetNgPlusText(ngLevel As String) As String
        If ngLevel = 0 Then
            Return "NG"
        ElseIf ngLevel = 1 Then
            Return "NG+"
        Else
            Return "NG+" & ngLevel
        End If
    End Function

    Public Shared Sub MsgBoxOK(text As String)
        SetGenDialog(text, 1, "OK")
    End Sub

    Public Shared Sub MsgBoxBtn(text As String, btnText As String)
        MsgBoxChoice(text, btnText, "")
    End Sub

    Public Shared Function MsgBoxChoice(text As String, choice1 As String, choice2 As String) As Integer
        Return SetGenDialog(text, 2, choice1, choice2).Response
    End Function

    Public Shared Sub Warp_Coords(ByVal x As Single, y As Single, z As Single, Optional rotx As Single? = Nothing)
        Dim charptr1 = RInt32(&H137DC70)
        charptr1 = RInt32(charptr1 + &H4)
        charptr1 = RInt32(charptr1)
        Dim charmapdataptr = RInt32(charptr1 + &H28)

        WFloat(charmapdataptr + &HD0, x)
        WFloat(charmapdataptr + &HD4, y)
        WFloat(charmapdataptr + &HD8, z)

        If rotx.HasValue Then
            Dim facing As Single
            facing = ((rotx / 360) * 2 * Math.PI) - Math.PI
            WFloat(charmapdataptr + &HE4, facing)
        Else
            WFloat(charmapdataptr + &HE4, Game.Player.Heading.Value)
        End If



        WBytes(charmapdataptr + &HC8, {1})
    End Sub

    Public Shared Sub WarpEntity_Coords(entityPtr As Integer, x As Single, y As Single, z As Single, rotx As Single)
        entityPtr = RInt32(entityPtr + &H28)
        
        WFloat(entityPtr + &HD0, x)
        WFloat(entityPtr + &HD4, y)
        WFloat(entityPtr + &HD8, z)

        Dim facing As Single
        facing = ((rotx / 360) * 2 * Math.PI) - Math.PI

        WFloat(entityPtr + &HE4, facing)
        WBytes(entityPtr + &HC8, {1})
    End Sub

    Public Shared Sub BlackScreen()
        Dim tmpptr As Integer
        tmpptr = RUInt32(&H1378520)
        tmpptr = RUInt32(tmpptr + &H10)

        WBytes(tmpptr + &H26D, {1})

        WFloat(tmpptr + &H270, 0)
        WFloat(tmpptr + &H274, 0)
        WFloat(tmpptr + &H278, 0)
    End Sub

    Public Shared Sub CamFocusEntity(entityptr As Integer)
        Dim camPtr As Integer = RInt32(&H137D648) + &HEC

        WInt32(camPtr, entityptr)
    End Sub

    Public Shared Sub ClearPlayTime()
        Dim tmpPtr As Integer = RIntPtr(&H1378700)
        WInt32(tmpPtr + &H68, 0)
    End Sub

    Public Shared Sub ControlEntity(entityPtr As Integer, state As Boolean)
        entityPtr = RInt32(entityPtr + &H28)

        Dim ctrlptr As Integer = RInt32(&H137DC70)
        ctrlptr = RInt32(ctrlptr + 4)
        ctrlptr = RInt32(ctrlptr)
        ctrlptr = RInt32(ctrlptr + &H28)
        ctrlptr = RInt32(ctrlptr + &H54)

        WInt32(entityPtr + &H244, ctrlptr * (state And 1))

    End Sub

    Public Shared Sub DisableAI(ByVal state As Boolean)
        WBool(&H13784EE, state)
    End Sub

    Public Shared Sub PlayerExterminate(ByVal state As Boolean)
        WBool(&H13784D3, state)
    End Sub

    Public Shared Sub FadeIn()
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

    Public Shared Sub FadeOut()
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

    Public Shared Sub ForceEntityDrawGroup(entityptr As Integer)
        WInt32(entityptr + &H264, -1)
        WInt32(entityptr + &H268, -1)
        WInt32(entityptr + &H26C, -1)
        WInt32(entityptr + &H270, -1)
    End Sub

    Public Shared Sub SetCamPos(ByVal xpos As Single, ypos As Single, zpos As Single, xrot As Single, yrot As Single)
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

    Public Shared Sub SetFreeCam(ByVal state As Boolean)
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

    Public Shared Sub SetClearCount(ByVal clearCount As Integer)
        Dim tmpPtr As Integer
        tmpPtr = RInt32(&H1378700)

        WInt32(tmpPtr + &H3C, clearCount)

    End Sub

    Private Shared Sub SetCaption(ByVal str As String)
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

        WUnicodeStr(tmpptr + &H12C, str & ChrW(0))

    End Sub

    Public Shared Sub SetSaveEnable(ByVal state As Boolean)
        Dim tmpPtr As Integer
        tmpPtr = RInt32(&H13784A0)

        WBool(tmpPtr + &HB40, state)
    End Sub

    Public Shared Sub SetSaveSlot(ByVal slot As Integer)
        WInt32(RInt32(&H13784A0) + &HA70, slot)
    End Sub

    Public Shared Sub SetUnknownNpcName(ByVal name As String)
        If name.Length > 21 Then name = Strings.Left(name, 21) 'Prevent runover into code
        WUnicodeStr(&H11A784C, name + ChrW(0))
    End Sub

    Public Shared Function GetClosestEntityToEntity(entityPtr As Integer) As Integer
        Dim ptrList = GetEntityPtrList()

        Dim closestDist = Single.PositiveInfinity
        Dim closestPtr As Integer = -1

        For Each p In ptrList
            If p = entityPtr Then Continue For
            Dim dist = GetDistanceBetweenEntities(entityPtr, p)
            If (dist < closestDist) Then
                closestPtr = p
                closestDist = dist
            End If
        Next

        Return closestPtr
    End Function

    Public Shared Function GetEntityPtrList() As Integer()

        Dim structPtr = RInt32(&H137D644)
        structPtr = RInt32(structPtr + &HE4)
        Dim entityCount = RInt32(structPtr)
        structPtr = RInt32(structPtr + 4)

        Dim resultList = New List(Of Integer)

        For i = 0 To entityCount - 1
            resultList.Add(RInt32(structPtr + i * &H20))
        Next

        Return resultList.ToArray()
    End Function

    Public Shared Function GetEntityVec3(entityPtr As Integer) As Vec3
        Return New Vec3(GetEntityPosX(entityPtr), GetEntityPosY(entityPtr), GetEntityPosZ(entityPtr))
    End Function

    Public Shared Sub MoveEntityLaterallyTowardEntity(entityFromPtr As Integer, entityToPtr As Integer, speed As Single)
        MoveEntityLaterally(entityFromPtr, GetAngleBetweenEntities(entityFromPtr, entityToPtr), speed)
    End Sub

    Public Shared Function GetAngleBetweenEntities(entityPtrA As Integer, entityPtrB As Integer) As Single
        Dim x1 = GetEntityPosX(entityPtrA)
        Dim z1 = GetEntityPosZ(entityPtrA)

        Dim x2 = GetEntityPosX(entityPtrB)
        Dim z2 = GetEntityPosZ(entityPtrB)

        Return Math.Atan2(z2 - z1, x2 - x1) 'TODO: Check my trig cuz I did this at 6:42 AM
    End Function

    Public Shared Function GetDistanceSqrdBetweenEntities(entityPtrA As Integer, entityPtrB As Integer) As Single
        Dim x1 = GetEntityPosX(entityPtrA)
        Dim y1 = GetEntityPosY(entityPtrA)
        Dim z1 = GetEntityPosZ(entityPtrA)

        Dim x2 = GetEntityPosX(entityPtrB)
        Dim y2 = GetEntityPosY(entityPtrB)
        Dim z2 = GetEntityPosZ(entityPtrB)

        Return Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2)
    End Function

    Public Shared Function GetDistanceBetweenEntities(entityPtrA As Integer, entityPtrB As Integer) As Single
        Return Math.Sqrt(GetDistanceSqrdBetweenEntities(entityPtrA, entityPtrB))
    End Function

    Public Shared Sub MoveEntityLaterally(entityPtr As Integer, angle As Single, speed As Single)
        MoveEntityAtSpeed(entityPtr, Math.Cos(angle) * speed, 0, Math.Sin(angle) * speed, 0)
    End Sub

    Public Shared Sub MoveEntityAtSpeed(entityPtr As Integer, speedX As Single, speedY As Single, speedZ As Single, Optional speedRot As Single = 0)

        SetEntityPosX(entityPtr, GetEntityPosX(entityPtr) + speedX)
        SetEntityPosY(entityPtr, GetEntityPosY(entityPtr) + speedY)
        SetEntityPosZ(entityPtr, GetEntityPosZ(entityPtr) + speedZ)
        SetEntityRotation(entityPtr, GetEntityRotation(entityPtr) + speedRot)

    End Sub

    Public Shared Function GetEntityPosX(entityPtr As Integer) As Single
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        Return RFloat(entityPosPtr + &H10)
    End Function

    Public Shared Function GetEntityPosY(entityPtr As Integer) As Single
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        Return RFloat(entityPosPtr + &H14)
    End Function

    Public Shared Function GetEntityPosZ(entityPtr As Integer) As Single
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        Return RFloat(entityPosPtr + &H18)
    End Function

    Public Shared Function GetEntityRotation(entityPtr As Integer) As Single
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        Return CType(RFloat(entityPosPtr + &H4) / Math.PI * 180, Single) + 180
    End Function

    Public Shared Sub SetEntityPosX(entityPtr As Integer, posX As Single)
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        WFloat(entityPosPtr + &H10, posX)
    End Sub

    Public Shared Sub SetEntityPosY(entityPtr As Integer, posY As Single)
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        WFloat(entityPosPtr + &H14, posY)
    End Sub

    Public Shared Sub SetEntityPosZ(entityPtr As Integer, posZ As Single)
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        WFloat(entityPosPtr + &H18, posZ)
    End Sub

    Public Shared Sub SetEntityRotation(entityPtr As Integer, angle As Single)
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        WFloat(entityPosPtr + &H4, CType(angle * Math.PI / 180, Single) - CType(Math.PI, Single))
    End Sub

    Public Shared Sub SetEntityXYZR(entityPtr As Integer, posX As Single, posY As Single, posZ As Single, angle As Single)
        Dim entityPosPtr = RInt32(entityPtr + &H28)
        entityPosPtr = RInt32(entityPosPtr + &H1C)
        WFloat(entityPosPtr + &H10, posX)
        WFloat(entityPosPtr + &H14, posY)
        WFloat(entityPosPtr + &H18, posZ)
        WFloat(entityPosPtr + &H4, CType(angle * Math.PI / 180, Single) - CType(Math.PI, Single))
    End Sub

    Public Shared Function GetInGameTimeInMs() As Integer
        Return RInt32(RInt32(&H1378700) + &H68)
    End Function

    Public Shared Sub SetEntityLocation(entityPtr As Integer, location As EntityLocation)
        SetEntityXYZR(entityPtr, location.Pos.X, location.Pos.Y, location.Pos.Z, location.Rot)
    End Sub

    Public Shared Sub PlayerHide(ByVal state As Boolean)
        WBool(&H13784E7, state)
    End Sub

    Public Shared Sub ShowHUD(ByVal state As Boolean)
        Dim tmpptr As UInteger
        tmpptr = RUInt32(&H1378700)
        tmpptr = RUInt32(tmpptr + &H2C)

        WBool(New IntPtr(tmpptr + &HD), state)
    End Sub

    Public Shared Sub WaitForLoadEnd() 'TODO: waitforload -> WaitForLoadEnd
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

    Public Shared Sub WaitForLoadStart() 'TODO: waittillload -> WaitForLoadStart
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

    Public Shared Sub WarpEntity_Player(entityptr As Integer)
        Dim playerptr As Integer = Lua.Expr(Of Integer)("GetEntityPtr(10000)")
        WarpEntity_Entity(entityptr, playerptr)
    End Sub

    Public Shared Sub WarpPlayer_Entity(entityptr As Integer)
        Dim playerptr As Integer = Lua.Expr(Of Integer)("GetEntityPtr(10000)")
        WarpEntity_Entity(playerptr, entityptr)
    End Sub

    Public Shared Sub WarpEntity_Entity(entityptrSrc As Integer, entityptrDest As Integer)
        'TODO: Check validity of entity pointers
        Dim destEntityPosPtr = RInt32(entityptrDest + &H28)
        destEntityPosPtr = RInt32(destEntityPosPtr + &H1C)
        Dim facing = RFloat(destEntityPosPtr + &H4)
        Dim posX = RFloat(destEntityPosPtr + &H10)
        Dim posY = RFloat(destEntityPosPtr + &H14)
        Dim posZ = RFloat(destEntityPosPtr + &H18)

        WarpEntity_Coords(entityptrSrc, posX, posY, posZ, facing)
    End Sub


    Public Shared Function GetEntityPtr(entityId As Integer) As Integer
        Return Lua.Expr(Of Integer)("ChrFadeIn(" & entityId & ", 1.0, 1.0)")
    End Function

    Public Shared Function GetEntityPtrByName(mapName As String, entName As String) As Integer
        Dim tmpStr As String = ""

        Dim tmpPtr As Integer = 0
        Dim tmpCnt As Integer = 0

        Dim mapCount As Integer = 0
        Dim mapPtrs As Integer = 0

        Dim entitiesPtr As Integer = 0
        Dim entitiesCnt As Integer = 0

        Dim entityPtr As Integer = 0

        tmpPtr = RInt32(&H137D644)

        mapPtrs = tmpPtr + &H74
        mapCount = RInt32(tmpPtr + &H70)

        For mapNum = 0 To mapCount - 1
            entitiesPtr = RInt32(mapPtrs + 4 * mapNum)

            tmpStr = RUnicodeStr(RInt32(RInt32(entitiesPtr + &H60) + 4))

            If tmpStr = mapName Then
                entitiesCnt = RInt32(entitiesPtr + &H3C)
                entitiesPtr = RInt32(entitiesPtr + &H40)

                For entityNum = 0 To entitiesCnt - 1
                    entityPtr = RInt32(entitiesPtr + entityNum * &H20)

                    tmpPtr = RInt32(entityPtr + &H54)
                    tmpCnt = RInt32(entityPtr + &H58) - 1

                    tmpPtr = RInt32(tmpPtr + &H28) + &H10
                    tmpPtr = RInt32(RInt32(tmpPtr + 4 * tmpCnt))

                    tmpStr = RAsciiStr(tmpPtr)

                    If tmpStr = entName Then
                        Return entityPtr
                    End If
                Next entityNum
            End If
        Next mapNum
        Return 0
    End Function

    Public Shared Sub SetBriefingMsg(ByVal str As String)
        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H7C)

        WUnicodeStr(tmpptr + &H3B7A, str + ChrW(0))
        Lua.Run("RequestOpenBriefingMsg(10010721, 1)")
    End Sub

    Public Class GenDiagResult
        Public Response As Integer = 0
        Public Val As Integer = 0

        Public Sub New(response, val)
            Me.Response = response
            Me.Val = val
        End Sub

    End Class

    'TODO: Make less bad
    Public Shared Function SetGenDialog(ByVal str As String, type As Integer, Optional btn0 As String = "", Optional btn1 As String = "") As GenDiagResult
        '50002 = Overridden Maintext
        '65000 = Overridden Button 0
        '70000 = Overridden Button 1

        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H174)

        str = str.Replace("\n", ChrW(&HA))

        'Weird issues if exactly 6 characters
        If str.Length = 6 Then str = str & "  "
        WUnicodeStr(tmpptr + &H1A5C, str + ChrW(0))

        'Set Default Ok/Cancel if not overridden
        WInt32(&H12E33E4, 1)
        WInt32(&H12E33E8, 2)

        'Clear previous values
        WInt32(&H12E33F8, -1)
        WInt32(&H12E33FC, -1)

        WInt32(&H12E33E0, 50002)
        If btn0.Length > 0 Then
            WInt32(&H12E33E4, 65000)
            WUnicodeStr(tmpptr + &H2226, btn0 + ChrW(0))
        End If
        If btn1.Length > 0 Then
            WInt32(&H12E33E8, 70000)
            WUnicodeStr(tmpptr + &H350C, btn1 + ChrW(0))
        End If

        tmpptr = RInt32(&H13786D0)
        WInt32(tmpptr + &H60, type)

        'Wait for response
        Dim genDiagResponse = -1
        Dim genDiagVal = -1

        tmpptr = &H12E33F8

        While genDiagResponse = -1
            genDiagResponse = RInt32(tmpptr)
            genDiagVal = RInt32(tmpptr + &H4)
            Thread.Sleep(33)
        End While
        Thread.Sleep(500)
        Return New GenDiagResult(genDiagResponse, genDiagVal)
    End Function

    Public Shared Sub Wait(val As Integer)
        Thread.Sleep(val)
    End Sub

    'Public Shared Function WaitForBossDeath(ByVal boost As Integer, match As Integer) As Boolean
    '    Dim eventPtr As Integer
    '    eventPtr = RInt32(&H137D7D4)
    '    eventPtr = RInt32(eventPtr)

    '    Dim hpPtr As Integer
    '    hpPtr = RInt32(&H137DC70)
    '    hpPtr = RInt32(hpPtr + 4)
    '    hpPtr = RInt32(hpPtr)
    '    hpPtr = hpPtr + &H2D4

    '    Dim bossdead As Boolean = False
    '    Dim selfdead As Boolean = False

    '    While Not (bossdead Or selfdead)
    '        bossdead = (RInt32(eventPtr + boost) And match)
    '        selfdead = (RInt32(hpPtr) = 0)
    '        Console.WriteLine(Hex(eventPtr) & " - " & Hex(RInt32(eventPtr)))
    '        Thread.Sleep(33)
    '    End While

    '    If bossdead Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function

    Public Shared Sub DropItem(ByVal cat As String, item As String, num As Integer)
        Dim TargetBufferSize = 1024
        Dim Rtn As Integer

        Dim bytes() As Byte
        Dim bytes2() As Byte

        Dim bytcat As Integer = &H1
        Dim bytitem As Integer = &H6
        Dim bytcount As Integer = &H10
        Dim bytptr1 As Integer = &H15
        Dim bytptr2 As Integer = &H32
        Dim bytjmp As Integer = &H38

        bytes = {&HBD, 0, 0, 0, 0, &HBB, &HF0, &H0, &H0, &H0, &HB9, &HFF, &HFF, &HFF, &HFF, &HBA, 0, 0, 0, 0, &HA1, &HD0, &H86, &H37, &H1, &H89, &HA8, &H28, &H8, &H0, &H0, &H89, &H98, &H2C, &H8, &H0, &H0, &H89, &H88, &H30, &H8, &H0, &H0, &H89, &H90, &H34, &H8, &H0, &H0, &HA1, &HBC, &HD6, &H37, &H1, &H50, &HE8, 0, 0, 0, 0, &HC3}

        'cllItemCatsIDs(clsItemCatsIDs("Weapons") / &H10000000)("Target Shield+15"))

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(ScriptRes.clsItemCatsIDs(cat)))
        Array.Copy(bytes2, 0, bytes, bytcat, bytes2.Length)

        Dim tmpCat As Integer
        tmpCat = Convert.ToInt32(ScriptRes.clsItemCatsIDs(cat) / &H10000000)
        If tmpCat = 4 Then tmpCat = 3

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(ScriptRes.cllItemCatsIDs(tmpCat)(item)))
        Array.Copy(bytes2, 0, bytes, bytitem, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(num))
        Array.Copy(bytes2, 0, bytes, bytcount, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H13786D0))
        Array.Copy(bytes2, 0, bytes, bytptr1, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H137D6BC))
        Array.Copy(bytes2, 0, bytes, bytptr2, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(0 - ((Game.Injected.ItemDropPtr.Address + &H3C) - (&HDC8C60))))
        Array.Copy(bytes2, 0, bytes, bytjmp, bytes2.Length)

        Rtn = WriteProcessMemory(_targetProcessHandle, Game.Injected.ItemDropPtr.Address, bytes, TargetBufferSize, 0)
        'MsgBox(Hex(dropPtr))
        CreateRemoteThread(_targetProcessHandle, 0, 0, Game.Injected.ItemDropPtr.Address, 0, 0, 0)

        Thread.Sleep(5)
    End Sub

    Public Shared Sub SetKeyGuideText(text As String)
        WInt32(Game.MenuPtr.Value + &H158, RInt32(Game.MenuPtr.Value + &H1C))
        WUnicodeStr(&H11A7770, text)
    End Sub

    Public Shared Sub SetLineHelpText(text As String)
        WInt32(Game.MenuPtr.Value + &H154, RInt32(Game.MenuPtr.Value + &H1C))
        WUnicodeStr(&H11A7758, text)
    End Sub

    Public Shared Sub SetKeyGuideTextPos(x As Single, y As Single)
        WFloat(Game.KeyPtr.Value + &H78, x)
        WFloat(Game.KeyPtr.Value + &H7C, y)
    End Sub

    Public Shared Sub SetLineHelpTextPos(x As Single, y As Single)
        WFloat(Game.LinePtr.Value + &H78, x)
        WFloat(Game.LinePtr.Value + &H7C, y)
    End Sub

    Public Shared Sub SetKeyGuideTextClear()
        WInt32(Game.MenuPtr.Value + &H158, -1)
    End Sub

    Public Shared Sub SetLineHelpTextClear()
        WInt32(Game.MenuPtr.Value + &H154, -1)
    End Sub

    Public Shared Sub ForcePlayerStableFootPos()
        Game.Player.StablePosX.Value = Game.Player.PosX.Value
        Game.Player.StablePosY.Value = Game.Player.PosY.Value
        Game.Player.StablePosZ.Value = Game.Player.PosZ.Value
    End Sub

#Region "Old Boss Rush Functions"

    'Public Shared Sub BossAsylumDemon()

    '    Dim bossDead As Boolean = False
    '    Dim firstTry As Boolean = True

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 16, False") 'Boss Death Flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11810000, False") 'Tutorial Complete Flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11815395, True") 'Boss at lower position

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1810998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")
    '        LegacyScripting.RunOneLine("Warp_Coords 3.15, 198.15, -6.0, 180")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")
    '        LegacyScripting.RunOneLine("SetEventFlag 11815390, True")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")

    '        If firstTry And rushName = "Normal" Then
    '            LegacyScripting.RunOneLine("ClearPlayTime")
    '            firstTry = False
    '        End If

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H8000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossBedOfChaos()

    '    LegacyScripting.RunOneLine("SetEventFlag 10, False") 'Boss

    '    LegacyScripting.RunOneLine("SetEventFlag 11410000, False")
    '    LegacyScripting.RunOneLine("SetEventFlag 11410200, False") 'Center Platform flag
    '    LegacyScripting.RunOneLine("SetEventFlag 11410291, False") 'Arm flag
    '    LegacyScripting.RunOneLine("SetEventFlag 11410292, False") 'Arm flag

    '    'warp before fog gate to set last solid position

    '    LegacyScripting.RunOneLine("PlayerHide 1")
    '    LegacyScripting.RunOneLine("ShowHUD False")
    '    LegacyScripting.RunOneLine("FadeOut")
    '    LegacyScripting.RunOneLine("SetHp 10000, 1.0")

    '    LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1410980")

    '    LegacyScripting.RunOneLine("Wait 1000")

    '    LegacyScripting.RunOneLine("WaitForLoadEnd")
    '    LegacyScripting.RunOneLine("BlackScreen")
    '    LegacyScripting.RunOneLine("PlayerHide 1")
    '    LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '    LegacyScripting.RunOneLine("Wait 500")
    '    LegacyScripting.RunOneLine("Warp 10000, 1412998")
    '    LegacyScripting.RunOneLine("Wait 250")
    '    LegacyScripting.RunOneLine("Warp 10000, 1412997")

    '    LegacyScripting.RunOneLine("Wait 1250")
    '    LegacyScripting.RunOneLine("FadeIn")
    '    LegacyScripting.RunOneLine("ShowHUD 1")
    '    LegacyScripting.RunOneLine("PlayerHide 0")
    '    LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")

    'End Sub
    'Public Shared Sub BossBellGargoyles()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")
    '        LegacyScripting.RunOneLine("SetEventFlag 3, False") 'Boss Death Flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11010000, False") 'Boss Cinematic Viewed Flag

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")
    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1010998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("SetEventFlag 11015390, True") 'Boss Fog Used
    '        LegacyScripting.RunOneLine("SetEventFlag 11015393, True") 'Boss Area Entered
    '        LegacyScripting.RunOneLine("Wait 250")

    '        'facing 0 degrees
    '        LegacyScripting.RunOneLine("Warp_Coords 10.8, 48.92, 87.26")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1250")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H10000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If
    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossBlackDragonKalameet()
    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11210004, False")

    '        LegacyScripting.RunOneLine("SetEventFlag 121, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210539, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210535, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210067, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210066, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210056, True")

    '        LegacyScripting.RunOneLine("SetEventFlag 1821, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210592, True")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1210998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")
    '        LegacyScripting.RunOneLine("Warp_Coords 876.04, -344.73, 749.75, 240")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H2300, &H8000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossCapraDemon()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11010902, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1010998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")
    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords -73.17, -43.56, -15.17, 321")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&HF70, &H2000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossCeaselessDischarge()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11410800, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11410801, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11410900, False") 'Boss death flag
    '        LegacyScripting.RunOneLine("SetEventFlag 51410180, True") 'Corpse Loot reset

    '        LegacyScripting.RunOneLine("SetEventFlag 11415379, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415385, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415378, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415373, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415372, True")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1410998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 250.53, -283.15, 72.1")
    '        LegacyScripting.RunOneLine("Wait 250")

    '        LegacyScripting.RunOneLine("Warp_Coords 402.45, -278.15, 15.5, 30")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1250")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H3C70, &H8000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossCentipedeDemon()
    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")
    '        LegacyScripting.RunOneLine("SetEventFlag 11410002, False") 'Cinematic flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11410901, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1410998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp 10000, 1412896")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415380, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415383, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11415382, True")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H3C70, &H4000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossChaosWitchQuelaag()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 9, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11400000, False") 'Cinematic flag

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1400980")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 17.2, -236.9, 113.6, 75")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H400000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossCrossbreedPriscilla()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")
    '        LegacyScripting.RunOneLine("SetEventFlag 4, False") 'Boss Death flag
    '        LegacyScripting.RunOneLine("SetEventFlag 1691, True") 'Priscilla Hostile flag
    '        LegacyScripting.RunOneLine("SetEventFlag 1692, True") 'Priscilla Dead flag

    '        LegacyScripting.RunOneLine("SetEventFlag 11100531, False") 'Boss Disabled flag

    '        LegacyScripting.RunOneLine("SetEventFlag 11100000, False") 'Previous victory flag

    '        'StandardTransition(1102961, 1102997)

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1102961")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords -22.72, 60.55, 711.86")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H8000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossDarkSunGwyndolin()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11510900, False") 'Boss Death Flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11510523, False") 'Boss Disabled Flag

    '        'StandardTransition(1510982, 1512896)
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1510982")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 435.1, 60.2, 255.0")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H4670, &H8000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossDemonFiresage()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11410410, False")
    '        'StandardTransition(1410998, 1412416)

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1410998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 148.04, -341.04, 95.57")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H3C30, &H20)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossFourKings()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 13, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 1677, True") 'Kaathe Angry/gone

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1600999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        'ScriptEnvironment.Run("Warp_Coords 82.24, -163.2, 0.29")
    '        'Facing 185.98
    '        LegacyScripting.RunOneLine("Warp_Coords 85.18, -191.99, 4.95, 185")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        Funcs.DropItem("Rings", "Covenant of Artorias", 1) 'TODO: dropitem
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H40000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                'ScriptEnvironment.Run("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("WaitTillLoad")
    '                LegacyScripting.RunOneLine("WaitForLoadEnd")
    '            End If
    '        End If
    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossGapingDragon()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 2, False") 'Boss Death Flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11000853, True") 'Channeler Death Flag
    '        'StandardTransition(1000999, 1002997)

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1000999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("Wait 500")
    '        LegacyScripting.RunOneLine("SetEventFlag 11005390, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11005392, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11005393, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11005394, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11005397, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11000000, False")

    '        LegacyScripting.RunOneLine("Warp 10000, 1002997")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H20000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")

    'End Sub
    'Public Shared Sub BossGravelordNito()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 7, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1310998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")
    '        LegacyScripting.RunOneLine("Wait 500")

    '        'ScriptEnvironment.Run("Warp 10000, 1312110)
    '        LegacyScripting.RunOneLine("Warp_Coords -126.84, -265.12, -30.78")
    '        LegacyScripting.RunOneLine("SetEventFlag 11315390, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11315393, True")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H1000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")

    'End Sub
    'Public Shared Sub BossGwynLordOfCinder()

    '    Dim bossDead As Boolean = False
    '    Dim firstTry As Boolean = True

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 15, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1800999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 418.15, -115.92, 169.58")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")

    '        If firstTry And rushName = "Reverse" Then
    '            LegacyScripting.RunOneLine("ClearPlayTime")
    '            firstTry = False
    '        End If

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H10000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossIronGolem()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11, False") 'Boss Death Flag
    '        LegacyScripting.RunOneLine("SetEventFlag 11500865, True") 'Bomb-Tossing Giant Death Flag

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1500999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 85.5, 82, 255.1")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H100000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossKnightArtorias()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11210001, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210513, False") 'Ciaran Present

    '        'Non-standard due to co-ords warp

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1210998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")
    '        'facing 75.8 degrees
    '        LegacyScripting.RunOneLine("Warp_Coords 1034.11, -330.0, 810.68")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H2300, &H40000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossManusFatherOfTheAbyss()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11210002, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1210982")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 857.53, -576.69, 873.38")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H2300, &H20000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossMoonlightButterfly()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11200900, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11205383, False")

    '        'timing of warp/flags matters

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1200999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")
    '        LegacyScripting.RunOneLine("Warp_Coords 181.39, 7.53, 29.01")
    '        Thread.Sleep(4000)
    '        LegacyScripting.RunOneLine("SetEventFlag 11205383, True")

    '        LegacyScripting.RunOneLine("Warp_Coords 178.82, 8.12, 30.77")

    '        Thread.Sleep(2000)
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")

    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H1E70, &H8000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")

    'End Sub
    'Public Shared Sub BossOrnsteinAndSmough()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 12, False")

    '        'Non-standard due to co-ords warp

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1510998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")
    '        'facing 90 degrees
    '        LegacyScripting.RunOneLine("Warp_Coords 539.9, 142.6, 254.79")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H80000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossPinwheel()
    '    Dim bossDead As Boolean = False

    '    Do
    '        'Pinwheel Entity ID = 1300800
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 6, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1300999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 46.0, -165.8, 152.02, 180")
    '        LegacyScripting.RunOneLine("CamReset 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H2000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossSanctuaryGuardian()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11210000, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11210001, False")

    '        'Non-standard due to co-ords warp

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")
    '        LegacyScripting.RunOneLine("SetHp 10000, 1.0")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1210998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")
    '        'facing = 45 deg
    '        LegacyScripting.RunOneLine("Warp_Coords 931.82, -318.63, 472.45")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H2300, &H80000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossSeathTheScaleless()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 14, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11700000, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1700999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 109, 134.05, 856.48")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H20000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossGreatGreyWolfSif()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 5, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11200000, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11200001, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11200002, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11205392, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11205393, False")
    '        LegacyScripting.RunOneLine("SetEventFlag 11205394, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("SetHp 10000, 1.0")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1200999")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")
    '        LegacyScripting.RunOneLine("Wait 500")
    '        'ScriptEnvironment.Run("Warp_Coords 274, -19.82, -266.43)
    '        LegacyScripting.RunOneLine("Wait 500")
    '        'ScriptEnvironment.Run("Warp 10000, 1202999)
    '        LegacyScripting.RunOneLine("Warp_Coords 254.31, -16.02, -320.32")

    '        LegacyScripting.RunOneLine("Wait 1000")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(0, &H4000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub
    'Public Shared Sub BossStrayDemon()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11810000, True")
    '        LegacyScripting.RunOneLine("SetEventFlag 11810900, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1810998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("DisableDamage 10000, 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp 10000, 1812996")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")
    '        LegacyScripting.RunOneLine("Wait 1000")
    '        LegacyScripting.RunOneLine("DisableDamage 10000, 0")
    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&H5A70, &H8000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")

    'End Sub
    'Public Shared Sub BossTaurusDemon()

    '    Dim bossDead As Boolean = False

    '    Do
    '        LegacyScripting.RunOneLine("RequestFullRecover")

    '        LegacyScripting.RunOneLine("SetEventFlag 11010901, False")

    '        LegacyScripting.RunOneLine("PlayerHide 1")
    '        LegacyScripting.RunOneLine("ShowHUD False")
    '        LegacyScripting.RunOneLine("FadeOut")

    '        LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1010998")

    '        LegacyScripting.RunOneLine("Wait 1000")

    '        LegacyScripting.RunOneLine("WaitForLoadEnd")
    '        LegacyScripting.RunOneLine("BlackScreen")
    '        LegacyScripting.RunOneLine("PlayerHide 1")

    '        LegacyScripting.RunOneLine("Wait 500")

    '        LegacyScripting.RunOneLine("Warp_Coords 49.81, 16.9, -118.87")

    '        LegacyScripting.RunOneLine("Wait 1500")
    '        LegacyScripting.RunOneLine("FadeIn")
    '        LegacyScripting.RunOneLine("ShowHUD 1")
    '        LegacyScripting.RunOneLine("PlayerHide 0")

    '        If rushMode Then
    '            bossDead = Funcs.WaitForBossDeath(&HF70, &H4000000)
    '            If Not bossDead Then
    '                LegacyScripting.RunOneLine("AddTrueDeathCount")
    '                LegacyScripting.RunOneLine("SetTextEffect 16")
    '                LegacyScripting.RunOneLine("Wait 5000")
    '            End If
    '        End If

    '    Loop While rushMode And Not bossDead
    '    LegacyScripting.RunOneLine("Wait 5000")
    'End Sub

    'Public Shared Sub ScenarioOandSandOandS_Debug()
    '    ScenarioOandSandOandS()
    '    'LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")
    '    'LegacyScripting.RunOneLine("SetIgnoreHit 10000, 1")
    '    'LegacyScripting.RunOneLine("PlayerExterminate 1")
    'End Sub

    'Public Shared Sub ScenarioOandSandOandS()
    '    'Data.Scripts("ScenarioOandSandOandS").Execute()
    'End Sub

    'Public Shared Sub ScenarioArtoriasAndCiaran()

    '    'LegacyScripting.RunOneLine("SetEventFlag 11210001, False") 'Artorias Disabled
    '    'LegacyScripting.RunOneLine("SetEventFlag 11210513, True") 'Ciaran Present

    '    'LegacyScripting.RunOneLine("SetEventFlag 1863, False") 'Ciaran Hostile
    '    'LegacyScripting.RunOneLine("SetEventFlag 1864, False") 'Ciaran Dead

    '    'LegacyScripting.RunOneLine("PlayerHide 1")
    '    'LegacyScripting.RunOneLine("ShowHUD False")
    '    'LegacyScripting.RunOneLine("FadeOut")

    '    'LegacyScripting.RunOneLine("SetHp 10000, 1.0")

    '    'LegacyScripting.RunOneLine("WarpNextStage_Bonfire 1210998")

    '    'LegacyScripting.RunOneLine("Wait 1000")

    '    'LegacyScripting.RunOneLine("WaitForLoadEnd")
    '    'LegacyScripting.RunOneLine("BlackScreen")

    '    'LegacyScripting.RunOneLine("PlayerHide 1")
    '    'LegacyScripting.RunOneLine("SetDisableGravity 10000, 1")

    '    'LegacyScripting.RunOneLine("Wait 500")
    '    ''facing 75.8 degrees
    '    'LegacyScripting.RunOneLine("Warp_Coords 1034.11, -330.0, 810.68")

    '    'LegacyScripting.RunOneLine("Wait 1500")
    '    'LegacyScripting.RunOneLine("FadeIn")
    '    'LegacyScripting.RunOneLine("ShowHUD 1")
    '    'LegacyScripting.RunOneLine("PlayerHide 0")
    '    'LegacyScripting.RunOneLine("SetDisableGravity 10000, 0")

    '    'LegacyScripting.RunOneLine("SetEventFlag 1863, True") 'Ciaran Hostile
    '    ''funccall_old("SetBossGauge", {6740, 1, 10001, 0, 0})
    '    'LegacyScripting.RunOneLine("SetBossGauge 6740, 1, 10001")
    '    'Funcs.SetUnknownNpcName("Lord's Blade Ciaran")
    'End Sub
    'Public Shared Sub ScenarioTripleSanctuaryGuardian()
    '    'Data.Scripts("scenariotriplesanctuaryguardian").Execute()
    'End Sub
    'Public Shared Sub ScenarioPinwheelDefense()
    '    'Data.Scripts("scenariopinwheeldefense").Execute()
    'End Sub

    '''' <summary>
    '''' TESTING STUFF AT END OF BOSS RUSH
    '''' </summary>
    'Public Shared Sub BeginTestBossRush()

    '    'Dim debug_skipToGwyn = True
    '    'Dim exterminate = True

    '    'Hook.UpdateHook()

    '    'Dim msg As String

    '    'LegacyScripting.RunOneLine("ShowHUD False")

    '    'Funcs.SetGenDialog(Hook.GenDiagResponse, Hook.GenDiagVal, "Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
    '    'If Not Hook.GenDiagResponse = 1 Then
    '    '    Funcs.SetGenDialog(Hook.GenDiagResponse, Hook.GenDiagVal, "So much shame...", 2, "I know", "I don't care")
    '    '    LegacyScripting.RunOneLine("ShowHUD 1")
    '    '    WInt32(RInt32(&H13786D0) + &H154, -1)
    '    '    WInt32(RInt32(&H13786D0) + &H158, -1)
    '    '    Return
    '    'End If
    '    'If Hook.GenDiagVal > 6 Then Hook.GenDiagVal = 6
    '    'Funcs.SetClearCount(Hook.GenDiagVal)

    '    'msg = "Welcome to the Boss Rush." & Environment.NewLine
    '    'msg = msg & "Saving has been disabled." & Environment.NewLine

    '    'For i = 10 To 1 Step -1
    '    '    Funcs.SetBriefingMsg(msg & i)
    '    '    LegacyScripting.RunOneLine("Wait 1000")
    '    'Next

    '    'Funcs.SetBriefingMsg("Begin")

    '    'LegacyScripting.RunOneLine("CroseBriefingMsg")
    '    'LegacyScripting.RunOneLine("Wait 1000")

    '    'Hook.rushTimer = New Thread(AddressOf BeginRushTimer)
    '    'Hook.rushTimer.IsBackground = True

    '    'Hook.rushTimer.Start()
    '    'Hook.rushMode = True
    '    'Hook.rushName = "Normal"

    '    'If exterminate Then LegacyScripting.RunOneLine("PlayerExterminate 1")

    '    'If Not debug_skipToGwyn Then
    '    '    BossAsylumDemon()
    '    '    BossTaurusDemon()
    '    '    BossBellGargoyles()
    '    '    BossCapraDemon()
    '    '    BossGapingDragon()
    '    '    BossMoonlightButterfly()
    '    '    BossGreatGreyWolfSif()
    '    '    BossChaosWitchQuelaag()
    '    '    BossStrayDemon()
    '    '    BossIronGolem()
    '    '    BossOrnsteinAndSmough()
    '    '    BossPinwheel()
    '    '    BossGravelordNito()
    '    '    BossSanctuaryGuardian()
    '    '    BossKnightArtorias()
    '    '    BossManusFatherOfTheAbyss()
    '    '    BossCeaselessDischarge()
    '    '    BossDemonFiresage()
    '    '    BossCentipedeDemon()
    '    '    BossBlackDragonKalameet()
    '    '    BossSeathTheScaleless()
    '    '    BossFourKings()
    '    '    BossCrossbreedPriscilla()
    '    '    BossDarkSunGwyndolin()
    '    'End If
    '    'BossGwynLordOfCinder()

    '    'If exterminate Then LegacyScripting.RunOneLine("PlayerExterminate 0")

    '    'Hook.UpdateHook()

    '    'LegacyScripting.RunOneLine("ShowHUD False")
    '    'Funcs.SetBriefingMsg("Congratulations." & ChrW(&HA) &
    '    '               Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(&HA) &
    '    '               GetNgPlusText(Hook.GameStats.ClearCount.ValueInt) & ChrW(&HA) &
    '    '               "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt & ChrW(&HA) & ChrW(&HA) &
    '    '               "Saving is still disabled." & ChrW(&HA) & "Quit to main menu now to preserve character data.")
    '    'LegacyScripting.RunOneLine("Wait 10000")
    '    'Hook.rushTimer.Abort()
    'End Sub

    'Public Shared Sub BeginBossRush()

    '    'Hook.UpdateHook()

    '    'Dim msg As String

    '    'LegacyScripting.RunOneLine("ShowHUD False")

    '    'Funcs.SetGenDialog(Hook.GenDiagResponse, Hook.GenDiagVal, "Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
    '    'If Not Hook.GenDiagResponse = 1 Then
    '    '    Funcs.SetGenDialog(Hook.GenDiagResponse, Hook.GenDiagVal, "So much shame...", 2, "I know", "I don't care")
    '    '    LegacyScripting.RunOneLine("ShowHUD 1")
    '    '    WInt32(RInt32(&H13786D0) + &H154, -1)
    '    '    WInt32(RInt32(&H13786D0) + &H158, -1)
    '    '    Return
    '    'End If
    '    'If Hook.GenDiagVal > 6 Then Hook.GenDiagVal = 6
    '    'Funcs.SetClearCount(Hook.GenDiagVal)

    '    'msg = "Welcome to the Boss Rush." & Environment.NewLine
    '    'msg = msg & "Saving has been disabled." & Environment.NewLine

    '    'For i = 10 To 1 Step -1
    '    '    Funcs.SetBriefingMsg(msg & i)
    '    '    LegacyScripting.RunOneLine("Wait 1000")
    '    'Next

    '    'Funcs.SetBriefingMsg("Begin")

    '    'LegacyScripting.RunOneLine("CroseBriefingMsg")
    '    'LegacyScripting.RunOneLine("Wait 1000")

    '    'Hook.rushTimer = New Thread(AddressOf BeginRushTimer)
    '    'Hook.rushTimer.IsBackground = True

    '    'Hook.rushTimer.Start()
    '    'Hook.rushMode = True
    '    'Hook.rushName = "Normal"

    '    'BossAsylumDemon()
    '    'BossTaurusDemon()
    '    'BossBellGargoyles()
    '    'BossCapraDemon()
    '    'BossGapingDragon()
    '    'BossMoonlightButterfly()
    '    'BossGreatGreyWolfSif()
    '    'BossChaosWitchQuelaag()
    '    'BossStrayDemon()
    '    'BossIronGolem()
    '    'BossOrnsteinAndSmough()
    '    'BossPinwheel()
    '    'BossGravelordNito()
    '    'BossSanctuaryGuardian()
    '    'BossKnightArtorias()
    '    'BossManusFatherOfTheAbyss()
    '    'BossCeaselessDischarge()
    '    'BossDemonFiresage()
    '    'BossCentipedeDemon()
    '    'BossBlackDragonKalameet()
    '    'BossSeathTheScaleless()
    '    'BossFourKings()
    '    'BossCrossbreedPriscilla()
    '    'BossDarkSunGwyndolin()
    '    'BossGwynLordOfCinder()

    '    'Hook.UpdateHook()

    '    'Funcs.SetBriefingMsg("Congratulations." & ChrW(&HA) &
    '    '               Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(&HA) &
    '    '               "NG: " & Hook.GameStats.ClearCount.ValueInt & ChrW(&HA) &
    '    '               "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt)
    '    'LegacyScripting.RunOneLine("BlackScreen")
    '    'LegacyScripting.RunOneLine("ShowHUD False")
    '    'LegacyScripting.RunOneLine("Wait 10000")
    '    'LegacyScripting.RunOneLine("FadeIn")
    '    'LegacyScripting.RunOneLine("ShowHUD 1")
    '    'LegacyScripting.RunOneLine("CroseBriefingMsg")

    '    'Hook.rushTimer.Abort()
    'End Sub

    ''TODO: Remove BeginReverseBossRush
    'Public Shared Sub BeginReverseBossRush()
    '    'Reverse Boss Order
    '    'Gwyn
    '    'Dark Sun Gwyndolin
    '    'Crossbreed Priscilla
    '    'Four Kings
    '    'Seath
    '    'Black Dragon Kalameet
    '    'Centipede Demon
    '    'Demon Firesage
    '    'Ceaseless Discharge
    '    'Manus
    '    'Knight Artorias
    '    'Sanctuary Guardian
    '    'Gravelord Nito
    '    'Pinwheel
    '    'Ornstein And Smough
    '    'Iron Golem
    '    'Stray Demon
    '    'Chaos Witch Quelaag
    '    'Sif
    '    'Moonlight Butterfly
    '    'Gaping Dragon
    '    'Capra Demon
    '    'Bell Gargoyles
    '    'Taurus Demon
    '    'Asylum Demon
    '    'Dim msg As String

    '    'LegacyScripting.RunOneLine("ShowHUD False")

    '    'Funcs.SetGenDialog(Hook.GenDiagResponse, Hook.GenDiagVal, "Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
    '    'If Not Hook.GenDiagResponse = 1 Then
    '    '    Funcs.SetGenDialog(Hook.GenDiagResponse, Hook.GenDiagVal, "So much shame...", 2, "I know", "I don't care")
    '    '    LegacyScripting.RunOneLine("ShowHUD 1")
    '    '    WInt32(RInt32(&H13786D0) + &H154, -1)
    '    '    WInt32(RInt32(&H13786D0) + &H158, -1)
    '    '    Return
    '    'End If
    '    'If Hook.GenDiagVal > 6 Then Hook.GenDiagVal = 6
    '    'Funcs.SetClearCount(Hook.GenDiagVal)

    '    'msg = "Welcome to the Reverse Boss Rush." & Environment.NewLine
    '    'msg = msg & "Saving has been disabled." & Environment.NewLine

    '    'For i = 10 To 1 Step -1
    '    '    Funcs.SetBriefingMsg(msg & i)
    '    '    LegacyScripting.RunOneLine("Wait 1000")
    '    'Next

    '    'Funcs.SetBriefingMsg("Begin")

    '    'LegacyScripting.RunOneLine("CroseBriefingMsg")
    '    'LegacyScripting.RunOneLine("Wait 1000")

    '    'Hook.rushTimer = New Thread(AddressOf BeginRushTimer)
    '    'Hook.rushTimer.IsBackground = True

    '    'Hook.rushTimer.Start()
    '    'Hook.rushMode = True
    '    'Hook.rushName = "Reverse"

    '    'BossGwynLordOfCinder()
    '    'BossDarkSunGwyndolin()
    '    'BossCrossbreedPriscilla()
    '    'BossFourKings()
    '    'BossSeathTheScaleless()
    '    'BossBlackDragonKalameet()
    '    'BossCentipedeDemon()
    '    'BossDemonFiresage()
    '    'BossCeaselessDischarge()
    '    'BossManusFatherOfTheAbyss()
    '    'BossKnightArtorias()
    '    'BossSanctuaryGuardian()
    '    'BossGravelordNito()
    '    'BossPinwheel()
    '    'BossOrnsteinAndSmough()
    '    'BossIronGolem()
    '    'BossStrayDemon()
    '    'BossChaosWitchQuelaag()
    '    'BossGreatGreyWolfSif()
    '    'BossMoonlightButterfly()
    '    'BossGapingDragon()
    '    'BossCapraDemon()
    '    'BossBellGargoyles()
    '    'BossTaurusDemon()
    '    'BossAsylumDemon()

    '    'Hook.UpdateHook()

    '    'Funcs.SetBriefingMsg("Congratulations." & ChrW(&HA) &
    '    '               Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(&HA) &
    '    '               "NG: " & Hook.GameStats.ClearCount.ValueInt & ChrW(&HA) &
    '    '               "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt)
    '    'LegacyScripting.RunOneLine("BlackScreen")
    '    'LegacyScripting.RunOneLine("ShowHUD False")
    '    'LegacyScripting.RunOneLine("Wait 10000")
    '    'LegacyScripting.RunOneLine("FadeIn")
    '    'LegacyScripting.RunOneLine("ShowHUD 1")
    '    'LegacyScripting.RunOneLine("CroseBriefingMsg")

    '    'Hook.rushTimer.Abort()

    'End Sub

#End Region

End Class