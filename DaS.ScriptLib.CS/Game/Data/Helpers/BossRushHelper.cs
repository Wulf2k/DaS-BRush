using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Threading;
using DaS.ScriptLib.LuaScripting;
using DaS.ScriptLib.Injection;
using DaS.ScriptLib.Game.Data.EventFlag;

namespace DaS.ScriptLib.Game.Data.Helpers
{
	public class BossRushType
	{
		public static readonly string Standard = "Standard";
		public static readonly string Reverse = "Reverse";
		public static readonly string Random = "Random";
		public static readonly string Custom = "Custom";
	}

	public class BossRushHelper
	{

		public static readonly int BossRushTimerInterval = 33;

		private static void BeginRushTimer()
		{
			string msg = null;

			Funcs.SetKeyGuideTextPos(600, 605);
			Funcs.SetLineHelpTextPos(1100, 675);

			//Clear TrueDeaths
			DaS.ScriptLib.Game.Mem.GameStats.TrueDeathCount.Value = 0;
			DaS.ScriptLib.Game.Mem.GameStats.TotalPlayTime.Value = 0;

			do {
				msg = Funcs.GetNgPlusText(DaS.ScriptLib.Game.Mem.GameStats.ClearCount.Value) + " - ";
				msg = msg + Strings.Left(TimeSpan.FromMilliseconds(DaS.ScriptLib.Game.Mem.GameStats.TotalPlayTime.Value).ToString(), 12) + Strings.ChrW(0);
				Funcs.SetKeyGuideText(msg);
				Hook.WUnicodeStr(0x11a7770, msg);
				msg = "Deaths: " + DaS.ScriptLib.Game.Mem.GameStats.TrueDeathCount.Value + Strings.ChrW(0);
				Funcs.SetLineHelpText(msg);
				Thread.Sleep(33);
			} while (true);
		}


		private static Thread rushTimer;
		public static void StartNewBossRushTimer()
		{
			rushTimer = new Thread(BeginRushTimer);
			rushTimer.IsBackground = true;
			rushTimer.Start();
		}

		public static string StopBossRushTimer()
		{
			if (rushTimer != null) {
				rushTimer.Abort();
			}
			Funcs.SetKeyGuideTextClear();
			Funcs.SetLineHelpTextClear();
			return Strings.Left(TimeSpan.FromMilliseconds(DaS.ScriptLib.Game.Mem.GameStats.TotalPlayTime.Value).ToString(), 12);
		}

		public static int[] GetBossRushOrder(string rushType, bool excludeBedOfChaos, string customOrder)
		{

			dynamic bossPool = Misc.BossFights.Keys.Cast<int>().ToList();

			if ((rushType != BossRushType.Custom) && excludeBedOfChaos) {
				bossPool.Remove(Boss.BedOfChaos);
			}

			if (rushType == BossRushType.Standard) {
				return bossPool.ToArray();
			} else if (rushType == BossRushType.Reverse) {
				dynamic reversedBossPool = new List<int>();
				for (i = bossPool.Count - 1; i >= 0; i += -1) {
					reversedBossPool.Add(bossPool[i]);
				}
				return reversedBossPool.ToArray();
			} else if (rushType == BossRushType.Random) {
				List<int> randomBossPool = new List<int>();

				while (randomBossPool.Count < bossPool.Count) {
					Random rand = new Random();
					dynamic randIndex = rand.Next(bossPool.Count);
					randomBossPool.Add(bossPool[randIndex]);
					bossPool.RemoveAt(randIndex);
				}

				return randomBossPool.ToArray();
			} else if (rushType == BossRushType.Custom) {
				return customOrder.Split(";").Select(x => Int32.Parse(x.Trim())).ToArray();
			}

			return new Boss[];
		}

		public static bool CheckWaitPlayerDead(int numFrames, int frameLength = 33)
		{
			dynamic numFramesAlive = 0;
			dynamic numFramesDead = 0;
			for (i = 1; i <= numFrames; i++) {
				if (DaS.ScriptLib.Game.Mem.Player.HP.Value == 0) {
					numFramesDead += 1;
				} else {
					numFramesAlive += 1;
				}

				Thread.Sleep(frameLength);
			}

			return numFramesDead > numFramesAlive;
		}

		// Will assume boss is dead if valid data isn't saved for it in the BossInfo yet.
		public static bool WaitForBossDeathByEventFlag(double bossFlag)
		{
			dynamic boss = Misc.BossFights[(Boss)Convert.ToInt32(bossFlag)];
			dynamic bossIsDead = false;
			dynamic playerIsDead = false;
			do {
				//boss dead
				if ((DSLua.Expr($"IsCompleteEvent({CType(boss.SpawnFlag, Integer)})"))) {
					bossIsDead = true;
				} else if (DaS.ScriptLib.Game.Mem.Player.HP.Value == 0) {
					playerIsDead = true;
				}
				Thread.Sleep(33);
			} while (!(bossIsDead | playerIsDead));

			if (bossIsDead) {
				return true;
			} else {
				return false;
				//Returns false to lua script and lua script handles permadeath failure message etc and retries
			}
		}

	}
}
