using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using DaS.ScriptLib.Injection.Structures;

namespace DaS.ScriptLib.Game.Mem
{
	public class GameStats
	{
		public static LivePtrVar<Int32> ClearCount = new LivePtrVar<Int32>(() => Ptr.GameStatsPtr.Value + 0x3c);
		public static LivePtrVar<Int32> TrueDeathCount = new LivePtrVar<Int32>(() => Ptr.GameStatsPtr.Value + 0x58);
		public static LivePtrVar<Int32> TotalPlayTime = new LivePtrVar<Int32>(() => Ptr.GameStatsPtr.Value + 0x68);
	}
}
