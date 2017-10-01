using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DaS.ScriptLib.LuaScripting;

namespace DaS.ScriptLib.Game.Data.Helpers
{
    public class EventFlagHelper
	{
		public int ID = 0;

		public bool State = false;
		public void ApplyFlag()
		{
			DSLua.Expr($"SetEventFlag({ID}, {State.ToString().ToLower()})");
		}

		public static void ApplyAll(int[] flagList)
		{
			foreach (int flag in flagList) {
				var fuckVb = new EventFlagHelper(flag);
				fuckVb.ApplyFlag();
			}
		}

		public EventFlagHelper(int id, bool state)
		{
			this.ID = id;
			this.State = state;
		}

		public EventFlagHelper(int id)
		{
			State = (id > 0);
			this.ID = Math.Abs(id);
		}

		public static List<EventFlagHelper> GetList(params int[] flags)
		{
			return flags.Select(x => new EventFlagHelper(x)).ToList();
		}

	}
}
