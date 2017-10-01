using DaS.ScriptLib.Injection;

namespace DaS.ScriptLib.Game.Data.Structures
{

    public class MSB
	{

		public static int Pointer {
			get { return Hook.RInt32(0x13785a0); }
		}

		public static int FirstEntryPointer {
			get { return Hook.RInt32(Pointer + 0xc); }
		}

		public static object Entries {
			get { return "fuck off vb"; }
		}


		private static void ReadEntries()
		{


		}

	}

}
