Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class Funcs

    Private Shared Sub BeginRushTimer()

        Dim msg As String

        WFloat(Hook.lineptr + &H78, 1100)
        WFloat(Hook.lineptr + &H7C, 675)

        WFloat(Hook.keyptr + &H78, 600)
        WFloat(Hook.keyptr + &H7C, 605)

        'Clear TrueDeaths
        Hook.GameStats.TrueDeathCount.ValueInt = 0
        Hook.GameStats.TotalPlayTime.ValueInt = 0

        Do
            WInt32(Hook.menuptr + &H154, RInt32(Hook.menuptr + &H1C)) 'LineHelp
            WInt32(Hook.menuptr + &H158, RInt32(Hook.menuptr + &H1C)) 'KeyGuide
            msg = GetNgPlusText(Hook.GameStats.ClearCount.ValueInt) & " - "
            msg = msg & Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(0)
            WUniStr(&H11A7770, msg)
            msg = "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt & ChrW(0)
            WUniStr(&H11A7758, msg) 'LineHelp
            Thread.Sleep(33)
        Loop
    End Sub

    Private Shared Property rushName As String
        Get
            Return Hook.rushName
        End Get
        Set(value As String)
            Hook.rushName = value
        End Set
    End Property

    Private Shared Property rushMode As Boolean
        Get
            Return Hook.rushMode
        End Get
        Set(value As Boolean)
            Hook.rushMode = value
        End Set
    End Property

    Public Shared Sub bossasylum()

        Dim bossDead As Boolean = False
        Dim firstTry As Boolean = True

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 16, False") 'Boss Death Flag
            Script.RunOneLine("SetEventFlag 11810000, False") 'Tutorial Complete Flag
            Script.RunOneLine("SetEventFlag 11815395, True") 'Boss at lower position

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1810998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")

            Script.RunOneLine("Wait 500")
            Script.RunOneLine("Warp_Coords 3.15, 198.15, -6.0, 180")
            Script.RunOneLine("CamReset 10000, 1")
            Script.RunOneLine("SetEventFlag 11815390, True")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")

            If firstTry And rushName = "Normal" Then
                Script.RunOneLine("ClearPlayTime")
                firstTry = False
            End If

            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H8000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossbedofchaos()


        Script.RunOneLine("SetEventFlag 10, False") 'Boss 

        Script.RunOneLine("SetEventFlag 11410000, False")
        Script.RunOneLine("SetEventFlag 11410200, False") 'Center Platform flag
        Script.RunOneLine("SetEventFlag 11410291, False") 'Arm flag
        Script.RunOneLine("SetEventFlag 11410292, False") 'Arm flag

        'warp before fog gate to set last solid position

        Script.RunOneLine("PlayerHide 1")
        Script.RunOneLine("ShowHUD False")
        Script.RunOneLine("FadeOut")
        Script.RunOneLine("SetHp 10000, 1.0")

        Script.RunOneLine("WarpNextStage_Bonfire 1410980")

        Script.RunOneLine("Wait 1000")

        Script.RunOneLine("WaitForLoad")
        Script.RunOneLine("BlackScreen")
        Script.RunOneLine("PlayerHide 1")
        Script.RunOneLine("SetDisableGravity 10000, 1")

        Script.RunOneLine("Wait 500")
        Script.RunOneLine("Warp 10000, 1412998")
        Script.RunOneLine("Wait 250")
        Script.RunOneLine("Warp 10000, 1412997")

        Script.RunOneLine("Wait 1250")
        Script.RunOneLine("FadeIn")
        Script.RunOneLine("ShowHUD 1")
        Script.RunOneLine("PlayerHide 0")
        Script.RunOneLine("SetDisableGravity 10000, 0")

    End Sub
    Public Shared Sub bossbellgargoyles()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")
            Script.RunOneLine("SetEventFlag 3, False") 'Boss Death Flag
            Script.RunOneLine("SetEventFlag 11010000, False") 'Boss Cinematic Viewed Flag

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")
            Script.RunOneLine("WarpNextStage_Bonfire 1010998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")

            Script.RunOneLine("Wait 500")

            Script.RunOneLine("SetEventFlag 11015390, True") 'Boss Fog Used
            Script.RunOneLine("SetEventFlag 11015393, True") 'Boss Area Entered
            Script.RunOneLine("Wait 250")

            'facing 0 degrees
            Script.RunOneLine("Warp_Coords 10.8, 48.92, 87.26")
            Script.RunOneLine("CamReset 10000, 1")


            Script.RunOneLine("Wait 1250")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")

            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H10000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If
        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossblackdragonkalameet()
        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")


            Script.RunOneLine("SetEventFlag 11210004, False")

            Script.RunOneLine("SetEventFlag 121, False")
            Script.RunOneLine("SetEventFlag 11210539, True")
            Script.RunOneLine("SetEventFlag 11210535, True")
            Script.RunOneLine("SetEventFlag 11210067, False")
            Script.RunOneLine("SetEventFlag 11210066, False")
            Script.RunOneLine("SetEventFlag 11210056, True")

            Script.RunOneLine("SetEventFlag 1821, True")
            Script.RunOneLine("SetEventFlag 11210592, True")


            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1210998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")

            Script.RunOneLine("Wait 500")
            Script.RunOneLine("Warp_Coords 876.04, -344.73, 749.75, 240")
            Script.RunOneLine("CamReset 10000, 1")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H2300, &H8000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosscaprademon()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11010902, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1010998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")
            Script.RunOneLine("Wait 500")

            Script.RunOneLine("Warp_Coords -73.17, -43.56, -15.17, 321")
            Script.RunOneLine("CamReset 10000, 1")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")


            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&HF70, &H2000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossceaselessdischarge()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11410800, False")
            Script.RunOneLine("SetEventFlag 11410801, False")
            Script.RunOneLine("SetEventFlag 11410900, False") 'Boss death flag
            Script.RunOneLine("SetEventFlag 51410180, True") 'Corpse Loot reset


            Script.RunOneLine("SetEventFlag 11415379, False")
            Script.RunOneLine("SetEventFlag 11415385, True")
            Script.RunOneLine("SetEventFlag 11415378, True")
            Script.RunOneLine("SetEventFlag 11415373, True")
            Script.RunOneLine("SetEventFlag 11415372, True")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1410998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")

            Script.RunOneLine("Wait 500")

            Script.RunOneLine("Warp_Coords 250.53, -283.15, 72.1")
            Script.RunOneLine("Wait 250")

            Script.RunOneLine("Warp_Coords 402.45, -278.15, 15.5, 30")
            Script.RunOneLine("CamReset 10000, 1")




            Script.RunOneLine("Wait 1250")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H3C70, &H8000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosscentipededemon()
        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")
            Script.RunOneLine("SetEventFlag 11410002, False") 'Cinematic flag
            Script.RunOneLine("SetEventFlag 11410901, False")


            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1410998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")

            Script.RunOneLine("Warp 10000, 1412896")
            Script.RunOneLine("SetEventFlag 11415380, True")
            Script.RunOneLine("SetEventFlag 11415383, True")
            Script.RunOneLine("SetEventFlag 11415382, True")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H3C70, &H4000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosschaoswitchquelaag()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 9, False")
            Script.RunOneLine("SetEventFlag 11400000, False") 'Cinematic flag

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1400980")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 17.2, -236.9, 113.6, 75")
            Script.RunOneLine("CamReset 10000, 1")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H400000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosscrossbreedpriscilla()


        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")
            Script.RunOneLine("SetEventFlag 4, False") 'Boss Death flag
            Script.RunOneLine("SetEventFlag 1691, True") 'Priscilla Hostile flag
            Script.RunOneLine("SetEventFlag 1692, True") 'Priscilla Dead flag

            Script.RunOneLine("SetEventFlag 11100531, False") 'Boss Disabled flag

            Script.RunOneLine("SetEventFlag 11100000, False") 'Previous victory flag




            'StandardTransition(1102961, 1102997)

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1102961")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords -22.72, 60.55, 711.86")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H8000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossdarksungwyndolin()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11510900, False") 'Boss Death Flag
            Script.RunOneLine("SetEventFlag 11510523, False") 'Boss Disabled Flag

            'StandardTransition(1510982, 1512896)
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1510982")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 435.1, 60.2, 255.0")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H4670, &H8000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossdemonfiresage()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11410410, False")
            'StandardTransition(1410998, 1412416)

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")


            Script.RunOneLine("WarpNextStage_Bonfire 1410998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 148.04, -341.04, 95.57")


            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H3C30, &H20)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossfourkings()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 13, False")
            Script.RunOneLine("SetEventFlag 1677, True") 'Kaathe Angry/gone

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1600999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            'ScriptEnvironment.Run("Warp_Coords 82.24, -163.2, 0.29")
            'Facing 185.98
            Script.RunOneLine("Warp_Coords 85.18, -191.99, 4.95, 185")
            Script.RunOneLine("CamReset 10000, 1")

            Script.RunOneLine("Wait 1500")
            Funcs.dropitem("Rings", "Covenant of Artorias", 1) 'TODO: dropitem
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")

            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H40000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    'ScriptEnvironment.Run("SetTextEffect 16")
                    Script.RunOneLine("WaitTillLoad")
                    Script.RunOneLine("WaitForLoad")
                End If
            End If
        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossgapingdragon()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 2, False") 'Boss Death Flag
            Script.RunOneLine("SetEventFlag 11000853, True") 'Channeler Death Flag
            'StandardTransition(1000999, 1002997)

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1000999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("Wait 500")
            Script.RunOneLine("SetEventFlag 11005390, True")
            Script.RunOneLine("SetEventFlag 11005392, True")
            Script.RunOneLine("SetEventFlag 11005393, True")
            Script.RunOneLine("SetEventFlag 11005394, True")
            Script.RunOneLine("SetEventFlag 11005397, True")
            Script.RunOneLine("SetEventFlag 11000000, False")


            Script.RunOneLine("Warp 10000, 1002997")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")

            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H20000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")

    End Sub
    Public Shared Sub bossgravelordnito()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")


            Script.RunOneLine("SetEventFlag 7, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")


            Script.RunOneLine("WarpNextStage_Bonfire 1310998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")
            Script.RunOneLine("Wait 500")

            'ScriptEnvironment.Run("Warp 10000, 1312110)
            Script.RunOneLine("Warp_Coords -126.84, -265.12, -30.78")
            Script.RunOneLine("SetEventFlag 11315390, True")
            Script.RunOneLine("SetEventFlag 11315393, True")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H1000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")

    End Sub
    Public Shared Sub bossgwyn()

        Dim bossDead As Boolean = False
        Dim firstTry As Boolean = True

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 15, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1800999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 418.15, -115.92, 169.58")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")

            If firstTry And rushName = "Reverse" Then
                Script.RunOneLine("ClearPlayTime")
                firstTry = False
            End If

            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H10000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossirongolem()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11, False") 'Boss Death Flag
            Script.RunOneLine("SetEventFlag 11500865, True") 'Bomb-Tossing Giant Death Flag

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1500999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 85.5, 82, 255.1")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H100000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossknightartorias()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")


            Script.RunOneLine("SetEventFlag 11210001, False")
            Script.RunOneLine("SetEventFlag 11210513, False") 'Ciaran Present

            'Non-standard due to co-ords warp

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")


            Script.RunOneLine("WarpNextStage_Bonfire 1210998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")

            Script.RunOneLine("Wait 500")
            'facing 75.8 degrees
            Script.RunOneLine("Warp_Coords 1034.11, -330.0, 810.68")


            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H2300, &H40000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossmanus()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11210002, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")


            Script.RunOneLine("WarpNextStage_Bonfire 1210982")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 857.53, -576.69, 873.38")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H2300, &H20000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossmoonlightbutterfly()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11200900, False")
            Script.RunOneLine("SetEventFlag 11205383, False")

            'timing of warp/flags matters

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1200999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")




            Script.RunOneLine("Wait 500")
            Script.RunOneLine("Warp_Coords 181.39, 7.53, 29.01")
            Thread.Sleep(4000)
            Script.RunOneLine("SetEventFlag 11205383, True")

            Script.RunOneLine("Warp_Coords 178.82, 8.12, 30.77")



            Thread.Sleep(2000)
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")

            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H1E70, &H8000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")

    End Sub
    Public Shared Sub bossoands()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 12, False")



            'Non-standard due to co-ords warp

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1510998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")

            Script.RunOneLine("Wait 500")
            'facing 90 degrees
            Script.RunOneLine("Warp_Coords 539.9, 142.6, 254.79")


            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H80000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosspinwheel()
        Dim bossDead As Boolean = False

        Do
            'Pinwheel Entity ID = 1300800
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 6, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")


            Script.RunOneLine("WarpNextStage_Bonfire 1300999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 46.0, -165.8, 152.02, 180")
            Script.RunOneLine("CamReset 10000, 1")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H2000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosssanctuaryguardian()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11210000, False")
            Script.RunOneLine("SetEventFlag 11210001, False")


            'Non-standard due to co-ords warp

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")
            Script.RunOneLine("SetHp 10000, 1.0")

            Script.RunOneLine("WarpNextStage_Bonfire 1210998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")


            Script.RunOneLine("Wait 500")
            'facing = 45 deg
            Script.RunOneLine("Warp_Coords 931.82, -318.63, 472.45")


            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H2300, &H80000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossseath()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 14, False")
            Script.RunOneLine("SetEventFlag 11700000, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")


            Script.RunOneLine("WarpNextStage_Bonfire 1700999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 109, 134.05, 856.48")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H20000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bosssif()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 5, False")
            Script.RunOneLine("SetEventFlag 11200000, False")
            Script.RunOneLine("SetEventFlag 11200001, False")
            Script.RunOneLine("SetEventFlag 11200002, False")
            Script.RunOneLine("SetEventFlag 11205392, False")
            Script.RunOneLine("SetEventFlag 11205393, False")
            Script.RunOneLine("SetEventFlag 11205394, False")


            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("SetHp 10000, 1.0")

            Script.RunOneLine("WarpNextStage_Bonfire 1200999")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("SetDisableGravity 10000, 1")
            Script.RunOneLine("Wait 500")
            'ScriptEnvironment.Run("Warp_Coords 274, -19.82, -266.43)
            Script.RunOneLine("Wait 500")
            'ScriptEnvironment.Run("Warp 10000, 1202999)
            Script.RunOneLine("Warp_Coords 254.31, -16.02, -320.32")

            Script.RunOneLine("Wait 1000")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("SetDisableGravity 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(0, &H4000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub
    Public Shared Sub bossstraydemon()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")


            Script.RunOneLine("SetEventFlag 11810000, True")
            Script.RunOneLine("SetEventFlag 11810900, False")


            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1810998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("DisableDamage 10000, 1")

            Script.RunOneLine("Wait 500")

            Script.RunOneLine("Warp 10000, 1812996")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")
            Script.RunOneLine("Wait 1000")
            Script.RunOneLine("DisableDamage 10000, 0")
            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&H5A70, &H8000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")


    End Sub
    Public Shared Sub bosstaurusdemon()

        Dim bossDead As Boolean = False

        Do
            Script.RunOneLine("RequestFullRecover")

            Script.RunOneLine("SetEventFlag 11010901, False")

            Script.RunOneLine("PlayerHide 1")
            Script.RunOneLine("ShowHUD False")
            Script.RunOneLine("FadeOut")

            Script.RunOneLine("WarpNextStage_Bonfire 1010998")

            Script.RunOneLine("Wait 1000")

            Script.RunOneLine("WaitForLoad")
            Script.RunOneLine("BlackScreen")
            Script.RunOneLine("PlayerHide 1")

            Script.RunOneLine("Wait 500")


            Script.RunOneLine("Warp_Coords 49.81, 16.9, -118.87")

            Script.RunOneLine("Wait 1500")
            Script.RunOneLine("FadeIn")
            Script.RunOneLine("ShowHUD 1")
            Script.RunOneLine("PlayerHide 0")

            If rushMode Then
                bossDead = Funcs.waitforbossdeath(&HF70, &H4000000)
                If Not bossDead Then
                    Script.RunOneLine("AddTrueDeathCount")
                    Script.RunOneLine("SetTextEffect 16")
                    Script.RunOneLine("Wait 5000")
                End If
            End If

        Loop While rushMode And Not bossDead
        Script.RunOneLine("Wait 5000")
    End Sub

    Public Shared Sub debug_scenariooandsandoands()
        scenariooandsandoands()
        Script.RunOneLine("SetDisableGravity 10000, 1")
        Script.RunOneLine("SetIgnoreHit 10000, 1")
        Script.RunOneLine("PlayerExterminate 1")
    End Sub

    Public Shared Sub scenariooandsandoands()
        Data.Scripts("scenariooandsandoands").Execute()
    End Sub

    Public Shared Sub scenarioartoriasandciaran()


        Script.RunOneLine("SetEventFlag 11210001, False") 'Artorias Disabled
        Script.RunOneLine("SetEventFlag 11210513, True") 'Ciaran Present


        Script.RunOneLine("SetEventFlag 1863, False") 'Ciaran Hostile
        Script.RunOneLine("SetEventFlag 1864, False") 'Ciaran Dead

        Script.RunOneLine("PlayerHide 1")
        Script.RunOneLine("ShowHUD False")
        Script.RunOneLine("FadeOut")

        Script.RunOneLine("SetHp 10000, 1.0")

        Script.RunOneLine("WarpNextStage_Bonfire 1210998")

        Script.RunOneLine("Wait 1000")

        Script.RunOneLine("WaitForLoad")
        Script.RunOneLine("BlackScreen")


        Script.RunOneLine("PlayerHide 1")
        Script.RunOneLine("SetDisableGravity 10000, 1")

        Script.RunOneLine("Wait 500")
        'facing 75.8 degrees
        Script.RunOneLine("Warp_Coords 1034.11, -330.0, 810.68")


        Script.RunOneLine("Wait 1500")
        Script.RunOneLine("FadeIn")
        Script.RunOneLine("ShowHUD 1")
        Script.RunOneLine("PlayerHide 0")
        Script.RunOneLine("SetDisableGravity 10000, 0")

        Script.RunOneLine("SetEventFlag 1863, True") 'Ciaran Hostile
        'funccall_old("SetBossGauge", {6740, 1, 10001, 0, 0})
        Script.RunOneLine("SetBossGauge 6740, 1, 10001")
        Funcs.setunknownnpcname("Lord's Blade Ciaran")
    End Sub
    Public Shared Sub scenariotriplesanctuaryguardian()
        Data.Scripts("scenariotriplesanctuaryguardian").Execute()
    End Sub
    Public Shared Sub scenariopinwheeldefense()
        Data.Scripts("scenariopinwheeldefense").Execute()
    End Sub

    ''' <summary>
    ''' TESTING STUFF AT END OF BOSS RUSH
    ''' </summary>
    Public Shared Sub begintestbossrush()

        Dim debug_skipToGwyn = True
        Dim exterminate = True

        Hook.UpdateHook()

        Dim msg As String

        Script.RunOneLine("ShowHUD False")

        Funcs.setgendialog(Hook.GenDiagResponse, Hook.GenDiagVal, "Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not Hook.GenDiagResponse = 1 Then
            Funcs.setgendialog(Hook.GenDiagResponse, Hook.GenDiagVal, "So much shame...", 2, "I know", "I don't care")
            Script.RunOneLine("ShowHUD 1")
            WInt32(RInt32(&H13786D0) + &H154, -1)
            WInt32(RInt32(&H13786D0) + &H158, -1)
            Return
        End If
        If Hook.GenDiagVal > 6 Then Hook.GenDiagVal = 6
        Funcs.setclearcount(Hook.GenDiagVal)

        msg = "Welcome to the Boss Rush." & Environment.NewLine
        msg = msg & "Saving has been disabled." & Environment.NewLine


        For i = 10 To 1 Step -1
            Funcs.setbriefingmsg(msg & i)
            Script.RunOneLine("Wait 1000")
        Next


        Funcs.setbriefingmsg("Begin")

        Script.RunOneLine("CroseBriefingMsg")
        Script.RunOneLine("Wait 1000")


        Hook.rushTimer = New Thread(AddressOf BeginRushTimer)
        Hook.rushTimer.IsBackground = True




        Hook.rushTimer.Start()
        Hook.rushMode = True
        Hook.rushName = "Normal"

        If exterminate Then Script.RunOneLine("PlayerExterminate 1")

        If Not debug_skipToGwyn Then
            bossasylum()
            bosstaurusdemon()
            bossbellgargoyles()
            bosscaprademon()
            bossgapingdragon()
            bossmoonlightbutterfly()
            bosssif()
            bosschaoswitchquelaag()
            bossstraydemon()
            bossirongolem()
            bossoands()
            bosspinwheel()
            bossgravelordnito()
            bosssanctuaryguardian()
            bossknightartorias()
            bossmanus()
            bossceaselessdischarge()
            bossdemonfiresage()
            bosscentipededemon()
            bossblackdragonkalameet()
            bossseath()
            bossfourkings()
            bosscrossbreedpriscilla()
            bossdarksungwyndolin()
        End If
        bossgwyn()

        If exterminate Then Script.RunOneLine("PlayerExterminate 0")

        Hook.UpdateHook()


        Script.RunOneLine("ShowHUD False")
        Script.RunOneLine("SetCompletelyNoMove 10000, 1")
        Script.RunOneLine("FadeOut")
        Script.RunOneLine("Wait 1000")
        Funcs.setbriefingmsg("Congratulations." & ChrW(&HA) &
                       Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(&HA) &
                       GetNgPlusText(Hook.GameStats.ClearCount.ValueInt) & ChrW(&HA) &
                       "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt & ChrW(&HA) &
                       "Returning to title screen...")
        Script.RunOneLine("Wait 10000")
        Script.RunOneLine("ReturnMapSelect") 'Return to title screen
        Hook.rushTimer.Abort()
    End Sub

    Public Shared Function GetNgPlusText(ngLevel As String) As String
        If ngLevel = 0 Then
            Return "NG"
        ElseIf ngLevel = 1 Then
            Return "NG+"
        Else
            Return "NG+" & ngLevel
        End If
    End Function

    Public Shared Sub beginbossrush()

        Hook.UpdateHook()

        Dim msg As String

        Script.RunOneLine("ShowHUD False")

        Funcs.setgendialog(Hook.GenDiagResponse, Hook.GenDiagVal, "Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not Hook.GenDiagResponse = 1 Then
            Funcs.setgendialog(Hook.GenDiagResponse, Hook.GenDiagVal, "So much shame...", 2, "I know", "I don't care")
            Script.RunOneLine("ShowHUD 1")
            WInt32(RInt32(&H13786D0) + &H154, -1)
            WInt32(RInt32(&H13786D0) + &H158, -1)
            Return
        End If
        If Hook.GenDiagVal > 6 Then Hook.GenDiagVal = 6
        Funcs.setclearcount(Hook.GenDiagVal)

        msg = "Welcome to the Boss Rush." & Environment.NewLine
        msg = msg & "Saving has been disabled." & Environment.NewLine


        For i = 10 To 1 Step -1
            Funcs.setbriefingmsg(msg & i)
            Script.RunOneLine("Wait 1000")
        Next


        Funcs.setbriefingmsg("Begin")

        Script.RunOneLine("CroseBriefingMsg")
        Script.RunOneLine("Wait 1000")


        Hook.rushTimer = New Thread(AddressOf BeginRushTimer)
        Hook.rushTimer.IsBackground = True




        Hook.rushTimer.Start()
        Hook.rushMode = True
        Hook.rushName = "Normal"

        bossasylum()
        bosstaurusdemon()
        bossbellgargoyles()
        bosscaprademon()
        bossgapingdragon()
        bossmoonlightbutterfly()
        bosssif()
        bosschaoswitchquelaag()
        bossstraydemon()
        bossirongolem()
        bossoands()
        bosspinwheel()
        bossgravelordnito()
        bosssanctuaryguardian()
        bossknightartorias()
        bossmanus()
        bossceaselessdischarge()
        bossdemonfiresage()
        bosscentipededemon()
        bossblackdragonkalameet()
        bossseath()
        bossfourkings()
        bosscrossbreedpriscilla()
        bossdarksungwyndolin()
        bossgwyn()

        Hook.UpdateHook()

        Funcs.setbriefingmsg("Congratulations." & ChrW(&HA) &
                       Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(&HA) &
                       "NG: " & Hook.GameStats.ClearCount.ValueInt & ChrW(&HA) &
                       "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt)
        Script.RunOneLine("BlackScreen")
        Script.RunOneLine("ShowHUD False")
        Script.RunOneLine("Wait 10000")
        Script.RunOneLine("FadeIn")
        Script.RunOneLine("ShowHUD 1")
        Script.RunOneLine("CroseBriefingMsg")

        Hook.rushTimer.Abort()
    End Sub
    Public Shared Sub beginreversebossrush()
        'Reverse Boss Order
        'Gwyn
        'Dark Sun Gwyndolin
        'Crossbreed Priscilla
        'Four Kings
        'Seath
        'Black Dragon Kalameet
        'Centipede Demon
        'Demon Firesage
        'Ceaseless Discharge
        'Manus
        'Knight Artorias
        'Sanctuary Guardian
        'Gravelord Nito
        'Pinwheel
        'Ornstein And Smough
        'Iron Golem
        'Stray Demon
        'Chaos Witch Quelaag
        'Sif
        'Moonlight Butterfly
        'Gaping Dragon
        'Capra Demon
        'Bell Gargoyles
        'Taurus Demon
        'Asylum Demon
        Dim msg As String

        Script.RunOneLine("ShowHUD False")

        Funcs.setgendialog(Hook.GenDiagResponse, Hook.GenDiagVal, "Choose your NG level wisely.\nValues above 6 are ignored.", 3, "Begin", "Wuss Out")
        If Not Hook.GenDiagResponse = 1 Then
            Funcs.setgendialog(Hook.GenDiagResponse, Hook.GenDiagVal, "So much shame...", 2, "I know", "I don't care")
            Script.RunOneLine("ShowHUD 1")
            WInt32(RInt32(&H13786D0) + &H154, -1)
            WInt32(RInt32(&H13786D0) + &H158, -1)
            Return
        End If
        If Hook.GenDiagVal > 6 Then Hook.GenDiagVal = 6
        Funcs.setclearcount(Hook.GenDiagVal)

        msg = "Welcome to the Reverse Boss Rush." & Environment.NewLine
        msg = msg & "Saving has been disabled." & Environment.NewLine


        For i = 10 To 1 Step -1
            Funcs.setbriefingmsg(msg & i)
            Script.RunOneLine("Wait 1000")
        Next


        Funcs.setbriefingmsg("Begin")

        Script.RunOneLine("CroseBriefingMsg")
        Script.RunOneLine("Wait 1000")


        Hook.rushTimer = New Thread(AddressOf BeginRushTimer)
        Hook.rushTimer.IsBackground = True




        Hook.rushTimer.Start()
        Hook.rushMode = True
        Hook.rushName = "Reverse"


        bossgwyn()
        bossdarksungwyndolin()
        bosscrossbreedpriscilla()
        bossfourkings()
        bossseath()
        bossblackdragonkalameet()
        bosscentipededemon()
        bossdemonfiresage()
        bossceaselessdischarge()
        bossmanus()
        bossknightartorias()
        bosssanctuaryguardian()
        bossgravelordnito()
        bosspinwheel()
        bossoands()
        bossirongolem()
        bossstraydemon()
        bosschaoswitchquelaag()
        bosssif()
        bossmoonlightbutterfly()
        bossgapingdragon()
        bosscaprademon()
        bossbellgargoyles()
        bosstaurusdemon()
        bossasylum()

        Hook.UpdateHook()

        Funcs.setbriefingmsg("Congratulations." & ChrW(&HA) &
                       Strings.Left(TimeSpan.FromMilliseconds(Hook.GameStats.TotalPlayTime.ValueInt).ToString, 12) & ChrW(&HA) &
                       "NG: " & Hook.GameStats.ClearCount.ValueInt & ChrW(&HA) &
                       "Deaths: " & Hook.GameStats.TrueDeathCount.ValueInt)
        Script.RunOneLine("BlackScreen")
        Script.RunOneLine("ShowHUD False")
        Script.RunOneLine("Wait 10000")
        Script.RunOneLine("FadeIn")
        Script.RunOneLine("ShowHUD 1")
        Script.RunOneLine("CroseBriefingMsg")

        Hook.rushTimer.Abort()

    End Sub

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
    Public Shared Sub disableai(ByVal state As Byte)
        WBytes(&H13784EE, {state})
    End Sub
    Public Shared Sub playerexterminate(ByVal state As Byte)
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
        Dim playerptr As Integer = Script.RunOneLine("GetEntityPtr 10000")
        Script.RunOneLine("warpentity_entity " & entityptr & ", " & playerptr)
    End Sub
    Public Shared Sub warpplayer_entity(entityptr As Integer)
        Dim playerptr As Integer = Script.RunOneLine("GetEntityPtr 10000")
        Script.RunOneLine("warpentity_entity " & playerptr & ", " & entityptr)
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
        Return Script.RunOneLine("ChrFadeIn " & entityId & ", 0, 0") 'TODO 
        'TODO 
    End Function 'TODO


    Public Shared Sub setbriefingmsg(ByVal str As String)
        Dim tmpptr As Integer
        tmpptr = RInt32(&H13785DC)
        tmpptr = RInt32(tmpptr + &H7C)

        WUniStr(tmpptr + &H3B7A, str + ChrW(0))
        Script.RunOneLine("RequestOpenBriefingMsg 10010721, 1")

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
        Script.RunOneLine("Wait 500")
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

    Public Shared Sub dropitem(ByVal cat As String, item As String, num As Integer)
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

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(Data.clsItemCatsIDs(cat)))
        Array.Copy(bytes2, 0, bytes, bytcat, bytes2.Length)

        Dim tmpCat As Integer
        tmpCat = Convert.ToInt32(Data.clsItemCatsIDs(cat) / &H10000000)
        If tmpCat = 4 Then tmpCat = 3

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(Data.cllItemCatsIDs(tmpCat)(item)))
        Array.Copy(bytes2, 0, bytes, bytitem, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(num))
        Array.Copy(bytes2, 0, bytes, bytcount, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H13786D0))
        Array.Copy(bytes2, 0, bytes, bytptr1, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(&H137D6BC))
        Array.Copy(bytes2, 0, bytes, bytptr2, bytes2.Length)

        bytes2 = BitConverter.GetBytes(Convert.ToInt32(0 - ((Hook.dropPtr + &H3C) - (&HDC8C60))))
        Array.Copy(bytes2, 0, bytes, bytjmp, bytes2.Length)

        Rtn = WriteProcessMemory(_targetProcessHandle, Hook.dropPtr, bytes, TargetBufferSize, 0)
        'MsgBox(Hex(dropPtr))
        CreateRemoteThread(_targetProcessHandle, 0, 0, Hook.dropPtr, 0, 0, 0)

        Thread.Sleep(5)
    End Sub

    Public Shared Function funccall(func As String, Optional param1 As String = "", Optional param2 As String = "", Optional param3 As String = "", Optional param4 As String = "", Optional param5 As String = "") As Integer

        Dim Params() As String = {param1, param2, param3, param4, param5}
        Dim param As IntPtr = Marshal.AllocHGlobal(4)
        Dim intParam As Integer
        Dim floatParam As Single
        Dim a As New asm

        func = func.ToLower

        Dim funcPtr = Hook.funcPtr

        a.pos = funcPtr
        a.AddVar("funcloc", Data.clsFuncLocs(func.ToLower))
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
