Imports System.Threading
Imports DaS.ScriptLib.Game.Data.Structures
Imports DaS.ScriptLib.Game.Mem
Imports DaS.ScriptLib.Injection
Imports DaS.ScriptLib.Lua.Structures

Namespace Lua

    Public Class Funcs
        Private Shared ReadOnly Property checkIfLoadingScreen_PrevFrameInGameTime

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetNgPlusText(ngLevel As String) As String
            If ngLevel = 0 Then
                Return "NG"
            ElseIf ngLevel = 1 Then
                Return "NG+"
            Else
                Return "NG+" & ngLevel
            End If
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub MsgBoxOK(text As String)
            SetGenDialog(text, 1, "OK")
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub MsgBoxBtn(text As String, btnText As String)
            MsgBoxChoice(text, btnText, "")
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function MsgBoxChoice(text As String, choice1 As String, choice2 As String) As Integer
            Return SetGenDialog(text, 2, choice1, choice2).Response
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
                WFloat(charmapdataptr + &HE4, Player.Heading.Value)
            End If

            WBytes(charmapdataptr + &HC8, {1})
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub BlackScreen()
            Dim tmpptr As Integer
            tmpptr = RUInt32(&H1378520)
            tmpptr = RUInt32(tmpptr + &H10)

            WBytes(tmpptr + &H26D, {1})

            WFloat(tmpptr + &H270, 0)
            WFloat(tmpptr + &H274, 0)
            WFloat(tmpptr + &H278, 0)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub CamFocusEntity(entityptr As Integer)
            Dim camPtr As Integer = RInt32(&H137D648) + &HEC

            WInt32(camPtr, entityptr)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub ClearPlayTime()
            Dim tmpPtr As Integer = RIntPtr(&H1378700)
            WInt32(tmpPtr + &H68, 0)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub ControlEntity(entityPtr As Integer, state As Boolean)
            entityPtr = RInt32(entityPtr + &H28)

            Dim ctrlptr As Integer = RInt32(&H137DC70)
            ctrlptr = RInt32(ctrlptr + 4)
            ctrlptr = RInt32(ctrlptr)
            ctrlptr = RInt32(ctrlptr + &H28)
            ctrlptr = RInt32(ctrlptr + &H54)

            WInt32(entityPtr + &H244, ctrlptr * (If(state, &HFF, 0) And 1))

        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub DisableAI(ByVal state As Boolean)
            WBool(&H13784EE, state)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub PlayerExterminate(ByVal state As Boolean)
            WBool(&H13784D3, state)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub ForceEntityDrawGroup(entityptr As Integer)
            WInt32(entityptr + &H264, -1)
            WInt32(entityptr + &H268, -1)
            WInt32(entityptr + &H26C, -1)
            WInt32(entityptr + &H270, -1)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetClearCount(ByVal clearCount As Integer)
            Dim tmpPtr As Integer
            tmpPtr = RInt32(&H1378700)

            WInt32(tmpPtr + &H3C, clearCount)

        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetSaveEnable(ByVal state As Boolean)
            Dim tmpPtr As Integer
            tmpPtr = RInt32(&H13784A0)

            WBool(tmpPtr + &HB40, state)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetSaveSlot(ByVal slot As Integer)
            WInt32(RInt32(&H13784A0) + &HA70, slot)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetUnknownNpcName(ByVal name As String)
            If name.Length > 21 Then name = Strings.Left(name, 21) 'Prevent runover into code
            WUnicodeStr(&H11A784C, name + ChrW(0))
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityPtrList() As Integer()
            Dim resultList = New List(Of Integer)

            Dim tmpPtr As Integer = 0

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
                entitiesCnt = RInt32(entitiesPtr + &H3C)
                entitiesPtr = RInt32(entitiesPtr + &H40)

                For entityNum = 0 To entitiesCnt - 1
                    entityPtr = RInt32(entitiesPtr + entityNum * &H20)

                    resultList.Add(entityPtr)
                Next entityNum

            Next mapNum

            Return resultList.ToArray()

        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetAllCurrentlyLoadedMsbData(getNewTable As NLua.LuaFunction) As NLua.LuaTable
            Dim tmpPtr As Integer = 0
            Dim tmpCnt As Integer = 0

            Dim mapCount As Integer = 0
            Dim mapPtrs As Integer = 0

            Dim entitiesPtr As Integer = 0
            Dim entitiesCnt As Integer = 0

            Dim entityPtr As Integer = 0

            Dim tableIndexMapName = ""
            Dim tableIndexEntityName = ""

            tmpPtr = RInt32(&H137D644)

            mapPtrs = tmpPtr + &H74
            mapCount = RInt32(tmpPtr + &H70)

            Const maxMapNameLength As Integer = 12 'Length of "mXX_XX_XX_XX"
            Const maxEntityNameLength As Integer = 10 'NEEDS TESTING

            Dim result = DirectCast(getNewTable.Call()(0), NLua.LuaTable)

            For mapNum = 0 To mapCount - 1
                entitiesPtr = RInt32(mapPtrs + 4 * mapNum)

                tableIndexMapName = RUnicodeStr(RInt32(RInt32(entitiesPtr + &H60) + 4), maxMapNameLength)

                'Console.WriteLine("MAP " & tableIndexMapName)

                entitiesCnt = RInt32(entitiesPtr + &H3C)
                entitiesPtr = RInt32(entitiesPtr + &H40)

                result(tableIndexMapName) = DirectCast(getNewTable.Call()(0), NLua.LuaTable)

                For entityNum = 0 To entitiesCnt - 1
                    entityPtr = RInt32(entitiesPtr + entityNum * &H20)

                    tmpPtr = RInt32(entityPtr + &H54)
                    tmpCnt = RInt32(entityPtr + &H58) - 1

                    tmpPtr = RInt32(tmpPtr + &H28) + &H10
                    tmpPtr = RInt32(RInt32(tmpPtr + 4 * tmpCnt))

                    tableIndexEntityName = RAsciiStr(tmpPtr, maxEntityNameLength)

                    'Console.WriteLine("ENTITY " & tableIndexEntityName)

                    Dim thisEntityPtr As Integer = entityPtr

                    result(tableIndexMapName & "." & tableIndexEntityName) = New Entity(Function() thisEntityPtr)
                Next entityNum

            Next mapNum

            Return result
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityVec3(entityPtr As Integer) As Vec3
            Return New Vec3(GetEntityPosX(entityPtr), GetEntityPosY(entityPtr), GetEntityPosZ(entityPtr))
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub MoveEntityLaterallyTowardEntity(entityFromPtr As Integer, entityToPtr As Integer, speed As Single)
            MoveEntityLaterally(entityFromPtr, GetAngleBetweenEntities(entityFromPtr, entityToPtr), speed)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetAngleBetweenEntities(entityPtrA As Integer, entityPtrB As Integer) As Single
            Dim x1 = GetEntityPosX(entityPtrA)
            Dim z1 = GetEntityPosZ(entityPtrA)

            Dim x2 = GetEntityPosX(entityPtrB)
            Dim z2 = GetEntityPosZ(entityPtrB)

            Return Math.Atan2(z2 - z1, x2 - x1) 'TODO: Check my trig cuz I did this at 6:42 AM
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetDistanceSqrdBetweenEntities(entityPtrA As Integer, entityPtrB As Integer) As Single
            Dim x1 = GetEntityPosX(entityPtrA)
            Dim y1 = GetEntityPosY(entityPtrA)
            Dim z1 = GetEntityPosZ(entityPtrA)

            Dim x2 = GetEntityPosX(entityPtrB)
            Dim y2 = GetEntityPosY(entityPtrB)
            Dim z2 = GetEntityPosZ(entityPtrB)

            Return Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2) + Math.Pow(z1 - z2, 2)
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetDistanceBetweenEntities(entityPtrA As Integer, entityPtrB As Integer) As Single
            Return Math.Sqrt(GetDistanceSqrdBetweenEntities(entityPtrA, entityPtrB))
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub MoveEntityLaterally(entityPtr As Integer, angle As Single, speed As Single)
            MoveEntityAtSpeed(entityPtr, Math.Cos(angle) * speed, 0, Math.Sin(angle) * speed, 0)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub MoveEntityAtSpeed(entityPtr As Integer, speedX As Single, speedY As Single, speedZ As Single, Optional speedRot As Single = 0)

            SetEntityPosX(entityPtr, GetEntityPosX(entityPtr) + speedX)
            SetEntityPosY(entityPtr, GetEntityPosY(entityPtr) + speedY)
            SetEntityPosZ(entityPtr, GetEntityPosZ(entityPtr) + speedZ)
            SetEntityRotation(entityPtr, GetEntityRotation(entityPtr) + speedRot)

        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityPosX(entityPtr As Integer) As Single
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            Return RFloat(entityPosPtr + &H10)
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityPosY(entityPtr As Integer) As Single
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            Return RFloat(entityPosPtr + &H14)
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityPosZ(entityPtr As Integer) As Single
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            Return RFloat(entityPosPtr + &H18)
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityRotation(entityPtr As Integer) As Single
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            Return CType(RFloat(entityPosPtr + &H4) / Math.PI * 180, Single) + 180
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetEntityPosX(entityPtr As Integer, posX As Single)
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            WFloat(entityPosPtr + &H10, posX)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetEntityPosY(entityPtr As Integer, posY As Single)
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            WFloat(entityPosPtr + &H14, posY)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetEntityPosZ(entityPtr As Integer, posZ As Single)
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            WFloat(entityPosPtr + &H18, posZ)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetEntityRotation(entityPtr As Integer, angle As Single)
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            WFloat(entityPosPtr + &H4, CType(angle * Math.PI / 180, Single) - CType(Math.PI, Single))
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetEntityCoordsDirectly(entityPtr As Integer, posX As Single, posY As Single, posZ As Single, angle As Single?)
            Dim entityPosPtr = RInt32(entityPtr + &H28)
            entityPosPtr = RInt32(entityPosPtr + &H1C)
            WFloat(entityPosPtr + &H10, posX)
            WFloat(entityPosPtr + &H14, posY)
            WFloat(entityPosPtr + &H18, posZ)
            WFloat(entityPosPtr + &H4, If(angle.HasValue, CType(angle * Math.PI / 180 - Math.PI, Single), 0))
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetInGameTimeInMs() As Integer
            Return RInt32(RInt32(&H1378700) + &H68)
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetEntityLoc(entityPtr As Integer, location As Loc)
            WarpEntity_Coords(entityPtr, location.Pos.X, location.Pos.Y, location.Pos.Z, location.Rot.HeadingValue)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub PlayerHide(ByVal state As Boolean)
            WBool(&H13784E7, state)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub ShowHUD(ByVal state As Boolean)
            Dim tmpptr As UInteger
            tmpptr = RUInt32(&H1378700)
            tmpptr = RUInt32(tmpptr + &H2C)

            WBool(New IntPtr(tmpptr + &HD), state)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub WarpEntity_Player(entityptr As Integer)
            Dim playerptr As Integer = LuaInterface.E("GetEntityPtr(10000)")
            WarpEntity_Entity(entityptr, playerptr)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub WarpPlayer_Entity(entityptr As Integer)
            Dim playerptr As Integer = LuaInterface.E("GetEntityPtr(10000)")
            WarpEntity_Entity(playerptr, entityptr)
        End Sub
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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

            Const maxMapNameLength As Integer = 12 'Length of "mXX_XX_XX_XX"
            Const maxEntityNameLength As Integer = 10 'NEEDS TESTING

            For mapNum = 0 To mapCount - 1
                entitiesPtr = RInt32(mapPtrs + 4 * mapNum)

                tmpStr = RUnicodeStr(RInt32(RInt32(entitiesPtr + &H60) + 4), maxMapNameLength)

                If tmpStr = mapName Then
                    entitiesCnt = RInt32(entitiesPtr + &H3C)
                    entitiesPtr = RInt32(entitiesPtr + &H40)

                    For entityNum = 0 To entitiesCnt - 1
                        entityPtr = RInt32(entitiesPtr + entityNum * &H20)

                        tmpPtr = RInt32(entityPtr + &H54)
                        tmpCnt = RInt32(entityPtr + &H58) - 1

                        tmpPtr = RInt32(tmpPtr + &H28) + &H10
                        tmpPtr = RInt32(RInt32(tmpPtr + 4 * tmpCnt))

                        tmpStr = RAsciiStr(tmpPtr, maxEntityNameLength)

                        If tmpStr = entName Then
                            Return entityPtr
                        End If
                    Next entityNum
                End If
            Next mapNum
            Return 0
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetBriefingMsg(ByVal str As String)
            Dim tmpptr As Integer
            tmpptr = RInt32(&H13785DC)
            tmpptr = RInt32(tmpptr + &H7C)

            WUnicodeStr(tmpptr + &H3B7A, str + ChrW(0))
            LuaInterface.DoString("RequestOpenBriefingMsg(int(10010721), 1)") 'TODO: Remove the int() after implementing RequestOpenBriefingMsg
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
        <NLua.LuaGlobal(Description:="Opens an ingame dialog box.")> 'TODO: Description
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

            'EVERY TIME I TRIED RETURNING A LUA TABLE ALL THE VALUES IN IT WERE NIL BECAUSE FUCK ME
            Return New GenDiagResult(genDiagResponse, genDiagVal)
        End Function
        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
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

            bytes2 = BitConverter.GetBytes(Convert.ToInt32(ScriptLibResources.clsItemCatsIDs(cat)))
            Array.Copy(bytes2, 0, bytes, bytcat, bytes2.Length)

            Dim tmpCat As Integer
            tmpCat = Convert.ToInt32(ScriptLibResources.clsItemCatsIDs(cat) / &H10000000)
            If tmpCat = 4 Then tmpCat = 3

            bytes2 = BitConverter.GetBytes(Convert.ToInt32(ScriptLibResources.cllItemCatsIDs(tmpCat)(item)))
            Array.Copy(bytes2, 0, bytes, bytitem, bytes2.Length)

            bytes2 = BitConverter.GetBytes(Convert.ToInt32(num))
            Array.Copy(bytes2, 0, bytes, bytcount, bytes2.Length)

            bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H13786D0))
            Array.Copy(bytes2, 0, bytes, bytptr1, bytes2.Length)

            bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H137D6BC))
            Array.Copy(bytes2, 0, bytes, bytptr2, bytes2.Length)

            bytes2 = BitConverter.GetBytes(Convert.ToInt32(0 - ((Injected.ItemDropPtr.GetHandle() + &H3C) - (&HDC8C60))))
            Array.Copy(bytes2, 0, bytes, bytjmp, bytes2.Length)

            Rtn = Kernel.WriteProcessMemory_SAFE(DARKSOULS.GetHandle(), Injected.ItemDropPtr.GetHandle(), bytes, TargetBufferSize, 0)
            'MsgBox(Hex(dropPtr))
            Kernel.CreateRemoteThread(DARKSOULS.GetHandle(), 0, 0, Injected.ItemDropPtr.GetHandle(), 0, 0, 0)

            Thread.Sleep(5)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetKeyGuideText(text As String)
            WInt32(Ptr.MenuPtr.Value + &H158, RInt32(Ptr.MenuPtr.Value + &H1C))
            WUnicodeStr(&H11A7770, text)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetLineHelpText(text As String)
            WInt32(Ptr.MenuPtr.Value + &H154, RInt32(Ptr.MenuPtr.Value + &H1C))
            WUnicodeStr(&H11A7758, text)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetKeyGuideTextPos(x As Single, y As Single)
            WFloat(Ptr.KeyPtr.Value + &H78, x)
            WFloat(Ptr.KeyPtr.Value + &H7C, y)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetLineHelpTextPos(x As Single, y As Single)
            WFloat(Ptr.LinePtr.Value + &H78, x)
            WFloat(Ptr.LinePtr.Value + &H7C, y)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetKeyGuideTextClear()
            WInt32(Ptr.MenuPtr.Value + &H158, -1)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub SetLineHelpTextClear()
            WInt32(Ptr.MenuPtr.Value + &H154, -1)
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Sub ForcePlayerStableFootPos()
            Player.StablePosX.Value = Player.PosX.Value
            Player.StablePosY.Value = Player.PosY.Value
            Player.StablePosZ.Value = Player.PosZ.Value
        End Sub

        <NLua.LuaGlobal(Description:="?Description?")> 'TODO: Description
        Public Shared Function GetEntityPtr(entityId As Integer) As Integer
            Dim reg As New Dictionary(Of String, Object)
            reg.Add("EAX", entityId)
            Return LuaInterface.Instance.CallIngameFuncREG(FuncReturnType.INT, &HD6C360, reg, entityId)
        End Function
    End Class

End Namespace