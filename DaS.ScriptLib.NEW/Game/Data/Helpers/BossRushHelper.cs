using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using DaS.ScriptLib.LuaScripting;
using DaS.ScriptLib.Injection;
using DaS.ScriptLib.Game.Data.EventFlag;
using DaS.ScriptLib.Game.Data.Structures;

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
            Mem.GameStats.TrueDeathCount = 0;
            Mem.GameStats.TotalPlayTime = 0;

			do {
				msg = Funcs.GetNgPlusText(Mem.GameStats.ClearCount) + " - ";
				msg = msg + TimeSpan.FromMilliseconds(Mem.GameStats.TotalPlayTime).ToString().Substring(0, 12) + (char)0;
				Funcs.SetKeyGuideText(msg);
				Hook.WUnicodeStr(0x11a7770, msg);
				msg = "Deaths: " + Mem.GameStats.TrueDeathCount + (char)0;
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
			return TimeSpan.FromMilliseconds(Mem.GameStats.TotalPlayTime).ToString().Substring(0, 12);
		}

		public static int[] GetBossRushOrder(string rushType, bool excludeBedOfChaos, string customOrder)
		{

			var bossPool = Misc.BossFights.Keys.Cast<int>().ToList();

			if ((rushType != BossRushType.Custom) && excludeBedOfChaos) {
				bossPool.Remove((int)Boss.BedOfChaos);
			}

			if (rushType == BossRushType.Standard) {
				return bossPool.ToArray();
			} else if (rushType == BossRushType.Reverse) {
				var reversedBossPool = new List<int>();
				for (int i = bossPool.Count - 1; i >= 0; i += -1) {
					reversedBossPool.Add(bossPool[i]);
				}
				return reversedBossPool.ToArray();
			} else if (rushType == BossRushType.Random) {
				List<int> randomBossPool = new List<int>();

				while (randomBossPool.Count < bossPool.Count) {
					Random rand = new Random();
					var randIndex = rand.Next(bossPool.Count);
					randomBossPool.Add(bossPool[randIndex]);
					bossPool.RemoveAt(randIndex);
				}

				return randomBossPool.ToArray();
			} else if (rushType == BossRushType.Custom) {
				return customOrder.Split(';').Select(x => Int32.Parse(x.Trim())).ToArray();
			}

			return new int[0];
		}

		public static bool CheckWaitPlayerDead(int numFrames, int frameLength = 33)
		{
			var numFramesAlive = 0;
			var numFramesDead = 0;
			for (int i = 1; i <= numFrames; i++) {
				if (Entity.Player.HP == 0 || Entity.Player.IsDead) {
					numFramesDead += 1;
				} else {
					numFramesAlive += 1;
				}

				Thread.Sleep(frameLength);
			}

			return numFramesDead > numFramesAlive;
		}

		// Will assume boss is dead if valid data isn't saved for it in the BossInfo yet.
		public static bool WaitForBossDeathByEventFlag(int bossFlag)
		{
			var boss = Misc.BossFights[(Boss)bossFlag];
			var bossIsDead = false;
			var playerIsDead = false;
			do {
				//boss dead
				if ((bool)(DSLua.Expr($"IsCompleteEvent({(int)boss.SpawnFlag})"))[0]) {
					bossIsDead = true;
				} else if (Entity.Player.HP == 0 || Entity.Player.IsDead) {
					playerIsDead = true;
				}
				Thread.Sleep(33);
			} while (!bossIsDead && !playerIsDead);

			if (bossIsDead) {
				return true;
			} else {
				return false;
				//Returns false to lua script and lua script handles permadeath failure message etc and retries
			}
		}

	}
}
