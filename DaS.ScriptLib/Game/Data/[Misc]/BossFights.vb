Imports System.Collections.ObjectModel
Imports DaS.ScriptLib.Game.Data.Structures

Namespace Game.Data

    Partial Public Class Misc

        Public Shared ReadOnly Property BossFights As ReadOnlyDictionary(Of EventFlag.Boss, BossFightInfo)

        Public Shared Function GetBossFight(bossFlag As Double) As BossFightInfo
            Return BossFights(CType(CType(bossFlag, Integer), EventFlag.Boss))
        End Function

        Private Shared Sub InitBossFights()
            'Note that the 5th and 6th parameters of the constructor are pre- and post- loading screen flags.
            'A negative flag ID indicates the flag should be set to false
            Dim __bossFights As BossFightInfo() = {
                New BossFightInfo(EventFlag.Boss.AsylumDemon, "Asylum Demon") With { 'NO ADJUSTMENTS NEEDED
                    .PlayerWarp = New Loc(3.157, 198.148, -3.425, 180),
                    .World = 18,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                        -11810000, 'first time asylum
                        11815395, 'fatass waiting for you...?
                        11815395 'lil door closed...?
                    }
                },
                New BossFightInfo(EventFlag.Boss.TaurusDemon, "Taurus Demon") With { 'NO ADJUSTMENTS NEEDED
                    .PlayerWarp = New Loc(51.89, 17.21, -118.47, 257),
                    .World = 10,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Gargoyles, "Belfry Gargoyles") With { 'NO ADJUSTMENTS NEEDED
                    .PlayerWarp = New Loc(17.37, 47.82, 73, 0),
                    .World = 10,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                        11010000, 'cutscene watched = true
                        -11015390, 'entered fog = false
                        -11015393 'entered boss area = false
                    }
                },
                New BossFightInfo(EventFlag.Boss.CapraDemon, "Capra Demon") With { 'NO ADJUSTMENTS NEEDED
                    .PlayerWarp = New Loc(-72.17, -43.56, -17.17, 321),
                    .World = 10,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.GapingDragon, "Gaping Dragon") With { 'NO ADJUSTMENTS NEEDED
                    .PlayerWarp = New Loc(-166.61, -100.1, -13.44, 0),
                    .World = 10,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {11000853    'Channeler death flag
                    }
                },
                New BossFightInfo(EventFlag.Boss.MoonlightButterfly, "Moonlight Butterfly") With { 'NO ADJUSTMENTS NEEDED, but check if he gets stuck flying down
                    .PlayerWarp = New Loc(180.48, 8, 29.19, 306),
                    .World = 12,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Sif, "Great Grey Wolf Sif") With { 'NEEDS BONFIRE, WARP POS, AND CUTSCENE FLAG
                    .PlayerWarp = New Loc(275, -19, -264.43, 210), '275, -19.82, -264.43, 210
                    .World = 12,
                    .Area = 0,
                    .WarpID = 1202997,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Quelaag, "Chaos Witch Quelaag") With { '
                    .PlayerWarp = New Loc(12.8, -237, 113.6, 75),
                    .World = 14,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {
                        -11400000 'btw what is this even
                    }
                },
                New BossFightInfo(EventFlag.Boss.StrayDemon, "Stray Demon") With { 'NEEDS BONFIRE, WARP POS, UNDEAD LATE-SYLUM FLAG. PLACE WARP POS SO THAT PLAYER ENDS FORWARD WALK ANIM ON BREAKABLE FLOOR.
                    .PlayerWarp = New Loc(), '2.6, 197.5, -18, 180
                    .World = 18,
                    .Area = 1,
                    .WarpID = 1812996,
                    .PlayerAnim = -1,
                    .AdditionalFlags = {
                        11810000 'second time asylum
                    }
                },
                New BossFightInfo(EventFlag.Boss.IronGolem, "Iron Golem") With { 'NEEDS BONFIRE, WARP POS, FIREBOMB WHORE FLAG
                    .PlayerWarp = New Loc(82.28, 82.25, 254.86, 82),
                    .World = 15,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.OrnsteinAndSmough, "Ornstein & Smough") With { 'NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG
                    .PlayerWarp = New Loc(536.1, 142.6, 255.1, 90),
                    .World = 15,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Pinwheel, "Pinwheel") With { 'NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG
                    .PlayerWarp = New Loc(),
                    .World = 13,
                    .Area = 0,
                    .WarpID = 1302998,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Nito, "Gravelord Nito") With { 'FULLY WORKING. FALL DAMAGE IS INCLUDED, AS IT IS PART OF THE EventID.Boss. PERIOD.
                    .PlayerWarp = New Loc(-111.53, -249.11, -33.67, 295),
                    .World = 13,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                        -7,
                        11315390,
                        11315393
                    }
                },
                New BossFightInfo(EventFlag.Boss.SanctuaryGuardian, "Sanctuary Guardian") With { 'NEEDS BONFIRE AND WARP POS
                    .PlayerWarp = New Loc(930.2, -318.6, 470.5, 30),
                    .World = 12,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Artorias, "Knight Artorias") With { 'NEEDS BONFIRE, WARP POS, AND CUTSCENE WATCH FLAG
                    .PlayerWarp = New Loc(1033.5, -330, 810.4, 70),
                    .World = 12,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Manus, "Manus, Father of the Abyss") With { 'NEEDS BONFIRE, WARP POS (DOES IT HAVE A CUTSCENE I FORGOT)
                    .PlayerWarp = New Loc(862.5, -538.3, 882.3, 220),
                    .World = 12,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.Ceaseless, "Ceaseless Discharge") With { 'NEEDS WARP POS, AGRO/FOG WALL FLAG, PLAYER STARTS AT FOG, MAKES FIGHT SLIGHTLY MORE CHALLENGING.
                    .PlayerWarp = New Loc(248.4, -283.1, 70, 40), 'todo
                    .World = 14,
                    .Area = 1,
                    .WarpID = -1,
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
                    }
                },
                New BossFightInfo(EventFlag.Boss.DemonFiresage, "Demon Firesage") With { 'NEEDS BONFIRE, WARP POS
                    .PlayerWarp = New Loc(150, -341, 94, 315),
                    .World = 14,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.CentipedeDemon, "Centipede Demon") With { 'NEEDS BONFIRE, WARP POS, WATCHED CUTSCENE FLAG, MAYBE MAIN BRUSH SCRIPT REMOVES NG OCR/GIVES NG+ OCR?
                    .PlayerWarp = New Loc(167, -383.4, 81.2, 135), 'todo todo todo
                    .World = 14,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                        -11410002,
                        -11410901,
                        11415380,
                        11415383,
                        11415382
                    }
                },
                New BossFightInfo(EventFlag.Boss.BedOfChaos, "Bed of Chaos") With { 'FUNCTIONING AT FIRST GLANCE. NEED TO REVIEW FLAGS. MICHAEL BAY EXPLOSION SLIDE DOWN IS INCLUDED.
                    .PlayerWarp = New Loc(453.3, -363.6, 337.29, 45.0),
                    .World = 14,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {
                        -11410000, 'idk please confirm wut dis is
                        -11410200, 'center platform
                        -11410291, 'arm
                        -11410292 'arm
                    }
                },
                New BossFightInfo(EventFlag.Boss.Kalameet, "Black Dragon Kalameet") With { 'NEEDS WARP POS, FLAGS REVIEW
                    .PlayerWarp = New Loc(877.4, -344.73, 751.3, 220), 'todo
                    .World = 12,
                    .Area = 1,
                    .WarpID = -1,
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
                    }
                },
                New BossFightInfo(EventFlag.Boss.Seath, "Seath the Scaleless") With { 'NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG, FOG WALL FLAG, START PLAYER AT FOG WALL AND HAVE THEM RUN ACROSS
                    .PlayerWarp = New Loc(135.5, 136.5, 829.2, 315),
                    .World = 17,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                },
                New BossFightInfo(EventFlag.Boss.FourKings, "Four Kings") With { 'NEEDS BONFIRE, WARP POS.
                    .PlayerWarp = New Loc(),
                    .World = 16,
                    .Area = 0,
                    .WarpID = 1602998,
                    .AdditionalFlags = {
                    },
                    .EntranceLua = "AddInventoryItem(138, 0x20000000, 1)"    'Removed MsgBox because it wouldn't clear?  Check again later.
                },
                New BossFightInfo(EventFlag.Boss.Priscilla, "Crossbreed Priscilla") With { 'NEEDS BONFIRE, WARP POS, AGRO FLAG(S), ALSO CHECK THAT THE FLAGS DONT ALLOW YOU TO JUMP OFF AND EXIT ARENA
                    .PlayerWarp = New Loc(-22.7, 60.7, 715, 180),
                    .World = 11,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {-4, 1691, 1692, -11100000, -11100531
                    }
                },
                New BossFightInfo(EventFlag.Boss.Gwyndolin, "Dark Sun Gwyndolin") With { 'NEEDS BONFIRE, WARP POS, AGRO FLAG(S), DARK ANOR LONDO FOR STYLE
                    .PlayerWarp = New Loc(432.5, 60.2, 254.9, 90),
                    .World = 15,
                    .Area = 1,
                    .WarpID = -1,
                    .AdditionalFlags = {-11510523, -11510900
                    }
                },
                New BossFightInfo(EventFlag.Boss.Gwyn, "Gwyn, Lord of Cinder") With {
                    .PlayerWarp = New Loc(420.2, -115.7, 168, 300),
                    .World = 18,
                    .Area = 0,
                    .WarpID = -1,
                    .AdditionalFlags = {
                    }
                }
            }
            Dim d = New Dictionary(Of EventFlag.Boss, BossFightInfo)()
            For Each boss In __bossFights
                d.Add(boss.SpawnFlag, boss)
            Next
            _BossFights = New ReadOnlyDictionary(Of EventFlag.Boss, BossFightInfo)(d)
            '"But what was the point of that?!"
            'The point was to get around VB's lack of a decent dictionary initialization syntax. Just easier to read the way I did it.
            'And the reason I wanted it to be dictionary is so it can hash lookup by boss name.
            __bossFights = Nothing
        End Sub

    End Class

End Namespace