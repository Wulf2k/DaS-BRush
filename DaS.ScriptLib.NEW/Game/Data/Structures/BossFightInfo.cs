namespace DaS.ScriptLib.Game.Data.Structures
{
    public class BossFightInfo
	{
		public EventFlag.Boss SpawnFlag { get; private set;  }
		public string Name { get; private set; }

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
			Name = name;
		}

	}
}
