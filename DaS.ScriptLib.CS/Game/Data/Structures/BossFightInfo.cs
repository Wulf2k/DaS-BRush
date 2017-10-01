using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace DaS.ScriptLib.Game.Data.Structures
{
	public class BossFightInfo
	{
		public EventFlag.Boss SpawnFlag { get; }
		public string Name { get; }

		public Loc PlayerWarp = Loc.Zero;
		public int World = 0;
		public int Area = 0;
		public int WarpID = -1;
		public int PlayerAnim = 7410;
		public int[] AdditionalFlags = {
			
		};

		public string EntranceLua = "";
		public BossFightInfo(EventFlag.Boss spawnFlag, string name)
		{
			this.SpawnFlag = spawnFlag;
			_Name = name;
		}

	}
}
