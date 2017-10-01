using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DaS.ScriptLib.Game.Data
{
	public partial class EntityName
	{

		public static ReadOnlyDictionary<EventFlag.Boss, string> Boss { get; }

		private static void InitBoss()
		{
			Dictionary<EventFlag.Boss, string> d = new Dictionary<EventFlag.Boss, string>();

			d.Add(DaS.ScriptLib.Game.Data.EventFlag.Boss.Artorias, "Knight Artorias");

			_Boss = new ReadOnlyDictionary<EventFlag.Boss, string>(d);
		}

	}
}
