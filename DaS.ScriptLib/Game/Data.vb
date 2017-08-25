Public Class Data
    Public Class Boss
        Public Shared ReadOnly AsylumDemon As String = "Asylum Demon"
        Public Shared ReadOnly TaurusDemon As String = "Taurus Demon"
        Public Shared ReadOnly Gargoyles As String = "Bell Gargoyles"
        Public Shared ReadOnly CapraDemon As String = "Capra Demon"
        Public Shared ReadOnly GapingDragon As String = "Gaping Dragon"
        Public Shared ReadOnly MoonlightButterfly As String = "Moonlight Butterfly"
        Public Shared ReadOnly Sif As String = "Great Grey Wolf Sif"
        Public Shared ReadOnly Quelaag As String = "Chaos Witch Quelaag"
        Public Shared ReadOnly StrayDemon As String = "Stray Demon"
        Public Shared ReadOnly IronGolem As String = "Iron Golem"
        Public Shared ReadOnly OrnsteinAndSmough As String = "Ornstein & Smough"
        Public Shared ReadOnly Pinwheel As String = "Pinwheel"
        Public Shared ReadOnly Nito As String = "Gravelord Nito"
        Public Shared ReadOnly SanctuaryGuardian As String = "Sanctuary Guardian"
        Public Shared ReadOnly Artorias As String = "Knight Artorias"
        Public Shared ReadOnly Manus As String = "Manus, Father of the Abyss"
        Public Shared ReadOnly Ceaseless As String = "Ceaseless Discharge"
        Public Shared ReadOnly DemonFiresage As String = "Demon Firesage"
        Public Shared ReadOnly CentipedeDemon As String = "Centipede Demon"
        Public Shared ReadOnly BedOfChaos As String = "Bed of Chaos"
        Public Shared ReadOnly Kalameet As String = "Black Dragon Kalameet"
        Public Shared ReadOnly Seath As String = "Seath the Scaleless"
        Public Shared ReadOnly FourKings As String = "Four Kings"
        Public Shared ReadOnly Priscilla As String = "Crossbreed Priscilla"
        Public Shared ReadOnly Gwyndolin As String = "Dark Sun Gwyndolin"
        Public Shared ReadOnly Gwyn As String = "Gwyn, Lord of Cinder"
    End Class

    Public Class BossEvent
        Public Shared ReadOnly AsylumDemon As Integer = 16
        Public Shared ReadOnly TaurusDemon As Integer = 11010901
        Public Shared ReadOnly Gargoyles As Integer = 3
        Public Shared ReadOnly CapraDemon As Integer = 11010902
        Public Shared ReadOnly GapingDragon As Integer = 2
        Public Shared ReadOnly MoonlightButterfly As Integer = 11200900
        Public Shared ReadOnly Sif As Integer = 5
        Public Shared ReadOnly Quelaag As Integer = 9
        Public Shared ReadOnly StrayDemon As Integer = 11810900 'TODO: CHECK
        Public Shared ReadOnly IronGolem As Integer = 11
        Public Shared ReadOnly OrnsteinAndSmough As Integer = 12
        Public Shared ReadOnly Pinwheel As Integer = 6
        Public Shared ReadOnly Nito As Integer = 7
        Public Shared ReadOnly SanctuaryGuardian As Integer = 11210000
        Public Shared ReadOnly Artorias As Integer = 11210001
        Public Shared ReadOnly Manus As Integer = 11210002
        Public Shared ReadOnly Ceaseless As Integer = 11410900
        Public Shared ReadOnly DemonFiresage As Integer = 11410410
        Public Shared ReadOnly CentipedeDemon As Integer = 11410901
        Public Shared ReadOnly BedOfChaos As Integer = 10
        Public Shared ReadOnly Kalameet As Integer = 11210004
        Public Shared ReadOnly Seath As Integer = 14
        Public Shared ReadOnly FourKings As Integer = 13
        Public Shared ReadOnly Priscilla As Integer = 4
        Public Shared ReadOnly Gwyndolin As Integer = 11510900
        Public Shared ReadOnly Gwyn As Integer = 15
    End Class

    ''' <summary>
    ''' TODO: PUT MORE ANIMATIONS IN HERE
    ''' </summary>
    Public Class PlayerAnim
        Public Shared ReadOnly Idle As Integer = 0
        Public Shared ReadOnly WalkForward As Integer = 200
        Public Shared ReadOnly RunForward As Integer = 500
        Public Shared ReadOnly FogWalk As Integer = 7410
    End Class

    Public Shared ReadOnly PlayerID As Integer = 10000

    Public Shared ReadOnly BossFights As Dictionary(Of String, BossFightInfo)

    Shared Sub New()
        'Note that the 5th and 6th parameters of the constructor are pre- and post- loading screen flags.
        'A negative flag ID indicates the flag should be set to false
        Dim __bossFights As BossFightInfo() = {
            New BossFightInfo(Boss.AsylumDemon) With { 'NO ADJUSTMENTS NEEDED
                .BonfireID = 1810998,
                .PlayerWarp = New EntityLocation(3.157, 198.148, -3.425, 180),
                .AdditionalFlags = {
                    -11810000, 'first time asylum
                    11815395, 'fatass waiting for you...?
                    11815395 'lil door closed...?
                },
                .EventFlag = BossEvent.AsylumDemon
            },
            New BossFightInfo(Boss.TaurusDemon) With { 'NEEDS BONFIRE AND WARP POS. NO FLAGS NEEDED
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.TaurusDemon
            },
            New BossFightInfo(Boss.Gargoyles) With { 'NEEDS WARP POS AND CUTSCENE WATCHED FLAG
                .BonfireID = 1010998,
                .PlayerWarp = New EntityLocation(10.8, 48.92, 87.26),
                .AdditionalFlags = {
                    11010000, 'cutscene watched = true
                    -11015390, 'entered fog = false
                    -11015393 'entered boss area = false
                },
                .EventFlag = BossEvent.Gargoyles
            },
            New BossFightInfo(Boss.CapraDemon) With { 'NEEDS WARP POS
                .BonfireID = 1010998,
                .PlayerWarp = New EntityLocation(-73.17, -43.56, -15.17, 321),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.CapraDemon
            },
            New BossFightInfo(Boss.GapingDragon) With { 'NEEDS BONFIRE, WARP POS, AND CHANNELER FUCK OFF FLAG
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.GapingDragon
            },
            New BossFightInfo(Boss.MoonlightButterfly) With { 'NEEDS BONFIRE AND WARP POS (NOT SURE IF NEEDS FOG FLAG)
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.MoonlightButterfly
            },
            New BossFightInfo(Boss.Sif) With { 'NEEDS BONFIRE, WARP POS, AND CUTSCENE FLAG
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Sif
            },
            New BossFightInfo(Boss.Quelaag) With { 'NEEDS WARP POS
                .BonfireID = 1400980,
                .PlayerWarp = New EntityLocation(17.2, -236.9, 113.6, 75),
                .AdditionalFlags = {
                    -11400000 'btw what is this even
                },
                .EventFlag = BossEvent.Quelaag
            },
            New BossFightInfo(Boss.StrayDemon) With { 'NEEDS BONFIRE, WARP POS, UNDEAD LATE-SYLUM FLAG. PLACE WARP POS SO THAT PLAYER ENDS FORWARD WALK ANIM ON BREAKABLE FLOOR.
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                    11810000 'second time asylum
                },
                .EventFlag = BossEvent.StrayDemon
            },
            New BossFightInfo(Boss.IronGolem) With { 'NEEDS BONFIRE, WARP POS, FIREBOMB WHORE FLAG
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
               .EventFlag = BossEvent.IronGolem
            },
            New BossFightInfo(Boss.OrnsteinAndSmough) With { 'NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.OrnsteinAndSmough
            },
            New BossFightInfo(Boss.Pinwheel) With { 'NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Pinwheel
            },
            New BossFightInfo(Boss.Nito) With { 'FULLY WORKING. FALL DAMAGE IS INCLUDED, AS IT IS PART OF THE BOSS. PERIOD.
                .BonfireID = 1310998,
                .PlayerWarp = New EntityLocation(-111.53, -249.11, -33.67, 295),
                .AdditionalFlags = {
                    -7,
                    11315390,
                    11315393
                },
                .EventFlag = BossEvent.Nito
            },
            New BossFightInfo(Boss.SanctuaryGuardian) With { 'NEEDS BONFIRE AND WARP POS
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.SanctuaryGuardian
            },
            New BossFightInfo(Boss.Artorias) With { 'NEEDS BONFIRE, WARP POS, AND CUTSCENE WATCH FLAG
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Artorias
            },
            New BossFightInfo(Boss.Manus) With { 'NEEDS BONFIRE, WARP POS (DOES IT HAVE A CUTSCENE I FORGOT)
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Manus
            },
            New BossFightInfo(Boss.Ceaseless) With { 'NEEDS WARP POS, AGRO/FOG WALL FLAG, PLAYER STARTS AT FOG, MAKES FIGHT SLIGHTLY MORE CHALLENGING.
                .BonfireID = 1410998,
                .PlayerWarp = New EntityLocation(402.45, -278.15, 15.5, 30), 'todo
                .AdditionalFlags = {
                    -11410800, 'I
                    -11410801, 'DONT
                    -11410900, 'KNOW
                    51410180, 'IF
                    -11415379, 'THESE
                    11415385, 'ARE
                    11415378, 'THE
                    11415373, 'AGRO
                    11415372 'FLAGS
                },
                .EventFlag = BossEvent.Ceaseless
            },
            New BossFightInfo(Boss.DemonFiresage) With { 'NEEDS BONFIRE, WARP POS
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.DemonFiresage
            },
            New BossFightInfo(Boss.CentipedeDemon) With { 'NEEDS BONFIRE, WARP POS, WATCHED CUTSCENE FLAG, MAYBE MAIN BRUSH SCRIPT REMOVES NG OCR/GIVES NG+ OCR?
                .BonfireID = 1410998,
                .PlayerWarp = New EntityLocation(), 'todo todo todo
                .AdditionalFlags = {
                    -11410002,
                    -11410901,
                    11415380,
                    11415383,
                    11415382
                },
                .EventFlag = BossEvent.CentipedeDemon
            },
            New BossFightInfo(Boss.BedOfChaos) With { 'FUNCTIONING AT FIRST GLANCE. NEED TO REVIEW FLAGS. MICHAEL BAY EXPLOSION SLIDE DOWN IS INCLUDED.
                .BonfireID = 1410980,
                .PlayerWarp = New EntityLocation(453.3, -363.6, 337.29, 45.0),
                .AdditionalFlags = {
                    -11410000, 'idk please confirm wut dis is
                    -11410200, 'center platform
                    -11410291, 'arm
                    -11410292 'arm
                },
                .EventFlag = BossEvent.BedOfChaos
            },
            New BossFightInfo(Boss.Kalameet) With { 'NEEDS WARP POS, FLAGS REVIEW
                .BonfireID = 1210998,
                .PlayerWarp = New EntityLocation(876.04, -344.73, 749.75, 240), 'todo
                .AdditionalFlags = {
                    -11210004,
                    -121,
                    11210539,
                    11210535,
                    11210056,
                    -11210066,
                    -11210067,
                    1821,
                    11210592
                },
                .EventFlag = BossEvent.Kalameet
            },
            New BossFightInfo(Boss.Seath) With { 'NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG, FOG WALL FLAG, START PLAYER AT FOG WALL AND HAVE THEM RUN ACROSS
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Seath
            },
            New BossFightInfo(Boss.FourKings) With { 'NEEDS BONFIRE, WARP POS.
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.FourKings,
                .EntranceLua = "DropItem('Rings', 'Covenant of Artorias', 1)
                MsgBoxOK('There is a Covenant of Artorias at your feet if you need it.')"
            },
            New BossFightInfo(Boss.Priscilla) With { 'NEEDS BONFIRE, WARP POS, AGRO FLAG(S), ALSO CHECK THAT THE FLAGS DONT ALLOW YOU TO JUMP OFF AND EXIT ARENA
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Priscilla
            },
            New BossFightInfo(Boss.Gwyndolin) With { 'NEEDS BONFIRE, WARP POS, AGRO FLAG(S), DARK ANOR LONDO FOR STYLE
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Gwyndolin
            },
            New BossFightInfo(Boss.Gwyn) With { 'NEEDS BONFIRE, WARP POS
                .BonfireID = 0,
                .PlayerWarp = New EntityLocation(),
                .AdditionalFlags = {
                },
                .EventFlag = BossEvent.Gwyn
            }
        }
        BossFights = New Dictionary(Of String, BossFightInfo)()
        For Each boss In __bossFights
            BossFights.Add(boss.Name, boss)
        Next
        '"But what was the point of that?!"
        'The point was to get around VB's lack of a non-cancer dictionary initialization syntax. Just easier to read the way I did it.
        'And the reason I wanted it to be dictionary is so it can hash lookup by boss name.
        __bossFights = Nothing
    End Sub
End Class
