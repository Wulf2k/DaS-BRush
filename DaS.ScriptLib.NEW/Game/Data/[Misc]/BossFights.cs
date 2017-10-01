using DaS.ScriptLib.Game.Data.EventFlag;
using DaS.ScriptLib.Game.Data.Structures;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DaS.ScriptLib.Game.Data
{

    public partial class Misc
	{

		public static ReadOnlyDictionary<Boss, BossFightInfo> BossFights { get; private set;  }

		public static BossFightInfo GetBossFight(int bossFlag)
		{
			return BossFights[(Boss)bossFlag];
		}
		
		private static void InitBossFights()
		{
			//Note that the 5th and 6th parameters of the constructor are pre- and post- loading screen flags.
			//A negative flag ID indicates the flag should be set to false
			BossFightInfo[] __bossFights = {
				new BossFightInfo(Boss.AsylumDemon, "Asylum Demon") {
					//NO ADJUSTMENTS NEEDED
					PlayerWarp = new Loc(3.157, 198.148, -3.425, 180),
					World = 18,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -11810000,
						//first time asylum
						11815395,
						//fatass waiting for you...?
						11815395
						//lil door closed...?
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.TaurusDemon, "Taurus Demon") {
					//NO ADJUSTMENTS NEEDED
					PlayerWarp = new Loc(51.89, 17.21, -118.47, 257),
					World = 10,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = {
						
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Gargoyles, "Belfry Gargoyles") {
					//NO ADJUSTMENTS NEEDED
					PlayerWarp = new Loc(17.37, 47.82, 73, 0),
					World = 10,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        11010000,
						//cutscene watched = true
						-11015390,
						//entered fog = false
						-11015393
						//entered boss area = false
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.CapraDemon, "Capra Demon") {
					//NO ADJUSTMENTS NEEDED
					PlayerWarp = new Loc(-72.17, -43.56, -17.17, 321),
					World = 10,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] { }
                },
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.GapingDragon, "Gaping Dragon") {
					//NO ADJUSTMENTS NEEDED
					PlayerWarp = new Loc(-166.61, -100.1, -13.44, 0),
					World = 10,
					Area = 0,
					WarpID = -1,
						//Channeler death flag
					AdditionalFlags = new int[] { 11000853 }
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.MoonlightButterfly, "Moonlight Butterfly") {
					//NO ADJUSTMENTS NEEDED, but check if he gets stuck flying down
					PlayerWarp = new Loc(180.48, 8, 29.19, 306),
					World = 12,
					Area = 0,
					WarpID = -1,
					AdditionalFlags = {
						
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Sif, "Great Grey Wolf Sif") {
					//NEEDS BONFIRE, WARP POS, AND CUTSCENE FLAG
					PlayerWarp = new Loc(275, -19, -264.43, 210),
					//275, -19.82, -264.43, 210
					World = 12,
					Area = 0,
					WarpID = 1202997,
					AdditionalFlags = new int[] { }
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Quelaag, "Chaos Witch Quelaag") {
					//
					PlayerWarp = new Loc(12.8, -237, 113.6, 75),
					World = 14,
					Area = 0,
					WarpID = -1,
						//btw what is this even
					AdditionalFlags = new int[] { -11400000 }
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.StrayDemon, "Stray Demon") {
					//NEEDS BONFIRE, WARP POS, UNDEAD LATE-SYLUM FLAG. PLACE WARP POS SO THAT PLAYER ENDS FORWARD WALK ANIM ON BREAKABLE FLOOR.
					PlayerWarp = new Loc(),
					//2.6, 197.5, -18, 180
					World = 18,
					Area = 1,
					WarpID = 1812996,
					PlayerAnim = -1,
						//second time asylum
					AdditionalFlags = new int[] { 11810000 }
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.IronGolem, "Iron Golem") {
					//NEEDS BONFIRE, WARP POS, FIREBOMB WHORE FLAG
					PlayerWarp = new Loc(82.28, 82.25, 254.86, 82),
					World = 15,
					Area = 0,
					WarpID = -1,
					AdditionalFlags = {
						
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.OrnsteinAndSmough, "Ornstein & Smough") {
					//NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG
					PlayerWarp = new Loc(536.1, 142.6, 255.1, 90),
					World = 15,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = {
						
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Pinwheel, "Pinwheel") {
					//NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG
					PlayerWarp = new Loc(),
					World = 13,
					Area = 0,
					WarpID = 1302998,
					AdditionalFlags = {
						
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Nito, "Gravelord Nito") {
					//FULLY WORKING. FALL DAMAGE IS INCLUDED, AS IT IS PART OF THE EventID.Boss. PERIOD.
					PlayerWarp = new Loc(-111.53, -249.11, -33.67, 295),
					World = 13,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -7,
						11315390,
						11315393
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.SanctuaryGuardian, "Sanctuary Guardian") {
					//NEEDS BONFIRE AND WARP POS
					PlayerWarp = new Loc(930.2, -318.6, 470.5, 30),
					World = 12,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] { }
                },
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Artorias, "Knight Artorias") {
					//NEEDS BONFIRE, WARP POS, AND CUTSCENE WATCH FLAG
					PlayerWarp = new Loc(1033.5, -330, 810.4, 70),
					World = 12,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = {
						
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Manus, "Manus, Father of the Abyss") {
					//NEEDS BONFIRE, WARP POS (DOES IT HAVE A CUTSCENE I FORGOT)
					PlayerWarp = new Loc(862.5, -538.3, 882.3, 220),
					World = 12,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] { }
                },
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Ceaseless, "Ceaseless Discharge") {
					//NEEDS WARP POS, AGRO/FOG WALL FLAG, PLAYER STARTS AT FOG, MAKES FIGHT SLIGHTLY MORE CHALLENGING.
					PlayerWarp = new Loc(248.4, -283.1, 70, 40),
					//todo
					World = 14,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -11410800,
						//I
						-11410801,
						//DONT
						-11410900,
						//KNOW
						51410180,
						//IF
						-11415379,
						//THESE
						11415385,
						//ARE
						11415378,
						//THE
						11415373,
						//AGRO
						11415372
						//FLAGS
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.DemonFiresage, "Demon Firesage") {
					//NEEDS BONFIRE, WARP POS
					PlayerWarp = new Loc(150, -341, 94, 315),
					World = 14,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] { }
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.CentipedeDemon, "Centipede Demon") {
					//NEEDS BONFIRE, WARP POS, WATCHED CUTSCENE FLAG, MAYBE MAIN BRUSH SCRIPT REMOVES NG OCR/GIVES NG+ OCR?
					PlayerWarp = new Loc(167, -383.4, 81.2, 135),
					//todo todo todo
					World = 14,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -11410002,
						-11410901,
						11415380,
						11415383,
						11415382
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.BedOfChaos, "Bed of Chaos") {
					//FUNCTIONING AT FIRST GLANCE. NEED TO REVIEW FLAGS. MICHAEL BAY EXPLOSION SLIDE DOWN IS INCLUDED.
					PlayerWarp = new Loc(453.3, -363.6, 337.29, 45.0),
					World = 14,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -11410000,
						//idk please confirm wut dis is
						-11410200,
						//center platform
						-11410291,
						//arm
						-11410292
						//arm
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Kalameet, "Black Dragon Kalameet") {
					//NEEDS WARP POS, FLAGS REVIEW
					PlayerWarp = new Loc(877.4, -344.73, 751.3, 220),
					//todo
					World = 12,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
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
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Seath, "Seath the Scaleless") {
					//NEEDS BONFIRE, WARP POS, CUTSCENE WATCH FLAG, FOG WALL FLAG, START PLAYER AT FOG WALL AND HAVE THEM RUN ACROSS
					PlayerWarp = new Loc(135.5, 136.5, 829.2, 315),
					World = 17,
					Area = 0,
					WarpID = -1,
					AdditionalFlags = new int[] { }
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.FourKings, "Four Kings") {
					//NEEDS BONFIRE, WARP POS.
					PlayerWarp = new Loc(),
					World = 16,
					Area = 0,
					WarpID = 1602998,
					AdditionalFlags = {
						
					},
					EntranceLua = "AddInventoryItem(138, 0x20000000, 1)"
					//Removed MsgBox because it wouldn't clear?  Check again later.
                    //Oh now realize what you meant lol
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Priscilla, "Crossbreed Priscilla") {
					//NEEDS BONFIRE, WARP POS, AGRO FLAG(S), ALSO CHECK THAT THE FLAGS DONT ALLOW YOU TO JUMP OFF AND EXIT ARENA
					PlayerWarp = new Loc(-22.7, 60.7, 715, 180),
					World = 11,
					Area = 0,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -4,
						1691,
						1692,
						-11100000,
						-11100531
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Gwyndolin, "Dark Sun Gwyndolin") {
					//NEEDS BONFIRE, WARP POS, AGRO FLAG(S), DARK ANOR LONDO FOR STYLE
					PlayerWarp = new Loc(432.5, 60.2, 254.9, 90),
					World = 15,
					Area = 1,
					WarpID = -1,
					AdditionalFlags = new int[] {
                        -11510523,
						-11510900
					}
				},
				new BossFightInfo(DaS.ScriptLib.Game.Data.EventFlag.Boss.Gwyn, "Gwyn, Lord of Cinder") {
					PlayerWarp = new Loc(420.2, -115.7, 168, 300),
					World = 18,
					Area = 0,
					WarpID = -1,
					AdditionalFlags = {
						
					}
				}
			};
			var d = new Dictionary<Boss, BossFightInfo>();
			foreach (BossFightInfo boss in __bossFights) {
				d.Add(boss.SpawnFlag, boss);
			}
			BossFights = new ReadOnlyDictionary<Boss, BossFightInfo>(d);
			//"But what was the point of that?!"
			//The point was to get around VB's lack of a decent dictionary initialization syntax. Just easier to read the way I did it.
			//And the reason I wanted it to be dictionary is so it can hash lookup by boss name.
			__bossFights = null;
		}

	}

}
