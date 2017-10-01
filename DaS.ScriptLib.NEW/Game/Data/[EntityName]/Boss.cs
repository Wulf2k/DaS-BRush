using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DaS.ScriptLib.Game.Data
{
    public partial class EntityName
	{

		public static ReadOnlyDictionary<EventFlag.Boss, string> Boss { get; private set;  }

		private static void InitBoss()
		{
			Dictionary<EventFlag.Boss, string> d = new Dictionary<EventFlag.Boss, string>();

			d.Add(DaS.ScriptLib.Game.Data.EventFlag.Boss.Artorias, "Knight Artorias");

			Boss = new ReadOnlyDictionary<EventFlag.Boss, string>(d);
		}

	}
}
