using static DaS.ScriptLib.Injection.Hook;

namespace DaS.ScriptLib.Game.Mem
{
    public class GameStats
    {
        public static int ClearCount
        {
            get => RInt32(Ptr.GameStatsPtr + 0x3c);
            set => WInt32(Ptr.GameStatsPtr + 0x3c, value);
        }

		public static int TrueDeathCount
        {
            get => RInt32(Ptr.GameStatsPtr + 0x58);
            set => WInt32(Ptr.GameStatsPtr + 0x58, value);
        }

		public static int TotalPlayTime
        {
            get => RInt32(Ptr.GameStatsPtr + 0x68);
            set => WInt32(Ptr.GameStatsPtr + 0x68, value);
        }
	}
}
